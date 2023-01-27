using ProductLib;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JobEditor
{
	public class ConverterEMeasuringTypeToImage : IValueConverter
	{
		public ConverterEMeasuringTypeToImage()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = "";
			if (value == null)
			{
				return str;
			}
			switch ((int)(EMeasuringType)value)
			{
				case 0:
				{
					str = "Absolute.png";
					break;
				}
				case 1:
				{
					str = "Relative.png";
					break;
				}
				case 2:
				{
					str = "Symmetric.png";
					break;
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