
using System;
using System.Globalization;
using System.Windows.Data;

namespace Utils
{
  public class ConverterIntToString : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return (object) "";
      return (object) value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
