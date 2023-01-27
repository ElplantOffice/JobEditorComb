
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class SlotsInfo : ICloneable
  {
    [XmlAttribute("NumberOfSlots")]
    public int NumberOfSlots { get; set; }

    [XmlAttribute("MeasuringType")]
    public EMeasuringType MeasuringType { get; set; }

    [XmlAttribute("Length1")]
    public double Length1 { get; set; }

    [XmlAttribute("Length2")]
    public double Length2 { get; set; }

    [XmlAttribute("Length3")]
    public double Length3 { get; set; }

    [XmlAttribute("Length4")]
    public double Length4 { get; set; }

    public object Clone()
    {
      return (object) new SlotsInfo()
      {
        NumberOfSlots = this.NumberOfSlots,
        MeasuringType = this.MeasuringType,
        Length1 = this.Length1,
        Length2 = this.Length2,
        Length3 = this.Length3,
        Length4 = this.Length4
      };
    }
  }
}
