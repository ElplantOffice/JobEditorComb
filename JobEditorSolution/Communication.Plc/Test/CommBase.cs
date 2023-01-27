// Decompiled with JetBrains decompiler
// Type: Communication.Plc.Test.CommBase
// Assembly: Communication.Plc, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F768B24-950C-48A5-887A-4AC7B9CF613B
// Assembly location: D:\El PLant\DOK Job Editor Priprema\v3 dlls\costura.Communication.Plc.dll

using Communication.Plc.Ads;
using Communication.Plc;

namespace Communication.Plc.Test
{
  public class CommBase
  {
    private PlcAdministrator adsAdministrator;

    public CommBase()
    {
      PlcAddress plcAddress = new PlcAddress("AdsServer");
      this.adsAdministrator = new PlcAdministrator(new PlcModuleInfo(new AdsSettings("192.168.13.137.1.1", 851), plcAddress), plcAddress);
    }
  }
}
