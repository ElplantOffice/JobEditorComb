
namespace Models
{
  public interface ILocalisation
  {
    void Clear();

    bool AddItem(ILocalisationItem item);

    bool AddItem(int localisationId, string textId, string text);

    bool Translate(int localisationId, string textId, out string text, string defaultText);

    bool Translate(int localisationId, ref string text, string defaultText = null);

    bool Translate(ILocalisationItem item, string defaultText = null);

    bool Load(bool waituntildone = true);

    bool Save(bool waituntildone = true);
  }
}
