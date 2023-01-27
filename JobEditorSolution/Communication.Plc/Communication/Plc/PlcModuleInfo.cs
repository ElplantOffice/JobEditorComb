
using Communication.Plc.Ads;
using Communication.Plc.Channel;
using Communication.Plc.Shared;
using System.Collections.Generic;
using System.Linq;
using TwinCAT.Ads;

namespace Communication.Plc
{
  public class PlcModuleInfo
  {
    public PlcAddressRegistry AddressRegistry { get; private set; }

    public PlcEventRegistry EventRegistry { get; private set; }

    public List<PlcChannelInfo> PlcChannelInfoList { get; private set; }

    public PlcAddress Ancestor { get; private set; }

    public AdsSettings Settings { get; private set; }

    public ulong AdministratorAddress { get; set; }

    public ulong MappingOffset { get; set; }

    public TcAdsSymbolInfo AdministratorSymbol { get; set; }

    public TcAdsSymbolInfo ComChannelInfoArraySymbol { get; set; }

    public TcAdsSymbolInfo AdminComChannelInfoSymbol { get; set; }

    public ulong AdsOffsetToAddress(uint adsOffset)
    {
      return (ulong) adsOffset + this.MappingOffset;
    }

    public uint AddressToAdsOffset(ulong address)
    {
      return (uint) (address - this.MappingOffset);
    }

    public PlcModuleInfo(AdsSettings settings, PlcAddress ancestor)
    {
      this.Ancestor = ancestor;
      this.Settings = settings;
      this.PlcChannelInfoList = new List<PlcChannelInfo>();
    }

    public PlcModuleInfo(PlcModuleInfo src, AdsSettings settings)
    {
      this.Settings = settings;
      this.Ancestor = src.Ancestor;
      this.PlcChannelInfoList = src.PlcChannelInfoList;
      this.AddressRegistry = src.AddressRegistry;
    }

    public void CalcOffset()
    {
      this.MappingOffset = this.AdministratorAddress - (ulong) (uint) this.AdministratorSymbol.IndexOffset;
    }

    public bool Connect()
    {
      if (this.IsConnected())
        return true;
      AdsClient adsClient = this.PlcChannelInfoList.ElementAt<PlcChannelInfo>(0).AdsClient;
      if (!adsClient.IsConnected)
        adsClient.Connect(this.Settings.AmsAddress);
      if (!adsClient.IsConnected)
        return false;
      AdsClient.Query query = new AdsClient.Query(adsClient.Contains);
      TcAdsSymbolInfoCollection allSymbols = adsClient.GetAllSymbols();
      this.AdministratorSymbol = adsClient.QuerySymbolsFlat(query, allSymbols, "", this.Settings.CommAdministratorSymbol.Type, 0L, 0L);
      if (this.AdministratorSymbol == null)
        return false;
      TcAdsSymbolInfo tcAdsSymbolInfo = adsClient.QuerySymbols(query, this.AdministratorSymbol.SubSymbols, "", this.Settings.BaseObjectDataSymbol.Type, 0L, 0L);
      if (tcAdsSymbolInfo == null)
        return false;
      TcAdsSymbolInfo symbol = adsClient.QuerySymbols(query, tcAdsSymbolInfo.SubSymbols, this.Settings.BaseObjectAdressSymbol.Name, this.Settings.BaseObjectAdressSymbol.Type, 0L, 0L);
      if (symbol == null)
        return false;
      this.AdministratorAddress = (ulong) adsClient.Read<long>(symbol);
      this.CalcOffset();
      this.ComChannelInfoArraySymbol = adsClient.QuerySymbols(query, this.AdministratorSymbol.SubSymbols, "", this.Settings.CommChannelInfoSymbol.Type, 0L, 0L);
      if (this.ComChannelInfoArraySymbol == null)
        return false;
      this.AdminComChannelInfoSymbol = adsClient.QuerySymbols(query, this.ComChannelInfoArraySymbol.SubSymbols, "", this.Settings.CommChannelInfoSymbol.Type, 0L, 0L);
      return this.AdminComChannelInfoSymbol != null && adsClient.Read<PlcChannelInfoRaw>(this.AdminComChannelInfoSymbol).InitDone;
    }

    public bool IsConnected()
    {
      return this.AdminComChannelInfoSymbol != null && this.PlcChannelInfoList.Count != 0 && this.PlcChannelInfoList.ElementAt<PlcChannelInfo>(0).AdsClient.Read<PlcChannelInfoRaw>(this.AdminComChannelInfoSymbol).InitDone;
    }

    public void AddChannel(PlcChannelInfo plcChannelInfo)
    {
      this.PlcChannelInfoList.Add(plcChannelInfo);
      if (this.AddressRegistry != null)
        return;
      this.AddressRegistry = new PlcAddressRegistry(this.PlcChannelInfoList.ElementAt<PlcChannelInfo>(0));
      this.EventRegistry = new PlcEventRegistry(this.PlcChannelInfoList.ElementAt<PlcChannelInfo>(0));
    }
  }
}
