
using System;
using System.Collections.Generic;

namespace Communication.Plc
{
  internal class PlcTypeContainer
  {
    internal Dictionary<string, Func<byte[], object>> Types { get; private set; }

    public PlcTypeContainer()
    {
      this.Types = new Dictionary<string, Func<byte[], object>>();
    }
  }
}
