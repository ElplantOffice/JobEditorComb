
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CustomControlLibrary
{
  public class AppBar : StatusBar
  {
    static AppBar()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (AppBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (AppBar)));
    }
  }
}
