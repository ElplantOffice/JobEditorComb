
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class HolesDefault : ICloneable
  {
    [XmlAttribute("Offset")]
    public double Offset { get; set; }

    public HolesDefault()
    {
    }

    public HolesDefault(double offset)
    {
      this.Offset = offset;
    }

    public object Clone()
    {
      return (object) new HolesDefault(this.Offset);
    }
  }
}
