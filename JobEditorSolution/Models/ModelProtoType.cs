
using Messages;
using System;
using System.Reflection;

namespace Models
{
  [Serializable]
  public class ModelProtoType
  {
    public ModelProtoType(IAddress address, Type type)
    {
      this.Address = address;
      this.Type = type;
      this.Assembly = this.Type.Assembly;
    }

    public IAddress Address { get; set; }

    public Assembly Assembly { get; private set; }

    public Type Type { get; private set; }

    public IAddress AddressModelAdministrator { get; set; }
  }
}
