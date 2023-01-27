
using System.Windows;

namespace CustomControlLibrary
{
  public class RadioButton : System.Windows.Controls.RadioButton
  {
    static RadioButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RadioButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RadioButton)));
    }
  }
}
