
using System;

namespace Communication.Plc.Shared
{
  [Serializable]
  public struct PlcTreeElementStringRaw : IMappable
  {
    public uint DataId;
    public PlcTreeElementIdentityRaw Id;
    public PlcTreeElementLocationRaw Location;
    public PlcControlDataRaw Data;
    public PlcCommandHandleRaw Handle;

    public string TypeName
    {
      get
      {
        return "T_UiTreeElement";
      }
    }

    public Func<byte[], object> Mapper
    {
      get
      {
        return new Func<byte[], object>(this.Map);
      }
    }

    private object Map(byte[] dataBlock)
    {
      return PlcMapper.Map<PlcTreeElementStringRaw>(dataBlock, this.TypeName);
    }
  }
}
