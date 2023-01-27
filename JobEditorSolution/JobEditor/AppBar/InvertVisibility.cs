using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JobEditor.AppBar
{
	public class InvertVisibility : IValueConverter
	{
		public InvertVisibility()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			if (parameter is string)
			{
				string lower = ((string)parameter).ToLower();
				if (lower.Contains("visible"))
				{
					visibility = Visibility.Visible;
				}
				if (lower.Contains("hidden"))
				{
					visibility = Visibility.Hidden;
				}
				if (lower.Contains("collapsed"))
				{
					visibility = Visibility.Collapsed;
				}
			}
			Visibility visibility1 = visibility;
			if (value is Visibility)
			{
				switch ((Visibility)value)
				{
					case Visibility.Visible:
					{
						return visibility;
					}
					case Visibility.Hidden:
					case Visibility.Collapsed:
					{
						return Visibility.Visible;
					}
				}
			}
			return visibility1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}