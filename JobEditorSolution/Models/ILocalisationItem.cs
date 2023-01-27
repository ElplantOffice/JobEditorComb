
namespace Models
{
  public interface ILocalisationItem
  {
    int LocalisationId { get; set; }

    string TextId { get; set; }

    string Text { get; set; }
  }
}
