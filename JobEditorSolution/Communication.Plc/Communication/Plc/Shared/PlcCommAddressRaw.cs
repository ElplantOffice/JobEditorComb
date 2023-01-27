
namespace Communication.Plc.Shared
{
  public struct PlcCommAddressRaw
  {
    public PlcBaseAddressRaw Owner;
    public PlcBaseAddressRaw Target;
    public ushort Command;
  }
}
