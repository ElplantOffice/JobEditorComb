
using System.Windows;

namespace CustomControlLibrary
{
  public class Button : System.Windows.Controls.Button
  {
    static Button()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Button), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Button)));
    }
  }
}
