
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace NetSerializer
{
  public class GenericSerializer : IDynamicTypeSerializer, ITypeSerializer
  {
    public bool Handles(Type type)
    {
      if (!type.IsSerializable)
        throw new NotSupportedException(string.Format("Type {0} is not marked as Serializable", (object) type.FullName));
      if (typeof (ISerializable).IsAssignableFrom(type))
        throw new NotSupportedException(string.Format("Cannot serialize {0}: ISerializable not supported", (object) type.FullName));
      return true;
    }

    public IEnumerable<Type> GetSubtypes(Type type)
    {
      IEnumerable<FieldInfo> fields = Helpers.GetFieldInfos(type);
      foreach (FieldInfo fieldInfo in fields)
        yield return fieldInfo.FieldType;
    }

    public void GenerateWriterMethod(Type type, CodeGenContext ctx, ILGenerator il)
    {
      foreach (FieldInfo fieldInfo in Helpers.GetFieldInfos(type))
      {
        il.Emit(OpCodes.Ldarg_0);
        if (type.IsValueType)
          il.Emit(OpCodes.Ldarga_S, 1);
        else
          il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldfld, fieldInfo);
        Helpers.GenSerializerCall(ctx, il, fieldInfo.FieldType);
      }
      il.Emit(OpCodes.Ret);
    }

    public void GenerateReaderMethod(Type type, CodeGenContext ctx, ILGenerator il)
    {
      if (type.IsClass)
      {
        il.Emit(OpCodes.Ldarg_1);
        MethodInfo method1 = typeof (Type).GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public);
        MethodInfo method2 = typeof (FormatterServices).GetMethod("GetUninitializedObject", BindingFlags.Static | BindingFlags.Public);
        il.Emit(OpCodes.Ldtoken, type);
        il.Emit(OpCodes.Call, method1);
        il.Emit(OpCodes.Call, method2);
        il.Emit(OpCodes.Castclass, type);
        il.Emit(OpCodes.Stind_Ref);
      }
      foreach (FieldInfo fieldInfo in Helpers.GetFieldInfos(type))
      {
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        if (type.IsClass)
          il.Emit(OpCodes.Ldind_Ref);
        il.Emit(OpCodes.Ldflda, fieldInfo);
        Helpers.GenDeserializerCall(ctx, il, fieldInfo.FieldType);
      }
      if (typeof (IDeserializationCallback).IsAssignableFrom(type))
      {
        MethodInfo method = typeof (IDeserializationCallback).GetMethod("OnDeserialization", BindingFlags.Instance | BindingFlags.Public, (Binder) null, new Type[1]
        {
          typeof (object)
        }, (ParameterModifier[]) null);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldnull);
        il.Emit(OpCodes.Constrained, type);
        il.Emit(OpCodes.Callvirt, method);
      }
      il.Emit(OpCodes.Ret);
    }
  }
}
