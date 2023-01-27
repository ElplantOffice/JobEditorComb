
using Communication.Plc.Ads;
using Communication.Plc.Shared;
using System.Text;
using TwinCAT.Ads;

namespace Communication.Plc.Channel
{
  public class PlcChannelInfo
  {
    private PlcAddress plcAddress;

    public string Name { get; private set; }

    public PlcChannelInfo.PlcChannelId ChannelId { get; private set; }

    public AdsClient AdsClient { get; private set; }

    public TcAdsSymbolInfo ChannelInfoSymbol { get; set; }

    public TcAdsSymbolInfo ChannelSymbol { get; set; }

    public TcAdsSymbolInfo AddressRegistrySymbol { get; set; }

    public TcAdsSymbolInfo AddressRegistryCountSymbol { get; set; }

    public TcAdsSymbolInfo AddressRegistryDataSymbol { get; set; }

    public TcAdsSymbolInfo AggregatorSymbol { get; set; }

    public TcAdsSymbolInfo EventSinkCountSymbol { get; set; }

    public TcAdsSymbolInfo EventSinkDataSymbol { get; set; }

    public TcAdsSymbolInfo EventSourceCountSymbol { get; set; }

    public TcAdsSymbolInfo EventSourceDataSymbol { get; set; }

    public TcAdsSymbolInfo NotifyPcHasWrittenReplySymbol { get; set; }

    public TcAdsSymbolInfo NotifyPlcHasWrittenSymbol { get; set; }

    public int NotifyPcHasWrittenReplyHandle { get; set; }

    public int NotifyPlcHasWrittenHandle { get; set; }

    public TcAdsSymbolInfo PcHasWrittenSymbol { get; set; }

    public TcAdsSymbolInfo PlcHasWrittenReplySymbol { get; set; }

    public TcAdsSymbolInfo PlcIsReadySymbol { get; set; }

    public TcAdsSymbolInfo PcIsReadySymbol { get; set; }

    public TcAdsSymbolInfo ConnectedSymbol { get; set; }

    public TcAdsSymbolInfo MessageInCountSymbol { get; set; }

    public TcAdsSymbolInfo MessageOutCountSymbol { get; set; }

    public TcAdsSymbolInfo MessageInQueueSymbol { get; set; }

    public TcAdsSymbolInfo MessageOutQueueSymbol { get; set; }

    public TcAdsSymbolInfo MessageOutDataSymbol { get; set; }

    public PlcModuleInfo PlcModuleInfo { get; private set; }

    public PlcChannelInfo(PlcModuleInfo plcModuleInfo, PlcAddress parent, PlcChannelInfo.PlcChannelId channelId)
    {
      this.PlcModuleInfo = plcModuleInfo;
      this.plcAddress = new PlcAddress(parent, this.GetType().ToString());
      this.AdsClient = new AdsClient();
      this.ChannelId = channelId;
      plcModuleInfo.AddChannel(this);
    }

    public bool Connect()
    {
      if (!this.PlcModuleInfo.Connect())
        return false;
      AdsClient.Query query1 = new AdsClient.Query(this.AdsClient.Contains);
      TcAdsSymbolInfoCollection allSymbols = this.AdsClient.GetAllSymbols();
      if (this.ChannelId == PlcChannelInfo.PlcChannelId.Channel0)
      {
        this.ChannelInfoSymbol = this.PlcModuleInfo.AdminComChannelInfoSymbol;
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder(this.PlcModuleInfo.Settings.CommChannelInfoSymbol.Name);
        stringBuilder.Append("[");
        stringBuilder.Append(this.ChannelId.ToString());
        stringBuilder.Append("]");
        AdsClient adsClient = this.AdsClient;
        AdsClient.Query query2 = query1;
        TcAdsSymbolInfoCollection subSymbols = this.PlcModuleInfo.AdministratorSymbol.SubSymbols;
        string type1 = this.PlcModuleInfo.Settings.CommChannelInfoSymbol.Type;
        string name = stringBuilder.ToString();
        string type2 = type1;
        long group = 0;
        long offset = 0;
        this.ChannelInfoSymbol = adsClient.QuerySymbols(query2, subSymbols, name, type2, group, offset);
      }
      if (this.ChannelInfoSymbol == null)
        return false;
      PlcChannelInfoRaw plcChannelInfoRaw = this.AdsClient.Read<PlcChannelInfoRaw>(this.ChannelInfoSymbol);
      if (!plcChannelInfoRaw.InitDone)
        return false;
      this.Name = plcChannelInfoRaw.Name;
      long adsOffset = (long) this.PlcModuleInfo.AddressToAdsOffset(plcChannelInfoRaw.CommChannelPntr);
      this.ChannelSymbol = this.AdsClient.QuerySymbolsFlat(query1, allSymbols, "", "", this.ChannelInfoSymbol.IndexGroup, adsOffset);
      if (this.ChannelSymbol == null || !this.GetChannelSubSymbols() || (!this.GetChannelSymbols() || !this.HookCallbacks()))
        return false;
      if (this.ChannelId != PlcChannelInfo.PlcChannelId.Channel0)
        return true;
      if (this.GetAddressRegistrySymbols())
        this.PlcModuleInfo.AddressRegistry.Build();
      if (this.GetAggegatorSymbols())
        this.PlcModuleInfo.EventRegistry.Build();
      return true;
    }

