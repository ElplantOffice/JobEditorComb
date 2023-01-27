
namespace ProductLib
{
  public class CutSeqShapeCenter
  {
    private EMeasuringType measuringType;

    public CenterProperties Properties { get; private set; }

    public CutSeqShapeCenter(CenterProperties properties)
    {
      this.Properties = properties;
    }

    public EMeasuringType MeasuringType
    {
      get
      {
        return this.measuringType;
      }
      set
      {
        this.measuringType = value;
      }
    }
  }
}
