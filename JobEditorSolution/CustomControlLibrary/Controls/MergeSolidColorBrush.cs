
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class MergeSolidColorBrush : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Color color1 = (value as SolidColorBrush).Color;
      Color color2 = (parameter as SolidColorBrush).Color;
      return (object) new SolidColorBrush(Color.FromArgb((byte) ((uint) color1.A & (uint) color2.A), (byte) ((uint) color1.R & (uint) color2.R), (byte) ((uint) color1.G & (uint) color2.G), (byte) ((uint) color1.B & (uint) color2.B)));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Binding.DoNothing;
    }
  }
}
