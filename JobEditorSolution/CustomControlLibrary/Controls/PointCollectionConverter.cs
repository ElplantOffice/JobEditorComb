
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class PointCollectionConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value.GetType() == typeof (double)) || !(targetType == typeof (PointCollection)))
        return (object) null;
      double num = (double) ((int) value / 2);
      return (object) new PointCollection()
      {
        new Point(0.0, 0.0),
        new Point(0.0, num),
        new Point(num, 0.0)
      };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
