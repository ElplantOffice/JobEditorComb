
using System;
using System.Collections.Generic;

namespace Patterns.EventAggregator
{
  public class FileResourceData
  {
    private Dictionary<Type, Dictionary<string, object>> container = new Dictionary<Type, Dictionary<string, object>>();

    public void Set<T>(string name, T value)
    {
      Dictionary<string, object> dictionary;
      if (!this.container.TryGetValue(typeof (T), out dictionary))
      {
        dictionary = new Dictionary<string, object>();
        this.container.Add(typeof (T), dictionary);
      }
      object obj1;
      if (!dictionary.TryGetValue(name.ToLower(), out obj1))
      {
        dictionary.Add(name.ToLower(), (object) value);
        this.OnSetNotify(new ResourceEventArgs(name.ToLower(), (object) value));
      }
      else
      {
        if (obj1.Equals((object) value))
          return;
        object obj2 = (object) value;
        this.OnSetNotify(new ResourceEventArgs(name.ToLower(), (object) value));
      }
    }

    public T Get<T>(string name)
    {
      Dictionary<string, object> dictionary;
      if (!this.container.TryGetValue(typeof (T), out dictionary))
        return default (T);
      object obj;
      if (dictionary.TryGetValue(name.ToLower(), out obj))
        return (T) obj;
      return default (T);
    }

    public event EventHandler SetNotify;

    protected virtual void OnSetNotify(ResourceEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.SetNotify == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.SetNotify((object) this, (EventArgs) e);
    }
  }
}
