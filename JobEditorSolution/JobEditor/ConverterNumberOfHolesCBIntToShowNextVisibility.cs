using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JobEditor
{
	public class ConverterNumberOfHolesCBIntToShowNextVisibility : IValueConverter
	{
		public ConverterNumberOfHolesCBIntToShowNextVisibility()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			if (value is int && (int)value == -1)
			{
				visibility = Visibility.Visible;
			}
			return visibility;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}