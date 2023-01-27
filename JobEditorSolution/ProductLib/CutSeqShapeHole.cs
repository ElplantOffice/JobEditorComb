
namespace ProductLib
{
  public class CutSeqShapeHole
  {
    private int numberOfHoles;
    private EMeasuringType measuringType;

    public HoleProperties Properties { get; private set; }

    public CutSeqShapeHole(HoleProperties properties)
    {
      this.Properties = properties;
    }

    public int NumberOfHoles
    {
      get
      {
        return this.numberOfHoles;
      }
      set
      {
        this.numberOfHoles = value;
      }
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
