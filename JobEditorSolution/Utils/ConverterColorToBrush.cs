
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Utils
{
  public class ConverterColorToBrush : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Color color = Colors.Transparent;
      if (value is Color)
        color = (Color) value;
      return (object) new SolidColorBrush(color);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
