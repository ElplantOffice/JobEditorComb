
using Communication.Plc.Shared;
using System.Collections.Generic;
using System.IO;

namespace Communication.Plc.Channel
{
  public class PlcServices
  {
    private PlcChannelInfo plcChannelInfo;

    public PlcServices(PlcChannelInfo plcChannelInfo)
    {
      this.plcChannelInfo = plcChannelInfo;
    }

    public bool GetEventSink(ulong handle, out PlcEventSinkRaw sink)
    {
      return this.plcChannelInfo.PlcModuleInfo.EventRegistry.GetSink(handle, out sink);
    }

    public bool GetEventSource(ulong handle, out PlcEventSourceRaw source)
    {
      return this.plcChannelInfo.PlcModuleInfo.EventRegistry.GetSource(handle, out source);
    }

    public PlcBaseRefPntrRaw GetRefPntr(ulong handle)
    {
      return this.plcChannelInfo.AdsClient.Read<PlcBaseRefPntrRaw>(this.plcChannelInfo.ChannelSymbol.IndexGroup, (long) this.plcChannelInfo.AddressToAdsOffset(handle));
    }

    public void WriteObject(object data, PlcBaseRefPntrRaw pntr)
    {
      if (pntr.IsArray)
      {
        if (data is List<object>)
          this.WriteArrayByRef(data as List<object>, pntr.Offset);
        else
          this.WriteByRef(data, pntr.Offset);
      }
      else
        this.WriteByRef(data, pntr.Offset);
    }

    public void WriteArrayByRef(List<object> data, ulong offset)
    {
      uint adsOffset = this.plcChannelInfo.AddressToAdsOffset(offset);
      List<byte> byteList = new List<byte>();
      foreach (object obj in data)
        byteList.AddRange((IEnumerable<byte>) PlcMapper.GetBytes(obj));
      this.plcChannelInfo.AdsClient.WriteArray<byte>((long) (int) this.plcChannelInfo.ChannelSymbol.IndexGroup, (long) adsOffset, byteList.ToArray());
    }

    public void WriteByRef(object data, ulong offset)
    {
      uint adsOffset = this.plcChannelInfo.AddressToAdsOffset(offset);
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) PlcMapper.GetBytes(data));
      this.plcChannelInfo.AdsClient.WriteArray<byte>((long) (int) this.plcChannelInfo.ChannelSymbol.IndexGroup, (long) adsOffset, byteList.ToArray());
    }

    public object ReadObject(PlcBaseRefPntrRaw pntr)
    {
      if (pntr.IsArray)
        return this.ReadArrayObject(pntr);
      return PlcMapper.Map(this.ReadBuffer(pntr), pntr.Type);
    }

    public object ReadArrayObject(PlcBaseRefPntrRaw pntr)
    {
      List<object> objectList = new List<object>();
      foreach (byte[] datablock in this.ReadArrayBuffer(pntr))
        objectList.Add(PlcMapper.Map(datablock, pntr.Type));
      return (object) objectList;
    }

    public List<byte[]> ReadArrayBuffer(PlcBaseRefPntrRaw pntr)
    {
      List<byte[]> numArrayList = new List<byte[]>();
      for (int index = 0; index < (int) pntr.Count; ++index)
      {
        numArrayList.Add(this.ReadBuffer(pntr));
        pntr.Offset += (ulong) pntr.Size;
      }
      return numArrayList;
    }

    public byte[] ReadBuffer(PlcBaseRefPntrRaw pntr)
    {
      return ((BinaryReader) this.plcChannelInfo.AdsClient.ReadBlock((int) this.plcChannelInfo.ChannelSymbol.IndexGroup, (int) this.plcChannelInfo.AddressToAdsOffset(pntr.Offset), pntr.Size)).ReadBytes((int) pntr.Size);
    }

    public BinaryReader Read(PlcBaseRefPntrRaw pntr)
    {
      return (BinaryReader) this.plcChannelInfo.AdsClient.ReadBlock((int) this.plcChannelInfo.ChannelSymbol.IndexGroup, (int) this.plcChannelInfo.AddressToAdsOffset(pntr.Offset), pntr.Size);
    }

    public object ReadByRefPntrHandle(ulong handle)
    {
      return this.ReadObject(this.GetRefPntr(handle));
    }

    public bool IsPlcAddress(string address)
    {
      string str = this.PlcName.Remove(this.PlcName.IndexOf("."));
      return address.Contains(str);
    }

    public string AddChannel(string address)
    {
      address = address.Remove(0, address.IndexOf("."));
      address = this.PlcName + address;
      return address;
    }

    public string PlcName
    {
      get
      {
        return this.plcChannelInfo.Name;
      }
    }
  }
}
