
using System.Windows;

namespace CustomControlLibrary
{
  public class ScrollBar : System.Windows.Controls.Primitives.ScrollBar
  {
    static ScrollBar()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ScrollBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ScrollBar)));
    }
  }
}
