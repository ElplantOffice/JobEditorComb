
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
  public class ModelsQueue
  {
    public List<ModelsQueueItem> Queues { get; private set; }

    public ModelsQueue()
    {
      this.Queues = new List<ModelsQueueItem>();
    }

    private void Dispose(string id)
    {
      this.Queues.RemoveAll((Predicate<ModelsQueueItem>) (p => p.id == id));
    }

    private bool IsCreated(string id)
    {
      bool flag = true;
      foreach (ModelsQueueItem modelsQueueItem in this.Queues.Where<ModelsQueueItem>((Func<ModelsQueueItem, bool>) (p => p.id == id)).ToList<ModelsQueueItem>())
      {
        if (!modelsQueueItem.IsCreated)
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    public void Add(string id, UIProtoType uiProtoType)
    {
      ModelsQueueItem modelsQueueItem = this.Queues.FirstOrDefault<ModelsQueueItem>((Func<ModelsQueueItem, bool>) (i => i.ProtoType.Model.Address.Owner == uiProtoType.Model.Address.Owner));
      if (modelsQueueItem == null)
        this.Queues.Add(new ModelsQueueItem(id, uiProtoType));
      else
        modelsQueueItem.IsCreated = false;
    }

    public void Dispose(UIProtoType uiProtoType)
    {
      ModelsQueueItem modelsQueueItem = this.Queues.FirstOrDefault<ModelsQueueItem>((Func<ModelsQueueItem, bool>) (i => i.ProtoType.Model.Address.Owner == uiProtoType.Model.Address.Owner));
      if (modelsQueueItem == null)
        return;
      this.Dispose(modelsQueueItem.id);
    }

    public bool IsCreated(UIProtoType uiProtoType)
    {
      ModelsQueueItem modelsQueueItem = this.Queues.FirstOrDefault<ModelsQueueItem>((Func<ModelsQueueItem, bool>) (i => i.ProtoType.Model.Address.Owner == uiProtoType.Model.Address.Owner));
      if (modelsQueueItem != null)
        return this.IsCreated(modelsQueueItem.id);
      return false;
    }

    public bool UpdateCreated(UIProtoType uiProtoType, bool flag)
    {
      ModelsQueueItem modelsQueueItem = this.Queues.FirstOrDefault<ModelsQueueItem>((Func<ModelsQueueItem, bool>) (i => i.ProtoType.Model.Address.Owner == uiProtoType.Model.Address.Owner));
      if (modelsQueueItem == null)
        return false;
      modelsQueueItem.IsCreated = flag;
      return true;
    }
  }
}
