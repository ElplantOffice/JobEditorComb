
using System.Windows.Media;

namespace ProductLib
{
  public class HoleProperties
  {
    private int diameter;
    private double strokeThickness;
    private Color fillColor;
    private Color strokeColor;
    private bool aliasedON;

    public LabelProperties LabelProperties { get; set; }

    public ArrowLineProperties ArrowLineProperties { get; set; }

    public AnnotationProperties AnnotationProperties { get; set; }

    public HoleProperties()
    {
      this.diameter = 8;
      this.strokeThickness = 0.0;
      this.fillColor = Color.FromRgb((byte) 183, (byte) 55, (byte) 20);
      this.strokeColor = Colors.Black;
      this.aliasedON = false;
      this.ArrowLineProperties = new ArrowLineProperties();
      this.AnnotationProperties = new AnnotationProperties();
      this.LabelProperties = new LabelProperties();
    }

    public bool AliasedON
    {
      get
      {
        return this.aliasedON;
      }
      set
      {
        this.aliasedON = value;
      }
    }

    public int Diameter
    {
      get
      {
        return this.diameter;
      }
      set
      {
        this.diameter = value;
      }
    }

    public double StrokeThickness
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
  }
}
