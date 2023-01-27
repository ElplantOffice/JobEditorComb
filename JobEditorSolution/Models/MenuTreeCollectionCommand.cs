
using Messages;
using Patterns.EventAggregator;
using System.Collections.Generic;

namespace Models
{
  public class MenuTreeCollectionCommand : ITreeCollectionCommand<UiElementData<string>, ModelAttributedEventType<string>>
  {
    private EventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;
    private TreeCollection<UiElementData<string>, ModelAttributedEventType<string>> treeCollection;
    private List<ModelAttributedEventType<string>> menuButtons;
    private ModelAttributedEventType<string> infoMenu;

    public MenuTreeCollectionCommand(List<ModelAttributedEventType<string>> menuButtons = null, ModelAttributedEventType<string> infoCommand = null, ModelAttributedEventType<string> infoMenu = null)
    {
      this.InfoCommand = infoCommand;
      this.menuButtons = menuButtons;
      this.infoMenu = infoMenu;
    }

    public void Init(TreeCollection<UiElementData<string>, ModelAttributedEventType<string>> treeCollection)
    {
      this.treeCollection = treeCollection;
      this.UpdateInfo();
    }

    public bool Execute(TreeElement<UiElementData<string>> element, ModelAttributedEventType<string> controlitem, int index)
    {
      bool flag;
      switch (index)
      {
        case 0:
          flag = this.treeCollection.SelectRoot();
          break;
        case 1:
          flag = this.treeCollection.SelectParent();
          break;
        default:
          flag = this.treeCollection.SelectChild(index);
          break;
      }
      if (flag)
      {
        this.UpdateInfo();
        this.InfoCommand.Value = "";
      }
      return flag;
    }

    public void SelectRoot(Telegram telegram)
    {
      this.treeCollection.SelectRoot();
    }

    public bool Update(UiElementData<string> dataitem, ModelAttributedEventType<string> controlitem, int index)
    {
      controlitem.Data = dataitem;
      return true;
    }

    public void UpdateInfo()
    {
      if (this.infoMenu == null)
        return;
      this.infoMenu.ContentText = this.GetFullMenuSelected(" \\ ", "");
    }

    public void UpdateInfo(string info)
    {
      if (this.infoMenu == null)
        return;
      this.infoMenu.ContentText = info;
    }

    private string GetFullMenuSelected(string menuSeparator, string rootMenuName = null)
    {
      string str1 = "";
      for (TreeElement<UiElementData<string>> treeElement = this.treeCollection.SelectedTreeElement; treeElement != null; treeElement = treeElement.Parent)
      {
        string str2 = "";
        if (treeElement.Data != null && treeElement.Data.ContentText != null)
          str2 = treeElement.Data.ContentText;
        if (str1.Length == 0)
          str1 = str2;
        else if (str2.Length != 0)
          str1 = str1.Insert(0, menuSeparator).Insert(0, str2);
      }
      if (rootMenuName != null)
        str1 = str1.Length != 0 ? str1.Insert(0, rootMenuName) : rootMenuName;
      return str1;
    }

    public ModelAttributedEventType<string> InfoCommand { get; private set; }
  }
}
