
using System;
using System.Runtime.InteropServices;

namespace Communication.Plc.Shared
{
  public struct PlcUiPrototypeRaw : IMappable
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Client;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Name;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Xaml;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string Model;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string ViewModels;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string Location;

    public string TypeName
    {
      get
      {
        return "T_UiProtoType";
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
      return PlcMapper.Map<PlcUiPrototypeRaw>(dataBlock, this.TypeName);
    }
  }
}
