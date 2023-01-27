
using System;
using System.Runtime.InteropServices;

namespace Communication.Plc.Shared
{
  [Serializable]
  public struct PlcCommandHandleRaw : IMappable
  {
    public short Type;
    public ulong Handle;
    public short Command;
    public short Alias;
    public ulong DataPntrHandle;
    public PlcBaseRefPntrRaw NextPntr;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string Target;

    public string TypeName
    {
      get
      {
        return "T_CommHandle";
      }
    }

    public Func<byte[], object> Mapper
    {
      get
      {
        return new Func<byte[], object>(this.Map);
      }
    }

    private object Map(byte[] dataBlock)
    {
      return PlcMapper.Map<PlcCommandHandleRaw>(dataBlock, this.TypeName);
    }
  }
}
