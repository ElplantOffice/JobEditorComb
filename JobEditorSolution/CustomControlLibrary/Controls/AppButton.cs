
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CustomControlLibrary
{
  public class AppButton : System.Windows.Controls.Button
  {
    private readonly Stopwatch _doubleTapStopwatch = new Stopwatch();
    public static readonly DependencyProperty DoubleTapCommandProperty = DependencyProperty.Register(nameof (DoubleTapCommand), typeof (ICommand), typeof (AppButton));
    public static readonly DependencyProperty TwoFingerTapCommandCommandProperty = DependencyProperty.Register(nameof (TwoFingerTapCommand), typeof (ICommand), typeof (AppButton));

    static AppButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (AppButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (AppButton)));
    }

    public ICommand DoubleTapCommand
    {
      get
      {
        return this.GetValue(AppButton.DoubleTapCommandProperty) as ICommand;
      }
      set
      {
        this.SetValue(AppButton.DoubleTapCommandProperty, (object) value);
      }
    }

    public ICommand TwoFingerTapCommand
    {
      get
      {
        return this.GetValue(AppButton.TwoFingerTapCommandCommandProperty) as ICommand;
      }
      set
      {
        this.SetValue(AppButton.TwoFingerTapCommandCommandProperty, (object) value);
      }
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.PreviewTouchDown += new EventHandler<TouchEventArgs>(this.OnPreviewTouchDown);
      this.StylusSystemGesture += new StylusSystemGestureEventHandler(this.AppButton_StylusSystemGesture);
    }

    private void AppButton_StylusSystemGesture(object sender, StylusSystemGestureEventArgs e)
    {
      if (e.SystemGesture != SystemGesture.TwoFingerTap || this.TwoFingerTapCommand == null)
        return;
      this.TwoFingerTapCommand.Execute((object) null);
    }

    private bool IsDoubleTap(TouchEventArgs e)
    {
      TimeSpan elapsed = this._doubleTapStopwatch.Elapsed;
      this._doubleTapStopwatch.Restart();
      if (elapsed != TimeSpan.Zero)
        return elapsed < TimeSpan.FromSeconds(0.7);
      return false;
    }

    private void OnPreviewTouchDown(object sender, TouchEventArgs e)
    {
      if (!this.IsDoubleTap(e) || this.DoubleTapCommand == null)
        return;
      this.DoubleTapCommand.Execute((object) null);
    }
  }
}
