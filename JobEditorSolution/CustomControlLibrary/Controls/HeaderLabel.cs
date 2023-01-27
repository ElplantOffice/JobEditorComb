
using System.Windows;

namespace CustomControlLibrary
{
  public class HeaderLabel : System.Windows.Controls.Label
  {
    static HeaderLabel()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (HeaderLabel), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (HeaderLabel)));
    }
  }
}
