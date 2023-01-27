
using System.Windows;

namespace CustomControlLibrary
{
  public class ListBoxItem : System.Windows.Controls.ListBoxItem
  {
    static ListBoxItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ListBoxItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ListBoxItem)));
    }
  }
}
