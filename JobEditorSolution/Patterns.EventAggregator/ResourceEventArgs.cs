
using System;

namespace Patterns.EventAggregator
{
  public class ResourceEventArgs : EventArgs
  {
    public ResourceEventArgs(string name, object value)
    {
      this.Name = name;
      this.Value = value;
    }

    public string Name { get; private set; }

    public object Value { get; private set; }
  }
}
