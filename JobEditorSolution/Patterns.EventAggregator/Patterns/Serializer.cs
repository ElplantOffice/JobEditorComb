
using NetSerializer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Patterns.EventAggregator
{
  public class Serializer
  {
    private static bool initDone;

    public Serializer(Type[] types)
    {
      if (!Serializer.initDone)
        Task.WaitAll(Task.Factory.StartNew((Action) (() => NetSerializer.Serializer.Initialize(types))));
      Serializer.initDone = true;
    }

    public static void Serialize(Stream stream, object data)
    {
      Serializer.Serialize(stream, data);
    }

    public static object Deserialize(Stream stream)
    {
      return Serializer.Deserialize(stream);
    }
  }
}
