
using System.Windows.Media;

namespace ProductLib
{
  public class CenterLineProperties
  {
    private Color lineColor;

    public CenterLineProperties()
    {
      this.lineColor = Colors.Black;
    }

    public CenterLineProperties(Color lineColor)
    {
      this.lineColor = lineColor;
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
  }
}
