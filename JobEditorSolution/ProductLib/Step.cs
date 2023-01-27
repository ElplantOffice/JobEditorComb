
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class Step : ICloneable
  {
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("X")]
    public double X { get; set; }

    [XmlAttribute("Y")]
    public double Y { get; set; }

    [XmlAttribute("ShapeId")]
    public int ShapeId { get; set; }

    public object Clone()
    {
      return (object) new Step()
      {
        Id = this.Id,
        X = this.X,
        Y = this.Y,
        ShapeId = this.ShapeId
      };
    }
  }
}
