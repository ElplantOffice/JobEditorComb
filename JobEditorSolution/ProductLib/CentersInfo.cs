
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class CentersInfo : ICloneable
  {
    public CentersInfo()
    {
      this.MeasuringType = EMeasuringType.Absolute;
      this.Length1 = 0.0;
      this.Length2 = 0.0;
      this.Length3 = 0.0;
      this.Length4 = 0.0;
    }

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
      return (object) new CentersInfo()
      {
        MeasuringType = this.MeasuringType,
        Length1 = this.Length1,
        Length2 = this.Length2,
        Length3 = this.Length3,
        Length4 = this.Length4
      };
    }
  }
}
