using JobEditor.Common;
using JobEditor.Views.ProductData;
using Messages;
using Patterns.EventAggregator;
using ProductLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace JobEditor.Views
{
    public partial class StepLapsUserControl : UserControl, INotifyPropertyChanged
    {
        private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

        private StepLapsUCPar stepLapsUCPar = new StepLapsUCPar();

        private ISubscription<Telegram> subscription;

        public static DependencyProperty ShapeViewDPProperty;

        public static DependencyProperty UCParProperty;

        //internal CustomControlLibrary.ListBox ListBox1;
        public ShapeInfoView ShapeViewDP
        {
            get
            {
                return (ShapeInfoView)base.GetValue(StepLapsUserControl.ShapeViewDPProperty);
            }
            set
            {
                base.SetValue(StepLapsUserControl.ShapeViewDPProperty, value);
            }
        }

        public StepLapsUCPar UCPar
        {
            get
            {
                return (StepLapsUCPar)base.GetValue(StepLapsUserControl.UCParProperty);
            }
            set
            {
                base.SetValue(StepLapsUserControl.UCParProperty, value);
            }
        }

        static StepLapsUserControl()
        {
            StepLapsUserControl.ShapeViewDPProperty = DependencyProperty.Register("ShapeViewDP", typeof(ShapeInfoView), typeof(StepLapsUserControl));
            StepLapsUserControl.UCParProperty = DependencyProperty.Register("UCPar", typeof(StepLapsUCPar), typeof(StepLapsUserControl));
        }

        public StepLapsUserControl()
        {
            this.InitializeComponent();
            this.subscription = this.aggregator.Subscribe<Telegram>(new Action<Telegram>(this.ReloadShapeView), "JobEditor.UserControl.StepLaps");
        }

        private void ComposeProperties(ref ShapeProperties shapeProperties)
        {
            shapeProperties.PolygonProperties.StrokeColor = ((SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush")).Color;
            shapeProperties.PolygonProperties.FillColor = ((SolidColorBrush)Application.Current.TryFindResource("ShapeFillColor")).Color;
            shapeProperties.PolygonProperties.ScallingFactor = 12;
            shapeProperties.PolygonProperties.StrokeTicknees = 2;
            shapeProperties.PolygonProperties.DistanceBetweenParts = 2;
            shapeProperties.StepLapProperties.Step = 5;
            shapeProperties.StepLapProperties.IsEnabled = true;
            shapeProperties.HoleProperties.FillColor = ((SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush")).Color;
            shapeProperties.HoleProperties.StrokeThickness = 0;
            shapeProperties.HoleProperties.Diameter = 8;
            shapeProperties.SlotProperties.FillColor = ((SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush")).Color;
            shapeProperties.SlotProperties.StrokeThickness = 0;
            shapeProperties.SlotProperties.Height = 4;
            shapeProperties.SlotProperties.Width = 50;
            shapeProperties.SlotProperties.VerticalDistance = 10;
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
            if (this.StepLapMainGrid.DataContext != null && this.StepLapMainGrid.DataContext is ShapeView)
            {
                Shape shape = ((ShapeView)this.StepLapMainGrid.DataContext).Shape;
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
        //    Application.LoadComponent(this, new Uri("/JobEditor;component/views/steplapsusercontrol.xaml", UriKind.Relative));
        //}

        private void LoadUserControl(object sender, RoutedEventArgs e)
        {
            ShapeView dataContext = (ShapeView)this.StepLapMainGrid.DataContext;
            this.RedrawShape(dataContext);
            int count = dataContext.ShapePartViews.Count - 3;
            if (count <= 0)
            {
                this.stepLapsUCPar.StepLapsUserControlWidth = 216;
            }
            else
            {
                this.stepLapsUCPar.StepLapsUserControlWidth = (double)(216 + count * 72);
            }
            this.UCPar = this.stepLapsUCPar;
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
                switch (shapePartView.Feature)
                {
                    case EFeature.Cut90:
                        {
                            if (this.ListBox1.Items.IndexOf(item) != 0)
                            {
                                thickness.Items.Add(EStepLapType.Cut90Right);
                                thickness.Items.Add(EStepLapType.Cut90Right_Right);
                                thickness.Items.Add(EStepLapType.Cut90Right_Left);
                                continue;
                            }
                            else
                            {
                                thickness.Items.Add(EStepLapType.Cut90Left);
                                thickness.Items.Add(EStepLapType.Cut90Left_Left);
                                thickness.Items.Add(EStepLapType.Cut90Left_Right);
                                continue;
                            }
                        }
                    case EFeature.Cut45:
                        {
                            if (this.ListBox1.Items.IndexOf(item) != 0)
                            {
                                thickness.Items.Add(EStepLapType.Cut45Right);
                                thickness.Items.Add(EStepLapType.Cut45Right_Right);
                                thickness.Items.Add(EStepLapType.Cut45Right_Left);
                                continue;
                            }
                            else
                            {
                                thickness.Items.Add(EStepLapType.Cut45Left);
                                thickness.Items.Add(EStepLapType.Cut45Left_Left);
                                thickness.Items.Add(EStepLapType.Cut45Left_Right);
                                continue;
                            }
                        }
                    case EFeature.Cut135:
                        {
                            if (this.ListBox1.Items.IndexOf(item) != 0)
                            {
                                thickness.Items.Add(EStepLapType.Cut135Right);
                                thickness.Items.Add(EStepLapType.Cut135Right_Right);
                                thickness.Items.Add(EStepLapType.Cut135Right_Left);
                                continue;
                            }
                            else
                            {
                                thickness.Items.Add(EStepLapType.Cut135Left);
                                thickness.Items.Add(EStepLapType.Cut135Left_Left);
                                thickness.Items.Add(EStepLapType.Cut135Left_Right);
                                continue;
                            }
                        }
                    case EFeature.VTop:
                        {
                            thickness.Items.Add(EStepLapType.VTop);
                            thickness.Items.Add(EStepLapType.VTop_Right);
                            thickness.Items.Add(EStepLapType.VTop_Left);
                            thickness.Items.Add(EStepLapType.VTop_Down);
                            thickness.Items.Add(EStepLapType.VTop_Up);
                            continue;
                        }
                    case EFeature.VBottom:
                        {
                            thickness.Items.Add(EStepLapType.VBottom);
                            thickness.Items.Add(EStepLapType.VBottom_Right);
                            thickness.Items.Add(EStepLapType.VBottom_Left);
                            thickness.Items.Add(EStepLapType.VBottom_Down);
                            thickness.Items.Add(EStepLapType.VBottom_Up);
                            continue;
                        }
                    case EFeature.CLeft:
                        {
                            thickness.Items.Add(EStepLapType.CTipLeft);
                            thickness.Items.Add(EStepLapType.CTipLeft_Left);
                            thickness.Items.Add(EStepLapType.CTipLeft_Right);
                            thickness.Items.Add(EStepLapType.CTipLeft_Up);
                            thickness.Items.Add(EStepLapType.CTipLeft_Down);
                            continue;
                        }
                    case EFeature.CRight:
                        {
                            thickness.Items.Add(EStepLapType.CTipRight);
                            thickness.Items.Add(EStepLapType.CTipRight_Right);
                            thickness.Items.Add(EStepLapType.CTipRight_Left);
                            thickness.Items.Add(EStepLapType.CTipRight_Up);
                            thickness.Items.Add(EStepLapType.CTipRight_Down);
                            continue;
                        }
                    default:
                        {
                            continue;
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
            Shape shape = shapeView.Shape;
            ShapeProperties shapeProperty = new ShapeProperties(shape);
            this.ComposeProperties(ref shapeProperty);
            this.ShapeViewDP = new ShapeInfoView(shape, shapeProperty);
        }

        public void ReloadShapeView(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => this.DoReloadShapeView()), DispatcherPriority.Background, null);
        }

        private void StepLapTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            this.RedrawShape((ShapeView)this.StepLapMainGrid.DataContext);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}