
using System.Windows;
using System.Windows.Input;

namespace Models
{
  public class EnterKeyTraversal
  {
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof (bool), typeof (EnterKeyTraversal), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(EnterKeyTraversal.IsEnabledChanged)));

    public static bool GetIsEnabled(DependencyObject obj)
    {
      return (bool) obj.GetValue(EnterKeyTraversal.IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject obj, bool value)
    {
      obj.SetValue(EnterKeyTraversal.IsEnabledProperty, (object) value);
    }

    private static void ue_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      FrameworkElement originalSource = e.OriginalSource as FrameworkElement;
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      originalSource.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
    }

    private static void ue_Unloaded(object sender, RoutedEventArgs e)
    {
      FrameworkElement frameworkElement = sender as FrameworkElement;
      if (frameworkElement == null)
        return;
      frameworkElement.Unloaded -= new RoutedEventHandler(EnterKeyTraversal.ue_Unloaded);
      frameworkElement.PreviewKeyDown -= new KeyEventHandler(EnterKeyTraversal.ue_PreviewKeyDown);
    }

    private static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      FrameworkElement frameworkElement = d as FrameworkElement;
      if (frameworkElement == null)
        return;
      if ((bool) e.NewValue)
      {
        frameworkElement.Unloaded += new RoutedEventHandler(EnterKeyTraversal.ue_Unloaded);
        frameworkElement.PreviewKeyDown += new KeyEventHandler(EnterKeyTraversal.ue_PreviewKeyDown);
      }
      else
        frameworkElement.PreviewKeyDown -= new KeyEventHandler(EnterKeyTraversal.ue_PreviewKeyDown);
    }
  }
}
