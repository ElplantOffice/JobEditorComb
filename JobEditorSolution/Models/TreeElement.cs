
using Communication.Plc;
using Communication.Plc.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
  public class TreeElement<TTreeElementData> : IPlcMappable
  {
    public Dictionary<uint, TTreeElementData> DataLinks;

    public uint DataId { get; set; }

    public List<TreeElement<TTreeElementData>> FlatTree { get; set; }

    public List<TreeElement<TTreeElementData>> Children { get; set; }

    public TreeElement<TTreeElementData> Parent { get; set; }

    public TTreeElementData Data { get; set; }

    public TreeElementIdentity Identity { get; set; }

    public TreeElementLocation Location { get; set; }

    public CommandHandle Handle { get; set; }

    public ITreeElementCommand<TTreeElementData> Command { get; set; }

    public TreeElement()
      : this(default (TTreeElementData), (Dictionary<uint, TTreeElementData>) null, (ITreeElementCommand<TTreeElementData>) null)
    {
    }

    public TreeElement(TTreeElementData data, Dictionary<uint, TTreeElementData> dataLinks, ITreeElementCommand<TTreeElementData> command = null)
    {
      this.Children = new List<TreeElement<TTreeElementData>>();
      this.Identity = new TreeElementIdentity();
      this.Parent = (TreeElement<TTreeElementData>) null;
      this.Data = data;
      this.Command = command;
      this.DataLinks = dataLinks;
    }

    public void ClearChildren()
    {
      this.Children.Clear();
    }

    public int ChildCount
    {
      get
      {
        if (this.Children == null)
          return 0;
        return this.Children.Count;
      }
    }

    public void AttachCommand(ITreeElementCommand<TTreeElementData> command)
    {
      this.Command = command;
      foreach (TreeElement<TTreeElementData> child in this.Children)
        child.AttachCommand(command);
    }

    public int AddChild(TreeElement<TTreeElementData> childTreeElement)
    {
      childTreeElement.Parent = this;
      this.Children.Add(childTreeElement);
      return this.Children.Count - 1;
    }

    public TreeElement<TTreeElementData> GetChild(int index, int rowSize)
    {
      TreeElement<TTreeElementData> treeElement = (TreeElement<TTreeElementData>) null;
      if (index == 0)
      {
        foreach (TreeElement<TTreeElementData> child in this.Children)
        {
          if (child.Location.Column == 0 || child.Location.Row == 0)
            return child;
        }
        return treeElement;
      }
      int num1 = (index - 1) % rowSize;
      int num2 = (index - 1) / rowSize;
      int num3 = num1 + 1;
      int num4 = num2 + 1;
      foreach (TreeElement<TTreeElementData> child in this.Children)
      {
        if (child.Location.Column == num3 && child.Location.Row == num4)
          return child;
      }
      return treeElement;
    }

    public string Map(List<byte> dataBlock)
    {
      return "";
    }

    public bool Map(object rawType)
    {
            TTreeElementData tTreeElementDatum;
            if (!(rawType is PlcTreeElementStringRaw))
            {
                if (!(rawType is List<object>))
                {
                    return false;
                }
                List<TreeElement<TTreeElementData>> treeElements = new List<TreeElement<TTreeElementData>>();
                foreach (object obj in rawType as List<object>)
                {
                    if (!(obj is PlcTreeElementStringRaw))
                    {
                        continue;
                    }
                    TreeElement<TTreeElementData> treeElement = new TreeElement<TTreeElementData>()
                    {
                        DataLinks = this.DataLinks
                    };
                    if (!treeElement.Map(obj))
                    {
                        continue;
                    }
                    treeElements.Add(treeElement);
                }
                this.ImportList(treeElements);
                return true;
            }
            if (typeof(TTreeElementData) != typeof(UiElementData<string>))
            {
                return false;
            }
            this.DataId = ((PlcTreeElementStringRaw)rawType).DataId;
            if (this.DataLinks.TryGetValue(this.DataId, out tTreeElementDatum))
            {
                this.Data = tTreeElementDatum;
            }
            else
            {
                UiElementData<string> uiElementDatum = new UiElementData<string>();
                if (uiElementDatum.Map(((PlcTreeElementStringRaw)rawType).Data))
                {
                    this.Data = (TTreeElementData)((object)uiElementDatum);
                    this.DataLinks.Add(this.DataId, this.Data);
                }
            }
            TreeElementIdentity treeElementIdentity = new TreeElementIdentity();
            if (treeElementIdentity.Map(((PlcTreeElementStringRaw)rawType).Id))
            {
                this.Identity = treeElementIdentity;
            }
            TreeElementLocation treeElementLocation = new TreeElementLocation();
            if (treeElementLocation.Map(((PlcTreeElementStringRaw)rawType).Location))
            {
                this.Location = treeElementLocation;
            }
            CommandHandle commandHandle = new CommandHandle();
            if (commandHandle.Map(((PlcTreeElementStringRaw)rawType).Handle))
            {
                this.Handle = commandHandle;
            }
            return true;
        }

    private void ImportList(List<TreeElement<TTreeElementData>> list)
    {
      this.FlatTree = this.ToList(this);
      this.FlatTree.OrderBy<TreeElement<TTreeElementData>, int>((Func<TreeElement<TTreeElementData>, int>) (x => x.Identity.Level)).ThenBy<TreeElement<TTreeElementData>, int>((Func<TreeElement<TTreeElementData>, int>) (y => y.Identity.Id));
      foreach (TreeElement<TTreeElementData> treeElement in list)
      {
        if (!this.Update(treeElement))
          this.Add(treeElement);
      }
    }

    private List<TreeElement<TTreeElementData>> ToList(TreeElement<TTreeElementData> source)
    {
      List<TreeElement<TTreeElementData>> result = new List<TreeElement<TTreeElementData>>();
      this.ToList(source, result);
      return result;
    }

    private void ToList(TreeElement<TTreeElementData> source, List<TreeElement<TTreeElementData>> result)
    {
      result.Add(source);
      foreach (TreeElement<TTreeElementData> child in source.Children)
        this.ToList(child, result);
    }

    private bool Update(TreeElement<TTreeElementData> item)
    {
      TreeElement<TTreeElementData> treeElement = this.FlatTree.Where<TreeElement<TTreeElementData>>((Func<TreeElement<TTreeElementData>, bool>) (x => x.Identity.Equals(item.Identity))).FirstOrDefault<TreeElement<TTreeElementData>>();
      if (treeElement == null)
        return false;
      treeElement.DataId = item.DataId;
      treeElement.Data = item.Data;
      treeElement.Handle = item.Handle;
      treeElement.Location = item.Location;
      return true;
    }

    private void Add(TreeElement<TTreeElementData> item)
    {
      TreeElement<TTreeElementData> treeElement = this.FlatTree.Where<TreeElement<TTreeElementData>>((Func<TreeElement<TTreeElementData>, bool>) (x => x.Identity.Equals(item.Identity.GetParentId()))).FirstOrDefault<TreeElement<TTreeElementData>>();
      if (treeElement == null)
        return;
      TreeElement<TTreeElementData> childTreeElement = new TreeElement<TTreeElementData>()
      {
        Data = item.Data,
        Handle = item.Handle,
        Identity = item.Identity,
        Location = item.Location,
        DataLinks = item.DataLinks,
        DataId = item.DataId
      };
      this.FlatTree.Add(childTreeElement);
      treeElement.AddChild(childTreeElement);
    }
  }
}
