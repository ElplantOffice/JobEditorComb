
namespace ProductLib
{
  public class StepLapProperties
  {
    private PolygonProperties polygonProperties;
    private double step;

    public bool IsEnabled { get; set; }

    public double Step
    {
      get
      {
        return this.step;
      }
      set
      {
        this.step = value / this.polygonProperties.ScallingFactor;
      }
    }

    public StepLapProperties(PolygonProperties polygonProperties)
    {
      this.IsEnabled = false;
      this.polygonProperties = polygonProperties;
    }
  }
}
