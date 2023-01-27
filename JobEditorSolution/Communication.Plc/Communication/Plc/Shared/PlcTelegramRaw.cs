
using System;

namespace Communication.Plc.Shared
{
  public struct PlcTelegramRaw : IMappable
  {
    public PlcCommAddressRaw CommAddress;
    public PlcBaseRefPntrRaw CommDataPntr;
    public ulong DataPntrHandle;
    public ulong AckPntrHandle;
    public PlcEventSinkRaw EventSink;

    public string TypeName
    {
      get
      {
        return "T_CommTelegram";
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
      return PlcMapper.Map<PlcTelegramRaw>(dataBlock, this.TypeName);
    }
  }
}
