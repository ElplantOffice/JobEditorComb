
using Communication.Plc.Shared;
using Messages;
using System.Text;
using TwinCAT.Ads;

namespace Communication.Plc
{
  public class PlcAddress : IAddress
  {
    private StringBuilder owner;
    private StringBuilder relay;
    private StringBuilder target;
    private StringBuilder channel;
    public TcAdsSymbolInfo ownerSymbol;
    public TcAdsSymbolInfo targetSymbol;

    public PlcAddress Parent { get; set; }

    public string Owner
    {
      get
      {
        return this.owner.ToString();
      }
    }

    public string Relay
    {
      get
      {
        return this.relay.ToString();
      }
    }

    public string Target
    {
      get
      {
        return this.target.ToString();
      }
    }

    public string Channel
    {
      get
      {
        return this.channel.ToString();
      }
    }

    public PlcAddress(string owner, string relay, string target, string channel)
    {
      this.Parent = (PlcAddress) null;
      this.owner = new StringBuilder(owner);
      this.target = new StringBuilder(target);
      this.relay = new StringBuilder(relay);
      this.channel = new StringBuilder(channel);
    }

    public PlcAddress(PlcCommAddressRaw addressRaw, string channel)
    {
      this.Parent = (PlcAddress) null;
      this.owner = new StringBuilder(addressRaw.Owner.Name);
      this.target = new StringBuilder(addressRaw.Target.Name);
      this.channel = new StringBuilder(channel);
      this.relay = (StringBuilder) null;
      if (string.IsNullOrEmpty(channel))
        return;
      this.owner.Remove(0, this.Owner.IndexOf(".") + 1).Insert(0, new StringBuilder(channel).Append(".").ToString());
    }

    public PlcAddress(PlcAddress parent, string objectName)
    {
      this.Parent = parent;
      this.owner = new StringBuilder(parent.Owner);
      this.owner.Append(".");
      this.owner.Append(objectName);
      this.target = new StringBuilder(parent.Target);
      this.target.Append(".");
      this.target.Append(objectName);
    }

    public PlcAddress(string applicationName)
    {
      this.owner = new StringBuilder(applicationName);
      this.target = new StringBuilder();
    }

    public string GetTargetAncestor()
    {
      int length = this.Target.IndexOf(".");
      if (length == 0)
        return this.Target;
      return this.target.ToString(0, length);
    }
  }
}
