using System;
using System.Globalization;
using System.Windows.Data;

namespace JobEditor
{
	public class ConverterNumberOfHolesCBIntToString : IValueConverter
	{
		public ConverterNumberOfHolesCBIntToString()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = "";
			if (value is int)
			{
				int num = (int)value;
				if (num > -1)
				{
					str = num.ToString();
				}
				if (num < -1)
				{
					str = "";
				}
				if (num == -1)
				{
					str = "...";
				}
			}
			return str;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}