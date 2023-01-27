
using System.Runtime.InteropServices;

namespace Communication.Plc.Shared
{
  public struct PlcBaseRefPntrRaw
  {
    [MarshalAs(UnmanagedType.I1)]
    public bool IsArray;
    public ulong Offset;
    public uint Size;
    public short Count;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Type;
  }
}
