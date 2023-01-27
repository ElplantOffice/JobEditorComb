
namespace ProductLib
{
  public class TipCutProperties
  {
    private PolygonProperties polygonProperties;

    public bool IsEnabled { get; set; }

    public TipCutProperties(PolygonProperties polygonProperties)
    {
      this.IsEnabled = false;
      this.polygonProperties = polygonProperties;
    }
  }
}
