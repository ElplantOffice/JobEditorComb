
namespace ProductLib
{
  public class CutSeqShapeSlot
  {
    private int numberOfSlotes;
    private EMeasuringType measuringType;

    public SlotProperties Properties { get; private set; }

    public CutSeqShapeSlot(SlotProperties properties)
    {
      this.Properties = properties;
    }

    public int NumberOfSlotes
    {
      get
      {
        return this.numberOfSlotes;
      }
      set
      {
        this.numberOfSlotes = value;
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
