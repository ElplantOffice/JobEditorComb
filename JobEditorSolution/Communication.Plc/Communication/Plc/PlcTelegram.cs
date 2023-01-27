
using Communication.Plc.Shared;
using Messages;
using System;

namespace Communication.Plc
{
  [Serializable]
  public class PlcTelegram
  {
    public PlcAddress Address { get; private set; }

    public PlcPntr ValuePntr { get; private set; }

    public object Value { get; private set; }

    public TelegramCommand Command { get; private set; }

    public PlcTelegramRaw PlcMessageRaw { get; private set; }

    public PlcTelegram(PlcAddress address, object value, TelegramCommand command)
    {
      this.Address = address;
      this.Value = value;
      this.Command = command;
    }

    public static IEventMessage ToTelegram()
    {
      return (IEventMessage) null;
    }
  }
}
