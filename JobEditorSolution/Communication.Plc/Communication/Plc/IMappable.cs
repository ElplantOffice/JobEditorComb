
using System;

namespace Communication.Plc
{
  public interface IMappable
  {
    string TypeName { get; }

    Func<byte[], object> Mapper { get; }
  }
}
