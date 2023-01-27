
using Communication.Plc.Channel;
using Communication.Plc.Shared;
using Messages;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Communication.Plc
{
  public class PlcAdministrator
  {
    private DispatcherTimer dispatcherTimer;
    private List<PlcChannel> plcChannels;
    private PlcAddress plcAddress;
    private PlcModuleInfo plcModuleInfo;
    private PlcAggregator aggregator;

    public PlcAdministrator(PlcModuleInfo plcModuleInfo, PlcAddress parent)
    {
      this.plcModuleInfo = plcModuleInfo;
      this.plcAddress = new PlcAddress(parent, this.GetType().ToString());
      this.plcChannels = new List<PlcChannel>();
      this.plcChannels.Add(new PlcChannel(plcModuleInfo, PlcChannelInfo.PlcChannelId.Channel0, this.plcAddress));
      this.plcChannels[0].OnStatusUpdate += new PlcChannel.OnStatusUpdateHandler(this.OnChannelStatusUpdate);
      PlcTypeResolver.Register<PlcCommandHandleRaw>();
      PlcTypeResolver.Register<PlcTelegramRaw>();
      PlcTypeResolver.Register<PlcUiPrototypeRaw>();
      PlcTypeResolver.Register<PlcControlDataRaw>();
      PlcTypeResolver.Register<PlcTreeElementStringRaw>();
      this.aggregator = new PlcAggregator();
      this.aggregator.Subscribe((Action<Telegram>) (telegram => this.OnReceive(telegram)), this.plcAddress.Target);
      this.StartTimer();
    }

    public event PlcAdministrator.OnStatusUpdateHandler OnStatusUpdate;

    public virtual void RaiseOnStatusUpdate(object sender, PlcChannel.StateEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.OnStatusUpdate == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.OnStatusUpdate(sender, e);
    }

    private void OnChannelStatusUpdate(object s, PlcChannel.StateEventArgs e)
    {
      this.RaiseOnStatusUpdate(s, e);
    }

    private void StartTimer()
    {
      this.dispatcherTimer = new DispatcherTimer();
      this.dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
      this.dispatcherTimer.Tick += new EventHandler(this.OnTimerElapsed);
      this.dispatcherTimer.Start();
    }

    public void OnReceive(Telegram telegram)
    {
      if (telegram.Command != 1)
        return;
      this.plcChannels.Add(new PlcChannel(this.plcModuleInfo, (PlcChannelInfo.PlcChannelId) telegram.Value, this.plcAddress));
      this.StartTimer();
    }

    public void SetPlcChannelConnected(Telegram telegram)
    {
      foreach (PlcChannel plcChannel in this.plcChannels)
      {
        if (plcChannel.PlcChannelState.CommState == PlcChannelState.State.Connected && plcChannel.PlcChannelInfo.ChannelId == (PlcChannelInfo.PlcChannelId) telegram.Value)
          plcChannel.SetConnected(telegram);
      }
    }

    public void OnTimerElapsed(object sender, EventArgs e)
    {
      bool flag = true;
      foreach (PlcChannel plcChannel in this.plcChannels)
      {
        if (plcChannel.PlcChannelState.CommState == PlcChannelState.State.Connected)
          return;
        plcChannel.Connect();
        flag = false;
      }
      if (!flag)
        return;
      this.dispatcherTimer.Stop();
    }

    public delegate void OnStatusUpdateHandler(object sender, PlcChannel.StateEventArgs e);
  }
}
