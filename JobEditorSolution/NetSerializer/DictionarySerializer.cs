
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NetSerializer
{
  public class DictionarySerializer : IStaticTypeSerializer, ITypeSerializer
  {
    public bool Handles(Type type)
    {
      if (!type.IsGenericType)
        return false;
      return type.GetGenericTypeDefinition() == typeof (Dictionary<,>);
    }

    public IEnumerable<Type> GetSubtypes(Type type)
    {
      Type[] genArgs = type.GetGenericArguments();
      Type serializedType = typeof (KeyValuePair<,>).MakeGenericType(genArgs).MakeArrayType();
      yield return serializedType;
    }

    public void GetStaticMethods(Type type, out MethodInfo writer, out MethodInfo reader)
    {
      if (!type.IsGenericType)
        throw new Exception();
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      Type type1 = this.GetType();
      writer = DictionarySerializer.GetGenWriter(type1, genericTypeDefinition);
      reader = DictionarySerializer.GetGenReader(type1, genericTypeDefinition);
      Type[] genericArguments = type.GetGenericArguments();
      writer = writer.MakeGenericMethod(genericArguments);
      reader = reader.MakeGenericMethod(genericArguments);
    }

    private static MethodInfo GetGenWriter(Type containerType, Type genType)
    {
      foreach (MethodInfo methodInfo in ((IEnumerable<MethodInfo>) containerType.GetMethods(BindingFlags.Static | BindingFlags.Public)).Where<MethodInfo>((Func<MethodInfo, bool>) (mi =>
      {
        if (mi.IsGenericMethod)
          return mi.Name == "WritePrimitive";
        return false;
      })))
      {
        ParameterInfo[] parameters = methodInfo.GetParameters();
        if (parameters.Length == 2 && !(parameters[0].ParameterType != typeof (Stream)))
        {
          Type parameterType = parameters[1].ParameterType;
          if (parameterType.IsGenericType)
          {
            Type genericTypeDefinition = parameterType.GetGenericTypeDefinition();
            if (genType == genericTypeDefinition)
              return methodInfo;
          }
        }
      }
      return (MethodInfo) null;
    }

    private static MethodInfo GetGenReader(Type containerType, Type genType)
    {
      foreach (MethodInfo methodInfo in ((IEnumerable<MethodInfo>) containerType.GetMethods(BindingFlags.Static | BindingFlags.Public)).Where<MethodInfo>((Func<MethodInfo, bool>) (mi =>
      {
        if (mi.IsGenericMethod)
          return mi.Name == "ReadPrimitive";
        return false;
      })))
      {
        ParameterInfo[] parameters = methodInfo.GetParameters();
        if (parameters.Length == 2 && !(parameters[0].ParameterType != typeof (Stream)))
        {
          Type parameterType = parameters[1].ParameterType;
          if (parameterType.IsByRef)
          {
            Type elementType = parameterType.GetElementType();
            if (elementType.IsGenericType)
            {
              Type genericTypeDefinition = elementType.GetGenericTypeDefinition();
              if (genType == genericTypeDefinition)
                return methodInfo;
            }
          }
        }
      }
      return (MethodInfo) null;
    }

    public static void WritePrimitive<TKey, TValue>(Stream stream, Dictionary<TKey, TValue> value)
    {
      KeyValuePair<TKey, TValue>[] keyValuePairArray = new KeyValuePair<TKey, TValue>[value.Count];
      int num = 0;
      foreach (KeyValuePair<TKey, TValue> keyValuePair in value)
        keyValuePairArray[num++] = keyValuePair;
      Serializer.SerializeInternal(stream, (object) keyValuePairArray);
    }

    public static void ReadPrimitive<TKey, TValue>(Stream stream, out Dictionary<TKey, TValue> value)
    {
      KeyValuePair<TKey, TValue>[] keyValuePairArray = (KeyValuePair<TKey, TValue>[]) Serializer.DeserializeInternal(stream);
      value = new Dictionary<TKey, TValue>(keyValuePairArray.Length);
      foreach (KeyValuePair<TKey, TValue> keyValuePair in keyValuePairArray)
        value.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }
}
