
using System;
using System.Runtime.InteropServices;

namespace Communication.Plc.Shared
{
  [Serializable]
  public struct PlcControlDataRaw : IMappable
  {
    [MarshalAs(UnmanagedType.I1)]
    public bool EventState;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsEnabled;
    public short ContentData;
    public short Visibility;
    public short EventType;
    public short Localisation;
    public short Format;
    public short Type;
    public uint BackGround;
    [MarshalAs(UnmanagedType.I1)]
    public bool Blink;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string TextId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string ContentText;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
    public string ContentImage;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string StringValue;
    public double RealValue;
    public ulong IntegerValue;
    [MarshalAs(UnmanagedType.I1)]
    public bool BoolValue;

    public string TypeName
    {
      get
      {
        return "T_UiControlData";
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
      return PlcMapper.Map<PlcControlDataRaw>(dataBlock, this.TypeName);
    }
  }
}
