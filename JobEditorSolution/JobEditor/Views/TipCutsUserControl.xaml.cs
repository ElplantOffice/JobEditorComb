using JobEditor.Common;
using JobEditor.Views.ProductData;
using Messages;
using Patterns.EventAggregator;
using ProductLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JobEditor.Views
{
    /// <summary>
    /// Interaction logic for TipCutsUserControl.xaml
    /// </summary>
    public partial class TipCutsUserControl : UserControl, INotifyPropertyChanged
    {
        private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

        private TipCutsUCPar tipCutsUCPar = new TipCutsUCPar();

        private ISubscription<Telegram> subscription;

        public static DependencyProperty ShapeViewDPProperty;

        public static DependencyProperty UCParProperty;

        //internal TipCutsUserControl UserControl;

        //internal Grid TipCutsMainGrid;

        //internal CustomControlLibrary.ListBox ListBox1;

        //private bool _contentLoaded;

        public ShapeInfoView ShapeViewDP
        {
            get
            {
                return (ShapeInfoView)base.GetValue(TipCutsUserControl.ShapeViewDPProperty);
            }
            set
            {
                base.SetValue(TipCutsUserControl.ShapeViewDPProperty, value);
            }
        }

        public TipCutsUCPar UCPar
        {
            get
            {
                return (TipCutsUCPar)base.GetValue(TipCutsUserControl.UCParProperty);
            }
            set
            {
                base.SetValue(TipCutsUserControl.UCParProperty, value);
            }
        }

        static TipCutsUserControl()
        {
            TipCutsUserControl.ShapeViewDPProperty = DependencyProperty.Register("ShapeViewDP", typeof(ShapeInfoView), typeof(TipCutsUserControl));
            TipCutsUserControl.UCParProperty = DependencyProperty.Register("UCPar", typeof(TipCutsUCPar), typeof(TipCutsUserControl));
        }

        public TipCutsUserControl()
        {
            this.InitializeComponent();
            this.subscription = this.aggregator.Subscribe<Telegram>(new Action<Telegram>(this.ReloadShapeView), "JobEditor.UserControl.TipCuts");
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
            shapeProperties.TipCutProperties.IsEnabled = true;
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
            shapeProperties.SlotProperties.AnnotationProperties.IsEnabled = false;
            shapeProperties.SlotProperties.ArrowLineProperties.IsEnabled = false;
            shapeProperties.SlotProperties.Width = 50;
            shapeProperties.SlotProperties.VerticalDistance = 10;
            shapeProperties.SlotProperties.LabelProperties.IsEnabled = false;
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
            if (this.TipCutsMainGrid.DataContext != null && this.TipCutsMainGrid.DataContext is ShapeView)
            {
                ProductLib.Shape shape = ((ShapeView)this.TipCutsMainGrid.DataContext).Shape;
                ShapeProperties shapeProperty = new ShapeProperties(shape);
                this.ComposeProperties(ref shapeProperty);
                this.ShapeViewDP = new ShapeInfoView(shape, shapeProperty);
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
        where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                childItem _childItem = this.FindVisualChild<childItem>(child);
                if (_childItem != null)
                {
                    return _childItem;
                }
            }
            return default(childItem);
        }

        //[DebuggerNonUserCode]
        //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        //public void InitializeComponent()
        //{
        //    if (this._contentLoaded)
        //    {
        //        return;
        //    }
        //    this._contentLoaded = true;
        //    Application.LoadComponent(this, new Uri("/JobEditor;component/views/tipcutsusercontrol.xaml", UriKind.Relative));
        //}

        private void LoadUserControl(object sender, RoutedEventArgs e)
        {
            EFeature feature;
            ShapeView dataContext = (ShapeView)this.TipCutsMainGrid.DataContext;
            this.RedrawShape(dataContext);
            int count = dataContext.ShapePartViews.Count - 3;
            if (count <= 0)
            {
                this.tipCutsUCPar.TipCutsUserControlWidth = 216;
            }
            else
            {
                this.tipCutsUCPar.TipCutsUserControlWidth = (double)(216 + count * 72);
            }
            this.UCPar = this.tipCutsUCPar;
            foreach (object item in (IEnumerable)this.ListBox1.Items)
            {
                CustomControlLibrary.ListBoxItem listBoxItem = (CustomControlLibrary.ListBoxItem)this.ListBox1.ItemContainerGenerator.ContainerFromItem(item);
                ContentPresenter contentPresenter = this.FindVisualChild<ContentPresenter>(listBoxItem);
                CustomControlLibrary.ComboBox thickness = (CustomControlLibrary.ComboBox)contentPresenter.ContentTemplate.FindName("ComboBox1", contentPresenter);
                if (!(item is ShapePartView))
                {
                    continue;
                }
                ShapePartView shapePartView = (ShapePartView)item;
                if (this.ListBox1.Items.Count == 2 && this.ListBox1.Items.IndexOf(item) == 1)
                {
                    CustomControlLibrary.ComboBox comboBox = new CustomControlLibrary.ComboBox();
                    thickness.Margin = new Thickness(71, -1, 0, 0);
                }
                if (shapePartView.Feature != EFeature.Cut135 && shapePartView.Feature != EFeature.Cut45)
                {
                    continue;
                }
                if (this.ListBox1.Items.IndexOf(item) != 0)
                {
                    feature = shapePartView.Feature;
                    if (feature == EFeature.Cut45)
                    {
                        thickness.Items.Add(ETipCutType.TipCut45RightOn);
                        thickness.Items.Add(ETipCutType.TipCut45RightOff);
                    }
                    else if (feature == EFeature.Cut135)
                    {
                        thickness.Items.Add(ETipCutType.TipCut135RightOn);
                        thickness.Items.Add(ETipCutType.TipCut135RightOff);
                    }
                }
                else
                {
                    feature = shapePartView.Feature;
                    if (feature == EFeature.Cut45)
                    {
                        thickness.Items.Add(ETipCutType.TipCut45LeftOn);
                        thickness.Items.Add(ETipCutType.TipCut45LeftOff);
                    }
                    else if (feature == EFeature.Cut135)
                    {
                        thickness.Items.Add(ETipCutType.TipCut135LeftOn);
                        thickness.Items.Add(ETipCutType.TipCut135LeftOff);
                    }
                }
            }
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
            ProductLib.Shape shape = shapeView.Shape;
            ShapeProperties shapeProperty = new ShapeProperties(shape);
            this.ComposeProperties(ref shapeProperty);
            this.ShapeViewDP = new ShapeInfoView(shape, shapeProperty);
        }

        public void ReloadShapeView(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => this.DoReloadShapeView()), DispatcherPriority.Background, null);
        }

        private void TipCutTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            this.RedrawShape((ShapeView)this.TipCutsMainGrid.DataContext);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
