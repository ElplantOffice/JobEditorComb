
namespace Models
{
  public class ModelsQueueItem
  {
    public UIProtoType ProtoType { get; private set; }

    public string id { get; private set; }

    public bool IsCreated { get; set; }

    public ModelsQueueItem(string id, UIProtoType uiProtoType)
    {
      this.id = id;
      this.ProtoType = uiProtoType;
      this.IsCreated = false;
    }
  }
}
