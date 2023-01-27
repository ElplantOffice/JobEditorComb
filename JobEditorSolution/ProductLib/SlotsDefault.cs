
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class SlotsDefault : ICloneable
  {
    [XmlAttribute("DistanceY")]
    public double DistanceY { get; set; }

    public object Clone()
    {
      return (object) new SlotsDefault()
      {
        DistanceY = this.DistanceY
      };
    }
  }
}
