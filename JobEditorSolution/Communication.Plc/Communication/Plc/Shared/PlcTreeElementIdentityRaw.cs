
using System;
using System.Runtime.InteropServices;

namespace Communication.Plc.Shared
{
  [Serializable]
  public struct PlcTreeElementIdentityRaw
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Name;
    public short Id;
    public short ParentId;
    public short LevelId;
    public short ChildCount;
    public ulong Parent;
    public ulong ChildFirst;
    public ulong ChildLast;
    public ulong NextItem;
    public ulong PrevItem;
  }
}
