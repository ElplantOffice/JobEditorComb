
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ProductLib
{
  public class ProductDefinition
  {
    public Product ProductData { get; set; }

    public ProductDefinition(string filePath)
    {
      this.ProductData = new Product();
      if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        return;
      this.Load(filePath);
    }

    public void Load(string filePath)
    {
      using (StreamReader streamReader = new StreamReader(filePath))
      {
        try
        {
          this.ProductData = (Product) new XmlSerializer(typeof (Product)).Deserialize((TextReader) streamReader);
        }
        catch (Exception ex)
        {
          IDictionary data = ex.Data;
        }
      }
    }

    public void TrySplit()
    {
      if (this.ProductData.Layers.Count == 0)
        return;
      if (string.Compare(this.ProductData.Layers[0].Name, "2004-004-2004-004-006") != 0)
      {
        foreach (Layer layer in this.ProductData.Layers)
          layer.Name = "." + layer.Name;
      }
      else
      {
        Product product = (Product) this.ProductData.Clone();
        List<Layer> layers = product.Layers;
        layers.Clear();
        foreach (Layer layer1 in this.ProductData.Layers)
        {
          Layer layer2 = (Layer) layer1.Clone();
          layer2.Shapes.RemoveRange(2, 3);
          layer2.Name = ".2004-004";
          layers.Add(layer2);
          Layer layer3 = (Layer) layer1.Clone();
          layer3.Shapes.RemoveRange(0, 2);
          layer3.Shapes.RemoveRange(2, 1);
          layer3.Name = ".2004-004";
          layers.Add(layer3);
          Layer layer4 = (Layer) layer1.Clone();
          layer4.Shapes.RemoveRange(0, 4);
          layer4.Name = ".006";
          layers.Add(layer4);
        }
        this.ProductData = product;
      }
    }
  }
}
