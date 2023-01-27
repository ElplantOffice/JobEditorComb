
using Communication.Plc;
using Communication.Plc.Shared;
using Messages;
using System;
using System.Collections.Generic;

namespace Models
{
  [Serializable]
  public class UIProtoType
  {
    public UIProtoType(List<ViewModelProtoType> viewModels, ModelProtoType model)
    {
      this.ViewModels = viewModels;
      this.Model = model;
    }

    public UIProtoType(PlcAddress address, PlcUiPrototypeRaw rawPrototype)
    {
      this.ViewModels = new List<ViewModelProtoType>();
      char[] chArray = new char[2]{ ';', ':' };
      foreach (string typeName in ((string) rawPrototype.ViewModels).Split(chArray))
        this.ViewModels.Add(new ViewModelProtoType((IAddress) new Address(address.Target, address.Relay, "", (string) null), Type.GetType(typeName), (string) rawPrototype.Xaml, (string) rawPrototype.Location));
      this.Model = new ModelProtoType((IAddress) new Address(address.Relay, address.Target, address.Owner, (string) null), Type.GetType((string) rawPrototype.Model));
    }

    public IEventMessage GetMessage(IAddress address, TelegramCommand command)
    {
      return (IEventMessage) new Telegram(address, (int) command, (object) this, (string) null);
    }

    public List<ViewModelProtoType> ViewModels { get; private set; }

    public ModelProtoType Model { get; private set; }
  }
}
