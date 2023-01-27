
using System.Windows;

namespace CustomControlLibrary
{
  public class BulletButton : System.Windows.Controls.Button
  {
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool), typeof (BulletButton), new PropertyMetadata((object) false));

    static BulletButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (BulletButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (BulletButton)));
    }

    public bool IsChecked
    {
      get
      {
        return (bool) this.GetValue(BulletButton.IsCheckedProperty);
      }
      set
      {
        this.SetValue(BulletButton.IsCheckedProperty, (object) value);
      }
    }
  }
}
