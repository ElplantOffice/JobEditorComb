
using TwinCAT.Ads;

namespace Communication.Plc.Ads
{
  public class AdsSettings
  {
    public AdsSettings(string netId, int port)
    {
      this.AmsAddress = new AmsAddress(new AmsNetId(netId), port);
      this.CommAdministratorSymbol = new AdsSymbolPrototype("CommAdministrator", "CommAdministrator");
      this.CommChannelInfoSymbol = new AdsSymbolPrototype("", "T_CommChannelInfo");
      this.BaseObjectDataSymbol = new AdsSymbolPrototype("", "T_BaseObjectData");
      this.BaseObjectAdressSymbol = new AdsSymbolPrototype("hAddress", "LWORD");
      this.AddressRegistrySymbol = new AdsSymbolPrototype("CommAddressRegistry", "CommAddressRegistry");
      this.AddressRegistryCountSymbol = new AdsSymbolPrototype("_cdiNumberDataItemsMax", "DINT");
      this.AddressRegistryDataSymbol = new AdsSymbolPrototype("_atData[0]", "T_BaseAddress");
      this.AggregatorSymbol = new AdsSymbolPrototype("Aggregator", "Aggregator");
      this.EventSinkCountSymbol = new AdsSymbolPrototype("_cdiNumberDataItemsMax", "DINT");
      this.EventSinkDataSymbol = new AdsSymbolPrototype("_atEventSinks[0]", "T_AggregatorEventSink");
      this.EventSourceCountSymbol = new AdsSymbolPrototype("_cdiNumberDataItemsMax", "DINT");
      this.EventSourceDataSymbol = new AdsSymbolPrototype("_atEventSources[0]", "T_AggregatorEventSource");
      this.NotifyPlcHasWrittenSymbol = new AdsSymbolPrototype("_tSendQueue.wCommReq", "WORD");
      this.NotifyPcHasWrittenReplySymbol = new AdsSymbolPrototype("_tReceiveQueue.wCommAck", "WORD");
      this.PcHasWrittenSymbol = new AdsSymbolPrototype("_tReceiveQueue.wCommReq", "WORD");
      this.PlcHasWrittenReplySymbol = new AdsSymbolPrototype("_tSendQueue.wCommAck", "WORD");
      this.ConnectedSymbol = new AdsSymbolPrototype("bConnected", "BOOL");
      this.MessageInCountSymbol = new AdsSymbolPrototype("_tSendQueue.wCount", "WORD");
      this.MessageOutCountSymbol = new AdsSymbolPrototype("_tReceiveQueue.wCount", "WORD");
      this.MessageInQueueSymbol = new AdsSymbolPrototype("_tSendQueue.atTelegram[0]", "T_CommTelegram");
      this.MessageOutQueueSymbol = new AdsSymbolPrototype("_tReceiveQueue.atTelegram[0]", "T_CommTelegram");
      this.MessageOutDataSymbol = new AdsSymbolPrototype("_tReceiveQueue.abtData[0]", "T_KByte");
    }

    public AdsSettings(AdsSettings source, string netId, int port)
    {
      this.AmsAddress = new AmsAddress(new AmsNetId(netId), port);
      this.CommAdministratorSymbol = source.CommAdministratorSymbol;
      this.CommChannelInfoSymbol = source.CommChannelInfoSymbol;
      this.BaseObjectDataSymbol = source.BaseObjectDataSymbol;
      this.BaseObjectAdressSymbol = source.BaseObjectAdressSymbol;
      this.NotifyPlcHasWrittenSymbol = source.NotifyPlcHasWrittenSymbol;
      this.NotifyPcHasWrittenReplySymbol = source.NotifyPcHasWrittenReplySymbol;
      this.PcHasWrittenSymbol = source.PcHasWrittenSymbol;
      this.PlcHasWrittenReplySymbol = source.PlcHasWrittenReplySymbol;
      this.MessageInCountSymbol = source.MessageInCountSymbol;
      this.MessageOutCountSymbol = source.MessageOutCountSymbol;
      this.MessageInQueueSymbol = source.MessageInQueueSymbol;
      this.MessageOutQueueSymbol = source.MessageOutQueueSymbol;
    }

    public AmsAddress AmsAddress { get; private set; }

    public AdsSymbolPrototype CommAdministratorSymbol { get; private set; }

    public AdsSymbolPrototype CommChannelInfoSymbol { get; private set; }

    public AdsSymbolPrototype BaseObjectDataSymbol { get; private set; }

    public AdsSymbolPrototype BaseObjectAdressSymbol { get; private set; }

    public AdsSymbolPrototype AddressRegistrySymbol { get; private set; }

    public AdsSymbolPrototype AddressRegistryCountSymbol { get; private set; }

    public AdsSymbolPrototype AddressRegistryDataSymbol { get; private set; }

    public AdsSymbolPrototype AggregatorSymbol { get; private set; }

    public AdsSymbolPrototype EventSinkCountSymbol { get; private set; }

    public AdsSymbolPrototype EventSinkDataSymbol { get; private set; }

    public AdsSymbolPrototype EventSourceCountSymbol { get; private set; }

    public AdsSymbolPrototype EventSourceDataSymbol { get; private set; }

    public AdsSymbolPrototype NotifyPlcHasWrittenSymbol { get; private set; }

    public AdsSymbolPrototype NotifyPcHasWrittenReplySymbol { get; private set; }

    public AdsSymbolPrototype PcHasWrittenSymbol { get; set; }

    public AdsSymbolPrototype PlcHasWrittenReplySymbol { get; set; }

    public AdsSymbolPrototype ConnectedSymbol { get; set; }

    public AdsSymbolPrototype MessageInCountSymbol { get; set; }

    public AdsSymbolPrototype MessageOutCountSymbol { get; set; }

    public AdsSymbolPrototype MessageInQueueSymbol { get; set; }

    public AdsSymbolPrototype MessageOutQueueSymbol { get; set; }

    public AdsSymbolPrototype MessageOutDataSymbol { get; set; }
  }
}
