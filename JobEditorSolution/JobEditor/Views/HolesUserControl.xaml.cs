using JobEditor.Common;
using JobEditor.Views.ProductData;
using Messages;
using Patterns.EventAggregator;
using ProductLib;
using CustomControlLibrary;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace JobEditor.Views
{
	public partial class HolesUserControl : System.Windows.Controls.UserControl, INotifyPropertyChanged
	{
		private ISubscription<Telegram> subscription;

		private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

		private int diff;

		public ShapeView shapeView = new ShapeView(null, null);

		private HolesUCPar holesUCPar = new HolesUCPar();

		private int showHolesIndex;

		private const int maxNumberHoles = 10;

		private const int maxShowNumberHolesAtOnce = 3;

		private const int maxShowNumberHolesIndex = 3;

		private int showNumberOfHolesIndex;

		public static DependencyProperty ShapeViewDPProperty;

		public static DependencyProperty UCParProperty;

		//internal CustomControlLibrary.ComboBox NumberOfHolesCB;

		//internal CustomControlLibrary.ComboBox MeasuringTypeCB;

		//internal CustomControlLibrary.ComboBox Shapehole1CB;

		//internal CustomControlLibrary.ComboBox Shapehole2symCB;

		//internal CustomControlLibrary.ComboBox Shapehole2CB;

		//internal CustomControlLibrary.ComboBox Shapehole3symCB;

		//internal CustomControlLibrary.ComboBox Shapehole3CB;

		//internal CustomControlLibrary.ComboBox Shapehole4symCB;

		//internal CustomControlLibrary.ComboBox Shapehole4CB;

		//internal CustomControlLibrary.ComboBox Shapehole5symCB;

		//internal CustomControlLibrary.ComboBox Shapehole5CB;

		//internal CustomControlLibrary.ComboBox Shapehole6symCB;

		//internal CustomControlLibrary.ComboBox Shapehole6CB;

		//internal CustomControlLibrary.ComboBox Shapehole7symCB;

		//internal CustomControlLibrary.ComboBox Shapehole7CB;

		//internal CustomControlLibrary.ComboBox Shapehole8symCB;

		//internal CustomControlLibrary.ComboBox Shapehole8CB;

		//internal CustomControlLibrary.ComboBox Shapehole9symCB;

		//internal CustomControlLibrary.ComboBox Shapehole9CB;

		//internal CustomControlLibrary.ComboBox Shapehole10symCB;

		//internal CustomControlLibrary.ComboBox Shapehole10CB;

		public ShapeInfoView ShapeViewDP
		{
			get
			{
				return (ShapeInfoView)base.GetValue(HolesUserControl.ShapeViewDPProperty);
			}
			set
			{
				base.SetValue(HolesUserControl.ShapeViewDPProperty, value);
			}
		}

		public HolesUCPar UCPar
		{
			get
			{
				return (HolesUCPar)base.GetValue(HolesUserControl.UCParProperty);
			}
			set
			{
				base.SetValue(HolesUserControl.UCParProperty, value);
			}
		}

		static HolesUserControl()
		{
			HolesUserControl.ShapeViewDPProperty = DependencyProperty.Register("ShapeViewDP", typeof(ShapeInfoView), typeof(HolesUserControl));
			HolesUserControl.UCParProperty = DependencyProperty.Register("UCPar", typeof(HolesUCPar), typeof(HolesUserControl));
		}

		public HolesUserControl()
		{
			this.InitializeComponent();
			this.subscription = this.aggregator.Subscribe<Telegram>(new Action<Telegram>(this.ReloadShapeView), "JobEditor.UserControl.Holes");
		}

		private void ComposeProperties(ref ShapeProperties shapeProperties)
		{
			shapeProperties.PolygonProperties.StrokeColor = ((SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush")).Color;
			shapeProperties.PolygonProperties.FillColor = ((SolidColorBrush)Application.Current.TryFindResource("ShapeFillColor")).Color;
			shapeProperties.PolygonProperties.ScallingFactor = 12;
			shapeProperties.PolygonProperties.StrokeTicknees = 2;
			shapeProperties.PolygonProperties.DistanceBetweenParts = 2;
			shapeProperties.StepLapProperties.Step = 5;
			shapeProperties.StepLapProperties.IsEnabled = false;
			shapeProperties.CenterProperties.AnnotationProperties.LineColor = ((SolidColorBrush)Application.Current.TryFindResource("AnotationLines")).Color;
			shapeProperties.HoleProperties.FillColor = ((SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush")).Color;
			shapeProperties.HoleProperties.StrokeThickness = 0;
			shapeProperties.HoleProperties.Diameter = 8;
			shapeProperties.HoleProperties.AnnotationProperties.IsEnabled = true;
			shapeProperties.HoleProperties.ArrowLineProperties.IsEnabled = true;
			shapeProperties.HoleProperties.LabelProperties.IsEnabled = true;
			shapeProperties.HoleProperties.AnnotationProperties.LineColor = Colors.Black;
			shapeProperties.HoleProperties.AnnotationProperties.StrokeThickness = 1;
			shapeProperties.SlotProperties.FillColor = ((SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush")).Color;
			shapeProperties.SlotProperties.StrokeThickness = 0;
			shapeProperties.SlotProperties.Height = 4;
			shapeProperties.SlotProperties.Width = 50;
			shapeProperties.SlotProperties.VerticalDistance = 10;
			shapeProperties.SlotProperties.AnnotationProperties.LineColor = ((SolidColorBrush)Application.Current.TryFindResource("AnotationLines")).Color;
		}

		private void Dispose(object sender, RoutedEventArgs e)
		{
			if (this.subscription != null)
			{
				this.aggregator.UnSubscribe<Telegram>(this.subscription);
			}
		}

		public void DoReloadShapeView()
		{
			if (this.HolesUserControlMainGrid.DataContext != null && this.HolesUserControlMainGrid.DataContext is ShapeView)
			{
				Shape shape = ((ShapeView)this.HolesUserControlMainGrid.DataContext).Shape;
				ShapeProperties shapeProperty = new ShapeProperties(shape);
				this.ComposeProperties(ref shapeProperty);
				this.ShapeViewDP = new ShapeInfoView(shape, shapeProperty);
				this.ShapeAndLengthVisuDef(this.shapeView.HolesView.NumberOfHoles, this.shapeView.HolesView.MeasuringType);
				this.NumberHolesVisuDef(this.shapeView.HolesView.NumberOfHoles);
			}
		}

		private void HolesMeasuringTypeChanged(object sender, SelectionChangedEventArgs e)
		{
			this.TextBoxesVisuDef(this.shapeView.HolesView.NumberOfHoles, this.shapeView.HolesView.MeasuringType, this.diff);
			this.ShapeAndLengthVisuDef(this.shapeView.HolesView.NumberOfHoles, this.shapeView.HolesView.MeasuringType);
			this.NumberHolesVisuDef(this.shapeView.HolesView.NumberOfHoles);
			this.RedrawShape((ShapeView)this.HolesUserControlMainGrid.DataContext);
		}

		private void LoadHolesUserControl(object sender, RoutedEventArgs e)
		{
			this.shapeView = (ShapeView)this.HolesUserControlMainGrid.DataContext;
			this.RedrawShape(this.shapeView);
			this.diff = this.shapeView.ShapePartViews.Count - 3;
			if (this.diff <= 0)
			{
				this.holesUCPar.MTComboBoxMargin = new Thickness(-1, -1, 0, 0);
			}
			else
			{
				this.holesUCPar.MTComboBoxMargin = new Thickness((double)(this.diff * 72 - 1), -1, 0, 0);
			}
			if (this.diff <= 0)
			{
				this.holesUCPar.HolesUserControlWidth = 216;
			}
			else
			{
				this.holesUCPar.HolesUserControlWidth = (double)(216 + this.diff * 72);
			}
			this.UCPar = this.holesUCPar;
			this.TextBoxesVisuDef(this.shapeView.HolesView.NumberOfHoles, this.shapeView.HolesView.MeasuringType, this.diff);
			this.ShapeAndLengthVisuDef(this.shapeView.HolesView.NumberOfHoles, this.shapeView.HolesView.MeasuringType);
			this.NumberHolesVisuDef(this.shapeView.HolesView.NumberOfHoles);
		}

		private void NumberHolesVisuDef(int NumberOfHoles)
		{
			if (this.showNumberOfHolesIndex > 3)
			{
				this.showNumberOfHolesIndex = 0;
			}
			List<int> nums = new List<int>();
			int num = 0;
			for (int i = 0; i <= 10; i++)
			{
				if (i != NumberOfHoles)
				{
					num++;
					if ((num - 1) / 3 == this.showNumberOfHolesIndex)
					{
						nums.Add(i);
					}
				}
			}
			for (int j = 0; j <= 10; j++)
			{
				switch (j)
				{
					case 0:
					{
						this.holesUCPar.NumberOfHoles0VisuEna = nums.Contains(j);
						break;
					}
					case 1:
					{
						this.holesUCPar.NumberOfHoles1VisuEna = nums.Contains(j);
						break;
					}
					case 2:
					{
						this.holesUCPar.NumberOfHoles2VisuEna = nums.Contains(j);
						break;
					}
					case 3:
					{
						this.holesUCPar.NumberOfHoles3VisuEna = nums.Contains(j);
						break;
					}
					case 4:
					{
						this.holesUCPar.NumberOfHoles4VisuEna = nums.Contains(j);
						break;
					}
					case 5:
					{
						this.holesUCPar.NumberOfHoles5VisuEna = nums.Contains(j);
						break;
					}
					case 6:
					{
						this.holesUCPar.NumberOfHoles6VisuEna = nums.Contains(j);
						break;
					}
					case 7:
					{
						this.holesUCPar.NumberOfHoles7VisuEna = nums.Contains(j);
						break;
					}
					case 8:
					{
						this.holesUCPar.NumberOfHoles8VisuEna = nums.Contains(j);
						break;
					}
					case 9:
					{
						this.holesUCPar.NumberOfHoles9VisuEna = nums.Contains(j);
						break;
					}
					case 10:
					{
						this.holesUCPar.NumberOfHoles10VisuEna = nums.Contains(j);
						break;
					}
				}
			}
			this.holesUCPar.NumberOfHolesEmptySpaceVisuEna = nums.Count < 3;
		}

		private void NumberOfHolesChanged(object sender, SelectionChangedEventArgs e)
		{
			System.Windows.Controls.ComboBox comboBox = sender as System.Windows.Controls.ComboBox;
			HolesView holesView = this.shapeView.HolesView;
			if (comboBox == null)
			{
				return;
			}
			int selectionBoxItem = 0;
			if (comboBox.SelectionBoxItem is int)
			{
				selectionBoxItem = (int)comboBox.SelectionBoxItem;
			}
			if (holesView.NumberOfHoles < 0)
			{
				holesView.NumberOfHoles = selectionBoxItem;
			}
			if (holesView.NumberOfHoles > 10)
			{
				holesView.NumberOfHoles = 10;
			}
			this.showNumberOfHolesIndex = 0;
			this.showHolesIndex = 0;
			this.TextBoxesVisuDef(holesView.NumberOfHoles, holesView.MeasuringType, this.diff);
			this.ShapeAndLengthVisuDef(holesView.NumberOfHoles, holesView.MeasuringType);
			this.NumberHolesVisuDef(holesView.NumberOfHoles);
			this.RedrawShape((ShapeView)this.HolesUserControlMainGrid.DataContext);
		}

		protected void onPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		private void RedrawShape(ShapeView shapeView)
		{
			Shape shape = shapeView.Shape;
			ShapeProperties shapeProperty = new ShapeProperties(shape);
			this.ComposeProperties(ref shapeProperty);
			this.ShapeViewDP = new ShapeInfoView(shape, shapeProperty);
		}

		public void ReloadShapeView(Telegram telegram)
		{
			Application.Current.Dispatcher.Invoke(new Action(() => this.DoReloadShapeView()), DispatcherPriority.Background, null);
		}

		private void ShapeAndLengthVisuDef(int numberOfHoles, EMeasuringType measuringType)
		{
			if (numberOfHoles <= 4 & this.showHolesIndex > 0)
			{
				this.showHolesIndex = 0;
			}
			if (numberOfHoles <= 6 & this.showHolesIndex > 1)
			{
				this.showHolesIndex = 0;
			}
			if (numberOfHoles <= 9 & this.showHolesIndex > 2)
			{
				this.showHolesIndex = 0;
			}
			if (numberOfHoles <= 12 & this.showHolesIndex > 3)
			{
				this.showHolesIndex = 0;
			}
			bool flag = false;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			bool flag13 = false;
			bool flag14 = false;
			bool flag15 = false;
			if (measuringType == EMeasuringType.Simetric)
			{
				flag = true;
				if (this.showHolesIndex == 0)
				{
					if (numberOfHoles >= 2)
					{
						flag1 = true;
					}
					if (numberOfHoles >= 3)
					{
						flag2 = true;
					}
					if (numberOfHoles == 4)
					{
						flag3 = true;
					}
					if (numberOfHoles <= 1)
					{
						flag14 = true;
					}
					if (numberOfHoles == 2)
					{
						flag12 = true;
					}
					if (numberOfHoles == 3)
					{
						flag11 = true;
					}
					if (numberOfHoles > 4)
					{
						flag15 = true;
					}
				}
				if (this.showHolesIndex == 1)
				{
					if (numberOfHoles >= 4)
					{
						flag3 = true;
					}
					if (numberOfHoles >= 5)
					{
						flag4 = true;
					}
					if (numberOfHoles >= 6)
					{
						flag5 = true;
					}
					if (numberOfHoles == 4)
					{
						flag12 = true;
					}
					if (numberOfHoles == 5)
					{
						flag11 = true;
					}
					flag15 = true;
				}
				if (this.showHolesIndex == 2)
				{
					if (numberOfHoles >= 7)
					{
						flag6 = true;
					}
					if (numberOfHoles >= 8)
					{
						flag7 = true;
					}
					if (numberOfHoles >= 9)
					{
						flag8 = true;
					}
					if (numberOfHoles == 7)
					{
						flag12 = true;
					}
					if (numberOfHoles == 8)
					{
						flag11 = true;
					}
					flag15 = true;
				}
				if (this.showHolesIndex == 3)
				{
					flag9 = true;
					flag12 = true;
					flag15 = true;
				}
			}
			else
			{
				if (this.showHolesIndex == 0)
				{
					if (numberOfHoles >= 1)
					{
						flag1 = true;
					}
					if (numberOfHoles >= 2)
					{
						flag2 = true;
					}
					if (numberOfHoles >= 3)
					{
						flag3 = true;
					}
					if (numberOfHoles == 4)
					{
						flag4 = true;
					}
					if (numberOfHoles == 0)
					{
						flag14 = true;
					}
					if (numberOfHoles == 1)
					{
						flag13 = true;
					}
					if (numberOfHoles == 2)
					{
						flag12 = true;
					}
					if (numberOfHoles == 3)
					{
						flag11 = true;
					}
					if (numberOfHoles > 4)
					{
						flag15 = true;
					}
				}
				if (this.showHolesIndex == 1)
				{
					if (numberOfHoles >= 4)
					{
						flag4 = true;
					}
					if (numberOfHoles >= 5)
					{
						flag5 = true;
					}
					if (numberOfHoles >= 6)
					{
						flag6 = true;
					}
					if (numberOfHoles == 4)
					{
						flag12 = true;
					}
					if (numberOfHoles == 5)
					{
						flag11 = true;
					}
					flag15 = true;
				}
				if (this.showHolesIndex == 2)
				{
					if (numberOfHoles >= 7)
					{
						flag7 = true;
					}
					if (numberOfHoles >= 8)
					{
						flag8 = true;
					}
					if (numberOfHoles >= 9)
					{
						flag9 = true;
					}
					if (numberOfHoles == 7)
					{
						flag12 = true;
					}
					if (numberOfHoles == 8)
					{
						flag11 = true;
					}
					flag15 = true;
				}
				if (this.showHolesIndex == 3)
				{
					flag10 = true;
					flag12 = true;
					flag15 = true;
				}
			}
			this.holesUCPar.ShapeAndLengthsSymVisuEna = flag;
			this.holesUCPar.ShapeAndLength1VisuEna = flag1;
			this.holesUCPar.ShapeAndLength2VisuEna = flag2;
			this.holesUCPar.ShapeAndLength3VisuEna = flag3;
			this.holesUCPar.ShapeAndLength4VisuEna = flag4;
			this.holesUCPar.ShapeAndLength5VisuEna = flag5;
			this.holesUCPar.ShapeAndLength6VisuEna = flag6;
			this.holesUCPar.ShapeAndLength7VisuEna = flag7;
			this.holesUCPar.ShapeAndLength8VisuEna = flag8;
			this.holesUCPar.ShapeAndLength9VisuEna = flag9;
			this.holesUCPar.ShapeAndLength10VisuEna = flag10;
			this.holesUCPar.ShapeAndLengthEmptySpace1RowVisuEna = flag11;
			this.holesUCPar.ShapeAndLengthEmptySpace2RowsVisuEna = flag12;
			this.holesUCPar.ShapeAndLengthEmptySpace3RowsVisuEna = flag13;
			this.holesUCPar.ShapeAndLengthEmptySpace4RowsVisuEna = flag14;
			this.holesUCPar.ShapeAndLengthShowNextHolesVisuEna = flag15;
		}

		private void ShowNextHoles(object sender, RoutedEventArgs e)
		{
			this.showHolesIndex++;
			this.ShapeAndLengthVisuDef(this.shapeView.HolesView.NumberOfHoles, this.shapeView.HolesView.MeasuringType);
		}

		private void ShowNextNumberHoles(object sender, RoutedEventArgs e)
		{
			this.showNumberOfHolesIndex++;
			this.NumberHolesVisuDef(this.shapeView.HolesView.NumberOfHoles);
		}

		private void TextBoxesVisuDef(int numberOfHoles, EMeasuringType measuringType, int diff)
		{
            this.holesUCPar.TextBoxesWidth = diff <= 0 ? this.holesUCPar.HolesTextBoxBaseWidth : this.holesUCPar.HolesTextBoxBaseWidth + (double)(diff * 72);
            this.holesUCPar.SimetricLineWidth = this.holesUCPar.TextBoxesWidth + 1.0;
            this.holesUCPar.TextBoxesHeight = this.holesUCPar.HolesTextBoxBaseHeight;
            if (measuringType == EMeasuringType.Simetric)
                this.holesUCPar.TextBoxesHeight = this.holesUCPar.HolesTextBoxBaseHeight * 2.0 + this.holesUCPar.HolesTextBoxColorGridHeight + 1.0;
            this.holesUCPar.RestGridEna = true;
            if (measuringType != EMeasuringType.Absolute)
                this.holesUCPar.RestGridEna = false;
            double textBoxBaseWidth = this.holesUCPar.HolesTextBoxBaseWidth;
            if (diff > 0)
                textBoxBaseWidth += (double)diff * 72.0;
            SolidColorBrush resource1 = (SolidColorBrush)Application.Current.TryFindResource((object)"ShapesTextBoxDistances1");
            SolidColorBrush resource2 = (SolidColorBrush)Application.Current.TryFindResource((object)"ShapesTextBoxDistances2");
            if (numberOfHoles == 1 && measuringType != EMeasuringType.Simetric)
            {
                this.holesUCPar.TB1Grid1Color = resource1;
                this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
            }
            if (numberOfHoles == 2)
            {
                switch (measuringType)
                {
                    case EMeasuringType.Absolute:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource1;
                        this.holesUCPar.TB2Grid2Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        break;
                    case EMeasuringType.Simetric:
                        this.holesUCPar.TB1Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        break;
                    default:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        break;
                }
            }
            if (numberOfHoles == 3)
            {
                switch (measuringType)
                {
                    case EMeasuringType.Absolute:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource1;
                        this.holesUCPar.TB2Grid2Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource1;
                        this.holesUCPar.TB3Grid2Color = resource2;
                        this.holesUCPar.TB3Grid3Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                        break;
                    case EMeasuringType.Simetric:
                        this.holesUCPar.TB1Grid1Color = resource2;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        break;
                    default:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        break;
                }
            }
            if (numberOfHoles == 4)
            {
                switch (measuringType)
                {
                    case EMeasuringType.Absolute:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource1;
                        this.holesUCPar.TB2Grid2Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource1;
                        this.holesUCPar.TB3Grid2Color = resource2;
                        this.holesUCPar.TB3Grid3Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource1;
                        this.holesUCPar.TB4Grid2Color = resource2;
                        this.holesUCPar.TB4Grid3Color = resource2;
                        this.holesUCPar.TB4Grid4Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB4Grid1Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid2Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid3Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid4Width = (textBoxBaseWidth - 3.0) / 4.0;
                        break;
                    case EMeasuringType.Simetric:
                        this.holesUCPar.TB1Grid1Color = resource2;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        break;
                    default:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        break;
                }
            }
            if (numberOfHoles == 5)
            {
                switch (measuringType)
                {
                    case EMeasuringType.Absolute:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource1;
                        this.holesUCPar.TB2Grid2Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource1;
                        this.holesUCPar.TB3Grid2Color = resource2;
                        this.holesUCPar.TB3Grid3Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource1;
                        this.holesUCPar.TB4Grid2Color = resource2;
                        this.holesUCPar.TB4Grid3Color = resource2;
                        this.holesUCPar.TB4Grid4Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource1;
                        this.holesUCPar.TB5Grid2Color = resource2;
                        this.holesUCPar.TB5Grid3Color = resource2;
                        this.holesUCPar.TB5Grid4Color = resource2;
                        this.holesUCPar.TB5Grid5Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB4Grid1Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid2Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid3Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid4Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB5Grid1Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid2Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid3Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid4Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid5Width = (textBoxBaseWidth - 4.0) / 5.0;
                        break;
                    case EMeasuringType.Simetric:
                        this.holesUCPar.TB1Grid1Color = resource2;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        break;
                    default:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                        break;
                }
            }
            if (numberOfHoles == 6)
            {
                switch (measuringType)
                {
                    case EMeasuringType.Absolute:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource1;
                        this.holesUCPar.TB2Grid2Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource1;
                        this.holesUCPar.TB3Grid2Color = resource2;
                        this.holesUCPar.TB3Grid3Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource1;
                        this.holesUCPar.TB4Grid2Color = resource2;
                        this.holesUCPar.TB4Grid3Color = resource2;
                        this.holesUCPar.TB4Grid4Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource1;
                        this.holesUCPar.TB5Grid2Color = resource2;
                        this.holesUCPar.TB5Grid3Color = resource2;
                        this.holesUCPar.TB5Grid4Color = resource2;
                        this.holesUCPar.TB5Grid5Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource1;
                        this.holesUCPar.TB6Grid2Color = resource2;
                        this.holesUCPar.TB6Grid3Color = resource2;
                        this.holesUCPar.TB6Grid4Color = resource2;
                        this.holesUCPar.TB6Grid5Color = resource2;
                        this.holesUCPar.TB6Grid6Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB4Grid1Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid2Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid3Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid4Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB5Grid1Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid2Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid3Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid4Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid5Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB6Grid1Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid2Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid3Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid4Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid5Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid6Width = (textBoxBaseWidth - 5.0) / 6.0;
                        break;
                    case EMeasuringType.Simetric:
                        this.holesUCPar.TB1Grid1Color = resource2;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                        break;
                    default:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB6Grid1Width = textBoxBaseWidth;
                        break;
                }
            }
            if (numberOfHoles == 7)
            {
                switch (measuringType)
                {
                    case EMeasuringType.Absolute:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource1;
                        this.holesUCPar.TB2Grid2Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource1;
                        this.holesUCPar.TB3Grid2Color = resource2;
                        this.holesUCPar.TB3Grid3Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource1;
                        this.holesUCPar.TB4Grid2Color = resource2;
                        this.holesUCPar.TB4Grid3Color = resource2;
                        this.holesUCPar.TB4Grid4Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource1;
                        this.holesUCPar.TB5Grid2Color = resource2;
                        this.holesUCPar.TB5Grid3Color = resource2;
                        this.holesUCPar.TB5Grid4Color = resource2;
                        this.holesUCPar.TB5Grid5Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource1;
                        this.holesUCPar.TB6Grid2Color = resource2;
                        this.holesUCPar.TB6Grid3Color = resource2;
                        this.holesUCPar.TB6Grid4Color = resource2;
                        this.holesUCPar.TB6Grid5Color = resource2;
                        this.holesUCPar.TB6Grid6Color = resource2;
                        this.holesUCPar.TB7Grid1Color = resource1;
                        this.holesUCPar.TB7Grid2Color = resource2;
                        this.holesUCPar.TB7Grid3Color = resource2;
                        this.holesUCPar.TB7Grid4Color = resource2;
                        this.holesUCPar.TB7Grid5Color = resource2;
                        this.holesUCPar.TB7Grid6Color = resource2;
                        this.holesUCPar.TB7Grid7Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB4Grid1Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid2Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid3Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid4Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB5Grid1Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid2Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid3Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid4Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid5Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB6Grid1Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid2Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid3Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid4Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid5Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid6Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB7Grid1Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid2Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid3Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid4Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid5Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid6Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid7Width = (textBoxBaseWidth - 6.0) / 7.0;
                        break;
                    case EMeasuringType.Simetric:
                        this.holesUCPar.TB1Grid1Color = resource2;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB6Grid1Width = textBoxBaseWidth;
                        break;
                    default:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource2;
                        this.holesUCPar.TB7Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB6Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB7Grid1Width = textBoxBaseWidth;
                        break;
                }
            }
            if (numberOfHoles == 8)
            {
                switch (measuringType)
                {
                    case EMeasuringType.Absolute:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource1;
                        this.holesUCPar.TB2Grid2Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource1;
                        this.holesUCPar.TB3Grid2Color = resource2;
                        this.holesUCPar.TB3Grid3Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource1;
                        this.holesUCPar.TB4Grid2Color = resource2;
                        this.holesUCPar.TB4Grid3Color = resource2;
                        this.holesUCPar.TB4Grid4Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource1;
                        this.holesUCPar.TB5Grid2Color = resource2;
                        this.holesUCPar.TB5Grid3Color = resource2;
                        this.holesUCPar.TB5Grid4Color = resource2;
                        this.holesUCPar.TB5Grid5Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource1;
                        this.holesUCPar.TB6Grid2Color = resource2;
                        this.holesUCPar.TB6Grid3Color = resource2;
                        this.holesUCPar.TB6Grid4Color = resource2;
                        this.holesUCPar.TB6Grid5Color = resource2;
                        this.holesUCPar.TB6Grid6Color = resource2;
                        this.holesUCPar.TB7Grid1Color = resource1;
                        this.holesUCPar.TB7Grid2Color = resource2;
                        this.holesUCPar.TB7Grid3Color = resource2;
                        this.holesUCPar.TB7Grid4Color = resource2;
                        this.holesUCPar.TB7Grid5Color = resource2;
                        this.holesUCPar.TB7Grid6Color = resource2;
                        this.holesUCPar.TB7Grid7Color = resource2;
                        this.holesUCPar.TB8Grid1Color = resource1;
                        this.holesUCPar.TB8Grid2Color = resource2;
                        this.holesUCPar.TB8Grid3Color = resource2;
                        this.holesUCPar.TB8Grid4Color = resource2;
                        this.holesUCPar.TB8Grid5Color = resource2;
                        this.holesUCPar.TB8Grid6Color = resource2;
                        this.holesUCPar.TB8Grid7Color = resource2;
                        this.holesUCPar.TB8Grid8Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB4Grid1Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid2Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid3Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid4Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB5Grid1Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid2Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid3Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid4Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid5Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB6Grid1Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid2Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid3Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid4Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid5Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid6Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB7Grid1Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid2Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid3Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid4Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid5Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid6Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid7Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB8Grid1Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid2Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid3Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid4Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid5Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid6Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid7Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid8Width = (textBoxBaseWidth - 7.0) / 8.0;
                        break;
                    case EMeasuringType.Simetric:
                        this.holesUCPar.TB1Grid1Color = resource2;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource2;
                        this.holesUCPar.TB7Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB6Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB7Grid1Width = textBoxBaseWidth;
                        break;
                    default:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource2;
                        this.holesUCPar.TB7Grid1Color = resource2;
                        this.holesUCPar.TB8Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB6Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB7Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB8Grid1Width = textBoxBaseWidth;
                        break;
                }
            }
            if (numberOfHoles == 9)
            {
                switch (measuringType)
                {
                    case EMeasuringType.Absolute:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource1;
                        this.holesUCPar.TB2Grid2Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource1;
                        this.holesUCPar.TB3Grid2Color = resource2;
                        this.holesUCPar.TB3Grid3Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource1;
                        this.holesUCPar.TB4Grid2Color = resource2;
                        this.holesUCPar.TB4Grid3Color = resource2;
                        this.holesUCPar.TB4Grid4Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource1;
                        this.holesUCPar.TB5Grid2Color = resource2;
                        this.holesUCPar.TB5Grid3Color = resource2;
                        this.holesUCPar.TB5Grid4Color = resource2;
                        this.holesUCPar.TB5Grid5Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource1;
                        this.holesUCPar.TB6Grid2Color = resource2;
                        this.holesUCPar.TB6Grid3Color = resource2;
                        this.holesUCPar.TB6Grid4Color = resource2;
                        this.holesUCPar.TB6Grid5Color = resource2;
                        this.holesUCPar.TB6Grid6Color = resource2;
                        this.holesUCPar.TB7Grid1Color = resource1;
                        this.holesUCPar.TB7Grid2Color = resource2;
                        this.holesUCPar.TB7Grid3Color = resource2;
                        this.holesUCPar.TB7Grid4Color = resource2;
                        this.holesUCPar.TB7Grid5Color = resource2;
                        this.holesUCPar.TB7Grid6Color = resource2;
                        this.holesUCPar.TB7Grid7Color = resource2;
                        this.holesUCPar.TB8Grid1Color = resource1;
                        this.holesUCPar.TB8Grid2Color = resource2;
                        this.holesUCPar.TB8Grid3Color = resource2;
                        this.holesUCPar.TB8Grid4Color = resource2;
                        this.holesUCPar.TB8Grid5Color = resource2;
                        this.holesUCPar.TB8Grid6Color = resource2;
                        this.holesUCPar.TB8Grid7Color = resource2;
                        this.holesUCPar.TB8Grid8Color = resource2;
                        this.holesUCPar.TB9Grid1Color = resource1;
                        this.holesUCPar.TB9Grid2Color = resource2;
                        this.holesUCPar.TB9Grid3Color = resource2;
                        this.holesUCPar.TB9Grid4Color = resource2;
                        this.holesUCPar.TB9Grid5Color = resource2;
                        this.holesUCPar.TB9Grid6Color = resource2;
                        this.holesUCPar.TB9Grid7Color = resource2;
                        this.holesUCPar.TB9Grid8Color = resource2;
                        this.holesUCPar.TB9Grid9Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.holesUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.holesUCPar.TB4Grid1Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid2Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid3Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB4Grid4Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.holesUCPar.TB5Grid1Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid2Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid3Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid4Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB5Grid5Width = (textBoxBaseWidth - 4.0) / 5.0;
                        this.holesUCPar.TB6Grid1Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid2Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid3Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid4Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid5Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB6Grid6Width = (textBoxBaseWidth - 5.0) / 6.0;
                        this.holesUCPar.TB7Grid1Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid2Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid3Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid4Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid5Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid6Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB7Grid7Width = (textBoxBaseWidth - 6.0) / 7.0;
                        this.holesUCPar.TB8Grid1Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid2Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid3Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid4Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid5Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid6Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid7Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB8Grid8Width = (textBoxBaseWidth - 7.0) / 8.0;
                        this.holesUCPar.TB9Grid1Width = (textBoxBaseWidth - 8.0) / 9.0;
                        this.holesUCPar.TB9Grid2Width = (textBoxBaseWidth - 8.0) / 9.0;
                        this.holesUCPar.TB9Grid3Width = (textBoxBaseWidth - 8.0) / 9.0;
                        this.holesUCPar.TB9Grid4Width = (textBoxBaseWidth - 8.0) / 9.0;
                        this.holesUCPar.TB9Grid5Width = (textBoxBaseWidth - 8.0) / 9.0;
                        this.holesUCPar.TB9Grid6Width = (textBoxBaseWidth - 8.0) / 9.0;
                        this.holesUCPar.TB9Grid7Width = (textBoxBaseWidth - 8.0) / 9.0;
                        this.holesUCPar.TB9Grid8Width = (textBoxBaseWidth - 8.0) / 9.0;
                        this.holesUCPar.TB9Grid9Width = (textBoxBaseWidth - 8.0) / 9.0;
                        break;
                    case EMeasuringType.Simetric:
                        this.holesUCPar.TB1Grid1Color = resource2;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource2;
                        this.holesUCPar.TB7Grid1Color = resource2;
                        this.holesUCPar.TB8Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB6Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB7Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB8Grid1Width = textBoxBaseWidth;
                        break;
                    default:
                        this.holesUCPar.TB1Grid1Color = resource1;
                        this.holesUCPar.TB2Grid1Color = resource2;
                        this.holesUCPar.TB3Grid1Color = resource2;
                        this.holesUCPar.TB4Grid1Color = resource2;
                        this.holesUCPar.TB5Grid1Color = resource2;
                        this.holesUCPar.TB6Grid1Color = resource2;
                        this.holesUCPar.TB7Grid1Color = resource2;
                        this.holesUCPar.TB8Grid1Color = resource2;
                        this.holesUCPar.TB9Grid1Color = resource2;
                        this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB6Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB7Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB8Grid1Width = textBoxBaseWidth;
                        this.holesUCPar.TB9Grid1Width = textBoxBaseWidth;
                        break;
                }
            }
            if (numberOfHoles != 10)
                return;
            if (measuringType != EMeasuringType.Simetric)
            {
                if (measuringType == EMeasuringType.Absolute)
                {
                    this.holesUCPar.TB1Grid1Color = resource1;
                    this.holesUCPar.TB2Grid1Color = resource1;
                    this.holesUCPar.TB2Grid2Color = resource2;
                    this.holesUCPar.TB3Grid1Color = resource1;
                    this.holesUCPar.TB3Grid2Color = resource2;
                    this.holesUCPar.TB3Grid3Color = resource2;
                    this.holesUCPar.TB4Grid1Color = resource1;
                    this.holesUCPar.TB4Grid2Color = resource2;
                    this.holesUCPar.TB4Grid3Color = resource2;
                    this.holesUCPar.TB4Grid4Color = resource2;
                    this.holesUCPar.TB5Grid1Color = resource1;
                    this.holesUCPar.TB5Grid2Color = resource2;
                    this.holesUCPar.TB5Grid3Color = resource2;
                    this.holesUCPar.TB5Grid4Color = resource2;
                    this.holesUCPar.TB5Grid5Color = resource2;
                    this.holesUCPar.TB6Grid1Color = resource1;
                    this.holesUCPar.TB6Grid2Color = resource2;
                    this.holesUCPar.TB6Grid3Color = resource2;
                    this.holesUCPar.TB6Grid4Color = resource2;
                    this.holesUCPar.TB6Grid5Color = resource2;
                    this.holesUCPar.TB6Grid6Color = resource2;
                    this.holesUCPar.TB7Grid1Color = resource1;
                    this.holesUCPar.TB7Grid2Color = resource2;
                    this.holesUCPar.TB7Grid3Color = resource2;
                    this.holesUCPar.TB7Grid4Color = resource2;
                    this.holesUCPar.TB7Grid5Color = resource2;
                    this.holesUCPar.TB7Grid6Color = resource2;
                    this.holesUCPar.TB7Grid7Color = resource2;
                    this.holesUCPar.TB8Grid1Color = resource1;
                    this.holesUCPar.TB8Grid2Color = resource2;
                    this.holesUCPar.TB8Grid3Color = resource2;
                    this.holesUCPar.TB8Grid4Color = resource2;
                    this.holesUCPar.TB8Grid5Color = resource2;
                    this.holesUCPar.TB8Grid6Color = resource2;
                    this.holesUCPar.TB8Grid7Color = resource2;
                    this.holesUCPar.TB8Grid8Color = resource2;
                    this.holesUCPar.TB9Grid1Color = resource1;
                    this.holesUCPar.TB9Grid2Color = resource2;
                    this.holesUCPar.TB9Grid3Color = resource2;
                    this.holesUCPar.TB9Grid4Color = resource2;
                    this.holesUCPar.TB9Grid5Color = resource2;
                    this.holesUCPar.TB9Grid6Color = resource2;
                    this.holesUCPar.TB9Grid7Color = resource2;
                    this.holesUCPar.TB9Grid8Color = resource2;
                    this.holesUCPar.TB9Grid9Color = resource2;
                    this.holesUCPar.TB10Grid1Color = resource1;
                    this.holesUCPar.TB10Grid2Color = resource2;
                    this.holesUCPar.TB10Grid3Color = resource2;
                    this.holesUCPar.TB10Grid4Color = resource2;
                    this.holesUCPar.TB10Grid5Color = resource2;
                    this.holesUCPar.TB10Grid6Color = resource2;
                    this.holesUCPar.TB10Grid7Color = resource2;
                    this.holesUCPar.TB10Grid8Color = resource2;
                    this.holesUCPar.TB10Grid9Color = resource2;
                    this.holesUCPar.TB10Grid10Color = resource2;
                    this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                    this.holesUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                    this.holesUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                    this.holesUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                    this.holesUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                    this.holesUCPar.TB4Grid1Width = (textBoxBaseWidth - 3.0) / 4.0;
                    this.holesUCPar.TB4Grid2Width = (textBoxBaseWidth - 3.0) / 4.0;
                    this.holesUCPar.TB4Grid3Width = (textBoxBaseWidth - 3.0) / 4.0;
                    this.holesUCPar.TB4Grid4Width = (textBoxBaseWidth - 3.0) / 4.0;
                    this.holesUCPar.TB5Grid1Width = (textBoxBaseWidth - 4.0) / 5.0;
                    this.holesUCPar.TB5Grid2Width = (textBoxBaseWidth - 4.0) / 5.0;
                    this.holesUCPar.TB5Grid3Width = (textBoxBaseWidth - 4.0) / 5.0;
                    this.holesUCPar.TB5Grid4Width = (textBoxBaseWidth - 4.0) / 5.0;
                    this.holesUCPar.TB5Grid5Width = (textBoxBaseWidth - 4.0) / 5.0;
                    this.holesUCPar.TB6Grid1Width = (textBoxBaseWidth - 5.0) / 6.0;
                    this.holesUCPar.TB6Grid2Width = (textBoxBaseWidth - 5.0) / 6.0;
                    this.holesUCPar.TB6Grid3Width = (textBoxBaseWidth - 5.0) / 6.0;
                    this.holesUCPar.TB6Grid4Width = (textBoxBaseWidth - 5.0) / 6.0;
                    this.holesUCPar.TB6Grid5Width = (textBoxBaseWidth - 5.0) / 6.0;
                    this.holesUCPar.TB6Grid6Width = (textBoxBaseWidth - 5.0) / 6.0;
                    this.holesUCPar.TB7Grid1Width = (textBoxBaseWidth - 6.0) / 7.0;
                    this.holesUCPar.TB7Grid2Width = (textBoxBaseWidth - 6.0) / 7.0;
                    this.holesUCPar.TB7Grid3Width = (textBoxBaseWidth - 6.0) / 7.0;
                    this.holesUCPar.TB7Grid4Width = (textBoxBaseWidth - 6.0) / 7.0;
                    this.holesUCPar.TB7Grid5Width = (textBoxBaseWidth - 6.0) / 7.0;
                    this.holesUCPar.TB7Grid6Width = (textBoxBaseWidth - 6.0) / 7.0;
                    this.holesUCPar.TB7Grid7Width = (textBoxBaseWidth - 6.0) / 7.0;
                    this.holesUCPar.TB8Grid1Width = (textBoxBaseWidth - 7.0) / 8.0;
                    this.holesUCPar.TB8Grid2Width = (textBoxBaseWidth - 7.0) / 8.0;
                    this.holesUCPar.TB8Grid3Width = (textBoxBaseWidth - 7.0) / 8.0;
                    this.holesUCPar.TB8Grid4Width = (textBoxBaseWidth - 7.0) / 8.0;
                    this.holesUCPar.TB8Grid5Width = (textBoxBaseWidth - 7.0) / 8.0;
                    this.holesUCPar.TB8Grid6Width = (textBoxBaseWidth - 7.0) / 8.0;
                    this.holesUCPar.TB8Grid7Width = (textBoxBaseWidth - 7.0) / 8.0;
                    this.holesUCPar.TB8Grid8Width = (textBoxBaseWidth - 7.0) / 8.0;
                    this.holesUCPar.TB9Grid1Width = (textBoxBaseWidth - 8.0) / 9.0;
                    this.holesUCPar.TB9Grid2Width = (textBoxBaseWidth - 8.0) / 9.0;
                    this.holesUCPar.TB9Grid3Width = (textBoxBaseWidth - 8.0) / 9.0;
                    this.holesUCPar.TB9Grid4Width = (textBoxBaseWidth - 8.0) / 9.0;
                    this.holesUCPar.TB9Grid5Width = (textBoxBaseWidth - 8.0) / 9.0;
                    this.holesUCPar.TB9Grid6Width = (textBoxBaseWidth - 8.0) / 9.0;
                    this.holesUCPar.TB9Grid7Width = (textBoxBaseWidth - 8.0) / 9.0;
                    this.holesUCPar.TB9Grid8Width = (textBoxBaseWidth - 8.0) / 9.0;
                    this.holesUCPar.TB9Grid9Width = (textBoxBaseWidth - 8.0) / 9.0;
                    this.holesUCPar.TB10Grid1Width = (textBoxBaseWidth - 9.0) / 10.0;
                    this.holesUCPar.TB10Grid2Width = (textBoxBaseWidth - 9.0) / 10.0;
                    this.holesUCPar.TB10Grid3Width = (textBoxBaseWidth - 9.0) / 10.0;
                    this.holesUCPar.TB10Grid4Width = (textBoxBaseWidth - 9.0) / 10.0;
                    this.holesUCPar.TB10Grid5Width = (textBoxBaseWidth - 9.0) / 10.0;
                    this.holesUCPar.TB10Grid6Width = (textBoxBaseWidth - 9.0) / 10.0;
                    this.holesUCPar.TB10Grid7Width = (textBoxBaseWidth - 9.0) / 10.0;
                    this.holesUCPar.TB10Grid8Width = (textBoxBaseWidth - 9.0) / 10.0;
                    this.holesUCPar.TB10Grid9Width = (textBoxBaseWidth - 9.0) / 10.0;
                    this.holesUCPar.TB10Grid10Width = (textBoxBaseWidth - 9.0) / 10.0;
                }
                else
                {
                    this.holesUCPar.TB1Grid1Color = resource1;
                    this.holesUCPar.TB2Grid1Color = resource2;
                    this.holesUCPar.TB3Grid1Color = resource2;
                    this.holesUCPar.TB4Grid1Color = resource2;
                    this.holesUCPar.TB5Grid1Color = resource2;
                    this.holesUCPar.TB6Grid1Color = resource2;
                    this.holesUCPar.TB7Grid1Color = resource2;
                    this.holesUCPar.TB8Grid1Color = resource2;
                    this.holesUCPar.TB9Grid1Color = resource2;
                    this.holesUCPar.TB10Grid1Color = resource2;
                    this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB6Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB7Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB8Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB9Grid1Width = textBoxBaseWidth;
                    this.holesUCPar.TB10Grid1Width = textBoxBaseWidth;
                }
            }
            else
            {
                this.holesUCPar.TB1Grid1Color = resource2;
                this.holesUCPar.TB2Grid1Color = resource2;
                this.holesUCPar.TB3Grid1Color = resource2;
                this.holesUCPar.TB4Grid1Color = resource2;
                this.holesUCPar.TB5Grid1Color = resource2;
                this.holesUCPar.TB6Grid1Color = resource2;
                this.holesUCPar.TB7Grid1Color = resource2;
                this.holesUCPar.TB8Grid1Color = resource2;
                this.holesUCPar.TB9Grid1Color = resource2;
                this.holesUCPar.TB1Grid1Width = textBoxBaseWidth;
                this.holesUCPar.TB2Grid1Width = textBoxBaseWidth;
                this.holesUCPar.TB3Grid1Width = textBoxBaseWidth;
                this.holesUCPar.TB4Grid1Width = textBoxBaseWidth;
                this.holesUCPar.TB5Grid1Width = textBoxBaseWidth;
                this.holesUCPar.TB6Grid1Width = textBoxBaseWidth;
                this.holesUCPar.TB7Grid1Width = textBoxBaseWidth;
                this.holesUCPar.TB8Grid1Width = textBoxBaseWidth;
                this.holesUCPar.TB9Grid1Width = textBoxBaseWidth;
            }
        }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}