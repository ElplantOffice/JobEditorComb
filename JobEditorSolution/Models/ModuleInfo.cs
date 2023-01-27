
using System;
using System.Windows;

namespace Models
{
  [Serializable]
  public class ModuleInfo
  {
    public ModuleInfo(string baseId)
    {
      this.BaseId = baseId;
    }

    public string GetId(ModuleInfo.Types type)
    {
      string str = (string) null;
      switch (type)
      {
        case ModuleInfo.Types.Base:
          str = this.BaseId;
          break;
        case ModuleInfo.Types.Application:
          str = this.BaseId + ".Application";
          break;
        case ModuleInfo.Types.ModelAdministrator:
          str = this.BaseId + ".ModelAdministrator";
          break;
        case ModuleInfo.Types.ViewModelAdministrator:
          str = this.BaseId + ".ViewModelAdministrator";
          break;
        case ModuleInfo.Types.CommAdministrator:
          str = this.BaseId + ".CommAdministrator";
          break;
        case ModuleInfo.Types.PlcAdministrator:
          str = this.BaseId + ".PlcAdministrator";
          break;
        case ModuleInfo.Types.FileAdministrator:
          str = this.BaseId + ".FileAdministrator";
          break;
      }
      return str;
    }

    public string BaseId { get; private set; }

    public WindowState WindowState { get; set; }

    public enum Types
    {
      Base,
      Application,
      ModelAdministrator,
      ViewModelAdministrator,
      CommAdministrator,
      PlcAdministrator,
      FileAdministrator,
    }
  }
}
