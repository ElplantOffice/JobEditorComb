using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace JobEditor.Common
{
	public class SlotsUCPar : INotifyPropertyChanged
	{
		private double slotsUserControlWidth;

		private bool shapeSlot1VisuEna;

		private bool shapeSlot2VisuEna;

		private bool shapeSlot3VisuEna;

		private bool shapeSlot4VisuEna;

		private bool textBox1VisuEna;

		private bool textBox2VisuEna;

		private bool textBox3VisuEna;

		private bool textBox4VisuEna;

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

		private bool restGridEna2;

		private bool restGridEna3;

		private bool restGridEna4;

		private bool tB2Grid2Ena;

		private Thickness mTComboBoxMargin;

		private bool simetricLineEna;

		private double simetricLineWidth;

		private double slotsTextBoxBaseWidth;

		private double slotsTextBoxColorGridHeight;

		private double slotsTextBoxBaseHeight;

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

		public bool RestGridEna2
		{
			get
			{
				return this.restGridEna2;
			}
			set
			{
				this.restGridEna2 = value;
				this.RaisePropertyChanged("RestGridEna2");
			}
		}

		public bool RestGridEna3
		{
			get
			{
				return this.restGridEna3;
			}
			set
			{
				this.restGridEna3 = value;
				this.RaisePropertyChanged("RestGridEna3");
			}
		}

		public bool RestGridEna4
		{
			get
			{
				return this.restGridEna4;
			}
			set
			{
				this.restGridEna4 = value;
				this.RaisePropertyChanged("RestGridEna4");
			}
		}

		public bool ShapeSlot1VisuEna
		{
			get
			{
				return this.shapeSlot1VisuEna;
			}
			set
			{
				this.shapeSlot1VisuEna = value;
				this.RaisePropertyChanged("ShapeSlot1VisuEna");
			}
		}

		public bool ShapeSlot2VisuEna
		{
			get
			{
				return this.shapeSlot2VisuEna;
			}
			set
			{
				this.shapeSlot2VisuEna = value;
				this.RaisePropertyChanged("ShapeSlot2VisuEna");
			}
		}

		public bool ShapeSlot3VisuEna
		{
			get
			{
				return this.shapeSlot3VisuEna;
			}
			set
			{
				this.shapeSlot3VisuEna = value;
				this.RaisePropertyChanged("ShapeSlot3VisuEna");
			}
		}

		public bool ShapeSlot4VisuEna
		{
			get
			{
				return this.shapeSlot4VisuEna;
			}
			set
			{
				this.shapeSlot4VisuEna = value;
				this.RaisePropertyChanged("ShapeSlot4VisuEna");
			}
		}

		public bool SimetricLineEna
		{
			get
			{
				return this.simetricLineEna;
			}
			set
			{
				this.simetricLineEna = value;
				this.RaisePropertyChanged("SimetricLineEna");
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

		public double SlotsTextBoxBaseHeight
		{
			get
			{
				return this.slotsTextBoxBaseHeight;
			}
			set
			{
				this.slotsTextBoxBaseHeight = value;
				this.RaisePropertyChanged("SlotsTextBoxBaseHeight");
			}
		}

		public double SlotsTextBoxBaseWidth
		{
			get
			{
				return this.slotsTextBoxBaseWidth;
			}
			set
			{
				this.slotsTextBoxBaseWidth = value;
				this.RaisePropertyChanged("SlotsTextBoxBaseWidth");
			}
		}

		public double SlotsTextBoxColorGridHeight
		{
			get
			{
				return this.slotsTextBoxColorGridHeight;
			}
			set
			{
				this.slotsTextBoxColorGridHeight = value;
				this.RaisePropertyChanged("SlotsTextBoxColorGridHeight");
			}
		}

		public double SlotsUserControlWidth
		{
			get
			{
				return this.slotsUserControlWidth;
			}
			set
			{
				this.slotsUserControlWidth = value;
				this.RaisePropertyChanged("SlotsUserControlWidth");
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

		public bool TB2Grid2Ena
		{
			get
			{
				return this.tB2Grid2Ena;
			}
			set
			{
				this.tB2Grid2Ena = value;
				this.RaisePropertyChanged("TB2Grid2Ena");
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

		public bool TextBox1VisuEna
		{
			get
			{
				return this.textBox1VisuEna;
			}
			set
			{
				this.textBox1VisuEna = value;
				this.RaisePropertyChanged("TextBox1VisuEna");
			}
		}

		public bool TextBox2VisuEna
		{
			get
			{
				return this.textBox2VisuEna;
			}
			set
			{
				this.textBox2VisuEna = value;
				this.RaisePropertyChanged("TextBox2VisuEna");
			}
		}

		public bool TextBox3VisuEna
		{
			get
			{
				return this.textBox3VisuEna;
			}
			set
			{
				this.textBox3VisuEna = value;
				this.RaisePropertyChanged("TextBox3VisuEna");
			}
		}

		public bool TextBox4VisuEna
		{
			get
			{
				return this.textBox4VisuEna;
			}
			set
			{
				this.textBox4VisuEna = value;
				this.RaisePropertyChanged("TextBox4VisuEna");
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

		public SlotsUCPar()
		{
			this.slotsUserControlWidth = 216;
			this.slotsTextBoxBaseWidth = 214;
			this.slotsTextBoxColorGridHeight = 6;
			this.slotsTextBoxBaseHeight = 70 - this.slotsTextBoxColorGridHeight;
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