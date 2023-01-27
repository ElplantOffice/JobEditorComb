
using System.Runtime.InteropServices;

namespace Communication.Plc.Shared
{
  public struct PlcChannelInfoRaw
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Name;
    [MarshalAs(UnmanagedType.I1)]
    public bool InitDone;
    [MarshalAs(UnmanagedType.I1)]
    public bool Connected;
    [MarshalAs(UnmanagedType.I1)]
    public bool Ready;
    public ulong CommChannelPntr;
  }
}
