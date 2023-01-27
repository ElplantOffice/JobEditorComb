
using System;
using System.Reflection;

namespace NetSerializer
{
  public interface IStaticTypeSerializer : ITypeSerializer
  {
    void GetStaticMethods(Type type, out MethodInfo writer, out MethodInfo reader);
  }
}
