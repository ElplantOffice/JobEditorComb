using ProductLib;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JobEditor
{
	public class ConverterEStepLapTypeToImageName : IValueConverter
	{
		public ConverterEStepLapTypeToImageName()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = "";
			if (value == null)
			{
				return str;
			}
			switch ((int)(EStepLapType)value)
			{
				case 0:
				{
					str = "Shape_Cut90Left.png";
					break;
				}
				case 1:
				{
					str = "Shape_Cut90Left_Right.png";
					break;
				}
				case 2:
				{
					str = "Shape_Cut90Left_Left.png";
					break;
				}
				case 3:
				{
					str = "Shape_Cut90Right.png";
					break;
				}
				case 4:
				{
					str = "Shape_Cut90Right_Left.png";
					break;
				}
				case 5:
				{
					str = "Shape_Cut90Right_Right.png";
					break;
				}
				case 6:
				{
					str = "Shape_Cut45Left.png";
					break;
				}
				case 7:
				{
					str = "Shape_Cut45Left_Right.png";
					break;
				}
				case 8:
				{
					str = "Shape_Cut45Left_Left.png";
					break;
				}
				case 9:
				{
					str = "Shape_Cut45Right.png";
					break;
				}
				case 10:
				{
					str = "Shape_Cut45Right_Left.png";
					break;
				}
				case 11:
				{
					str = "Shape_Cut45Right_Right.png";
					break;
				}
				case 12:
				{
					str = "Shape_Cut135Left.png";
					break;
				}
				case 13:
				{
					str = "Shape_Cut135Left_Right.png";
					break;
				}
				case 14:
				{
					str = "Shape_Cut135Left_Left.png";
					break;
				}
				case 15:
				{
					str = "Shape_Cut135Right.png";
					break;
				}
				case 16:
				{
					str = "Shape_Cut135Right_Left.png";
					break;
				}
				case 17:
				{
					str = "Shape_Cut135Right_Right.png";
					break;
				}
				case 18:
				{
					str = "Shape_CTipLeft.png";
					break;
				}
				case 19:
				{
					str = "Shape_CTipLeft_Up.png";
					break;
				}
				case 20:
				{
					str = "Shape_CTipLeft_Down.png";
					break;
				}
				case 21:
				{
					str = "Shape_CTipLeft_Left.png";
					break;
				}
				case 22:
				{
					str = "Shape_CTipLeft_Right.png";
					break;
				}
				case 23:
				{
					str = "Shape_CTipRight.png";
					break;
				}
				case 24:
				{
					str = "Shape_CTipRight_Up.png";
					break;
				}
				case 25:
				{
					str = "Shape_CTipRight_Down.png";
					break;
				}
				case 26:
				{
					str = "Shape_CTipRight_Left.png";
					break;
				}
				case 27:
				{
					str = "Shape_CTipRight_Right.png";
					break;
				}
				case 28:
				{
					str = "Shape_VTop.png";
					break;
				}
				case 29:
				{
					str = "Shape_VTop_Right.png";
					break;
				}
				case 30:
				{
					str = "Shape_VTop_Left.png";
					break;
				}
				case 31:
				{
					str = "Shape_VTop_Down.png";
					break;
				}
				case 32:
				{
					str = "Shape_VTop_Up.png";
					break;
				}
				case 33:
				{
					str = "Shape_VBottom.png";
					break;
				}
				case 34:
				{
					str = "Shape_VBottom_Right.png";
					break;
				}
				case 35:
				{
					str = "Shape_VBottom_Left.png";
					break;
				}
				case 36:
				{
					str = "Shape_VBottom_Down.png";
					break;
				}
				case 37:
				{
					str = "Shape_VBottom_Up.png";
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