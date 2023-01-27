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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace JobEditor.Views
{
	public partial class CentersUserControl : System.Windows.Controls.UserControl
	{
		private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

		private int diff;

		public ShapeView shapeView = new ShapeView(null, null);

		private CentersUCPar centersUCPar = new CentersUCPar();

		private string[] drawingParts;

		private ISubscription<Telegram> subscription;

		public static DependencyProperty ShapeViewDPProperty;

		public static DependencyProperty UCParProperty;

		//internal CustomControlLibrary.ComboBox MeasuringTypeCB;

		public ShapeInfoView ShapeViewDP
		{
			get
			{
				return (ShapeInfoView)base.GetValue(CentersUserControl.ShapeViewDPProperty);
			}
			set
			{
				base.SetValue(CentersUserControl.ShapeViewDPProperty, value);
			}
		}

		public CentersUCPar UCPar
		{
			get
			{
				return (CentersUCPar)base.GetValue(CentersUserControl.UCParProperty);
			}
			set
			{
				base.SetValue(CentersUserControl.UCParProperty, value);
			}
		}

		static CentersUserControl()
		{
			CentersUserControl.ShapeViewDPProperty = DependencyProperty.Register("ShapeViewDP", typeof(ShapeInfoView), typeof(CentersUserControl));
			CentersUserControl.UCParProperty = DependencyProperty.Register("UCPar", typeof(CentersUCPar), typeof(CentersUserControl));
		}

		public CentersUserControl()
		{
			this.InitializeComponent();
			this.subscription = this.aggregator.Subscribe<Telegram>(new Action<Telegram>(this.ReloadShapeView), "JobEditor.UserControl.Centers");
		}

		private void CentersMeasuringTypeChanged(object sender, SelectionChangedEventArgs e)
		{
			this.TextBoxesVisuDef(this.shapeView.ShapePartViews.Count, this.shapeView.CentersView.MeasuringType, this.drawingParts);
			this.RedrawShape((ShapeView)this.CentersUserControlMainGrid.DataContext);
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
			shapeProperties.TipCutProperties.IsEnabled = true;
			shapeProperties.HoleProperties.FillColor = ((SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush")).Color;
			shapeProperties.HoleProperties.StrokeThickness = 0;
			shapeProperties.HoleProperties.Diameter = 8;
			shapeProperties.HoleProperties.AnnotationProperties.LineColor = ((SolidColorBrush)Application.Current.TryFindResource("AnotationLines")).Color;
			shapeProperties.SlotProperties.FillColor = ((SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush")).Color;
			shapeProperties.SlotProperties.StrokeThickness = 0;
			shapeProperties.SlotProperties.Height = 4;
			shapeProperties.SlotProperties.Width = 50;
			shapeProperties.SlotProperties.VerticalDistance = 10;
			shapeProperties.SlotProperties.AnnotationProperties.LineColor = ((SolidColorBrush)Application.Current.TryFindResource("AnotationLines")).Color;
			shapeProperties.CenterProperties.AnnotationProperties.IsEnabled = true;
			shapeProperties.CenterProperties.ArrowLineProperties.IsEnabled = true;
			shapeProperties.CenterProperties.AnnotationProperties.LineColor = Colors.Black;
			shapeProperties.CenterProperties.AnnotationProperties.StrokeThickness = 1;
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
			if (this.CentersUserControlMainGrid.DataContext != null && this.CentersUserControlMainGrid.DataContext is ShapeView)
			{
				Shape shape = ((ShapeView)this.CentersUserControlMainGrid.DataContext).Shape;
				ShapeProperties shapeProperty = new ShapeProperties(shape);
				this.ComposeProperties(ref shapeProperty);
				this.ShapeViewDP = new ShapeInfoView(shape, shapeProperty);
			}
		}

		private void LoadCentersUserControl(object sender, RoutedEventArgs e)
		{
			this.shapeView = (ShapeView)this.CentersUserControlMainGrid.DataContext;
			this.RedrawShape(this.shapeView);
			this.diff = this.shapeView.ShapePartViews.Count - 3;
			if (this.diff <= 0)
			{
				this.centersUCPar.MTComboBoxMargin = new Thickness(107, -1, 0, 0);
			}
			else
			{
				this.centersUCPar.MTComboBoxMargin = new Thickness((double)(108 + this.diff * 72 - 1), -1, 0, 0);
			}
			if (this.diff <= 0)
			{
				this.centersUCPar.CentersUserControlWidth = 216;
			}
			else
			{
				this.centersUCPar.CentersUserControlWidth = (double)(216 + this.diff * 72);
			}
			this.drawingParts = this.shapeView.Drawing.Split(new char[] { '|' });
			this.UCPar = this.centersUCPar;
			this.TextBoxesVisuDef(this.shapeView.ShapePartViews.Count, this.shapeView.CentersView.MeasuringType, this.drawingParts);
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

		private void TextBoxesVisuDef(int shapePartsNumber, EMeasuringType measuringType, string[] drawingParts)
		{
            int num = shapePartsNumber - 3;
            double textBoxBaseWidth = this.centersUCPar.CentersTextBoxBaseWidth;
            if (num > 0)
                textBoxBaseWidth += (double)num * 72.0;
            SolidColorBrush resource1 = (SolidColorBrush)Application.Current.TryFindResource((object)"ShapesTextBoxDistances1");
            SolidColorBrush resource2 = (SolidColorBrush)Application.Current.TryFindResource((object)"ShapesTextBoxDistances2");
            SolidColorBrush resource3 = (SolidColorBrush)Application.Current.TryFindResource((object)"ShapesTextBoxDistances3");
            SolidColorBrush resource4 = (SolidColorBrush)Application.Current.TryFindResource((object)"ShapesTextBoxDistances4");
            this.centersUCPar.TextBoxesWidth = textBoxBaseWidth;
            this.centersUCPar.SimetricLineWidth = this.centersUCPar.TextBoxesWidth + 1.0;
            this.centersUCPar.TextBoxesHeight = this.centersUCPar.CentersTextBoxBaseHeight;
            this.centersUCPar.RestGridEna2 = true;
            this.centersUCPar.RestGridEna3 = true;
            this.centersUCPar.RestGridEna4 = true;
            if (measuringType == EMeasuringType.Relative)
            {
                this.centersUCPar.RestGridEna2 = false;
                this.centersUCPar.RestGridEna3 = false;
                this.centersUCPar.RestGridEna4 = false;
            }
            this.centersUCPar.TB2Grid2Ena = this.centersUCPar.RestGridEna2;
            if (measuringType == EMeasuringType.Simetric && shapePartsNumber >= 5)
                this.centersUCPar.TB2Grid2Ena = false;
            if (shapePartsNumber == 2)
            {
                this.centersUCPar.TextBox1VisuEna = true;
                this.centersUCPar.TextBox2VisuEna = false;
                this.centersUCPar.TextBox3VisuEna = false;
                this.centersUCPar.TextBox4VisuEna = false;
                this.centersUCPar.TB1Grid1Color = resource1;
                this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
            }
            if (shapePartsNumber == 3)
            {
                if (measuringType != EMeasuringType.Simetric)
                {
                    this.centersUCPar.TextBox1VisuEna = true;
                    this.centersUCPar.TextBox2VisuEna = true;
                    this.centersUCPar.TextBox3VisuEna = false;
                    this.centersUCPar.TextBox4VisuEna = false;
                    if (measuringType == EMeasuringType.Absolute)
                    {
                        this.centersUCPar.TB1Grid1Color = resource1;
                        this.centersUCPar.TB2Grid1Color = resource1;
                        this.centersUCPar.TB2Grid2Color = resource2;
                        this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.centersUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.centersUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                    }
                    else
                    {
                        this.centersUCPar.TB1Grid1Color = resource1;
                        this.centersUCPar.TB2Grid1Color = resource2;
                        this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.centersUCPar.TB2Grid1Width = textBoxBaseWidth;
                    }
                }
                else
                {
                    this.centersUCPar.TextBox1VisuEna = true;
                    this.centersUCPar.TextBox2VisuEna = false;
                    this.centersUCPar.TextBox3VisuEna = false;
                    this.centersUCPar.TextBox4VisuEna = false;
                    this.centersUCPar.TB1Grid1Color = resource1;
                    this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
                }
            }
            if (shapePartsNumber == 4)
            {
                if (measuringType != EMeasuringType.Simetric)
                {
                    this.centersUCPar.TextBox1VisuEna = true;
                    this.centersUCPar.TextBox2VisuEna = true;
                    this.centersUCPar.TextBox3VisuEna = true;
                    this.centersUCPar.TextBox4VisuEna = false;
                    if (measuringType == EMeasuringType.Absolute)
                    {
                        this.centersUCPar.TB1Grid1Color = resource1;
                        this.centersUCPar.TB2Grid1Color = resource1;
                        this.centersUCPar.TB2Grid2Color = resource2;
                        this.centersUCPar.TB3Grid1Color = resource1;
                        this.centersUCPar.TB3Grid2Color = resource2;
                        this.centersUCPar.TB3Grid3Color = resource3;
                        this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.centersUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.centersUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.centersUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.centersUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.centersUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                    }
                    else
                    {
                        this.centersUCPar.TB1Grid1Color = resource1;
                        this.centersUCPar.TB2Grid1Color = resource2;
                        this.centersUCPar.TB3Grid1Color = resource3;
                        this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.centersUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.centersUCPar.TB3Grid1Width = textBoxBaseWidth;
                    }
                }
                else
                {
                    this.centersUCPar.TextBox1VisuEna = true;
                    this.centersUCPar.TextBox2VisuEna = false;
                    this.centersUCPar.TextBox3VisuEna = true;
                    this.centersUCPar.TextBox4VisuEna = false;
                    this.centersUCPar.TB1Grid1Color = resource2;
                    this.centersUCPar.TB3Grid1Color = resource1;
                    this.centersUCPar.TB3Grid2Color = resource2;
                    this.centersUCPar.TB3Grid3Color = resource3;
                    this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
                    this.centersUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                    this.centersUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                    this.centersUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                }
            }
            if (shapePartsNumber >= 5)
            {
                if (measuringType != EMeasuringType.Simetric)
                {
                    this.centersUCPar.TextBox1VisuEna = true;
                    this.centersUCPar.TextBox2VisuEna = true;
                    this.centersUCPar.TextBox3VisuEna = true;
                    this.centersUCPar.TextBox4VisuEna = true;
                    if (measuringType == EMeasuringType.Absolute)
                    {
                        this.centersUCPar.TB1Grid1Color = resource1;
                        this.centersUCPar.TB2Grid1Color = resource1;
                        this.centersUCPar.TB2Grid2Color = resource2;
                        this.centersUCPar.TB3Grid1Color = resource1;
                        this.centersUCPar.TB3Grid2Color = resource2;
                        this.centersUCPar.TB3Grid3Color = resource3;
                        this.centersUCPar.TB4Grid1Color = resource1;
                        this.centersUCPar.TB4Grid2Color = resource2;
                        this.centersUCPar.TB4Grid3Color = resource3;
                        this.centersUCPar.TB4Grid4Color = resource4;
                        this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.centersUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.centersUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.centersUCPar.TB3Grid1Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.centersUCPar.TB3Grid2Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.centersUCPar.TB3Grid3Width = (textBoxBaseWidth - 2.0) / 3.0;
                        this.centersUCPar.TB4Grid1Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.centersUCPar.TB4Grid2Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.centersUCPar.TB4Grid3Width = (textBoxBaseWidth - 3.0) / 4.0;
                        this.centersUCPar.TB4Grid4Width = (textBoxBaseWidth - 3.0) / 4.0;
                    }
                    else
                    {
                        this.centersUCPar.TB1Grid1Color = resource1;
                        this.centersUCPar.TB2Grid1Color = resource2;
                        this.centersUCPar.TB3Grid1Color = resource3;
                        this.centersUCPar.TB4Grid1Color = resource4;
                        this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.centersUCPar.TB2Grid1Width = textBoxBaseWidth;
                        this.centersUCPar.TB3Grid1Width = textBoxBaseWidth;
                        this.centersUCPar.TB4Grid1Width = textBoxBaseWidth;
                    }
                }
                else
                {
                    this.centersUCPar.TextBox1VisuEna = true;
                    this.centersUCPar.TextBox2VisuEna = true;
                    this.centersUCPar.TextBox3VisuEna = false;
                    this.centersUCPar.TextBox4VisuEna = true;
                    this.centersUCPar.TB1Grid1Color = resource2;
                    this.centersUCPar.TB2Grid1Color = resource3;
                    this.centersUCPar.TB4Grid1Color = resource1;
                    this.centersUCPar.TB4Grid2Color = resource2;
                    this.centersUCPar.TB4Grid3Color = resource3;
                    this.centersUCPar.TB4Grid4Color = resource4;
                    this.centersUCPar.TB1Grid1Width = textBoxBaseWidth;
                    this.centersUCPar.TB2Grid1Width = textBoxBaseWidth;
                    this.centersUCPar.TB4Grid1Width = (textBoxBaseWidth - 3.0) / 4.0;
                    this.centersUCPar.TB4Grid2Width = (textBoxBaseWidth - 3.0) / 4.0;
                    this.centersUCPar.TB4Grid3Width = (textBoxBaseWidth - 3.0) / 4.0;
                    this.centersUCPar.TB4Grid4Width = (textBoxBaseWidth - 3.0) / 4.0;
                }
            }
            if (shapePartsNumber != 2 && shapePartsNumber != 3)
                return;
            if (((IEnumerable<string>)drawingParts).First<string>() == "CB" || ((IEnumerable<string>)drawingParts).First<string>() == "CF")
            {
                this.centersUCPar.TextBox3VisuEna = true;
                this.centersUCPar.TB3Grid1Color = resource3;
                this.centersUCPar.RestGridEna3 = false;
                this.centersUCPar.TB3Grid1Width = textBoxBaseWidth;
            }
            if (!(((IEnumerable<string>)drawingParts).Last<string>() == "CB") && !(((IEnumerable<string>)drawingParts).Last<string>() == "CF"))
                return;
            this.centersUCPar.TextBox4VisuEna = true;
            this.centersUCPar.TB4Grid1Color = resource4;
            this.centersUCPar.RestGridEna4 = false;
            this.centersUCPar.TB4Grid1Width = textBoxBaseWidth;
        }
	}
}