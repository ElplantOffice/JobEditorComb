
using System.Collections.Generic;
using System.Windows;

namespace ProductLib
{
  public class UIElementCollection
  {
    public List<UIElement> ShapeElements { get; set; }

    public double ShapeLength { get; set; }

    public UIElementCollection(double length)
    {
      this.ShapeLength = length;
      this.ShapeElements = new List<UIElement>();
    }

    public UIElementCollection()
    {
      this.ShapeElements = new List<UIElement>();
    }
  }
}
