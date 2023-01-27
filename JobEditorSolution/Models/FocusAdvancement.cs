
using System.Windows;
using System.Windows.Input;

namespace Models
{
  public static class FocusAdvancement
  {
    public static readonly DependencyProperty AdvancesByEnterKeyProperty = DependencyProperty.RegisterAttached("AdvancesByEnterKey", typeof (bool), typeof (FocusAdvancement), (PropertyMetadata) new UIPropertyMetadata(new PropertyChangedCallback(FocusAdvancement.OnAdvancesByEnterKeyPropertyChanged)));

    public static bool GetAdvancesByEnterKey(DependencyObject obj)
    {
      return (bool) obj.GetValue(FocusAdvancement.AdvancesByEnterKeyProperty);
    }

    public static void SetAdvancesByEnterKey(DependencyObject obj, bool value)
    {
      obj.SetValue(FocusAdvancement.AdvancesByEnterKeyProperty, (object) value);
    }

    private static void OnAdvancesByEnterKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      UIElement uiElement = d as UIElement;
      if (uiElement == null)
        return;
      if ((bool) e.NewValue)
        uiElement.KeyDown += new KeyEventHandler(FocusAdvancement.Keydown);
      else
        uiElement.KeyDown -= new KeyEventHandler(FocusAdvancement.Keydown);
    }

    private static void Keydown(object sender, KeyEventArgs e)
    {
      if (!e.Key.Equals((object) Key.Return))
        return;
      (sender as UIElement)?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
    }
  }
}
