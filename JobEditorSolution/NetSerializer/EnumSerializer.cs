
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetSerializer
{
  public class EnumSerializer : IStaticTypeSerializer, ITypeSerializer
  {
    public bool Handles(Type type)
    {
      return type.IsEnum;
    }

    public IEnumerable<Type> GetSubtypes(Type type)
    {
      Type underlyingType = Enum.GetUnderlyingType(type);
      yield return underlyingType;
    }

    public void GetStaticMethods(Type type, out MethodInfo writer, out MethodInfo reader)
    {
      Type underlyingType = Enum.GetUnderlyingType(type);
      writer = Primitives.GetWritePrimitive(underlyingType);
      reader = Primitives.GetReaderPrimitive(underlyingType);
    }
  }
}
