
namespace Communication.Plc.Shared
{
  public struct PlcEventSinkRaw
  {
    public short Alias;
    public short Command;
    public short Id;
    public ulong Handle;
    public PlcBaseAddressRaw Owner;
    public ulong DataPntrHandle;
    public ulong ActionHandle;
    public PlcEventSourceSiblings tSiblings;
  }
}
