
namespace Messages
{
  public interface IAddress
  {
    string Owner { get; }

    string Target { get; }

    string Relay { get; }
  }
}
