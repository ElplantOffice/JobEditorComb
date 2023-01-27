
using TwinCAT.Ads;

namespace Communication.Plc.Channel
{
  public class PlcChannelState
  {
    public StateInfo AdsState { get; set; }

    public PlcChannelState.State CommState { get; set; }

    public AdsErrorCode AdsErrorCode { get; set; }

    public PlcChannelState()
    {
      this.CommState = PlcChannelState.State.Init;
    }

    public enum State
    {
      Init,
      Connected,
      Ready,
      Fault,
    }
  }
}
