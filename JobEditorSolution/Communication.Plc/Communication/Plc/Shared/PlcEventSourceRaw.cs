
using System.Runtime.InteropServices;

namespace Communication.Plc.Shared
{
  public struct PlcEventSourceRaw
  {
    [MarshalAs(UnmanagedType.I1)]
    public bool Linked;
    public short Command;
    public short Id;
    public ulong Handle;
    public PlcBaseAddressRaw Target;
    public ulong DataPntrHandle;
    public ulong tAckPntrHandle;
    public ulong ActionHandle;
    public PlcEventSourceSiblings tSiblings;
  }
}
