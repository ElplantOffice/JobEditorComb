
namespace Models
{
  public class UiLocalisationSettings
  {
    public UiLocalisationSettings()
    {
      this.LoadFilename = (string) null;
      this.SaveFilename = (string) null;
      this.LocalisationId = 0;
    }

    public string LoadFilename { get; set; }

    public string SaveFilename { get; set; }

    public int LocalisationId { get; set; }

    public int ForceTranslationId
    {
      get
      {
        return 9999;
      }
    }
  }
}
