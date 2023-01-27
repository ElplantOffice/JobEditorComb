
using System;

namespace Messages
{
  [Serializable]
  public class Telegram : IMessage, IEventMessage
  {
    public IAddress Address { get; private set; }

    public object Value { get; private set; }

    public string ChannelId { get; private set; }

    public int Command { get; private set; }

    public object ServiceLink { get; set; }

    public Telegram(IAddress address, int command, object value, object serviceLink, string channelid = null)
    {
      this.Address = address;
      this.Value = value;
      this.ChannelId = channelid;
      this.Command = command;
      this.ServiceLink = serviceLink;
    }

    public Telegram(IAddress address, int command, object value, string channelid = null)
    {
      this.Address = address;
      this.Value = value;
      this.ChannelId = channelid;
      this.Command = command;
      this.ServiceLink = (object) null;
    }

    public static IMessage GetMessage(IMessage message_In, string channelId)
    {
      Telegram telegram = message_In as Telegram;
      telegram.ChannelId = channelId;
      return (IMessage) telegram;
    }
  }
}
