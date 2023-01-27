
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class Hole : ICloneable
  {
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("X")]
    public double X { get; set; }

    [XmlAttribute("Y")]
    public double Y { get; set; }

    [XmlAttribute("Shape")]
    public EHoleShape Shape { get; set; }

    [XmlAttribute("Size")]
    public string Size { get; set; }

    public object Clone()
    {
      return (object) new Hole()
      {
        Id = this.Id,
        X = this.X,
        Y = this.Y,
        Shape = this.Shape,
        Size = this.Size
      };
    }
  }
}
