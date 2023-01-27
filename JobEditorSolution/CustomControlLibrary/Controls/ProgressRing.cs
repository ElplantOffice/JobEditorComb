
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CustomControlLibrary
{
  [TemplateVisualState(GroupName = "ActiveStates", Name = "Active")]
  [TemplateVisualState(GroupName = "ActiveStates", Name = "Inactive")]
  [TemplateVisualState(GroupName = "SizeStates", Name = "Large")]
  [TemplateVisualState(GroupName = "SizeStates", Name = "Small")]
  public class ProgressRing : Control
  {
    private List<Action> _deferredActions = new List<Action>();
    public static readonly DependencyProperty BindableWidthProperty = DependencyProperty.Register(nameof (BindableWidth), typeof (double), typeof (ProgressRing), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ProgressRing.BindableWidthCallback)));
    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof (IsActive), typeof (bool), typeof (ProgressRing), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ProgressRing.IsActiveChanged)));
    public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register(nameof (IsLarge), typeof (bool), typeof (ProgressRing), new PropertyMetadata((object) true, new PropertyChangedCallback(ProgressRing.IsLargeChangedCallback)));
    public static readonly DependencyProperty MaxSideLengthProperty = DependencyProperty.Register(nameof (MaxSideLength), typeof (double), typeof (ProgressRing), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register(nameof (EllipseDiameter), typeof (double), typeof (ProgressRing), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty EllipseOffsetProperty = DependencyProperty.Register(nameof (EllipseOffset), typeof (Thickness), typeof (ProgressRing), new PropertyMetadata((object) new Thickness()));

    public double BindableWidth
    {
      get
      {
        return (double) this.GetValue(ProgressRing.BindableWidthProperty);
      }
      private set
      {
        this.SetValue(ProgressRing.BindableWidthProperty, (object) value);
      }
    }

    public double EllipseDiameter
    {
      get
      {
        return (double) this.GetValue(ProgressRing.EllipseDiameterProperty);
      }
      private set
      {
        this.SetValue(ProgressRing.EllipseDiameterProperty, (object) value);
      }
    }

    public Thickness EllipseOffset
    {
      get
      {
        return (Thickness) this.GetValue(ProgressRing.EllipseOffsetProperty);
      }
      private set
      {
        this.SetValue(ProgressRing.EllipseOffsetProperty, (object) value);
      }
    }

    public bool IsActive
    {
      get
      {
        return (bool) this.GetValue(ProgressRing.IsActiveProperty);
      }
      set
      {
        this.SetValue(ProgressRing.IsActiveProperty, (object) value);
      }
    }

    public bool IsLarge
    {
      get
      {
        return (bool) this.GetValue(ProgressRing.IsLargeProperty);
      }
      set
      {
        this.SetValue(ProgressRing.IsLargeProperty, (object) value);
      }
    }

    public double MaxSideLength
    {
      get
      {
        return (double) this.GetValue(ProgressRing.MaxSideLengthProperty);
      }
      private set
      {
        this.SetValue(ProgressRing.MaxSideLengthProperty, (object) value);
      }
    }

    static ProgressRing()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ProgressRing), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ProgressRing)));
    }

    public ProgressRing()
    {
      this.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
    }

    private static void BindableWidthCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      ProgressRing progressRing = dependencyObject as ProgressRing;
      if (progressRing == null)
        return;
      Action action = (Action) (() =>
      {
        progressRing.SetEllipseDiameter((double) dependencyPropertyChangedEventArgs.NewValue);
        progressRing.SetEllipseOffset((double) dependencyPropertyChangedEventArgs.NewValue);
        progressRing.SetMaxSideLength((double) dependencyPropertyChangedEventArgs.NewValue);
      });
      if (progressRing._deferredActions == null)
        action();
      else
        progressRing._deferredActions.Add(action);
    }

    private static void IsActiveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      (dependencyObject as ProgressRing)?.UpdateActiveState();
    }

    private static void IsLargeChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      (dependencyObject as ProgressRing)?.UpdateLargeState();
    }

    public override void OnApplyTemplate()
    {
      this.UpdateLargeState();
      this.UpdateActiveState();
      base.OnApplyTemplate();
      if (this._deferredActions != null)
      {
        foreach (Action deferredAction in this._deferredActions)
          deferredAction();
      }
      this._deferredActions = (List<Action>) null;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
      this.BindableWidth = this.ActualWidth;
    }

    private void SetEllipseDiameter(double width)
    {
      this.EllipseDiameter = width / 8.0;
    }

    private void SetEllipseOffset(double width)
    {
      this.EllipseOffset = new Thickness(0.0, width / 2.0, 0.0, 0.0);
    }

    private void SetMaxSideLength(double width)
    {
      this.MaxSideLength = width <= 20.0 ? 20.0 : width;
    }

    private void UpdateActiveState()
    {
      Action action = !this.IsActive ? (Action) (() => VisualStateManager.GoToState((FrameworkElement) this, "Inactive", true)) : (Action) (() => VisualStateManager.GoToState((FrameworkElement) this, "Active", true));
      if (this._deferredActions == null)
        action();
      else
        this._deferredActions.Add(action);
    }

    private void UpdateLargeState()
    {
      Action action = !this.IsLarge ? (Action) (() => VisualStateManager.GoToState((FrameworkElement) this, "Small", true)) : (Action) (() => VisualStateManager.GoToState((FrameworkElement) this, "Large", true));
      if (this._deferredActions == null)
        action();
      else
        this._deferredActions.Add(action);
    }
  }
}
