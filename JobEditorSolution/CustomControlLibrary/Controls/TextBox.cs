
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class TextBox : System.Windows.Controls.TextBox
  {
    private bool isEditable = true;
    public static readonly DependencyProperty IsMonitoringProperty = DependencyProperty.RegisterAttached("IsMonitoring", typeof (bool), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(TextBox.OnIsMonitoringChanged)));
    public static readonly DependencyProperty TextLengthProperty = DependencyProperty.RegisterAttached("TextLength", typeof (int), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) 0));
    public static readonly DependencyProperty ClearTextButtonProperty = DependencyProperty.Register(nameof (ClearTextButton), typeof (bool), typeof (TextBox), new PropertyMetadata((object) true, new PropertyChangedCallback(TextBox.ClearTextChanged)));
    public static readonly DependencyProperty IsNumericProperty = DependencyProperty.Register(nameof (IsNumeric), typeof (bool), typeof (TextBox), new PropertyMetadata((object) false, new PropertyChangedCallback(TextBox.IsNumericChanged)));
    public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.Register(nameof (SelectAllOnFocus), typeof (bool), typeof (TextBox), new PropertyMetadata((object) true));
    private static readonly DependencyProperty hasTextProperty = DependencyProperty.RegisterAttached(nameof (HasText), typeof (bool), typeof (TextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    public static readonly RoutedEvent TapToKeyboardEvent = EventManager.RegisterRoutedEvent("TapToKeyboard", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (TextBox));
    private bool handleTouchFailure;

    static TextBox()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (TextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (TextBox)));
    }

    public bool IsEditable
    {
      get
      {
        return this.isEditable;
      }
      private set
      {
        this.isEditable = value;
      }
    }

    public bool ClearTextButton
    {
      get
      {
        return (bool) this.GetValue(TextBox.ClearTextButtonProperty);
      }
      set
      {
        this.SetValue(TextBox.ClearTextButtonProperty, (object) value);
      }
    }

    public bool IsNumeric
    {
      get
      {
        return (bool) this.GetValue(TextBox.IsNumericProperty);
      }
      set
      {
        this.SetValue(TextBox.IsNumericProperty, (object) value);
      }
    }

    public bool SelectAllOnFocus
    {
      get
      {
        return (bool) this.GetValue(TextBox.SelectAllOnFocusProperty);
      }
      set
      {
        this.SetValue(TextBox.SelectAllOnFocusProperty, (object) value);
      }
    }

    public static void SetIsMonitoring(DependencyObject obj, bool value)
    {
      obj.SetValue(TextBox.IsMonitoringProperty, (object) value);
    }

    private static void SetTextLength(DependencyObject obj, int value)
    {
      obj.SetValue(TextBox.TextLengthProperty, (object) value);
      obj.SetValue(TextBox.hasTextProperty, (object) (value >= 1));
    }

    public bool HasText
    {
      get
      {
        return (bool) this.GetValue(TextBox.hasTextProperty);
      }
    }

    private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is TextBox))
        return;
      System.Windows.Controls.TextBox textBox = d as System.Windows.Controls.TextBox;
      if ((bool) e.NewValue)
      {
        textBox.TextChanged += new TextChangedEventHandler(TextBox.TextIsChanged);
        textBox.GotFocus += new RoutedEventHandler(TextBox.TextBoxGotFocus);
      }
      else
      {
        textBox.TextChanged -= new TextChangedEventHandler(TextBox.TextIsChanged);
        textBox.GotFocus -= new RoutedEventHandler(TextBox.TextBoxGotFocus);
      }
    }

    private static void ClearTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      TextBox textBox = d as TextBox;
      if (textBox == null)
        return;
      if ((bool) e.NewValue)
        textBox.Loaded += new RoutedEventHandler(TextBox.TextBoxLoaded);
      else
        textBox.Loaded -= new RoutedEventHandler(TextBox.TextBoxLoaded);
    }

    private static void IsNumericChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      TextBox textBox = d as TextBox;
      if (textBox == null)
        return;
      d.SetValue(NumericTextBoxBehavior.IsEnabledProperty, e.NewValue);
      if ((bool) e.NewValue)
        textBox.InputScope = new InputScope()
        {
          Names = {
            (object) new InputScopeName(InputScopeNameValue.NumberFullWidth)
          }
        };
      else
        textBox.InputScope = new InputScope()
        {
          Names = {
            (object) new InputScopeName(InputScopeNameValue.RegularExpression)
          }
        };
    }

    private static void TextIsChanged(object sender, TextChangedEventArgs e)
    {
      System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;
      if (textBox == null)
        return;
      TextBox.SetTextLength((DependencyObject) textBox, textBox.Text.Length);
    }

    private static void TextBoxGotFocus(object sender, RoutedEventArgs e)
    {
      TextBox textBox = sender as TextBox;
      if (textBox == null || !textBox.SelectAllOnFocus)
        return;
      textBox.Dispatcher.BeginInvoke((Delegate) new Action(((TextBoxBase) textBox).SelectAll));
    }

    private static void TextBoxLoaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is System.Windows.Controls.TextBox))
        return;
      TextBox textBox = sender as TextBox;
      ControlTemplate template = textBox.Template;
      if (template == null)
        return;
      Button name = template.FindName("PART_ClearText", (FrameworkElement) textBox) as Button;
      if (name == null)
        return;
      if (textBox.ClearTextButton)
        name.Click += new RoutedEventHandler(TextBox.ClearTextClicked);
      else
        name.Click -= new RoutedEventHandler(TextBox.ClearTextClicked);
    }

    private static void ClearTextClicked(object sender, RoutedEventArgs e)
    {
      DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject) sender);
      while (!(parent is TextBox))
      {
        parent = VisualTreeHelper.GetParent(parent);
        if (parent == null)
          return;
      }
      TextBox textBox = (TextBox) parent;
      textBox.Focus();
      textBox.Clear();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      if (this.ClearTextButton)
      {
        Button templateChild = this.GetTemplateChild("PART_ClearText") as Button;
        if (templateChild != null)
          templateChild.Click += new RoutedEventHandler(TextBox.ClearTextClicked);
      }
      this.AddHandler(UIElement.TouchDownEvent, (Delegate) new EventHandler<TouchEventArgs>(this.TextBox_TouchDown), true);
      this.AddHandler(UIElement.TouchUpEvent, (Delegate) new EventHandler<TouchEventArgs>(this.TextBox_TouchUp), true);
    }

    private void TextBox_TouchDown(object sender, TouchEventArgs e)
    {
      if (!this.IsEnabled)
        return;
      e.Handled = true;
      TextBox textBox = sender as TextBox;
      if (textBox == null || !this.handleTouchFailure)
        return;
      Button name = textBox.Template.FindName("PART_ClearText", (FrameworkElement) textBox) as Button;
      if (name == null || !name.IsMouseOver)
        return;
      TextBox.ClearTextClicked((object) name, new RoutedEventArgs());
    }

    private void TextBox_TouchUp(object sender, TouchEventArgs e)
    {
      if (!this.IsEnabled)
        return;
      this.RaiseTapEvent();
      TextBox textBox = sender as TextBox;
      if (textBox != null)
      {
        this.handleTouchFailure = !textBox.IsMouseDirectlyOver;
        if (this.handleTouchFailure)
          textBox.Dispatcher.BeginInvoke((Delegate) new Func<bool>(((UIElement) textBox).Focus));
      }
      e.Handled = true;
    }

    public event RoutedEventHandler Tap
    {
      add
      {
        this.AddHandler(TextBox.TapToKeyboardEvent, (Delegate) value);
      }
      remove
      {
        this.RemoveHandler(TextBox.TapToKeyboardEvent, (Delegate) value);
      }
    }

    private void RaiseTapEvent()
    {
      this.RaiseEvent(new RoutedEventArgs(TextBox.TapToKeyboardEvent, (object) this));
    }
  }
}
