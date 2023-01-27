
using Communication.Plc.Ads;
using Communication.Plc.Channel;
using Communication.Plc.Shared;
using Messages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwinCAT.Ads;

namespace Communication.Plc
{
  internal class PlcPacket
  {
    private AdsBinaryReader dataBlock;
    private AdsClient adsClient;
    private PlcTelegramRaw[] rawMessages;
    private PlcChannelInfo plcChannelInfo;

    public List<Telegram> Messages { get; private set; }

    public int MessageCnt { get; private set; }

    public int NotifyId { get; private set; }

    public PlcPacket(PlcChannelInfo plcChannelInfo)
    {
      this.adsClient = plcChannelInfo.AdsClient;
      this.plcChannelInfo = plcChannelInfo;
      this.Messages = new List<Telegram>();
    }

    public List<Telegram> Read(ushort notifyId)
    {
      this.NotifyId = (int) notifyId;
      this.MessageCnt = (int) this.adsClient.Read<ushort>(this.plcChannelInfo.MessageInCountSymbol);
      if (this.MessageCnt != 0)
      {
        this.rawMessages = new PlcTelegramRaw[this.MessageCnt];
        this.rawMessages = this.adsClient.ReadArray<PlcTelegramRaw>(this.plcChannelInfo.MessageInQueueSymbol, this.MessageCnt);
        ulong size = (ulong) this.rawMessages[this.MessageCnt - 1].CommDataPntr.Size;
        if (this.rawMessages[this.MessageCnt - 1].CommDataPntr.IsArray)
          size *= (ulong) (uint) this.rawMessages[this.MessageCnt - 1].CommDataPntr.Count;
        this.dataBlock = this.adsClient.ReadBlock((int) this.plcChannelInfo.MessageInQueueSymbol.IndexGroup, (int) this.plcChannelInfo.AddressToAdsOffset(this.rawMessages[0].CommDataPntr.Offset), (uint) (size + (this.rawMessages[this.MessageCnt - 1].CommDataPntr.Offset - this.rawMessages[0].CommDataPntr.Offset)));
        this.Messages.Clear();
        foreach (PlcTelegramRaw rawMessage in this.rawMessages)
          this.Messages.Add(this.GetMessage(rawMessage, this.dataBlock));
      }
      this.adsClient.Write<ushort>(this.plcChannelInfo.MessageInCountSymbol, (ushort) 0);
      this.adsClient.Write<ushort>(this.plcChannelInfo.PlcHasWrittenReplySymbol, notifyId);
      return this.Messages;
    }

    public void Add(Telegram telegram)
    {
      this.Messages.Add(telegram);
    }

    public void TryFlush()
    {
      ushort num = this.adsClient.Read<ushort>(this.plcChannelInfo.NotifyPcHasWrittenReplySymbol);
      ushort writeId = this.adsClient.Read<ushort>(this.plcChannelInfo.PcHasWrittenSymbol);
      if ((int) writeId != (int) num)
        return;
      this.Flush(writeId);
    }

    public void Flush(ushort writeId)
    {
      if (this.Messages.Count == 0)
        return;
      this.rawMessages = new PlcTelegramRaw[this.Messages.Count];
      ushort num = 0;
      List<byte> dataBlock = new List<byte>();
      foreach (Telegram message in this.Messages)
      {
        PlcCommAddressRaw plcCommAddressRaw = PlcMapper.Map(message, this.plcChannelInfo.Name);
        PlcEventSinkRaw plcEventSinkRaw;
        if (this.plcChannelInfo.PlcModuleInfo.EventRegistry.GetSink(plcCommAddressRaw.Target.Name, out plcEventSinkRaw))
        {
          PlcTelegramRaw plcTelegramRaw = new PlcTelegramRaw();
          plcTelegramRaw.CommAddress = plcCommAddressRaw;
          plcTelegramRaw.CommDataPntr.Offset = this.plcChannelInfo.AdsOffsetToAddress((uint) this.plcChannelInfo.MessageOutDataSymbol.IndexOffset);
          if (PlcMapper.Map(message.Value, ref plcTelegramRaw.CommDataPntr, dataBlock))
          {
            plcTelegramRaw.EventSink = plcEventSinkRaw;
            plcTelegramRaw.EventSink.Command = (short) message.Command;
            plcTelegramRaw.CommAddress.Target = plcEventSinkRaw.Owner;
            this.rawMessages[(int) num] = plcTelegramRaw;
            ++num;
          }
        }
      }
      this.adsClient.WriteArray<PlcTelegramRaw>(this.plcChannelInfo.MessageOutQueueSymbol, this.rawMessages);
      this.adsClient.WriteArray<byte>(this.plcChannelInfo.MessageOutDataSymbol, dataBlock.ToArray());
      this.adsClient.Write<ushort>(this.plcChannelInfo.MessageOutCountSymbol, (ushort) ((IEnumerable<PlcTelegramRaw>) this.rawMessages).Count<PlcTelegramRaw>());
      this.adsClient.Write<ushort>(this.plcChannelInfo.PcHasWrittenSymbol, ++writeId);
      this.Messages.Clear();
    }

    public Telegram GetMessage(PlcTelegramRaw rawMessage, AdsBinaryReader dataBlock)
    {
      return PlcMapper.ConvertToTelegram(rawMessage, (BinaryReader) dataBlock, this.plcChannelInfo.Name);
    }

    private PlcTelegramRaw GetRawMessage(PlcTelegram message)
    {
      return new PlcTelegramRaw();
    }
  }
}
