
using Messages;
using System;
using System.Reflection;

namespace Models
{
  [Serializable]
  public class ViewModelProtoType
  {
    public ViewModelProtoType(IAddress address, Type type, string xaml, string location = null)
    {
      this.Address = address;
      this.Type = type;
      this.Assembly = this.Type.Assembly;
      this.Xaml = xaml;
      this.Location = location;
    }

    public IAddress Address { get; set; }

    public string Xaml { get; private set; }

    public Assembly Assembly { get; private set; }

    public Type Type { get; private set; }

    public IAddress AddressViewModelAdministrator { get; set; }

    public string Location { get; private set; }

    public IEventMessage GetMessage(IAddress address, TelegramCommand command)
    {
      return (IEventMessage) new Telegram(address, (int) command, (object) this, (string) null);
    }
  }
}
