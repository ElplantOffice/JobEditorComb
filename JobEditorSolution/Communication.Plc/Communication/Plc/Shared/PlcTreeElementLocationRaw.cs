
using System;

namespace Communication.Plc.Shared
{
  [Serializable]
  public struct PlcTreeElementLocationRaw
  {
    public short Row;
    public short Column;
  }
}
