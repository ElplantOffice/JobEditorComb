using System;
using System.Globalization;
using System.Windows.Data;

namespace JobEditor
{
	public class ConverterEMeasuringTypeSelToImage : IValueConverter
	{
		public ConverterEMeasuringTypeSelToImage()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = "";
			if (value == null)
			{
				return str;
			}
			if (parameter == null)
			{
				return str;
			}
			if (value is bool)
			{
				if (!(bool)value)
				{
					str = (!bool.Parse((string)parameter) ? "Relative_Layers_NotSel.png" : "Absolute_Layers_NotSel.png");
				}
				else
				{
					str = (!bool.Parse((string)parameter) ? "Relative_Layers_Sel.png" : "Absolute_Layers_Sel.png");
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