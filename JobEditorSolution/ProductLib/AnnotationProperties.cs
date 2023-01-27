
using System.Windows.Media;

namespace ProductLib
{
  public class AnnotationProperties
  {
    private Color lineColor;
    private double strokeThickness;
    private bool aliasedON;

    public bool IsEnabled { get; set; }

    public AnnotationProperties()
    {
      this.lineColor = Colors.Black;
      this.strokeThickness = 2.0;
      this.aliasedON = true;
      this.IsEnabled = false;
    }

    public Color LineColor
    {
      get
      {
        return this.lineColor;
      }
      set
      {
        this.lineColor = value;
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
  }
}
