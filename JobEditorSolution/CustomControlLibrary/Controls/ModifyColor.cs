
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class ModifyColor : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Color color1 = new Color();
      Color color2 = (Color) value;
      double num = (double) parameter;
      return (object) new Color()
      {
        A = color2.A,
        R = (byte) ((double) color2.R * num),
        G = (byte) ((double) color2.G * num),
        B = (byte) ((double) color2.B * num)
      };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Binding.DoNothing;
    }
  }
}
