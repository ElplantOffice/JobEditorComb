
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class Slot : ICloneable
  {
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("X")]
    public double X { get; set; }

    [XmlAttribute("Y")]
    public double Y { get; set; }

    [XmlAttribute("Size")]
    public string Size { get; set; }

    [XmlAttribute("Length")]
    public double Length { get; set; }

    public object Clone()
    {
      return (object) new Slot()
      {
        X = this.X,
        Y = this.Y,
        Size = this.Size,
        Length = this.Length
      };
    }
  }
}
