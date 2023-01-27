using ProductLib;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JobEditor
{
	public class ConverterEHeightCorrTypeSelToImage : IValueConverter
	{
		public ConverterEHeightCorrTypeSelToImage()
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
			EHeightCorrectionType eHeightCorrectionType = (EHeightCorrectionType)parameter;
			if (value is bool)
			{
				if (!(bool)value)
				{
					switch ((int)eHeightCorrectionType)
					{
						case 0:
						{
							str = "Stacking_No_NotSel.png";
							break;
						}
						case 1:
						{
							str = "Stacking_Cc_Up_NotSel.png";
							break;
						}
						case 2:
						{
							str = "Stacking_Cc_Down_NotSel.png";
							break;
						}
						case 3:
						{
							str = "Stacking_P_Up_NotSel.png";
							break;
						}
						case 4:
						{
							str = "Stacking_P_Down_NotSel.png";
							break;
						}
					}
				}
				else
				{
					switch ((int)eHeightCorrectionType)
					{
						case 0:
						{
							str = "Stacking_No_Sel.png";
							break;
						}
						case 1:
						{
							str = "Stacking_Cc_Up_Sel.png";
							break;
						}
						case 2:
						{
							str = "Stacking_Cc_Down_Sel.png";
							break;
						}
						case 3:
						{
							str = "Stacking_P_Up_Sel.png";
							break;
						}
						case 4:
						{
							str = "Stacking_P_Down_Sel.png";
							break;
						}
					}
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