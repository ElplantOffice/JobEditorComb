
using System.Windows.Media;

namespace ProductLib
{
  public class SlotProperties
  {
    private double strokeThickness;
    private Color fillColor;
    private Color strokeColor;
    private int width;
    private int height;
    private bool aliasedON;
    private double verticalDistance;

    public LabelProperties LabelProperties { get; set; }

    public ArrowLineProperties ArrowLineProperties { get; set; }

    public AnnotationProperties AnnotationProperties { get; set; }

    public SlotProperties()
    {
      this.width = 40;
      this.height = 6;
      this.strokeThickness = 0.0;
      this.fillColor = Color.FromRgb((byte) 183, (byte) 55, (byte) 20);
      this.strokeColor = Colors.Black;
      this.aliasedON = false;
      this.verticalDistance = 10.0;
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

    public int Width
    {
      get
      {
        return this.width;
      }
      set
      {
        this.width = value;
      }
    }

    public int Height
    {
      get
      {
        return this.height;
      }
      set
      {
        this.height = value;
      }
    }

    public double VerticalDistance
    {
      get
      {
        return this.verticalDistance;
      }
      set
      {
        this.verticalDistance = value;
      }
    }
  }
}
