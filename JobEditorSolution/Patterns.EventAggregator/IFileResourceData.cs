
namespace Patterns.EventAggregator
{
  public interface IFileResourceData
  {
    T Get<T>(string name);

    void Set<T>(string name, T value);

    event ResourceEventHandler SetNotify;
  }
}
