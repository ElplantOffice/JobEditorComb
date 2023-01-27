
using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControlLibrary
{
  public class PercentageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
            int num = System.Convert.ToInt16(parameter);
            if (num == 0)
            {
                return value;
            }
            double num1 = (double)num / 100;
            return System.Convert.ToDouble(value) * num1;
        }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
