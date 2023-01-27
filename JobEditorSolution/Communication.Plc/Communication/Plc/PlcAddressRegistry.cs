
using Communication.Plc.Channel;
using Communication.Plc.Shared;
using System.Collections.Generic;

namespace Communication.Plc
{
  public class PlcAddressRegistry
  {
    private Dictionary<string, PlcBaseAddressRaw> registry;
    private PlcChannelInfo plcChannelInfo;

    public PlcAddressRegistry(PlcChannelInfo plcChannelInfo)
    {
      this.plcChannelInfo = plcChannelInfo;
      this.registry = new Dictionary<string, PlcBaseAddressRaw>();
    }

    public bool Build()
    {
      PlcBaseAddressRaw[] elements;
      if (!this.LoadSymbols(out elements))
        return false;
      this.registry.Clear();
      foreach (PlcBaseAddressRaw plcBaseAddressRaw in elements)
      {
        if (!string.IsNullOrEmpty(plcBaseAddressRaw.Name))
          this.registry.Add(plcBaseAddressRaw.Name, plcBaseAddressRaw);
      }
      return true;
    }

    public bool Get(string key, PlcBaseAddressRaw value)
    {
      if (this.registry.TryGetValue(key, out value))
        return true;
      this.Build();
      return this.registry.TryGetValue(key, out value);
    }

    private bool LoadSymbols(out PlcBaseAddressRaw[] elements)
    {
      elements = (PlcBaseAddressRaw[]) null;
      if (this.plcChannelInfo.AddressRegistrySymbol == null || this.plcChannelInfo.AddressRegistryCountSymbol == null)
        return false;
      int count = (int) this.plcChannelInfo.AdsClient.Read<uint>(this.plcChannelInfo.AddressRegistryCountSymbol);
      if (count < 1)
        return false;
      return this.ReadTable(out elements, count);
    }

    private bool ReadTable(out PlcBaseAddressRaw[] elements, int count)
    {
      elements = this.plcChannelInfo.AdsClient.ReadArray<PlcBaseAddressRaw>(this.plcChannelInfo.AddressRegistryDataSymbol, count);
      return elements != null;
    }
  }
}
