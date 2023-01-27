
using System.Windows;

namespace CustomControlLibrary
{
  public class TreeView : System.Windows.Controls.TreeView
  {
    static TreeView()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (TreeView), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (TreeView)));
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new TreeViewItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return item is TreeViewItem;
    }
  }
}
