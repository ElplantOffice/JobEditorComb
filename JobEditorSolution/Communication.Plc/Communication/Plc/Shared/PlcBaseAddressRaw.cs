
using System.Runtime.InteropServices;

namespace Communication.Plc.Shared
{
  public struct PlcBaseAddressRaw
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string Name;
    public ulong hAddress;
    public ulong NameHash;
    public ulong AddressHash;
  }
}
