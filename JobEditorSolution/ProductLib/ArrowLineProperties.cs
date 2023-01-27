
using System.Windows.Media;

namespace ProductLib
{
  public class ArrowLineProperties
  {
    public Color Line1Color { get; set; }

    public Color Line2Color { get; set; }

    public Color Line3Color { get; set; }

    public Color Line4Color { get; set; }

    public bool AliasedON { get; set; }

    public double StrokeThickness { get; set; }

    public bool IsEnabled { get; set; }

    public ArrowLineProperties()
    {
      this.Line1Color = Color.FromRgb((byte) 253, (byte) 200, (byte) 35);
      this.Line2Color = Color.FromRgb((byte) 148, (byte) 186, (byte) 80);
      this.Line3Color = Color.FromRgb((byte) 83, (byte) 134, (byte) 173);
      this.Line4Color = Color.FromRgb(byte.MaxValue, (byte) 105, (byte) 180);
      this.AliasedON = true;
      this.StrokeThickness = 5.0;
      this.IsEnabled = false;
    }
  }
}
