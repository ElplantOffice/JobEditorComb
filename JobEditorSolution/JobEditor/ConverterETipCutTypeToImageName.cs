using ProductLib;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JobEditor
{
	public class ConverterETipCutTypeToImageName : IValueConverter
	{
		public ConverterETipCutTypeToImageName()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = "";
			if (value == null)
			{
				return str;
			}
			switch ((int)(ETipCutType)value)
			{
				case 0:
				{
					str = "TipCut_Cut135Left_On.png";
					break;
				}
				case 1:
				{
					str = "TipCut_Cut45Left_On.png";
					break;
				}
				case 2:
				{
					str = "TipCut_Cut135Left_Off.png";
					break;
				}
				case 3:
				{
					str = "TipCut_Cut45Left_Off.png";
					break;
				}
				case 4:
				{
					str = "TipCut_Cut135Right_On.png";
					break;
				}
				case 5:
				{
					str = "TipCut_Cut45Right_On.png";
					break;
				}
				case 6:
				{
					str = "TipCut_Cut135Right_Off.png";
					break;
				}
				case 7:
				{
					str = "TipCut_Cut45Right_Off.png";
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