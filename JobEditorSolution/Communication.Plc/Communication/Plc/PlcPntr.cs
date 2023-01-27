
using System;
using TwinCAT.Ads;

namespace Communication.Plc
{
  public class PlcPntr
  {
    public uint Group { get; set; }

    public uint Offset { get; set; }

    public TcAdsSymbolInfo Symbol { get; set; }

    public Type Type { get; set; }
  }
}
