
using System.Windows.Media;

namespace ProductLib
{
  public class LabelProperties
  {
    public Color FontColor { get; set; }

    public double FontSize { get; set; }

    public bool IsEnabled { get; set; }

    public LabelProperties()
    {
      this.FontColor = Colors.Black;
      this.FontSize = 10.0;
      this.IsEnabled = false;
    }
  }
}
