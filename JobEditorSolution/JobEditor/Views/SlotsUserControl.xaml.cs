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
    public partial class SlotsUserControl : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

        private int diff;

        public ShapeView shapeView = new ShapeView(null, null);

        private SlotsUCPar slotsUCPar = new SlotsUCPar();

        private ISubscription<Telegram> subscription;

        public static DependencyProperty ShapeViewDPProperty;

        public static DependencyProperty UCParProperty;

        //internal CustomControlLibrary.ComboBox NumberOfSlotsCB;

        //internal CustomControlLibrary.ComboBox MeasuringTypeCB;

        public ShapeInfoView ShapeViewDP
        {
            get
            {
                return (ShapeInfoView)base.GetValue(SlotsUserControl.ShapeViewDPProperty);
            }
            set
            {
                base.SetValue(SlotsUserControl.ShapeViewDPProperty, value);
            }
        }

        public SlotsUCPar UCPar
        {
            get
            {
                return (SlotsUCPar)base.GetValue(SlotsUserControl.UCParProperty);
            }
            set
            {
                base.SetValue(SlotsUserControl.UCParProperty, value);
            }
        }

        static SlotsUserControl()
        {
            SlotsUserControl.ShapeViewDPProperty = DependencyProperty.Register("ShapeViewDP", typeof(ShapeInfoView), typeof(SlotsUserControl));
            SlotsUserControl.UCParProperty = DependencyProperty.Register("UCPar", typeof(SlotsUCPar), typeof(SlotsUserControl));
        }

        public SlotsUserControl()
        {
            this.InitializeComponent();
            this.subscription = this.aggregator.Subscribe<Telegram>(new Action<Telegram>(this.ReloadShapeView), "JobEditor.UserControl.Slots");
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
            shapeProperties.HoleProperties.AnnotationProperties.IsEnabled = false;
            shapeProperties.HoleProperties.ArrowLineProperties.IsEnabled = false;
            shapeProperties.HoleProperties.LabelProperties.IsEnabled = false;
            shapeProperties.HoleProperties.AnnotationProperties.LineColor = Colors.Black;
            shapeProperties.HoleProperties.AnnotationProperties.StrokeThickness = 1;
            shapeProperties.SlotProperties.FillColor = ((SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush")).Color;
            shapeProperties.SlotProperties.StrokeThickness = 0;
            shapeProperties.SlotProperties.Height = 4;
            shapeProperties.SlotProperties.AnnotationProperties.IsEnabled = true;
            shapeProperties.SlotProperties.ArrowLineProperties.IsEnabled = true;
            shapeProperties.SlotProperties.Width = 50;
            shapeProperties.SlotProperties.VerticalDistance = 10;
            shapeProperties.SlotProperties.LabelProperties.IsEnabled = true;
            shapeProperties.SlotProperties.AnnotationProperties.LineColor = Colors.Black;
            shapeProperties.SlotProperties.AnnotationProperties.StrokeThickness = 1;
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
            if (this.SlotsUserControlMainGrid.DataContext != null && this.SlotsUserControlMainGrid.DataContext is ShapeView)
            {
                Shape shape = ((ShapeView)this.SlotsUserControlMainGrid.DataContext).Shape;
                ShapeProperties shapeProperty = new ShapeProperties(shape);
                this.ComposeProperties(ref shapeProperty);
                this.ShapeViewDP = new ShapeInfoView(shape, shapeProperty);
            }
        }

        private void LoadSlotsUserControl(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.shapeView = (ShapeView)this.SlotsUserControlMainGrid.DataContext;
            this.RedrawShape(this.shapeView);
            this.diff = this.shapeView.ShapePartViews.Count - 3;
            if (this.diff <= 0)
            {
                this.slotsUCPar.MTComboBoxMargin = new Thickness(-1, -1, 0, 0);
            }
            else
            {
                this.slotsUCPar.MTComboBoxMargin = new Thickness((double)(this.diff * 72 - 1), -1, 0, 0);
            }
            if (this.diff <= 0)
            {
                this.slotsUCPar.SlotsUserControlWidth = 216;
            }
            else
            {
                this.slotsUCPar.SlotsUserControlWidth = (double)(216 + this.diff * 72);
            }
            this.UCPar = this.slotsUCPar;
            this.ShapeSlotsVisuDef(this.shapeView.SlotsView.NumberOfSlots);
            this.TextBoxesVisuDef(this.shapeView.SlotsView.NumberOfSlots, this.shapeView.SlotsView.MeasuringType, this.diff);
            stopwatch.Stop();
        }

        private void NumberOfSlotsChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ShapeSlotsVisuDef(this.shapeView.SlotsView.NumberOfSlots);
            this.TextBoxesVisuDef(this.shapeView.SlotsView.NumberOfSlots, this.shapeView.SlotsView.MeasuringType, this.diff);
            this.RedrawShape((ShapeView)this.SlotsUserControlMainGrid.DataContext);
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

        private void ShapeSlotsVisuDef(int numberOfSlots)
        {
            if (numberOfSlots <= 0)
            {
                this.slotsUCPar.ShapeSlot1VisuEna = false;
            }
            else
            {
                this.slotsUCPar.ShapeSlot1VisuEna = true;
            }
            if (numberOfSlots <= 1)
            {
                this.slotsUCPar.ShapeSlot2VisuEna = false;
            }
            else
            {
                this.slotsUCPar.ShapeSlot2VisuEna = true;
            }
            if (numberOfSlots <= 2)
            {
                this.slotsUCPar.ShapeSlot3VisuEna = false;
            }
            else
            {
                this.slotsUCPar.ShapeSlot3VisuEna = true;
            }
            if (numberOfSlots > 3)
            {
                this.slotsUCPar.ShapeSlot4VisuEna = true;
                return;
            }
            this.slotsUCPar.ShapeSlot4VisuEna = false;
        }

        private void SlotsMeasuringTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ShapeSlotsVisuDef(this.shapeView.SlotsView.NumberOfSlots);
            this.TextBoxesVisuDef(this.shapeView.SlotsView.NumberOfSlots, this.shapeView.SlotsView.MeasuringType, this.diff);
            this.RedrawShape((ShapeView)this.SlotsUserControlMainGrid.DataContext);
        }

        private void TextBoxesVisuDef(int nmbrSlots, EMeasuringType measuringType, int diff)
        {
            double textBoxBaseWidth = this.slotsUCPar.SlotsTextBoxBaseWidth;
            if (diff > 0)
                textBoxBaseWidth += (double)diff * 72.0;
            SolidColorBrush resource1 = (SolidColorBrush)Application.Current.TryFindResource((object)"ShapesTextBoxDistances1");
            SolidColorBrush resource2 = (SolidColorBrush)Application.Current.TryFindResource((object)"ShapesTextBoxDistances2");
            this.slotsUCPar.TextBoxesWidth = textBoxBaseWidth;
            this.slotsUCPar.SimetricLineWidth = this.slotsUCPar.TextBoxesWidth + 1.0;
            this.slotsUCPar.TextBoxesHeight = this.slotsUCPar.SlotsTextBoxBaseHeight;
            this.slotsUCPar.TB2Grid2Ena = true;
            if (measuringType == EMeasuringType.Relative || measuringType == EMeasuringType.Simetric)
                this.slotsUCPar.TB2Grid2Ena = false;
            if (nmbrSlots > 0)
            {
                if (measuringType != EMeasuringType.Simetric)
                {
                    this.slotsUCPar.TextBox1VisuEna = true;
                    this.slotsUCPar.TextBox2VisuEna = true;
                    if (measuringType == EMeasuringType.Absolute)
                    {
                        this.slotsUCPar.TB1Grid1Color = resource1;
                        this.slotsUCPar.TB2Grid1Color = resource1;
                        this.slotsUCPar.TB2Grid2Color = resource2;
                        this.slotsUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.slotsUCPar.TB2Grid1Width = (textBoxBaseWidth - 1.0) / 2.0;
                        this.slotsUCPar.TB2Grid2Width = (textBoxBaseWidth - 1.0) / 2.0;
                    }
                    else
                    {
                        this.slotsUCPar.TB1Grid1Color = resource1;
                        this.slotsUCPar.TB2Grid1Color = resource2;
                        this.slotsUCPar.TB1Grid1Width = textBoxBaseWidth;
                        this.slotsUCPar.TB2Grid1Width = textBoxBaseWidth;
                    }
                }
                else
                {
                    this.slotsUCPar.TextBox1VisuEna = true;
                    this.slotsUCPar.TextBox2VisuEna = false;
                    this.slotsUCPar.TB1Grid1Color = resource2;
                    this.slotsUCPar.TB1Grid1Width = textBoxBaseWidth;
                }
            }
            else
            {
                this.slotsUCPar.TextBox1VisuEna = false;
                this.slotsUCPar.TextBox2VisuEna = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}