    public bool HookCallbacks()
    {
      AdsClient.Query query = new AdsClient.Query(this.AdsClient.Contains);
      this.NotifyPcHasWrittenReplySymbol = this.AdsClient.QuerySymbols(query, this.ChannelSymbol.SubSymbols, this.PlcModuleInfo.Settings.NotifyPcHasWrittenReplySymbol.Name, this.PlcModuleInfo.Settings.NotifyPcHasWrittenReplySymbol.Type, 0L, 0L);
      if (this.NotifyPcHasWrittenReplySymbol == null)
        return false;
      this.NotifyPlcHasWrittenSymbol = this.AdsClient.QuerySymbols(query, this.ChannelSymbol.SubSymbols, this.PlcModuleInfo.Settings.NotifyPlcHasWrittenSymbol.Name, this.PlcModuleInfo.Settings.NotifyPlcHasWrittenSymbol.Type, 0L, 0L);
      if (this.NotifyPcHasWrittenReplySymbol == null)
        return false;
      this.NotifyPlcHasWrittenHandle = this.HookCallBack<ushort>(this.NotifyPlcHasWrittenSymbol);
      if (this.NotifyPlcHasWrittenHandle == 0)
        return false;
      this.NotifyPcHasWrittenReplyHandle = this.HookCallBack<ushort>(this.NotifyPcHasWrittenReplySymbol);
      return this.NotifyPcHasWrittenReplyHandle != 0;
    }

    public bool GetChannelSubSymbols()
    {
      this.ConnectedSymbol = this.AdsClient.QuerySymbols(new AdsClient.Query(this.AdsClient.Contains), this.PlcModuleInfo.AdministratorSymbol.SubSymbols, this.PlcModuleInfo.Settings.ConnectedSymbol.Name, this.PlcModuleInfo.Settings.ConnectedSymbol.Type, 0L, 0L);
      return this.ConnectedSymbol != null;
    }

    public bool GetChannelSymbols()
    {
      AdsClient.Query query = new AdsClient.Query(this.AdsClient.Contains);
      this.PcHasWrittenSymbol = this.AdsClient.QuerySymbols(query, this.ChannelSymbol.SubSymbols, this.PlcModuleInfo.Settings.PcHasWrittenSymbol.Name, this.PlcModuleInfo.Settings.PcHasWrittenSymbol.Type, 0L, 0L);
      if (this.PcHasWrittenSymbol == null)
        return false;
      this.PlcHasWrittenReplySymbol = this.AdsClient.QuerySymbols(query, this.ChannelSymbol.SubSymbols, this.PlcModuleInfo.Settings.PlcHasWrittenReplySymbol.Name, this.PlcModuleInfo.Settings.PlcHasWrittenReplySymbol.Type, 0L, 0L);
      if (this.PlcHasWrittenReplySymbol == null)
        return false;
      this.MessageInCountSymbol = this.AdsClient.QuerySymbols(query, this.ChannelSymbol.SubSymbols, this.PlcModuleInfo.Settings.MessageInCountSymbol.Name, this.PlcModuleInfo.Settings.MessageInCountSymbol.Type, 0L, 0L);
      if (this.MessageInCountSymbol == null)
        return false;
      this.MessageOutCountSymbol = this.AdsClient.QuerySymbols(query, this.ChannelSymbol.SubSymbols, this.PlcModuleInfo.Settings.MessageOutCountSymbol.Name, this.PlcModuleInfo.Settings.MessageOutCountSymbol.Type, 0L, 0L);
      if (this.MessageOutCountSymbol == null)
        return false;
      this.MessageInQueueSymbol = this.AdsClient.QuerySymbols(query, this.ChannelSymbol.SubSymbols, this.PlcModuleInfo.Settings.MessageInQueueSymbol.Name, this.PlcModuleInfo.Settings.MessageInQueueSymbol.Type, 0L, 0L);
      if (this.MessageInQueueSymbol == null)
        return false;
      this.MessageOutQueueSymbol = this.AdsClient.QuerySymbols(query, this.ChannelSymbol.SubSymbols, this.PlcModuleInfo.Settings.MessageOutQueueSymbol.Name, this.PlcModuleInfo.Settings.MessageOutQueueSymbol.Type, 0L, 0L);
      if (this.MessageOutQueueSymbol == null)
        return false;
      this.MessageOutDataSymbol = this.AdsClient.QuerySymbols(query, this.ChannelSymbol.SubSymbols, this.PlcModuleInfo.Settings.MessageOutDataSymbol.Name, this.PlcModuleInfo.Settings.MessageOutDataSymbol.Type, 0L, 0L);
      return this.MessageOutDataSymbol != null;
    }

