
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductLib
{
  public class Product : ICloneable
  {
    [XmlArray(ElementName = "Layers")]
    [XmlArrayItem(ElementName = "Layer")]
    public List<Layer> Layers = new List<Layer>();

    [XmlAttribute("HeightRefType")]
    public EHeightRefType HeightRefType { get; set; }

    [XmlAttribute("HeightMeasType")]
    public EHeightMeasType HeightMeasType { get; set; }

    [XmlAttribute("NumberOfLayers")]
    public int NumberOfLayers { get; set; }

    [XmlAttribute("UnitsInInches")]
    public bool UnitsInInches { get; set; }

    public object Clone()
    {
      Product product = (Product) this.MemberwiseClone();
      product.Layers = new List<Layer>();
      foreach (Layer layer in this.Layers)
        product.Layers.Add((Layer) layer.Clone());
      return (object) product;
    }
  }
}
