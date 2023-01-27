
using System.Windows;

namespace CustomControlLibrary
{
  public class SubHeaderLabel : System.Windows.Controls.Label
  {
    static SubHeaderLabel()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (SubHeaderLabel), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (SubHeaderLabel)));
    }
  }
}
