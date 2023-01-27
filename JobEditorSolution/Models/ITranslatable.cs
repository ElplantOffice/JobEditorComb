
namespace Models
{
  public interface ITranslatable
  {
    string TextId { get; set; }

    string ContentText { get; set; }

    int Localisation { get; set; }
  }
}
