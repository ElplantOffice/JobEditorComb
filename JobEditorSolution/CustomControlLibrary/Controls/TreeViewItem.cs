
using System.Windows;

namespace CustomControlLibrary
{
  public class TreeViewItem : System.Windows.Controls.TreeViewItem
  {
    static TreeViewItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (TreeViewItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (TreeViewItem)));
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new TreeViewItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return item is TreeViewItem;
    }

    protected override void OnGotFocus(RoutedEventArgs e)
    {
      this.IsSelected = true;
      this.RaiseEvent(e);
    }
  }
}
