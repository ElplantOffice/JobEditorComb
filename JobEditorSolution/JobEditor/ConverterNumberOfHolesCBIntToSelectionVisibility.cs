using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JobEditor
{
	public class ConverterNumberOfHolesCBIntToSelectionVisibility : IValueConverter
	{
		public ConverterNumberOfHolesCBIntToSelectionVisibility()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			if (value is int && (int)value >= 0)
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