
namespace ProductLib
{
  public class ShapeProperties
  {
    public PolygonProperties PolygonProperties { get; set; }

    public HoleProperties HoleProperties { get; set; }

    public SlotProperties SlotProperties { get; set; }

    public CenterProperties CenterProperties { get; set; }

    public StepLapProperties StepLapProperties { get; set; }

    public TipCutProperties TipCutProperties { get; set; }

    public ShapeProperties()
    {
      this.PolygonProperties = new PolygonProperties();
      this.HoleProperties = new HoleProperties();
      this.SlotProperties = new SlotProperties();
      this.CenterProperties = new CenterProperties();
      this.StepLapProperties = new StepLapProperties(this.PolygonProperties);
      this.TipCutProperties = new TipCutProperties(this.PolygonProperties);
    }

    public ShapeProperties(Shape shape)
    {
      this.PolygonProperties = new PolygonProperties(shape);
      this.HoleProperties = new HoleProperties();
      this.SlotProperties = new SlotProperties();
      this.CenterProperties = new CenterProperties();
      this.StepLapProperties = new StepLapProperties(this.PolygonProperties);
      this.TipCutProperties = new TipCutProperties(this.PolygonProperties);
    }
  }
}
