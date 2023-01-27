
using System.Windows;

namespace CustomControlLibrary
{
  public class Label : System.Windows.Controls.Label
  {
    static Label()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Label), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Label)));
    }
  }
}
