
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductLib
{
  public class CutSequence
  {
    [XmlArray(ElementName = "Shapes")]
    [XmlArrayItem(ElementName = "Shape")]
    public List<Shape> Shapes = new List<Shape>();

    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }
  }
}
