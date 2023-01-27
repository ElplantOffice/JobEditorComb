
using Communication.Plc.Channel;
using Communication.Plc.Shared;
using System.Collections.Generic;

namespace Communication.Plc
{
  public class PlcEventRegistry
  {
    private Dictionary<string, PlcEventSinkRaw> sinkByNameRegistry;
    private Dictionary<string, PlcEventSourceRaw> sourceByNameRegistry;
    private Dictionary<ulong, PlcEventSinkRaw> sinkByHandleRegistry;
    private Dictionary<ulong, PlcEventSourceRaw> sourceByHandleRegistry;
    private PlcChannelInfo plcChannelInfo;

    public PlcEventRegistry(PlcChannelInfo plcChannelInfo)
    {
      this.plcChannelInfo = plcChannelInfo;
      this.sinkByNameRegistry = new Dictionary<string, PlcEventSinkRaw>();
      this.sourceByNameRegistry = new Dictionary<string, PlcEventSourceRaw>();
      this.sinkByHandleRegistry = new Dictionary<ulong, PlcEventSinkRaw>();
      this.sourceByHandleRegistry = new Dictionary<ulong, PlcEventSourceRaw>();
    }

    public void Build()
    {
      this.BuildSinks();
      this.BuildSources();
    }

    public bool GetSink(string key, out PlcEventSinkRaw value)
    {
      if (this.sinkByNameRegistry.TryGetValue(key, out value))
        return true;
      this.BuildSinks();
      return this.sinkByNameRegistry.TryGetValue(key, out value);
    }

    public bool GetSource(string key, out PlcEventSourceRaw value)
    {
      if (this.sourceByNameRegistry.TryGetValue(key, out value))
        return true;
      this.BuildSources();
      return this.sourceByNameRegistry.TryGetValue(key, out value);
    }

    public bool GetSink(ulong handle, out PlcEventSinkRaw value)
    {
      if (this.sinkByHandleRegistry.TryGetValue(handle, out value))
        return true;
      this.BuildSinks();
      return this.sinkByHandleRegistry.TryGetValue(handle, out value);
    }

    public bool GetSource(ulong handle, out PlcEventSourceRaw value)
    {
      if (this.sourceByHandleRegistry.TryGetValue(handle, out value))
        return true;
      this.BuildSources();
      return this.sourceByHandleRegistry.TryGetValue(handle, out value);
    }

    private bool BuildSinks()
    {
      PlcEventSinkRaw[] elements;
      if (!this.LoadSinkSymbols(out elements))
        return false;
      this.sinkByNameRegistry.Clear();
      this.sinkByHandleRegistry.Clear();
      foreach (PlcEventSinkRaw plcEventSinkRaw1 in elements)
      {
        PlcEventSinkRaw plcEventSinkRaw2;
        if (!string.IsNullOrEmpty(plcEventSinkRaw1.Owner.Name) && !this.sinkByNameRegistry.TryGetValue(plcEventSinkRaw1.Owner.Name, out plcEventSinkRaw2))
          this.sinkByNameRegistry.Add(plcEventSinkRaw1.Owner.Name, plcEventSinkRaw1);
        if (plcEventSinkRaw1.Handle != 0UL && !this.sinkByHandleRegistry.TryGetValue(plcEventSinkRaw1.ActionHandle, out plcEventSinkRaw2))
          this.sinkByHandleRegistry.Add(plcEventSinkRaw1.ActionHandle, plcEventSinkRaw1);
      }
      return true;
    }

    private bool BuildSources()
    {
      PlcEventSourceRaw[] elements;
      if (!this.LoadSourceSymbols(out elements))
        return false;
      this.sourceByNameRegistry.Clear();
      this.sourceByHandleRegistry.Clear();
      foreach (PlcEventSourceRaw plcEventSourceRaw1 in elements)
      {
        PlcEventSourceRaw plcEventSourceRaw2;
        if (!string.IsNullOrEmpty(plcEventSourceRaw1.Target.Name) && !this.sourceByNameRegistry.TryGetValue(plcEventSourceRaw1.Target.Name, out plcEventSourceRaw2))
          this.sourceByNameRegistry.Add(plcEventSourceRaw1.Target.Name, plcEventSourceRaw1);
        if (plcEventSourceRaw1.Handle != 0UL && !this.sourceByHandleRegistry.TryGetValue(plcEventSourceRaw1.Handle, out plcEventSourceRaw2))
          this.sourceByHandleRegistry.Add(plcEventSourceRaw1.Handle, plcEventSourceRaw1);
      }
      return true;
    }

    private bool LoadSinkSymbols(out PlcEventSinkRaw[] elements)
    {
      elements = (PlcEventSinkRaw[]) null;
      if (this.plcChannelInfo.AggregatorSymbol == null || this.plcChannelInfo.EventSinkCountSymbol == null)
        return false;
      int count = (int) this.plcChannelInfo.AdsClient.Read<uint>(this.plcChannelInfo.EventSinkCountSymbol);
      if (count < 1)
        return false;
      return this.ReadSinkTable(out elements, count);
    }

    private bool LoadSourceSymbols(out PlcEventSourceRaw[] elements)
    {
      elements = (PlcEventSourceRaw[]) null;
      if (this.plcChannelInfo.AggregatorSymbol == null || this.plcChannelInfo.EventSourceCountSymbol == null)
        return false;
      int count = (int) this.plcChannelInfo.AdsClient.Read<uint>(this.plcChannelInfo.EventSourceCountSymbol);
      if (count < 1)
        return false;
      return this.ReadSourceTable(out elements, count);
    }

    private bool ReadSinkTable(out PlcEventSinkRaw[] elements, int count)
    {
      elements = this.plcChannelInfo.AdsClient.ReadArray<PlcEventSinkRaw>(this.plcChannelInfo.EventSinkDataSymbol, count);
      return elements != null;
    }

    private bool ReadSourceTable(out PlcEventSourceRaw[] elements, int count)
    {
      elements = this.plcChannelInfo.AdsClient.ReadArray<PlcEventSourceRaw>(this.plcChannelInfo.EventSourceDataSymbol, count);
      return elements != null;
    }
  }
}
