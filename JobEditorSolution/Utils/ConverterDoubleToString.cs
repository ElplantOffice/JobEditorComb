
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Utils
{
  public class ConverterDoubleToString : IValueConverter
  {
    public static bool UnitInches { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = "";
      if (value is double)
      {
        if (parameter is string[])
        {
          string[] strArray = (string[]) parameter;
          if (((IEnumerable<string>) strArray).Count<string>() > 0)
            str = !ConverterDoubleToString.UnitInches || ((IEnumerable<string>) strArray).Count<string>() < 2 ? this.ConvertToStringValue((double) value, strArray[0], culture) : this.ConvertToStringValue((double) value, strArray[1], culture);
        }
        if (parameter is string)
        {
          string format = (string) parameter;
          str = this.ConvertToStringValue((double) value, format, culture);
        }
      }
      return (object) str;
    }

    private string ConvertToStringValue(double doublevalue, string format, CultureInfo culture)
    {
      string str;
      if (format.Contains("{0:"))
        str = string.Format((IFormatProvider) culture, format, new object[1]
        {
          (object) doublevalue
        });
      else
        str = doublevalue.ToString(format, (IFormatProvider) culture);
      return str;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      double result = 0.0;
      if (value is string && !double.TryParse(Regex.Replace((string) value, "[^-0-9.,]", ""), NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) culture, out result))
        result = 0.0;
      return (object) result;
    }
  }
}
