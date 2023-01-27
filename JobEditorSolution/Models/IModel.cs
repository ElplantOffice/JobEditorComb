
namespace Models
{
  public interface IModel
  {
    void AddAttributedEventType(IModelAttributedEventType modelAttributedEventType);

    void Dispose();
  }
}
