
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class Thumb : System.Windows.Controls.Primitives.Thumb
  {
    public static readonly DependencyProperty ScrollBarThumbBrushProperty = DependencyProperty.Register(nameof (ScrollBarThumbBrush), typeof (SolidColorBrush), typeof (Thumb), new PropertyMetadata((object) Brushes.Black));

    static Thumb()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Thumb), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Thumb)));
    }

    public SolidColorBrush ScrollBarThumbBrush
    {
      get
      {
        return (SolidColorBrush) this.GetValue(Thumb.ScrollBarThumbBrushProperty);
      }
      set
      {
        this.SetValue(Thumb.ScrollBarThumbBrushProperty, (object) value);
      }
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      SolidColorBrush solidColorBrush1 = new SolidColorBrush();
      SolidColorBrush solidColorBrush2 = this.ScrollBarThumbBrush;
      if (solidColorBrush2 == null || this.Background == null)
        return;
      Color color1 = solidColorBrush2.Color;
      if (color1.A == (byte) 0)
      {
        color1 = (this.Background as SolidColorBrush).Color;
        if (color1.A == byte.MaxValue)
        {
          solidColorBrush2 = new SolidColorBrush(Color.FromArgb(color1.A, (byte) ((double) color1.R / 1.2), (byte) ((double) color1.G / 1.2), (byte) ((double) color1.B / 1.2)));
        }
        else
        {
          Color color2 = (Application.Current.Resources[(object) "BaseBackgroundThemeBrush"] as SolidColorBrush).Color;
          solidColorBrush2 = new SolidColorBrush(Color.FromArgb(color2.A, (byte) ((double) color2.R / 1.2), (byte) ((double) color2.G / 1.2), (byte) ((double) color2.B / 1.2)));
        }
      }
      (this.GetTemplateChild("PART_ThumbBorder") as Border).Background = (Brush) solidColorBrush2;
    }
  }
}
