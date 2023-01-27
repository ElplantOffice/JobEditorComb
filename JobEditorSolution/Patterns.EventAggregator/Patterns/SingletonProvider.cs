
using System;

namespace Patterns.EventAggregator
{
  public class SingletonProvider<T> where T : new()
  {
    private SingletonProvider()
    {
    }

    public static T Instance
    {
      get
      {
        return SingletonProvider<T>.SingletonCreator.instance;
      }
    }

    private class SingletonCreator
    {
      internal static readonly T instance = Activator.CreateInstance<T>();
    }
  }
}
