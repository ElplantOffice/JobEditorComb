
using System.Windows;

namespace CustomControlLibrary
{
  public class ListBox : System.Windows.Controls.ListBox
  {
    static ListBox()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ListBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ListBox)));
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new ListBoxItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return item is ListBoxItem;
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
    }
  }
}
