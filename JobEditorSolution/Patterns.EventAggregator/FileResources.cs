
using Patterns.EventAggregator;

namespace Patterns.EventAggregator
{
  public class FileResources
  {
    private FileResourceDataInfo dataInfo;

    public FileResources()
    {
      if (this.dataInfo != null)
        return;
      this.dataInfo = SingletonProvider<FileResourceDataInfo>.Instance;
    }

    public string AssemblyName
    {
      get
      {
        return this.dataInfo.Assembly.GetName().Name;
      }
    }

    public IFileResourceData Data
    {
      get
      {
        return this.dataInfo.Data;
      }
    }
  }
}
