
using Communication.Plc;
using Communication.Plc.Shared;
using System.Collections.Generic;

namespace Models
{
  public class TreeElementLocation : IPlcMappable
  {
    public TreeElementLocation()
    {
      this.Row = -1;
      this.Column = -1;
    }

    public bool Map(object rawType)
    {
      if (!(rawType is PlcTreeElementLocationRaw))
        return false;
      PlcTreeElementLocationRaw elementLocationRaw = (PlcTreeElementLocationRaw) rawType;
      this.Row = (int) elementLocationRaw.Row;
      this.Column = (int) elementLocationRaw.Column;
      return true;
    }

    public string Map(List<byte> dataBlock)
    {
      return (string) null;
    }

    public int Row { get; set; }

    public int Column { get; set; }
  }
}
