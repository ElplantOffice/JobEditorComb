
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
  public class TreeCollection<TTreeElementData, TTreeElementControl>
  {
    public Action<CommandHandle> CommandHandleAction;
    private TreeElement<TTreeElementData> rootTreeElement;
    private List<TTreeElementControl> attachedTreeElementControls;
    private Dictionary<uint, List<TreeElement<TTreeElementData>>> lookUp;
    private Dictionary<uint, TTreeElementData> dataLinks;

    private int rowSize { get; set; }

    public ITreeCollectionCommand<TTreeElementData, TTreeElementControl> Command { get; set; }

    public TTreeElementData InactiveTreeElementDataTemplate { get; set; }

    public TreeElement<TTreeElementData> SelectedTreeElement { get; private set; }

    public Func<Telegram, bool> PrePublishAction { get; set; }

    public TreeCollection(int rowSize, Action<CommandHandle> commandHandleAction)
    {
      this.rowSize = rowSize;
      this.CommandHandleAction = commandHandleAction;
      this.attachedTreeElementControls = new List<TTreeElementControl>();
      this.SelectedTreeElement = (TreeElement<TTreeElementData>) null;
    }

    public void AttachCommand(ITreeElementCommand<TTreeElementData> command)
    {
      foreach (TreeElement<TTreeElementData> child in this.rootTreeElement.Children)
        child.AttachCommand(command);
    }

    public void AttachRootTreeElement(TreeElement<TTreeElementData> rootTreeElement)
    {
      this.rootTreeElement = rootTreeElement;
      this.dataLinks = rootTreeElement.DataLinks;
      this.SelectRoot();
    }

    public void AttachTreeElementControl(int index, TTreeElementControl treeElementControl)
    {
      if (index < 0)
        throw new ArgumentException("index < 0");
      while (this.attachedTreeElementControls.Count <= index)
        this.attachedTreeElementControls.Add(default (TTreeElementControl));
      this.attachedTreeElementControls[index] = treeElementControl;
    }

    public void UpdateAttachedTreeElementData(bool enaUpdateMenuInfo)
    {
      if (this.Command == null)
        return;
      for (int index = 0; index < this.attachedTreeElementControls.Count; ++index)
      {
        TreeElement<TTreeElementData> treeElement = (TreeElement<TTreeElementData>) null;
        if (this.SelectedTreeElement != null)
          treeElement = this.SelectedTreeElement.GetChild(index, this.rowSize);
        TTreeElementControl treeElementControl = this.attachedTreeElementControls[index];
        if (treeElement != null)
        {
          Telegram telegram = new Telegram((IAddress) new Address("", "", "", (string) null), 0, (object) treeElement.Data, (string) null);
          if (this.PrePublishAction != null)
          {
            int num = this.PrePublishAction(telegram) ? 1 : 0;
          }
          this.Command.Update(treeElement.Data, treeElementControl, index);
        }
        else
          this.Command.Update(this.InactiveTreeElementDataTemplate, treeElementControl, index);
      }
      if (!enaUpdateMenuInfo)
        return;
      this.Command.UpdateInfo();
    }

    public bool UpdateMenuInfoIfSelectedElement(TreeElement<TTreeElementData> selectedElement)
    {
      if (selectedElement == null)
        return false;
      UiElementData<string> data = (object) selectedElement.Data as UiElementData<string>;
      if (data.ContentText != null)
        this.Command.UpdateInfo(data.ContentText);
      else
        this.Command.UpdateInfo();
      return true;
    }

    public void UpdateElement(TreeElement<TTreeElementData> element)
    {
      bool flag = false;
      if (this.lookUp == null)
        return;
      List<TreeElement<TTreeElementData>> treeElementList;
      if (this.lookUp.TryGetValue(element.DataId, out treeElementList))
      {
        foreach (TreeElement<TTreeElementData> treeElement in treeElementList)
        {
          treeElement.Data = element.Data;
          if (this.isVisual(treeElement.Identity))
            flag = true;
        }
      }
      if (!flag)
        return;
      this.UpdateAttachedTreeElementData(true);
    }

    private bool isVisual(TreeElementIdentity identity)
    {
      return this.SelectedTreeElement.Identity.Id == identity.Parent;
    }

    private bool Select(TreeElement<TTreeElementData> treeElement, bool enaUpdateMenuInfo)
    {
      if (treeElement == null)
        return false;
      this.SelectedTreeElement = treeElement;
      this.UpdateAttachedTreeElementData(enaUpdateMenuInfo);
      return true;
    }

    public bool SelectRoot()
    {
      return this.Select(this.rootTreeElement, true);
    }

    public bool SelectMenuWithId(int dataId, bool enaUpdateMenuInfo, bool executeAction)
    {
      if (this.rootTreeElement == null)
        return false;
      TreeElement<TTreeElementData> treeElement = this.rootTreeElement.FlatTree.Where<TreeElement<TTreeElementData>>((Func<TreeElement<TTreeElementData>, bool>) (x => (long) x.DataId == (long) dataId)).FirstOrDefault<TreeElement<TTreeElementData>>();
      if (!this.Select(treeElement, enaUpdateMenuInfo))
        return false;
      if (executeAction && treeElement.Handle.Target != null)
        this.CommandHandleAction(treeElement.Handle);
      return true;
    }

    public bool SelectLevel(int level)
    {
      if (this.SelectedTreeElement == null)
        return false;
      TreeElement<TTreeElementData> treeElement = this.SelectedTreeElement;
      while (treeElement.Identity.Level != level && treeElement.Parent != null)
        treeElement = treeElement.Parent;
      return this.Select(treeElement, true);
    }

    public bool SelectChild(int childIndex)
    {
      if (childIndex < 0 || childIndex >= this.attachedTreeElementControls.Count)
        return false;
      TTreeElementControl treeElementControl = this.attachedTreeElementControls[childIndex];
      TreeElement<TTreeElementData> child = this.SelectedTreeElement.GetChild(childIndex, this.rowSize);
      if (child == null || child.ChildCount <= 0)
        return false;
      return this.Select(child, true);
    }

    public bool SelectParent()
    {
      if (this.SelectedTreeElement == null)
        return false;
      return this.Select(this.SelectedTreeElement.Parent, true);
    }

    public void CreateLookup()
    {
      if (this.lookUp == null)
        this.lookUp = new Dictionary<uint, List<TreeElement<TTreeElementData>>>();
      this.lookUp.Clear();
      foreach (TreeElement<TTreeElementData> treeElement1 in this.rootTreeElement.FlatTree)
      {
        List<TreeElement<TTreeElementData>> treeElementList;
        if (!this.lookUp.TryGetValue(treeElement1.DataId, out treeElementList))
        {
          treeElementList = new List<TreeElement<TTreeElementData>>();
          treeElementList.Add(treeElement1);
          this.lookUp.Add(treeElement1.DataId, treeElementList);
        }
        else
        {
          TreeElement<TTreeElementData> treeElement2 = treeElementList[0];
          treeElement1.Handle = treeElement2.Handle;
          treeElementList.Add(treeElement1);
        }
      }
    }

    public void RunCommand(TreeElement<TTreeElementData> child)
    {
      if (child == null)
        return;
      switch (child.Handle.Type)
      {
        case 0:
          TreeElement<TTreeElementData> rootTreeElement1 = this.rootTreeElement;
          TreeElement<TTreeElementData> current1 = child;
          int command1 = current1.Handle.Command;
          this.Select(this.Navigate(rootTreeElement1, current1, command1), true);
          break;
        case 4:
          TreeElement<TTreeElementData> rootTreeElement2 = this.rootTreeElement;
          TreeElement<TTreeElementData> current2 = child;
          int command2 = current2.Handle.Command;
          this.Select(this.Navigate(rootTreeElement2, current2, command2), true);
          if (child.Handle.Target == null)
            break;
          this.CommandHandleAction(child.Handle);
          break;
        case 5:
          TreeElement<TTreeElementData> rootTreeElement3 = this.rootTreeElement;
          TreeElement<TTreeElementData> current3 = child;
          int command3 = current3.Handle.Command;
          this.Select(this.NavigateToNode(rootTreeElement3, current3, command3), true);
          if (child.Handle.Target == null)
            break;
          this.CommandHandleAction(child.Handle);
          break;
        case 6:
          TreeElement<TTreeElementData> rootTreeElement4 = this.rootTreeElement;
          TreeElement<TTreeElementData> current4 = child;
          int command4 = current4.Handle.Command;
          this.Select(this.Navigate(rootTreeElement4, current4, command4), false);
          break;
        case 7:
          TreeElement<TTreeElementData> rootTreeElement5 = this.rootTreeElement;
          TreeElement<TTreeElementData> current5 = child;
          int command5 = current5.Handle.Command;
          this.Select(this.Navigate(rootTreeElement5, current5, command5), false);
          if (child.Handle.Target == null)
            break;
          this.CommandHandleAction(child.Handle);
          break;
        case 8:
          TreeElement<TTreeElementData> rootTreeElement6 = this.rootTreeElement;
          TreeElement<TTreeElementData> current6 = child;
          int command6 = current6.Handle.Command;
          this.Select(this.NavigateToNode(rootTreeElement6, current6, command6), false);
          if (child.Handle.Target == null)
            break;
          this.CommandHandleAction(child.Handle);
          break;
        default:
          this.CommandHandleAction(child.Handle);
          break;
      }
    }

    private TreeElement<TTreeElementData> NavigateToNode(TreeElement<TTreeElementData> root, TreeElement<TTreeElementData> current, int dataId)
    {
      current = root.FlatTree.Where<TreeElement<TTreeElementData>>((Func<TreeElement<TTreeElementData>, bool>) (x => (long) x.DataId == (long) dataId)).FirstOrDefault<TreeElement<TTreeElementData>>();
      if (current.Children.Count <= 0)
        return (TreeElement<TTreeElementData>) null;
      return current;
    }

    private TreeElement<TTreeElementData> Navigate(TreeElement<TTreeElementData> root, TreeElement<TTreeElementData> current, int relativeMove)
    {
      if (relativeMove == 3840)
        return root;
      if (relativeMove > 0)
      {
        if (current.Children.Count <= 0)
          return (TreeElement<TTreeElementData>) null;
        return current;
      }
      for (int index = Math.Abs(relativeMove); index >= 0; --index)
      {
        current = root.FlatTree.Where<TreeElement<TTreeElementData>>((Func<TreeElement<TTreeElementData>, bool>) (x => x.Identity.Equals(current.Identity.GetParentId()))).FirstOrDefault<TreeElement<TTreeElementData>>();
        if (current == null)
          return (TreeElement<TTreeElementData>) null;
      }
      if (current.Children.Count <= 0)
        return (TreeElement<TTreeElementData>) null;
      return current;
    }

    public bool Execute(int childIndex)
    {
      if (this.attachedTreeElementControls == null || childIndex < 0 || childIndex >= this.attachedTreeElementControls.Count)
        return false;
      TTreeElementControl treeElementControl = this.attachedTreeElementControls[childIndex];
      this.RunCommand(this.SelectedTreeElement.GetChild(childIndex, this.rowSize));
      return true;
    }
  }
}
