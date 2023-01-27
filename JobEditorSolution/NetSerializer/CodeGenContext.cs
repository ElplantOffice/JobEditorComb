
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetSerializer
{
  public sealed class CodeGenContext
  {
    private readonly Dictionary<Type, TypeData> m_typeMap;

    public CodeGenContext(Dictionary<Type, TypeData> typeMap, MethodInfo serializerSwitch, MethodInfo deserializerSwitch)
    {
      this.m_typeMap = typeMap;
      this.SerializerSwitchMethodInfo = serializerSwitch;
      this.DeserializerSwitchMethodInfo = deserializerSwitch;
    }

    public MethodInfo SerializerSwitchMethodInfo { get; private set; }

    public MethodInfo DeserializerSwitchMethodInfo { get; private set; }

    public MethodInfo GetWriterMethodInfo(Type type)
    {
      return this.m_typeMap[type].WriterMethodInfo;
    }

    public MethodInfo GetReaderMethodInfo(Type type)
    {
      return this.m_typeMap[type].ReaderMethodInfo;
    }

    public bool IsGenerated(Type type)
    {
      return this.m_typeMap[type].IsGenerated;
    }
  }
}
