
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class LayerDefault : ICloneable
  {
    [XmlAttribute("Height")]
    public double Height { get; set; }

    [XmlAttribute("Width")]
    public double Width { get; set; }

    [XmlAttribute("MaterialThickness")]
    public double MaterialThickness { get; set; }

    [XmlAttribute("NumberOfSteps")]
    public int NumberOfSteps { get; set; }

    [XmlAttribute("NumberOfSame")]
    public int NumberOfSame { get; set; }

    [XmlAttribute("StepLapValue")]
    public double StepLapValue { get; set; }

    [XmlAttribute("YOffset")]
    public double YOffset { get; set; }

    [XmlAttribute("VOffset")]
    public double VOffset { get; set; }

    [XmlAttribute("DistanceY")]
    public double DistanceY { get; set; }

    [XmlAttribute("HeightCorrType")]
    public EHeightCorrectionType HeightCorrType { get; set; }

    public LayerDefault()
    {
    }

    public LayerDefault(StepLapsDefault stepLapDefaults, HolesDefault holesDefault, CentersDefault centerDefaults, SlotsDefault slotsDefaults, double height, double width, double materialThickness)
    {
      this.Height = height;
      this.Width = width;
      this.MaterialThickness = materialThickness;
      this.HeightCorrType = EHeightCorrectionType.None;
      this.NumberOfSteps = stepLapDefaults.NumberOfSteps;
      this.NumberOfSame = stepLapDefaults.NumberOfSame;
      this.StepLapValue = stepLapDefaults.Value;
      this.YOffset = holesDefault.Offset;
      this.VOffset = centerDefaults.VOffset;
      this.DistanceY = slotsDefaults.DistanceY;
    }

    public object Clone()
    {
      return (object) new LayerDefault()
      {
        Height = this.Height,
        Width = this.Width,
        MaterialThickness = this.MaterialThickness,
        NumberOfSteps = this.NumberOfSteps,
        NumberOfSame = this.NumberOfSame,
        StepLapValue = this.StepLapValue,
        YOffset = this.YOffset,
        VOffset = this.VOffset,
        DistanceY = this.DistanceY,
        HeightCorrType = this.HeightCorrType
      };
    }
  }
}
