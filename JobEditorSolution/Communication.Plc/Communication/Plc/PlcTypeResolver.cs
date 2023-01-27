
using Patterns.EventAggregator;
using System;

namespace Communication.Plc
{
  public static class PlcTypeResolver
  {
    public static void Register<T>() where T : IMappable
    {
      PlcTypeContainer instance = SingletonProvider<PlcTypeContainer>.Instance;
      T obj = default (T);
      if (instance.Types.ContainsKey(obj.TypeName))
        return;
      instance.Types.Add(obj.TypeName, obj.Mapper);
    }

    public static object Map(byte[] datablock, string typeName)
    {
      PlcTypeContainer instance = SingletonProvider<PlcTypeContainer>.Instance;
      Func<byte[], object> func = (Func<byte[], object>) null;
      if (instance.Types.TryGetValue(typeName, out func))
        return func(datablock);
      return (object) null;
    }
  }
}
