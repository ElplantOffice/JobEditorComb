
using System.Windows;
using System.Windows.Media;

namespace ProductLib
{
  public class PolygonProperties
  {
    private double strokeThickness;
    private double strokeOpacity;
    private double fillOpacity;
    private Color fillColor;
    private Color strokeColor;
    private double distanceBetweenParts;
    private double scallingFactor;
    private Thickness margin;
    private Shape shape;

    public PolygonProperties()
    {
      this.strokeThickness = 2.0;
      this.strokeOpacity = 1.0;
      this.fillOpacity = 0.600000023841858;
      this.fillColor = Colors.White;
      this.strokeColor = Colors.Black;
      this.distanceBetweenParts = 2.0;
      this.scallingFactor = 14.0;
      this.margin = new Thickness();
    }

    public PolygonProperties(Shape shape)
    {
      this.strokeThickness = 2.0;
      this.strokeOpacity = 1.0;
      this.fillOpacity = 0.600000023841858;
      this.fillColor = Colors.White;
      this.strokeColor = Colors.Black;
      this.distanceBetweenParts = 2.0;
      this.scallingFactor = 14.0;
      this.margin = new Thickness();
      this.shape = shape;
    }

    public PolygonProperties(double strokeTicknees, double strokeOpacity, double fillOpacity, Color fillColor, Color strokeColor, float distanceBetweenParts, float scallingFactor, Thickness margin)
    {
      this.strokeThickness = strokeTicknees;
      this.strokeOpacity = strokeOpacity;
      this.fillOpacity = fillOpacity;
      this.fillColor = fillColor;
      this.strokeColor = strokeColor;
      this.distanceBetweenParts = (double) distanceBetweenParts;
      this.scallingFactor = (double) scallingFactor;
      this.Margin = margin;
    }

    public double Length
    {
      get
      {
        int count = this.shape.ShapeParts.Count;
        if (count < 4)
          return (12.0 + 2.0 * this.distanceBetweenParts) * this.scallingFactor;
        return ((double) (count * 4) + (double) (count - 1) * this.distanceBetweenParts) * this.scallingFactor;
      }
    }

    public double StrokeTicknees
    {
      get
      {
        return this.strokeThickness;
      }
      set
      {
        this.strokeThickness = value;
      }
    }

    public double StrokeOpacity
    {
      get
      {
        return this.strokeOpacity;
      }
      set
      {
        this.strokeOpacity = value;
      }
    }

    public double FillOpacity
    {
      get
      {
        return this.fillOpacity;
      }
      set
      {
        this.fillOpacity = value;
      }
    }

    public Color FillColor
    {
      get
      {
        return this.fillColor;
      }
      set
      {
        this.fillColor = value;
      }
    }

    public Color StrokeColor
    {
      get
      {
        return this.strokeColor;
      }
      set
      {
        this.strokeColor = value;
      }
    }

    public double DistanceBetweenParts
    {
      get
      {
        return this.distanceBetweenParts;
      }
      set
      {
        this.distanceBetweenParts = value;
      }
    }

    public double ScallingFactor
    {
      get
      {
        return this.scallingFactor;
      }
      set
      {
        this.scallingFactor = value;
      }
    }

    public Thickness Margin
    {
      get
      {
        return this.margin;
      }
      set
      {
        this.margin = value;
      }
    }
  }
}
