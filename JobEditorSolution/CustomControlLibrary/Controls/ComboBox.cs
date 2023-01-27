
using System;
using System.Windows;
using System.Windows.Input;

namespace CustomControlLibrary
{
  public class ComboBox : System.Windows.Controls.ComboBox
  {
    public static readonly DependencyProperty DropDownSymbolProperty = DependencyProperty.Register(nameof (DropDownSymbol), typeof (string), typeof (ComboBox), new PropertyMetadata((object) "⋁"));
    public static readonly RoutedEvent TapToKeyboardEvent = EventManager.RegisterRoutedEvent("TapToKeyboard", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (ComboBox));

    static ComboBox()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ComboBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ComboBox)));
    }

    public string DropDownSymbol
    {
      get
      {
        return (string) this.GetValue(ComboBox.DropDownSymbolProperty);
      }
      set
      {
        this.SetValue(ComboBox.DropDownSymbolProperty, (object) value);
      }
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.AddHandler(UIElement.TouchDownEvent, (Delegate) new EventHandler<TouchEventArgs>(this.ComboBox_TouchDown), true);
      this.AddHandler(UIElement.MouseDownEvent, (Delegate) new RoutedEventHandler(this.ComboBox_MouseDown), true);
      this.AddHandler(UIElement.TouchUpEvent, (Delegate) new EventHandler<TouchEventArgs>(this.ComboBox_TouchUp), true);
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new ComboBoxItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return item is ComboBoxItem;
    }

    private void ComboBox_MouseDown(object sender, RoutedEventArgs e)
    {
      if (!this.IsEnabled)
        return;
      e.Handled = true;
    }

    private void ComboBox_TouchDown(object sender, TouchEventArgs e)
    {
      if (!this.IsEnabled)
        return;
      e.Handled = true;
    }

    private void ComboBox_TouchUp(object sender, TouchEventArgs e)
    {
      if (!this.IsEnabled)
        return;
      this.RaiseTapEvent();
      e.Handled = true;
    }

    public event RoutedEventHandler Tap
    {
      add
      {
        this.AddHandler(ComboBox.TapToKeyboardEvent, (Delegate) value);
      }
      remove
      {
        this.RemoveHandler(ComboBox.TapToKeyboardEvent, (Delegate) value);
      }
    }

    private void RaiseTapEvent()
    {
      this.RaiseEvent(new RoutedEventArgs(ComboBox.TapToKeyboardEvent, (object) this));
    }
  }
}
