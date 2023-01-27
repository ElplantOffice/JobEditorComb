
using System.Windows;

namespace CustomControlLibrary
{
  public class ComboBoxItem : System.Windows.Controls.ComboBoxItem
  {
    static ComboBoxItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ComboBoxItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ComboBoxItem)));
    }
  }
}
