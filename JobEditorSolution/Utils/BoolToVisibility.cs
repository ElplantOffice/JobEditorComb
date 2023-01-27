
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Utils
{
  public class BoolToVisibility : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      bool flag = false;
      if (value is bool)
        flag = (bool) value;
      else if (value is bool?)
        flag = ((bool?) value).GetValueOrDefault();
      if (parameter != null && bool.Parse((string) parameter))
        flag = !flag;
      if (flag)
        return (object) Visibility.Visible;
      return (object) Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      bool flag = value is Visibility && (Visibility) value == Visibility.Visible;
      if (parameter != null && (bool) parameter)
        flag = !flag;
      return (object) flag;
    }
  }
}
