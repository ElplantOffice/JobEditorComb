
using Communication.Plc;
using Communication.Plc.Shared;
using System.Collections.Generic;

namespace Models
{
  public class TreeElementIdentity : IPlcMappable
  {
    public TreeElementIdentity()
    {
      this.Id = this.Level = 0;
      this.Parent = -1;
    }

    public TreeElementIdentity GetParentId()
    {
      return new TreeElementIdentity()
      {
        Id = this.Parent,
        Level = this.Level - 1,
        Parent = -1
      };
    }

    public bool Map(object rawType)
    {
      if (!(rawType is PlcTreeElementIdentityRaw))
        return false;
      PlcTreeElementIdentityRaw elementIdentityRaw = (PlcTreeElementIdentityRaw) rawType;
      this.Id = (int) elementIdentityRaw.Id;
      this.Parent = (int) elementIdentityRaw.ParentId;
      this.Level = (int) elementIdentityRaw.LevelId;
      return true;
    }

    public string Map(List<byte> dataBlock)
    {
      return (string) null;
    }

    public void AsChildOf(TreeElementIdentity parentIdentity, int id)
    {
      this.Id = id;
      this.Parent = parentIdentity.Id;
      this.Level = parentIdentity.Level + 1;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      TreeElementIdentity identity = obj as TreeElementIdentity;
      if (identity == null)
        return false;
      return this.Equals(identity);
    }

    public bool Equals(TreeElementIdentity identity)
    {
      if (this.Id == identity.Id)
        return this.Level == identity.Level;
      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public string GetKey()
    {
      return this.Id.ToString() + (object) '.' + this.Level.ToString() + (object) '.' + this.Parent.ToString();
    }

    public int Id { get; set; }

    public int Parent { get; set; }

    public int Level { get; set; }
  }
}
