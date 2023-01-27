using ProductLib;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JobEditor
{
	public class ConverterEHoleShapeToImageName : IValueConverter
	{
		public ConverterEHoleShapeToImageName()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = "";
			if (value == null)
			{
				return str;
			}
			EHoleShape eHoleShape = (EHoleShape)value;
			if (eHoleShape == EHoleShape.Round)
			{
				str = "RoundHole.png";
			}
			else if (eHoleShape == EHoleShape.Oblong)
			{
				str = "OblongHole.png";
			}
			return str;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}