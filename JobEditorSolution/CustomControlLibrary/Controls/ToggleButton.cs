
using System.Windows;

namespace CustomControlLibrary
{
  public class ToggleButton : System.Windows.Controls.Primitives.ToggleButton
  {
    static ToggleButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ToggleButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ToggleButton)));
    }
  }
}
