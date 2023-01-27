
using System;
using System.Reflection;

namespace Patterns.EventAggregator
{
  public class FileResourceDataInfo
  {
    private Assembly assembly;
    private Type dataType;
    private IFileResourceData data;

    public void Init(IFileResourceDataInfo dataInfo)
    {
      if (this.assembly != (Assembly) null)
        return;
      this.assembly = dataInfo.Assembly;
      this.dataType = dataInfo.DataType;
      this.data = dataInfo.Data;
    }

    public Assembly Assembly
    {
      get
      {
        return this.assembly;
      }
    }

    public Type DataType
    {
      get
      {
        return this.dataType;
      }
    }

    public IFileResourceData Data
    {
      get
      {
        return this.data;
      }
    }
  }
}
