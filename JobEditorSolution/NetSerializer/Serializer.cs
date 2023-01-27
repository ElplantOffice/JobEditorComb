using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace NetSerializer
{
    public static class Serializer
    {
        private static Dictionary<Type, ushort> s_typeIDMap;

        private static Serializer.SerializerSwitch s_serializerSwitch;

        private static Serializer.DeserializerSwitch s_deserializerSwitch;

        private static ITypeSerializer[] s_typeSerializers;

        private static ITypeSerializer[] s_userTypeSerializers;

        private static bool s_initialized;

        static Serializer()
        {
            ITypeSerializer[] primitivesSerializer = new ITypeSerializer[] { new PrimitivesSerializer(), new ArraySerializer(), new EnumSerializer(), new DictionarySerializer(), new GenericSerializer() };
            Serializer.s_typeSerializers = primitivesSerializer;
        }

        public static object Deserialize(Stream stream)
        {
            object obj = null;
            if (!Serializer.s_initialized)
            {
                throw new InvalidOperationException("NetSerializer not initialized");
            }
            Serializer.s_deserializerSwitch(stream, out obj);
            return obj;
        }

        internal static object DeserializeInternal(Stream stream)
        {
            object obj = null;
            Serializer.s_deserializerSwitch(stream, out obj);
            return obj;
        }

        private static void GenerateDynamic(Dictionary<Type, TypeData> map)
        {
            foreach (KeyValuePair<Type, TypeData> keyValuePair in map)
            {
                Type key = keyValuePair.Key;
                TypeData value = keyValuePair.Value;
                if (!value.IsGenerated)
                {
                    continue;
                }
                DynamicMethod dynamicMethod = SerializerCodegen.GenerateDynamicSerializerStub(key);
                value.WriterMethodInfo = dynamicMethod;
                value.WriterILGen = dynamicMethod.GetILGenerator();
                DynamicMethod dynamicMethod1 = DeserializerCodegen.GenerateDynamicDeserializerStub(key);
                value.ReaderMethodInfo = dynamicMethod1;
                value.ReaderILGen = dynamicMethod1.GetILGenerator();
            }
            Type[] typeArray = new Type[] { typeof(Stream), typeof(object) };
            DynamicMethod dynamicMethod2 = new DynamicMethod("SerializerSwitch", null, typeArray, typeof(Serializer), true);
            dynamicMethod2.DefineParameter(1, ParameterAttributes.None, "stream");
            dynamicMethod2.DefineParameter(2, ParameterAttributes.None, "value");
            DynamicMethod dynamicMethod3 = dynamicMethod2;
            Type[] typeArray1 = new Type[] { typeof(Stream), typeof(object).MakeByRefType() };
            DynamicMethod dynamicMethod4 = new DynamicMethod("DeserializerSwitch", null, typeArray1, typeof(Serializer), true);
            dynamicMethod4.DefineParameter(1, ParameterAttributes.None, "stream");
            dynamicMethod4.DefineParameter(2, ParameterAttributes.Out, "value");
            CodeGenContext codeGenContext = new CodeGenContext(map, dynamicMethod3, dynamicMethod4);
            foreach (KeyValuePair<Type, TypeData> keyValuePair1 in map)
            {
                Type type = keyValuePair1.Key;
                TypeData typeDatum = keyValuePair1.Value;
                if (!typeDatum.IsGenerated)
                {
                    continue;
                }
                typeDatum.TypeSerializer.GenerateWriterMethod(type, codeGenContext, typeDatum.WriterILGen);
                typeDatum.TypeSerializer.GenerateReaderMethod(type, codeGenContext, typeDatum.ReaderILGen);
            }
            SerializerCodegen.GenerateSerializerSwitch(codeGenContext, dynamicMethod2.GetILGenerator(), map);
            Serializer.s_serializerSwitch = (Serializer.SerializerSwitch)dynamicMethod2.CreateDelegate(typeof(Serializer.SerializerSwitch));
            DeserializerCodegen.GenerateDeserializerSwitch(codeGenContext, dynamicMethod4.GetILGenerator(), map);
            Serializer.s_deserializerSwitch = (Serializer.DeserializerSwitch)dynamicMethod4.CreateDelegate(typeof(Serializer.DeserializerSwitch));
        }

        private static Dictionary<Type, TypeData> GenerateTypeData(Type[] rootTypes)
        {
            TypeData typeDatum;
            MethodInfo methodInfo;
            MethodInfo methodInfo1;
            Dictionary<Type, TypeData> types = new Dictionary<Type, TypeData>();
            Stack<Type> types1 = new Stack<Type>(PrimitivesSerializer.GetSupportedTypes().Concat<Type>(rootTypes));
            ushort num = 1;
            while (types1.Count > 0)
            {
                Type type = types1.Pop();
                if (types.ContainsKey(type) || type.IsAbstract || type.IsInterface)
                {
                    continue;
                }
                if (type.ContainsGenericParameters)
                {
                    throw new NotSupportedException(string.Format("Type {0} contains generic parameters", type.FullName));
                }
                ITypeSerializer typeSerializer = Serializer.s_userTypeSerializers.FirstOrDefault<ITypeSerializer>((ITypeSerializer h) => h.Handles(type)) ?? Serializer.s_typeSerializers.FirstOrDefault<ITypeSerializer>((ITypeSerializer h) => h.Handles(type));
                if (typeSerializer == null)
                {
                    throw new NotSupportedException(string.Format("No serializer for {0}", type.FullName));
                }
                foreach (Type subtype in typeSerializer.GetSubtypes(type))
                {
                    types1.Push(subtype);
                }
                if (!(typeSerializer is IStaticTypeSerializer))
                {
                    if (!(typeSerializer is IDynamicTypeSerializer))
                    {
                        throw new Exception();
                    }
                    IDynamicTypeSerializer dynamicTypeSerializer = (IDynamicTypeSerializer)typeSerializer;
                    ushort num1 = num;
                    num = (ushort)(num1 + 1);
                    typeDatum = new TypeData(num1, dynamicTypeSerializer);
                }
                else
                {
                    ((IStaticTypeSerializer)typeSerializer).GetStaticMethods(type, out methodInfo, out methodInfo1);
                    ushort num2 = num;
                    num = (ushort)(num2 + 1);
                    typeDatum = new TypeData(num2, methodInfo, methodInfo1);
                }
                types[type] = typeDatum;
            }
            return types;
        }

        private static ushort GetTypeID(object ob)
        {
            ushort num;
            if (ob == null)
            {
                return (ushort)0;
            }
            Type type = ob.GetType();
            if (!Serializer.s_typeIDMap.TryGetValue(type, out num))
            {
                throw new InvalidOperationException(string.Format("Unknown type {0}", type.FullName));
            }
            return num;
        }

        public static void Initialize(Type[] rootTypes)
        {
            Serializer.Initialize(rootTypes, new ITypeSerializer[0]);
        }

        public static void Initialize(Type[] rootTypes, ITypeSerializer[] userTypeSerializers)
        {
            if (Serializer.s_initialized)
            {
                throw new InvalidOperationException("NetSerializer already initialized");
            }
            if (!((IEnumerable<ITypeSerializer>)userTypeSerializers).All<ITypeSerializer>((ITypeSerializer s) => {
                if (s is IDynamicTypeSerializer)
                {
                    return true;
                }
                return s is IStaticTypeSerializer;
            }))
            {
                throw new ArgumentException("TypeSerializers have to implement IDynamicTypeSerializer or  IStaticTypeSerializer");
            }
            Serializer.s_userTypeSerializers = userTypeSerializers;
            Dictionary<Type, TypeData> types = Serializer.GenerateTypeData(rootTypes);
            Serializer.GenerateDynamic(types);
            Serializer.s_typeIDMap = types.ToDictionary<KeyValuePair<Type, TypeData>, Type, ushort>((KeyValuePair<Type, TypeData> kvp) => kvp.Key, (KeyValuePair<Type, TypeData> kvp) => kvp.Value.TypeID);
            Serializer.s_initialized = true;
        }

        public static void Serialize(Stream stream, object data)
        {
            if (!Serializer.s_initialized)
            {
                throw new InvalidOperationException("NetSerializer not initialized");
            }
            Serializer.s_serializerSwitch(stream, data);
        }

        internal static void SerializeInternal(Stream stream, object data)
        {
            Serializer.s_serializerSwitch(stream, data);
        }

        private delegate void DeserializerSwitch(Stream stream, out object ob);

        private delegate void SerializerSwitch(Stream stream, object ob);
    }
}