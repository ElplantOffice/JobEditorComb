
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CustomControlLibrary
{
  [TemplateVisualState(GroupName = "CommonStates", Name = "Normal")]
  [TemplateVisualState(GroupName = "CommonStates", Name = "Disabled")]
  [TemplateVisualState(GroupName = "CheckStates", Name = "Checked")]
  [TemplateVisualState(GroupName = "CheckStates", Name = "Dragging")]
  [TemplateVisualState(GroupName = "CheckStates", Name = "Unchecked")]
  [TemplatePart(Name = "SwitchRoot", Type = typeof (Grid))]
  [TemplatePart(Name = "SwitchBackground", Type = typeof (UIElement))]
  [TemplatePart(Name = "SwitchTrack", Type = typeof (Grid))]
  [TemplatePart(Name = "SwitchThumb", Type = typeof (FrameworkElement))]
  public class ToggleSwitchButton : System.Windows.Controls.Primitives.ToggleButton
  {
    public static readonly DependencyProperty SwitchForegroundProperty = DependencyProperty.Register(nameof (SwitchForeground), typeof (Brush), typeof (ToggleSwitchButton), new PropertyMetadata((PropertyChangedCallback) null));
    private const string CommonStates = "CommonStates";
    private const string NormalState = "Normal";
    private const string DisabledState = "Disabled";
    private const string CheckStates = "CheckStates";
    private const string CheckedState = "Checked";
    private const string DraggingState = "Dragging";
    private const string UncheckedState = "Unchecked";
    private const string SwitchRootPart = "SwitchRoot";
    private const string SwitchBackgroundPart = "SwitchBackground";
    private const string SwitchTrackPart = "SwitchTrack";
    private const string SwitchThumbPart = "SwitchThumb";
    private TranslateTransform _backgroundTranslation;
    private TranslateTransform _thumbTranslation;
    private Grid _root;
    private Grid _track;
    private FrameworkElement _thumb;
    private DoubleAnimation _checked_anim;
    private DoubleAnimationUsingKeyFrames _checked_keyframes;
    private bool _isDragging;
    public double DWidth;

    public Brush SwitchForeground
    {
      get
      {
        return (Brush) this.GetValue(ToggleSwitchButton.SwitchForegroundProperty);
      }
      set
      {
        this.SetValue(ToggleSwitchButton.SwitchForegroundProperty, (object) value);
      }
    }

    public ToggleSwitchButton()
    {
      this.DefaultStyleKey = (object) typeof (ToggleSwitchButton);
    }

    private new void ChangeVisualState(bool useTransitions)
    {
      VisualStateManager.GoToState((FrameworkElement) this, this.IsEnabled ? "Normal" : "Disabled", useTransitions);
      if (this._isDragging)
      {
        VisualStateManager.GoToState((FrameworkElement) this, "Dragging", useTransitions);
      }
      else
      {
        bool? isChecked = this.IsChecked;
        bool flag = true;
        if ((isChecked.GetValueOrDefault() == flag ? (isChecked.HasValue ? 1 : 0) : 0) != 0)
          VisualStateManager.GoToState((FrameworkElement) this, "Checked", useTransitions);
        else
          VisualStateManager.GoToState((FrameworkElement) this, "Unchecked", useTransitions);
      }
    }

    protected override void OnToggle()
    {
      bool? isChecked = this.IsChecked;
      bool flag = true;
      this.IsChecked = new bool?(isChecked.GetValueOrDefault() != flag || !isChecked.HasValue);
      this.ChangeVisualState(true);
    }

    public override void OnApplyTemplate()
    {
      if (this._track != null)
        this._track.SizeChanged -= new SizeChangedEventHandler(this.SizeChangedHandler);
      if (this._thumb != null)
        this._thumb.SizeChanged -= new SizeChangedEventHandler(this.SizeChangedHandler);
      base.OnApplyTemplate();
      this._root = this.GetTemplateChild("SwitchRoot") as Grid;
      UIElement templateChild = this.GetTemplateChild("SwitchBackground") as UIElement;
      this._backgroundTranslation = templateChild == null ? (TranslateTransform) null : templateChild.RenderTransform as TranslateTransform;
      this._track = this.GetTemplateChild("SwitchTrack") as Grid;
      this._thumb = (FrameworkElement) (this.GetTemplateChild("SwitchThumb") as Border);
      this._thumbTranslation = this._thumb == null ? (TranslateTransform) null : this._thumb.RenderTransform as TranslateTransform;
      this._checked_anim = this.GetTemplateChild("CheckedDoubleAnimation") as DoubleAnimation;
      this._checked_keyframes = this.GetTemplateChild("CheckedEasingKeyFrame") as DoubleAnimationUsingKeyFrames;
      if (this._root != null && this._track != null && this._thumb != null && (this._backgroundTranslation != null || this._thumbTranslation != null))
      {
        this._track.SizeChanged += new SizeChangedEventHandler(this.SizeChangedHandler);
        this._thumb.SizeChanged += new SizeChangedEventHandler(this.SizeChangedHandler);
      }
      this.ChangeVisualState(false);
    }

    private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
    {
      this._track.Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(0.0, 0.0, this._track.ActualWidth, this._track.ActualHeight)
      };
      double actualWidth1 = this._track.ActualWidth;
      double actualWidth2 = this._thumb.ActualWidth;
      double left = this._thumb.Margin.Left;
      double right = this._thumb.Margin.Right;
      this._checked_anim.To = new double?(this._track.ActualWidth - this._thumb.ActualWidth);
      if (this._checked_keyframes.KeyFrames.Count == 0)
        return;
      this._checked_keyframes.KeyFrames[0].Value = this._track.ActualWidth - this._thumb.ActualWidth;
    }
  }
}
