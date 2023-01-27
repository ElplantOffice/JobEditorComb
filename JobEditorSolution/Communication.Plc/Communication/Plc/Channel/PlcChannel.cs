
using Communication.Plc.Ads;
using Messages;
using System;
using System.Diagnostics;

namespace Communication.Plc.Channel
{
  public class PlcChannel
  {
    private AdsClient adsClient;
    private PlcServices plcServices;
    private PlcAddress adsAddress;
    private PlcPacket receivePacket;
    private PlcPacket sendPacket;
    private PlcAggregator aggregator;

    public PlcChannelInfo PlcChannelInfo { get; private set; }

    public PlcChannelState PlcChannelState { get; private set; }

    public PlcChannel(PlcModuleInfo plcModuleInfo, PlcChannelInfo.PlcChannelId channelId, PlcAddress parent)
    {
      this.adsAddress = new PlcAddress(parent, this.GetType().ToString());
      this.PlcChannelInfo = new PlcChannelInfo(plcModuleInfo, this.adsAddress, channelId);
      this.receivePacket = new PlcPacket(this.PlcChannelInfo);
      this.sendPacket = new PlcPacket(this.PlcChannelInfo);
      this.adsClient = this.PlcChannelInfo.AdsClient;
      this.aggregator = new PlcAggregator();
      this.PlcChannelState = new PlcChannelState();
      this.plcServices = new PlcServices(this.PlcChannelInfo);
      this.adsClient.Events.OnReceive += (AdsClientEvents.OnReceiveHandler) ((s, e) => this.OnPlcReceive(s, e));
      this.adsClient.Events.OnStatusUpdate += (AdsClientEvents.OnStatusUpdateHandler) ((s, e) => this.StatusUpdate(s, e));
    }

    public event PlcChannel.OnStatusUpdateHandler OnStatusUpdate;

    public virtual void RaiseOnStatusUpdate(object sender, PlcChannel.StateEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.OnStatusUpdate == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.OnStatusUpdate(sender, e);
    }

    public void Connect()
    {
      if (!this.PlcChannelInfo.Connect())
        return;
      this.PlcChannelState.CommState = PlcChannelState.State.Connected;
      if (this.aggregator.IsListening)
        return;
      this.aggregator.Subscribe((Action<Telegram>) (telegram => this.OnPlcSend(telegram)), this.PlcChannelInfo.Name);
      this.aggregator.Subscribe((Action<Telegram>) (telegram => this.SetConnected(telegram)), this.PlcChannelInfo.Name + ".SetConnected");
      this.RaiseOnStatusUpdate((object) this, new PlcChannel.StateEventArgs());
    }

    public void SetConnected(Telegram telegram)
    {
      if (this.PlcChannelState.CommState != PlcChannelState.State.Connected)
        return;
      this.adsClient.Write<bool>(this.PlcChannelInfo.ConnectedSymbol, true);
    }

    private void OnPlcSend(Telegram telegram)
    {
      this.sendPacket.Add(telegram);
      this.sendPacket.TryFlush();
    }

    public void OnPlcReceive(object sender, AdsClientEvents.AdsClientReceiveEventArgs e)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Reset();
      stopwatch.Start();
      if (e.Symbol == null)
        return;
      if (((object) e.Symbol).Equals((object) this.PlcChannelInfo.NotifyPlcHasWrittenSymbol))
      {
        foreach (Telegram message in this.receivePacket.Read((ushort) e.NotificationEx.Value))
        {
          message.ServiceLink = (object) this.plcServices;
          this.aggregator.Publish(message, false);
        }
        stopwatch.Stop();
        stopwatch.Elapsed.ToString();
      }
      else
      {
        if (!((object) e.Symbol).Equals((object) this.PlcChannelInfo.NotifyPcHasWrittenReplySymbol))
          return;
        this.sendPacket.Flush((ushort) e.NotificationEx.Value);
      }
    }

    public void StatusUpdate(object sender, AdsClientEvents.AdsClientStateEventArgs e)
    {
      this.RaiseOnStatusUpdate((object) this, new PlcChannel.StateEventArgs());
    }

    public class StateEventArgs : EventArgs
    {
    }

    public delegate void OnStatusUpdateHandler(object sender, PlcChannel.StateEventArgs e);
  }
}
