
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CustomControlLibrary
{
  public class AppBarItem : StatusBarItem
  {
    static AppBarItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (AppBarItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (AppBarItem)));
    }
  }
}
