
using System.Windows;

namespace CustomControlLibrary
{
  public class SqRadioButton : System.Windows.Controls.RadioButton
  {
    static SqRadioButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (SqRadioButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (SqRadioButton)));
    }
  }
}
