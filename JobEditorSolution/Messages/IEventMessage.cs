
namespace Messages
{
  public interface IEventMessage
  {
    IAddress Address { get; }

    int Command { get; }

    object Value { get; }
  }
}
