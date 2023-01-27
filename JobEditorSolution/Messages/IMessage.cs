
namespace Messages
{
  public interface IMessage
  {
    IAddress Address { get; }

    int Command { get; }

    object Value { get; }

    string ChannelId { get; }
  }
}
