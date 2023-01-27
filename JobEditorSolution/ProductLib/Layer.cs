
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductLib
{
  public class Layer : ICloneable
  {
    [XmlArray(ElementName = "Shapes")]
    [XmlArrayItem(ElementName = "Shape")]
    public List<Shape> Shapes = new List<Shape>();

    [XmlElement(ElementName = "LayerDefault")]
    public LayerDefault LayerDefault { get; set; }

    [XmlElement(ElementName = "StepLapsDefault")]
    public StepLapsDefault StepLapsDefault { get; set; }

    [XmlElement(ElementName = "TipCutsDefault")]
    public TipCutsDefault TipCutsDefault { get; set; }

    [XmlElement(ElementName = "HolesDefault")]
    public HolesDefault HolesDefault { get; set; }

    [XmlElement(ElementName = "SlotsDefault")]
    public SlotsDefault SlotsDefault { get; set; }

    [XmlElement(ElementName = "CentersDefault")]
    public CentersDefault CentersDefault { get; set; }

    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("Info")]
    public string Info { get; set; }

    public object Clone()
    {
      Layer layer = new Layer();
      List<Shape> shapeList = new List<Shape>();
      foreach (Shape shape in this.Shapes)
        shapeList.Add((Shape) shape.Clone());
      layer.Shapes = shapeList;
      layer.LayerDefault = (LayerDefault) this.LayerDefault.Clone();
      layer.StepLapsDefault = (StepLapsDefault) this.StepLapsDefault.Clone();
      layer.TipCutsDefault = (TipCutsDefault) this.TipCutsDefault.Clone();
      layer.HolesDefault = (HolesDefault) this.HolesDefault.Clone();
      layer.SlotsDefault = (SlotsDefault) this.SlotsDefault.Clone();
      layer.CentersDefault = (CentersDefault) this.CentersDefault.Clone();
      layer.Id = this.Id;
      layer.Name = this.Name;
      layer.Info = this.Info;
      return (object) layer;
    }
  }
}