    public uint AddressToAdsOffset(ulong address)
    {
      return this.PlcModuleInfo.AddressToAdsOffset(address);
    }

    public ulong AdsOffsetToAddress(uint adsOffset)
    {
      return this.PlcModuleInfo.AdsOffsetToAddress(adsOffset);
    }

    private bool IsReady()
    {
      return this.AdsClient.Read<PlcChannelInfoRaw>(this.ChannelInfoSymbol).Ready;
    }

    private int HookCallBack<TType>(TcAdsSymbolInfo symbol)
    {
      return this.AdsClient.HookCallBack<TType>(symbol);
    }

    private bool GetAddressRegistrySymbols()
    {
      AdsClient.Query query = new AdsClient.Query(this.AdsClient.Contains);
      TcAdsSymbolInfoCollection allSymbols = this.AdsClient.GetAllSymbols();
      this.AddressRegistrySymbol = this.AdsClient.QuerySymbolsFlat(query, allSymbols, this.PlcModuleInfo.Settings.AddressRegistrySymbol.Name, this.PlcModuleInfo.Settings.AddressRegistrySymbol.Type, 0L, 0L);
      if (this.AddressRegistrySymbol == null)
        return false;
      this.AddressRegistryCountSymbol = this.AdsClient.QuerySymbols(query, this.AddressRegistrySymbol.SubSymbols, this.PlcModuleInfo.Settings.AddressRegistryCountSymbol.Name, this.PlcModuleInfo.Settings.AddressRegistryCountSymbol.Type, 0L, 0L);
      if (this.AddressRegistryCountSymbol == null)
        return false;
      this.AddressRegistryDataSymbol = this.AdsClient.QuerySymbols(query, this.AddressRegistrySymbol.SubSymbols, this.PlcModuleInfo.Settings.AddressRegistryDataSymbol.Name, this.PlcModuleInfo.Settings.AddressRegistryDataSymbol.Type, 0L, 0L);
      return this.AddressRegistryDataSymbol != null;
    }

    private bool GetAggegatorSymbols()
    {
      AdsClient.Query query1 = new AdsClient.Query(this.AdsClient.Equals);
      AdsClient.Query query2 = new AdsClient.Query(this.AdsClient.Contains);
      TcAdsSymbolInfoCollection allSymbols = this.AdsClient.GetAllSymbols();
      this.AggregatorSymbol = this.AdsClient.QuerySymbolsFlat(query1, allSymbols, this.PlcModuleInfo.Settings.AggregatorSymbol.Name, this.PlcModuleInfo.Settings.AggregatorSymbol.Type, 0L, 0L);
      if (this.AggregatorSymbol == null)
        return false;
      this.EventSinkCountSymbol = this.AdsClient.QuerySymbols(query2, this.AggregatorSymbol.SubSymbols, this.PlcModuleInfo.Settings.EventSinkCountSymbol.Name, this.PlcModuleInfo.Settings.EventSinkCountSymbol.Type, 0L, 0L);
      if (this.EventSinkCountSymbol == null)
        return false;
      this.EventSinkDataSymbol = this.AdsClient.QuerySymbols(query2, this.AggregatorSymbol.SubSymbols, this.PlcModuleInfo.Settings.EventSinkDataSymbol.Name, this.PlcModuleInfo.Settings.EventSinkDataSymbol.Type, 0L, 0L);
      if (this.EventSinkDataSymbol == null)
        return false;
      this.EventSourceCountSymbol = this.AdsClient.QuerySymbols(query2, this.AggregatorSymbol.SubSymbols, this.PlcModuleInfo.Settings.EventSourceCountSymbol.Name, this.PlcModuleInfo.Settings.EventSourceCountSymbol.Type, 0L, 0L);
      if (this.EventSourceCountSymbol == null)
        return false;
      this.EventSourceDataSymbol = this.AdsClient.QuerySymbols(query2, this.AggregatorSymbol.SubSymbols, this.PlcModuleInfo.Settings.EventSourceDataSymbol.Name, this.PlcModuleInfo.Settings.EventSourceDataSymbol.Type, 0L, 0L);
      return this.EventSourceDataSymbol != null;
    }

    public enum PlcChannelId
    {
      Channel0,
      Channel1,
      Channel2,
      Channel3,
      Channel4,
    }
  }
}
