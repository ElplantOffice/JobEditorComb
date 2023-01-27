using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace JobEditor.Common
{
	public class HolesUCPar : INotifyPropertyChanged
	{
		private double holesUserControlWidth;

		private bool numberOfHoles0VisuEna;

		private bool numberOfHoles1VisuEna;

		private bool numberOfHoles2VisuEna;

		private bool numberOfHoles3VisuEna;

		private bool numberOfHoles4VisuEna;

		private bool numberOfHoles5VisuEna;

		private bool numberOfHoles6VisuEna;

		private bool numberOfHoles7VisuEna;

		private bool numberOfHoles8VisuEna;

		private bool numberOfHoles9VisuEna;

		private bool numberOfHoles10VisuEna;

		private bool numberOfHolesEmptySpaceVisuEna;

		private bool shapeAndLengthsSymVisuEna;

		private bool shapeAndLength1VisuEna;

		private bool shapeAndLength2VisuEna;

		private bool shapeAndLength3VisuEna;

		private bool shapeAndLength4VisuEna;

		private bool shapeAndLength5VisuEna;

		private bool shapeAndLength6VisuEna;

		private bool shapeAndLength7VisuEna;

		private bool shapeAndLength8VisuEna;

		private bool shapeAndLength9VisuEna;

		private bool shapeAndLength10VisuEna;

		private bool shapeAndLengthEmptySpace1RowVisuEna;

		private bool shapeAndLengthEmptySpace2RowsVisuEna;

		private bool shapeAndLengthEmptySpace3RowsVisuEna;

		private bool shapeAndLengthEmptySpace4RowsVisuEna;

		private bool shapeAndLengthShowNextHolesVisuEna;

		private double textBoxesWidth;

		private double textBoxesHeight;

		private SolidColorBrush tB1Grid1Color;

		private double tB1Grid1Width;

		private SolidColorBrush tB2Grid1Color;

		private SolidColorBrush tB2Grid2Color;

		private double tB2Grid1Width;

		private double tB2Grid2Width;

		private SolidColorBrush tB3Grid1Color;

		private SolidColorBrush tB3Grid2Color;

		private SolidColorBrush tB3Grid3Color;

		private double tB3Grid1Width;

		private double tB3Grid2Width;

		private double tB3Grid3Width;

		private SolidColorBrush tB4Grid1Color;

		private SolidColorBrush tB4Grid2Color;

		private SolidColorBrush tB4Grid3Color;

		private SolidColorBrush tB4Grid4Color;

		private double tB4Grid1Width;

		private double tB4Grid2Width;

		private double tB4Grid3Width;

		private double tB4Grid4Width;

		private SolidColorBrush tB5Grid1Color;

		private SolidColorBrush tB5Grid2Color;

		private SolidColorBrush tB5Grid3Color;

		private SolidColorBrush tB5Grid4Color;

		private SolidColorBrush tB5Grid5Color;

		private double tB5Grid1Width;

		private double tB5Grid2Width;

		private double tB5Grid3Width;

		private double tB5Grid4Width;

		private double tB5Grid5Width;

		private SolidColorBrush tB6Grid1Color;

		private SolidColorBrush tB6Grid2Color;

		private SolidColorBrush tB6Grid3Color;

		private SolidColorBrush tB6Grid4Color;

		private SolidColorBrush tB6Grid5Color;

		private SolidColorBrush tB6Grid6Color;

		private double tB6Grid1Width;

		private double tB6Grid2Width;

		private double tB6Grid3Width;

		private double tB6Grid4Width;

		private double tB6Grid5Width;

		private double tB6Grid6Width;

		private SolidColorBrush tB7Grid1Color;

		private SolidColorBrush tB7Grid2Color;

		private SolidColorBrush tB7Grid3Color;

		private SolidColorBrush tB7Grid4Color;

		private SolidColorBrush tB7Grid5Color;

		private SolidColorBrush tB7Grid6Color;

		private SolidColorBrush tB7Grid7Color;

		private double tB7Grid1Width;

		private double tB7Grid2Width;

		private double tB7Grid3Width;

		private double tB7Grid4Width;

		private double tB7Grid5Width;

		private double tB7Grid6Width;

		private double tB7Grid7Width;

		private SolidColorBrush tB8Grid1Color;

		private SolidColorBrush tB8Grid2Color;

		private SolidColorBrush tB8Grid3Color;

		private SolidColorBrush tB8Grid4Color;

		private SolidColorBrush tB8Grid5Color;

		private SolidColorBrush tB8Grid6Color;

		private SolidColorBrush tB8Grid7Color;

		private SolidColorBrush tB8Grid8Color;

		private double tB8Grid1Width;

		private double tB8Grid2Width;

		private double tB8Grid3Width;

		private double tB8Grid4Width;

		private double tB8Grid5Width;

		private double tB8Grid6Width;

		private double tB8Grid7Width;

		private double tB8Grid8Width;

		private SolidColorBrush tB9Grid1Color;

		private SolidColorBrush tB9Grid2Color;

		private SolidColorBrush tB9Grid3Color;

		private SolidColorBrush tB9Grid4Color;

		private SolidColorBrush tB9Grid5Color;

		private SolidColorBrush tB9Grid6Color;

		private SolidColorBrush tB9Grid7Color;

		private SolidColorBrush tB9Grid8Color;

		private SolidColorBrush tB9Grid9Color;

		private double tB9Grid1Width;

		private double tB9Grid2Width;

		private double tB9Grid3Width;

		private double tB9Grid4Width;

		private double tB9Grid5Width;

		private double tB9Grid6Width;

		private double tB9Grid7Width;

		private double tB9Grid8Width;

		private double tB9Grid9Width;

		private SolidColorBrush tB10Grid1Color;

		private SolidColorBrush tB10Grid2Color;

		private SolidColorBrush tB10Grid3Color;

		private SolidColorBrush tB10Grid4Color;

		private SolidColorBrush tB10Grid5Color;

		private SolidColorBrush tB10Grid6Color;

		private SolidColorBrush tB10Grid7Color;

		private SolidColorBrush tB10Grid8Color;

		private SolidColorBrush tB10Grid9Color;

		private SolidColorBrush tB10Grid10Color;

		private double tB10Grid1Width;

		private double tB10Grid2Width;

		private double tB10Grid3Width;

		private double tB10Grid4Width;

		private double tB10Grid5Width;

		private double tB10Grid6Width;

		private double tB10Grid7Width;

		private double tB10Grid8Width;

		private double tB10Grid9Width;

		private double tB10Grid10Width;

		private bool restGridEna;

		private Thickness mTComboBoxMargin;

		private double simetricLineWidth;

		private double holesTextBoxBaseWidth;

		private double holesTextBoxColorGridHeight;

		private double holesTextBoxBaseHeight;

		public double HolesTextBoxBaseHeight
		{
			get
			{
				return this.holesTextBoxBaseHeight;
			}
			set
			{
				this.holesTextBoxBaseHeight = value;
				this.RaisePropertyChanged("HolesTextBoxBaseHeight");
			}
		}

		public double HolesTextBoxBaseWidth
		{
			get
			{
				return this.holesTextBoxBaseWidth;
			}
			set
			{
				this.holesTextBoxBaseWidth = value;
				this.RaisePropertyChanged("HolesTextBoxBaseWidth");
			}
		}

		public double HolesTextBoxColorGridHeight
		{
			get
			{
				return this.holesTextBoxColorGridHeight;
			}
			set
			{
				this.holesTextBoxColorGridHeight = value;
				this.RaisePropertyChanged("HolesTextBoxColorGridHeight");
			}
		}

		public double HolesUserControlWidth
		{
			get
			{
				return this.holesUserControlWidth;
			}
			set
			{
				this.holesUserControlWidth = value;
				this.RaisePropertyChanged("HolesUserControlWidth");
			}
		}

		public Thickness MTComboBoxMargin
		{
			get
			{
				return this.mTComboBoxMargin;
			}
			set
			{
				this.mTComboBoxMargin = value;
				this.RaisePropertyChanged("MTComboBoxMargin");
			}
		}

		public bool NumberOfHoles0VisuEna
		{
			get
			{
				return this.numberOfHoles0VisuEna;
			}
			set
			{
				this.numberOfHoles0VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles0VisuEna");
			}
		}

		public bool NumberOfHoles10VisuEna
		{
			get
			{
				return this.numberOfHoles10VisuEna;
			}
			set
			{
				this.numberOfHoles10VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles10VisuEna");
			}
		}

		public bool NumberOfHoles1VisuEna
		{
			get
			{
				return this.numberOfHoles1VisuEna;
			}
			set
			{
				this.numberOfHoles1VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles1VisuEna");
			}
		}

		public bool NumberOfHoles2VisuEna
		{
			get
			{
				return this.numberOfHoles2VisuEna;
			}
			set
			{
				this.numberOfHoles2VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles2VisuEna");
			}
		}

		public bool NumberOfHoles3VisuEna
		{
			get
			{
				return this.numberOfHoles3VisuEna;
			}
			set
			{
				this.numberOfHoles3VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles3VisuEna");
			}
		}

		public bool NumberOfHoles4VisuEna
		{
			get
			{
				return this.numberOfHoles4VisuEna;
			}
			set
			{
				this.numberOfHoles4VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles4VisuEna");
			}
		}

		public bool NumberOfHoles5VisuEna
		{
			get
			{
				return this.numberOfHoles5VisuEna;
			}
			set
			{
				this.numberOfHoles5VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles5VisuEna");
			}
		}

		public bool NumberOfHoles6VisuEna
		{
			get
			{
				return this.numberOfHoles6VisuEna;
			}
			set
			{
				this.numberOfHoles6VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles6VisuEna");
			}
		}

		public bool NumberOfHoles7VisuEna
		{
			get
			{
				return this.numberOfHoles7VisuEna;
			}
			set
			{
				this.numberOfHoles7VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles7VisuEna");
			}
		}

		public bool NumberOfHoles8VisuEna
		{
			get
			{
				return this.numberOfHoles8VisuEna;
			}
			set
			{
				this.numberOfHoles8VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles8VisuEna");
			}
		}

		public bool NumberOfHoles9VisuEna
		{
			get
			{
				return this.numberOfHoles9VisuEna;
			}
			set
			{
				this.numberOfHoles9VisuEna = value;
				this.RaisePropertyChanged("NumberOfHoles9VisuEna");
			}
		}

		public bool NumberOfHolesEmptySpaceVisuEna
		{
			get
			{
				return this.numberOfHolesEmptySpaceVisuEna;
			}
			set
			{
				this.numberOfHolesEmptySpaceVisuEna = value;
				this.RaisePropertyChanged("NumberOfHolesEmptySpaceVisuEna");
			}
		}

		public bool RestGridEna
		{
			get
			{
				return this.restGridEna;
			}
			set
			{
				this.restGridEna = value;
				this.RaisePropertyChanged("RestGridEna");
			}
		}

		public bool ShapeAndLength10VisuEna
		{
			get
			{
				return this.shapeAndLength10VisuEna;
			}
			set
			{
				this.shapeAndLength10VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength10VisuEna");
			}
		}

		public bool ShapeAndLength1VisuEna
		{
			get
			{
				return this.shapeAndLength1VisuEna;
			}
			set
			{
				this.shapeAndLength1VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength1VisuEna");
			}
		}

		public bool ShapeAndLength2VisuEna
		{
			get
			{
				return this.shapeAndLength2VisuEna;
			}
			set
			{
				this.shapeAndLength2VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength2VisuEna");
			}
		}

		public bool ShapeAndLength3VisuEna
		{
			get
			{
				return this.shapeAndLength3VisuEna;
			}
			set
			{
				this.shapeAndLength3VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength3VisuEna");
			}
		}

		public bool ShapeAndLength4VisuEna
		{
			get
			{
				return this.shapeAndLength4VisuEna;
			}
			set
			{
				this.shapeAndLength4VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength4VisuEna");
			}
		}

		public bool ShapeAndLength5VisuEna
		{
			get
			{
				return this.shapeAndLength5VisuEna;
			}
			set
			{
				this.shapeAndLength5VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength5VisuEna");
			}
		}

		public bool ShapeAndLength6VisuEna
		{
			get
			{
				return this.shapeAndLength6VisuEna;
			}
			set
			{
				this.shapeAndLength6VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength6VisuEna");
			}
		}

		public bool ShapeAndLength7VisuEna
		{
			get
			{
				return this.shapeAndLength7VisuEna;
			}
			set
			{
				this.shapeAndLength7VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength7VisuEna");
			}
		}

		public bool ShapeAndLength8VisuEna
		{
			get
			{
				return this.shapeAndLength8VisuEna;
			}
			set
			{
				this.shapeAndLength8VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength8VisuEna");
			}
		}

		public bool ShapeAndLength9VisuEna
		{
			get
			{
				return this.shapeAndLength9VisuEna;
			}
			set
			{
				this.shapeAndLength9VisuEna = value;
				this.RaisePropertyChanged("ShapeAndLength9VisuEna");
			}
		}

		public bool ShapeAndLengthEmptySpace1RowVisuEna
		{
			get
			{
				return this.shapeAndLengthEmptySpace1RowVisuEna;
			}
			set
			{
				this.shapeAndLengthEmptySpace1RowVisuEna = value;
				this.RaisePropertyChanged("ShapeAndLengthEmptySpace1RowVisuEna");
			}
		}

		public bool ShapeAndLengthEmptySpace2RowsVisuEna
		{
			get
			{
				return this.shapeAndLengthEmptySpace2RowsVisuEna;
			}
			set
			{
				this.shapeAndLengthEmptySpace2RowsVisuEna = value;
				this.RaisePropertyChanged("ShapeAndLengthEmptySpace2RowsVisuEna");
			}
		}

		public bool ShapeAndLengthEmptySpace3RowsVisuEna
		{
			get
			{
				return this.shapeAndLengthEmptySpace3RowsVisuEna;
			}
			set
			{
				this.shapeAndLengthEmptySpace3RowsVisuEna = value;
				this.RaisePropertyChanged("ShapeAndLengthEmptySpace3RowsVisuEna");
			}
		}

		public bool ShapeAndLengthEmptySpace4RowsVisuEna
		{
			get
			{
				return this.shapeAndLengthEmptySpace4RowsVisuEna;
			}
			set
			{
				this.shapeAndLengthEmptySpace4RowsVisuEna = value;
				this.RaisePropertyChanged("ShapeAndLengthEmptySpace4RowsVisuEna");
			}
		}

		public bool ShapeAndLengthShowNextHolesVisuEna
		{
			get
			{
				return this.shapeAndLengthShowNextHolesVisuEna;
			}
			set
			{
				this.shapeAndLengthShowNextHolesVisuEna = value;
				this.RaisePropertyChanged("ShapeAndLengthShowNextHolesVisuEna");
			}
		}

		public bool ShapeAndLengthsSymVisuEna
		{
			get
			{
				return this.shapeAndLengthsSymVisuEna;
			}
			set
			{
				this.shapeAndLengthsSymVisuEna = value;
				this.RaisePropertyChanged("ShapeAndLengthsSymVisuEna");
			}
		}

		public double SimetricLineWidth
		{
			get
			{
				return this.simetricLineWidth;
			}
			set
			{
				this.simetricLineWidth = value;
				this.RaisePropertyChanged("SimetricLineWidth");
			}
		}

		public SolidColorBrush TB10Grid10Color
		{
			get
			{
				return this.tB10Grid10Color;
			}
			set
			{
				this.tB10Grid10Color = value;
				this.RaisePropertyChanged("TB10Grid10Color");
			}
		}

		public double TB10Grid10Width
		{
			get
			{
				return this.tB10Grid10Width;
			}
			set
			{
				this.tB10Grid10Width = value;
				this.RaisePropertyChanged("TB10Grid10Width");
			}
		}

		public SolidColorBrush TB10Grid1Color
		{
			get
			{
				return this.tB10Grid1Color;
			}
			set
			{
				this.tB10Grid1Color = value;
				this.RaisePropertyChanged("TB10Grid1Color");
			}
		}

		public double TB10Grid1Width
		{
			get
			{
				return this.tB10Grid1Width;
			}
			set
			{
				this.tB10Grid1Width = value;
				this.RaisePropertyChanged("TB10Grid1Width");
			}
		}

		public SolidColorBrush TB10Grid2Color
		{
			get
			{
				return this.tB10Grid2Color;
			}
			set
			{
				this.tB10Grid2Color = value;
				this.RaisePropertyChanged("TB10Grid2Color");
			}
		}

		public double TB10Grid2Width
		{
			get
			{
				return this.tB10Grid2Width;
			}
			set
			{
				this.tB10Grid2Width = value;
				this.RaisePropertyChanged("TB10Grid2Width");
			}
		}

		public SolidColorBrush TB10Grid3Color
		{
			get
			{
				return this.tB10Grid3Color;
			}
			set
			{
				this.tB10Grid3Color = value;
				this.RaisePropertyChanged("TB10Grid3Color");
			}
		}

		public double TB10Grid3Width
		{
			get
			{
				return this.tB10Grid3Width;
			}
			set
			{
				this.tB10Grid3Width = value;
				this.RaisePropertyChanged("TB10Grid3Width");
			}
		}

		public SolidColorBrush TB10Grid4Color
		{
			get
			{
				return this.tB10Grid4Color;
			}
			set
			{
				this.tB10Grid4Color = value;
				this.RaisePropertyChanged("TB10Grid4Color");
			}
		}

		public double TB10Grid4Width
		{
			get
			{
				return this.tB10Grid4Width;
			}
			set
			{
				this.tB10Grid4Width = value;
				this.RaisePropertyChanged("TB10Grid4Width");
			}
		}

		public SolidColorBrush TB10Grid5Color
		{
			get
			{
				return this.tB10Grid5Color;
			}
			set
			{
				this.tB10Grid5Color = value;
				this.RaisePropertyChanged("TB10Grid5Color");
			}
		}

		public double TB10Grid5Width
		{
			get
			{
				return this.tB10Grid5Width;
			}
			set
			{
				this.tB10Grid5Width = value;
				this.RaisePropertyChanged("TB10Grid5Width");
			}
		}

		public SolidColorBrush TB10Grid6Color
		{
			get
			{
				return this.tB10Grid6Color;
			}
			set
			{
				this.tB10Grid6Color = value;
				this.RaisePropertyChanged("TB10Grid6Color");
			}
		}

		public double TB10Grid6Width
		{
			get
			{
				return this.tB10Grid6Width;
			}
			set
			{
				this.tB10Grid6Width = value;
				this.RaisePropertyChanged("TB10Grid6Width");
			}
		}

		public SolidColorBrush TB10Grid7Color
		{
			get
			{
				return this.tB10Grid7Color;
			}
			set
			{
				this.tB10Grid7Color = value;
				this.RaisePropertyChanged("TB10Grid7Color");
			}
		}

		public double TB10Grid7Width
		{
			get
			{
				return this.tB10Grid7Width;
			}
			set
			{
				this.tB10Grid7Width = value;
				this.RaisePropertyChanged("TB10Grid7Width");
			}
		}

		public SolidColorBrush TB10Grid8Color
		{
			get
			{
				return this.tB10Grid8Color;
			}
			set
			{
				this.tB10Grid8Color = value;
				this.RaisePropertyChanged("TB10Grid8Color");
			}
		}

		public double TB10Grid8Width
		{
			get
			{
				return this.tB10Grid8Width;
			}
			set
			{
				this.tB10Grid8Width = value;
				this.RaisePropertyChanged("TB10Grid8Width");
			}
		}

		public SolidColorBrush TB10Grid9Color
		{
			get
			{
				return this.tB10Grid9Color;
			}
			set
			{
				this.tB10Grid9Color = value;
				this.RaisePropertyChanged("TB10Grid9Color");
			}
		}

		public double TB10Grid9Width
		{
			get
			{
				return this.tB10Grid9Width;
			}
			set
			{
				this.tB10Grid9Width = value;
				this.RaisePropertyChanged("TB10Grid9Width");
			}
		}

		public SolidColorBrush TB1Grid1Color
		{
			get
			{
				return this.tB1Grid1Color;
			}
			set
			{
				this.tB1Grid1Color = value;
				this.RaisePropertyChanged("TB1Grid1Color");
			}
		}

		public double TB1Grid1Width
		{
			get
			{
				return this.tB1Grid1Width;
			}
			set
			{
				this.tB1Grid1Width = value;
				this.RaisePropertyChanged("TB1Grid1Width");
			}
		}

		public SolidColorBrush TB2Grid1Color
		{
			get
			{
				return this.tB2Grid1Color;
			}
			set
			{
				this.tB2Grid1Color = value;
				this.RaisePropertyChanged("TB2Grid1Color");
			}
		}

		public double TB2Grid1Width
		{
			get
			{
				return this.tB2Grid1Width;
			}
			set
			{
				this.tB2Grid1Width = value;
				this.RaisePropertyChanged("TB2Grid1Width");
			}
		}

		public SolidColorBrush TB2Grid2Color
		{
			get
			{
				return this.tB2Grid2Color;
			}
			set
			{
				this.tB2Grid2Color = value;
				this.RaisePropertyChanged("TB2Grid2Color");
			}
		}

		public double TB2Grid2Width
		{
			get
			{
				return this.tB2Grid2Width;
			}
			set
			{
				this.tB2Grid2Width = value;
				this.RaisePropertyChanged("TB2Grid2Width");
			}
		}

		public SolidColorBrush TB3Grid1Color
		{
			get
			{
				return this.tB3Grid1Color;
			}
			set
			{
				this.tB3Grid1Color = value;
				this.RaisePropertyChanged("TB3Grid1Color");
			}
		}

		public double TB3Grid1Width
		{
			get
			{
				return this.tB3Grid1Width;
			}
			set
			{
				this.tB3Grid1Width = value;
				this.RaisePropertyChanged("TB3Grid1Width");
			}
		}

		public SolidColorBrush TB3Grid2Color
		{
			get
			{
				return this.tB3Grid2Color;
			}
			set
			{
				this.tB3Grid2Color = value;
				this.RaisePropertyChanged("TB3Grid2Color");
			}
		}

		public double TB3Grid2Width
		{
			get
			{
				return this.tB3Grid2Width;
			}
			set
			{
				this.tB3Grid2Width = value;
				this.RaisePropertyChanged("TB3Grid2Width");
			}
		}

		public SolidColorBrush TB3Grid3Color
		{
			get
			{
				return this.tB3Grid3Color;
			}
			set
			{
				this.tB3Grid3Color = value;
				this.RaisePropertyChanged("TB3Grid3Color");
			}
		}

		public double TB3Grid3Width
		{
			get
			{
				return this.tB3Grid3Width;
			}
			set
			{
				this.tB3Grid3Width = value;
				this.RaisePropertyChanged("TB3Grid3Width");
			}
		}

		public SolidColorBrush TB4Grid1Color
		{
			get
			{
				return this.tB4Grid1Color;
			}
			set
			{
				this.tB4Grid1Color = value;
				this.RaisePropertyChanged("TB4Grid1Color");
			}
		}

		public double TB4Grid1Width
		{
			get
			{
				return this.tB4Grid1Width;
			}
			set
			{
				this.tB4Grid1Width = value;
				this.RaisePropertyChanged("TB4Grid1Width");
			}
		}

		public SolidColorBrush TB4Grid2Color
		{
			get
			{
				return this.tB4Grid2Color;
			}
			set
			{
				this.tB4Grid2Color = value;
				this.RaisePropertyChanged("TB4Grid2Color");
			}
		}

		public double TB4Grid2Width
		{
			get
			{
				return this.tB4Grid2Width;
			}
			set
			{
				this.tB4Grid2Width = value;
				this.RaisePropertyChanged("TB4Grid2Width");
			}
		}

		public SolidColorBrush TB4Grid3Color
		{
			get
			{
				return this.tB4Grid3Color;
			}
			set
			{
				this.tB4Grid3Color = value;
				this.RaisePropertyChanged("TB4Grid3Color");
			}
		}

		public double TB4Grid3Width
		{
			get
			{
				return this.tB4Grid3Width;
			}
			set
			{
				this.tB4Grid3Width = value;
				this.RaisePropertyChanged("TB4Grid3Width");
			}
		}

		public SolidColorBrush TB4Grid4Color
		{
			get
			{
				return this.tB4Grid4Color;
			}
			set
			{
				this.tB4Grid4Color = value;
				this.RaisePropertyChanged("TB4Grid4Color");
			}
		}

		public double TB4Grid4Width
		{
			get
			{
				return this.tB4Grid4Width;
			}
			set
			{
				this.tB4Grid4Width = value;
				this.RaisePropertyChanged("TB4Grid4Width");
			}
		}

		public SolidColorBrush TB5Grid1Color
		{
			get
			{
				return this.tB5Grid1Color;
			}
			set
			{
				this.tB5Grid1Color = value;
				this.RaisePropertyChanged("TB5Grid1Color");
			}
		}

		public double TB5Grid1Width
		{
			get
			{
				return this.tB5Grid1Width;
			}
			set
			{
				this.tB5Grid1Width = value;
				this.RaisePropertyChanged("TB5Grid1Width");
			}
		}

		public SolidColorBrush TB5Grid2Color
		{
			get
			{
				return this.tB5Grid2Color;
			}
			set
			{
				this.tB5Grid2Color = value;
				this.RaisePropertyChanged("TB5Grid2Color");
			}
		}

		public double TB5Grid2Width
		{
			get
			{
				return this.tB5Grid2Width;
			}
			set
			{
				this.tB5Grid2Width = value;
				this.RaisePropertyChanged("TB5Grid2Width");
			}
		}

		public SolidColorBrush TB5Grid3Color
		{
			get
			{
				return this.tB5Grid3Color;
			}
			set
			{
				this.tB5Grid3Color = value;
				this.RaisePropertyChanged("TB5Grid3Color");
			}
		}

		public double TB5Grid3Width
		{
			get
			{
				return this.tB5Grid3Width;
			}
			set
			{
				this.tB5Grid3Width = value;
				this.RaisePropertyChanged("TB5Grid3Width");
			}
		}

		public SolidColorBrush TB5Grid4Color
		{
			get
			{
				return this.tB5Grid4Color;
			}
			set
			{
				this.tB5Grid4Color = value;
				this.RaisePropertyChanged("TB5Grid4Color");
			}
		}

		public double TB5Grid4Width
		{
			get
			{
				return this.tB5Grid4Width;
			}
			set
			{
				this.tB5Grid4Width = value;
				this.RaisePropertyChanged("TB5Grid4Width");
			}
		}

		public SolidColorBrush TB5Grid5Color
		{
			get
			{
				return this.tB5Grid5Color;
			}
			set
			{
				this.tB5Grid5Color = value;
				this.RaisePropertyChanged("TB5Grid5Color");
			}
		}

		public double TB5Grid5Width
		{
			get
			{
				return this.tB5Grid5Width;
			}
			set
			{
				this.tB5Grid5Width = value;
				this.RaisePropertyChanged("TB5Grid5Width");
			}
		}

		public SolidColorBrush TB6Grid1Color
		{
			get
			{
				return this.tB6Grid1Color;
			}
			set
			{
				this.tB6Grid1Color = value;
				this.RaisePropertyChanged("TB6Grid1Color");
			}
		}

		public double TB6Grid1Width
		{
			get
			{
				return this.tB6Grid1Width;
			}
			set
			{
				this.tB6Grid1Width = value;
				this.RaisePropertyChanged("TB6Grid1Width");
			}
		}

		public SolidColorBrush TB6Grid2Color
		{
			get
			{
				return this.tB6Grid2Color;
			}
			set
			{
				this.tB6Grid2Color = value;
				this.RaisePropertyChanged("TB6Grid2Color");
			}
		}

		public double TB6Grid2Width
		{
			get
			{
				return this.tB6Grid2Width;
			}
			set
			{
				this.tB6Grid2Width = value;
				this.RaisePropertyChanged("TB6Grid2Width");
			}
		}

		public SolidColorBrush TB6Grid3Color
		{
			get
			{
				return this.tB6Grid3Color;
			}
			set
			{
				this.tB6Grid3Color = value;
				this.RaisePropertyChanged("TB6Grid3Color");
			}
		}

		public double TB6Grid3Width
		{
			get
			{
				return this.tB6Grid3Width;
			}
			set
			{
				this.tB6Grid3Width = value;
				this.RaisePropertyChanged("TB6Grid3Width");
			}
		}

		public SolidColorBrush TB6Grid4Color
		{
			get
			{
				return this.tB6Grid4Color;
			}
			set
			{
				this.tB6Grid4Color = value;
				this.RaisePropertyChanged("TB6Grid4Color");
			}
		}

		public double TB6Grid4Width
		{
			get
			{
				return this.tB6Grid4Width;
			}
			set
			{
				this.tB6Grid4Width = value;
				this.RaisePropertyChanged("TB6Grid4Width");
			}
		}

		public SolidColorBrush TB6Grid5Color
		{
			get
			{
				return this.tB6Grid5Color;
			}
			set
			{
				this.tB6Grid5Color = value;
				this.RaisePropertyChanged("TB6Grid5Color");
			}
		}

		public double TB6Grid5Width
		{
			get
			{
				return this.tB6Grid5Width;
			}
			set
			{
				this.tB6Grid5Width = value;
				this.RaisePropertyChanged("TB6Grid5Width");
			}
		}

		public SolidColorBrush TB6Grid6Color
		{
			get
			{
				return this.tB6Grid6Color;
			}
			set
			{
				this.tB6Grid6Color = value;
				this.RaisePropertyChanged("TB6Grid6Color");
			}
		}

		public double TB6Grid6Width
		{
			get
			{
				return this.tB6Grid6Width;
			}
			set
			{
				this.tB6Grid6Width = value;
				this.RaisePropertyChanged("TB6Grid6Width");
			}
		}

		public SolidColorBrush TB7Grid1Color
		{
			get
			{
				return this.tB7Grid1Color;
			}
			set
			{
				this.tB7Grid1Color = value;
				this.RaisePropertyChanged("TB7Grid1Color");
			}
		}

		public double TB7Grid1Width
		{
			get
			{
				return this.tB7Grid1Width;
			}
			set
			{
				this.tB7Grid1Width = value;
				this.RaisePropertyChanged("TB7Grid1Width");
			}
		}

		public SolidColorBrush TB7Grid2Color
		{
			get
			{
				return this.tB7Grid2Color;
			}
			set
			{
				this.tB7Grid2Color = value;
				this.RaisePropertyChanged("TB7Grid2Color");
			}
		}

		public double TB7Grid2Width
		{
			get
			{
				return this.tB7Grid2Width;
			}
			set
			{
				this.tB7Grid2Width = value;
				this.RaisePropertyChanged("TB7Grid2Width");
			}
		}

		public SolidColorBrush TB7Grid3Color
		{
			get
			{
				return this.tB7Grid3Color;
			}
			set
			{
				this.tB7Grid3Color = value;
				this.RaisePropertyChanged("TB7Grid3Color");
			}
		}

		public double TB7Grid3Width
		{
			get
			{
				return this.tB7Grid3Width;
			}
			set
			{
				this.tB7Grid3Width = value;
				this.RaisePropertyChanged("TB7Grid3Width");
			}
		}

		public SolidColorBrush TB7Grid4Color
		{
			get
			{
				return this.tB7Grid4Color;
			}
			set
			{
				this.tB7Grid4Color = value;
				this.RaisePropertyChanged("TB7Grid4Color");
			}
		}

		public double TB7Grid4Width
		{
			get
			{
				return this.tB7Grid4Width;
			}
			set
			{
				this.tB7Grid4Width = value;
				this.RaisePropertyChanged("TB7Grid4Width");
			}
		}

		public SolidColorBrush TB7Grid5Color
		{
			get
			{
				return this.tB7Grid5Color;
			}
			set
			{
				this.tB7Grid5Color = value;
				this.RaisePropertyChanged("TB7Grid5Color");
			}
		}

		public double TB7Grid5Width
		{
			get
			{
				return this.tB7Grid5Width;
			}
			set
			{
				this.tB7Grid5Width = value;
				this.RaisePropertyChanged("TB7Grid5Width");
			}
		}

		public SolidColorBrush TB7Grid6Color
		{
			get
			{
				return this.tB7Grid6Color;
			}
			set
			{
				this.tB7Grid6Color = value;
				this.RaisePropertyChanged("TB7Grid6Color");
			}
		}

		public double TB7Grid6Width
		{
			get
			{
				return this.tB7Grid6Width;
			}
			set
			{
				this.tB7Grid6Width = value;
				this.RaisePropertyChanged("TB7Grid6Width");
			}
		}

		public SolidColorBrush TB7Grid7Color
		{
			get
			{
				return this.tB7Grid7Color;
			}
			set
			{
				this.tB7Grid7Color = value;
				this.RaisePropertyChanged("TB7Grid7Color");
			}
		}

		public double TB7Grid7Width
		{
			get
			{
				return this.tB7Grid7Width;
			}
			set
			{
				this.tB7Grid7Width = value;
				this.RaisePropertyChanged("TB7Grid7Width");
			}
		}

		public SolidColorBrush TB8Grid1Color
		{
			get
			{
				return this.tB8Grid1Color;
			}
			set
			{
				this.tB8Grid1Color = value;
				this.RaisePropertyChanged("TB8Grid1Color");
			}
		}

		public double TB8Grid1Width
		{
			get
			{
				return this.tB8Grid1Width;
			}
			set
			{
				this.tB8Grid1Width = value;
				this.RaisePropertyChanged("TB8Grid1Width");
			}
		}

		public SolidColorBrush TB8Grid2Color
		{
			get
			{
				return this.tB8Grid2Color;
			}
			set
			{
				this.tB8Grid2Color = value;
				this.RaisePropertyChanged("TB8Grid2Color");
			}
		}

		public double TB8Grid2Width
		{
			get
			{
				return this.tB8Grid2Width;
			}
			set
			{
				this.tB8Grid2Width = value;
				this.RaisePropertyChanged("TB8Grid2Width");
			}
		}

		public SolidColorBrush TB8Grid3Color
		{
			get
			{
				return this.tB8Grid3Color;
			}
			set
			{
				this.tB8Grid3Color = value;
				this.RaisePropertyChanged("TB8Grid3Color");
			}
		}

		public double TB8Grid3Width
		{
			get
			{
				return this.tB8Grid3Width;
			}
			set
			{
				this.tB8Grid3Width = value;
				this.RaisePropertyChanged("TB8Grid3Width");
			}
		}

		public SolidColorBrush TB8Grid4Color
		{
			get
			{
				return this.tB8Grid4Color;
			}
			set
			{
				this.tB8Grid4Color = value;
				this.RaisePropertyChanged("TB8Grid4Color");
			}
		}

		public double TB8Grid4Width
		{
			get
			{
				return this.tB8Grid4Width;
			}
			set
			{
				this.tB8Grid4Width = value;
				this.RaisePropertyChanged("TB8Grid4Width");
			}
		}

		public SolidColorBrush TB8Grid5Color
		{
			get
			{
				return this.tB8Grid5Color;
			}
			set
			{
				this.tB8Grid5Color = value;
				this.RaisePropertyChanged("TB8Grid5Color");
			}
		}

		public double TB8Grid5Width
		{
			get
			{
				return this.tB8Grid5Width;
			}
			set
			{
				this.tB8Grid5Width = value;
				this.RaisePropertyChanged("TB8Grid5Width");
			}
		}

		public SolidColorBrush TB8Grid6Color
		{
			get
			{
				return this.tB8Grid6Color;
			}
			set
			{
				this.tB8Grid6Color = value;
				this.RaisePropertyChanged("TB8Grid6Color");
			}
		}

		public double TB8Grid6Width
		{
			get
			{
				return this.tB8Grid6Width;
			}
			set
			{
				this.tB8Grid6Width = value;
				this.RaisePropertyChanged("TB8Grid6Width");
			}
		}

		public SolidColorBrush TB8Grid7Color
		{
			get
			{
				return this.tB8Grid7Color;
			}
			set
			{
				this.tB8Grid7Color = value;
				this.RaisePropertyChanged("TB8Grid7Color");
			}
		}

		public double TB8Grid7Width
		{
			get
			{
				return this.tB8Grid7Width;
			}
			set
			{
				this.tB8Grid7Width = value;
				this.RaisePropertyChanged("TB8Grid7Width");
			}
		}

		public SolidColorBrush TB8Grid8Color
		{
			get
			{
				return this.tB8Grid8Color;
			}
			set
			{
				this.tB8Grid8Color = value;
				this.RaisePropertyChanged("TB8Grid8Color");
			}
		}

		public double TB8Grid8Width
		{
			get
			{
				return this.tB8Grid8Width;
			}
			set
			{
				this.tB8Grid8Width = value;
				this.RaisePropertyChanged("TB8Grid8Width");
			}
		}

		public SolidColorBrush TB9Grid1Color
		{
			get
			{
				return this.tB9Grid1Color;
			}
			set
			{
				this.tB9Grid1Color = value;
				this.RaisePropertyChanged("TB9Grid1Color");
			}
		}

		public double TB9Grid1Width
		{
			get
			{
				return this.tB9Grid1Width;
			}
			set
			{
				this.tB9Grid1Width = value;
				this.RaisePropertyChanged("TB9Grid1Width");
			}
		}

		public SolidColorBrush TB9Grid2Color
		{
			get
			{
				return this.tB9Grid2Color;
			}
			set
			{
				this.tB9Grid2Color = value;
				this.RaisePropertyChanged("TB9Grid2Color");
			}
		}

		public double TB9Grid2Width
		{
			get
			{
				return this.tB9Grid2Width;
			}
			set
			{
				this.tB9Grid2Width = value;
				this.RaisePropertyChanged("TB9Grid2Width");
			}
		}

		public SolidColorBrush TB9Grid3Color
		{
			get
			{
				return this.tB9Grid3Color;
			}
			set
			{
				this.tB9Grid3Color = value;
				this.RaisePropertyChanged("TB9Grid3Color");
			}
		}

		public double TB9Grid3Width
		{
			get
			{
				return this.tB9Grid3Width;
			}
			set
			{
				this.tB9Grid3Width = value;
				this.RaisePropertyChanged("TB9Grid3Width");
			}
		}

		public SolidColorBrush TB9Grid4Color
		{
			get
			{
				return this.tB9Grid4Color;
			}
			set
			{
				this.tB9Grid4Color = value;
				this.RaisePropertyChanged("TB9Grid4Color");
			}
		}

		public double TB9Grid4Width
		{
			get
			{
				return this.tB9Grid4Width;
			}
			set
			{
				this.tB9Grid4Width = value;
				this.RaisePropertyChanged("TB9Grid4Width");
			}
		}

		public SolidColorBrush TB9Grid5Color
		{
			get
			{
				return this.tB9Grid5Color;
			}
			set
			{
				this.tB9Grid5Color = value;
				this.RaisePropertyChanged("TB9Grid5Color");
			}
		}

		public double TB9Grid5Width
		{
			get
			{
				return this.tB9Grid5Width;
			}
			set
			{
				this.tB9Grid5Width = value;
				this.RaisePropertyChanged("TB9Grid5Width");
			}
		}

		public SolidColorBrush TB9Grid6Color
		{
			get
			{
				return this.tB9Grid6Color;
			}
			set
			{
				this.tB9Grid6Color = value;
				this.RaisePropertyChanged("TB9Grid6Color");
			}
		}

		public double TB9Grid6Width
		{
			get
			{
				return this.tB9Grid6Width;
			}
			set
			{
				this.tB9Grid6Width = value;
				this.RaisePropertyChanged("TB9Grid6Width");
			}
		}

		public SolidColorBrush TB9Grid7Color
		{
			get
			{
				return this.tB9Grid7Color;
			}
			set
			{
				this.tB9Grid7Color = value;
				this.RaisePropertyChanged("TB9Grid7Color");
			}
		}

		public double TB9Grid7Width
		{
			get
			{
				return this.tB9Grid7Width;
			}
			set
			{
				this.tB9Grid7Width = value;
				this.RaisePropertyChanged("TB9Grid7Width");
			}
		}

		public SolidColorBrush TB9Grid8Color
		{
			get
			{
				return this.tB9Grid8Color;
			}
			set
			{
				this.tB9Grid8Color = value;
				this.RaisePropertyChanged("TB9Grid8Color");
			}
		}

		public double TB9Grid8Width
		{
			get
			{
				return this.tB9Grid8Width;
			}
			set
			{
				this.tB9Grid8Width = value;
				this.RaisePropertyChanged("TB9Grid8Width");
			}
		}

		public SolidColorBrush TB9Grid9Color
		{
			get
			{
				return this.tB9Grid9Color;
			}
			set
			{
				this.tB9Grid9Color = value;
				this.RaisePropertyChanged("TB9Grid9Color");
			}
		}

		public double TB9Grid9Width
		{
			get
			{
				return this.tB9Grid9Width;
			}
			set
			{
				this.tB9Grid9Width = value;
				this.RaisePropertyChanged("TB9Grid9Width");
			}
		}

		public double TextBoxesHeight
		{
			get
			{
				return this.textBoxesHeight;
			}
			set
			{
				this.textBoxesHeight = value;
				this.RaisePropertyChanged("TextBoxesHeight");
			}
		}

		public double TextBoxesWidth
		{
			get
			{
				return this.textBoxesWidth;
			}
			set
			{
				this.textBoxesWidth = value;
				this.RaisePropertyChanged("TextBoxesWidth");
			}
		}

		public HolesUCPar()
		{
			this.holesUserControlWidth = 216;
			this.holesTextBoxBaseWidth = 143;
			this.holesTextBoxColorGridHeight = 6;
			this.holesTextBoxBaseHeight = 70 - this.holesTextBoxColorGridHeight;
		}

		private void RaisePropertyChanged(string prop)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}