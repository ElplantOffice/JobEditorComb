
using System.Collections.Generic;

namespace Communication.Plc
{
  public interface IPlcMappable
  {
    string Map(List<byte> dataBlock);
  }
}
