
using System.Runtime.InteropServices;

namespace Messages
{
  [StructLayout(LayoutKind.Sequential)]
  public class UiPrototype
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Client;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Xaml;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Model;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string ViemModel;
  }
}
