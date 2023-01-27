
namespace Messages
{
  public enum ApplicationCommand
  {
    CommAdminStateChanged = 0,
    ModelCreated = 3,
    ModelDisposed = 4,
    ClientInitialize = 11, // 0x0000000B
    ClientInitialized = 12, // 0x0000000C
    ClientDisposed = 13, // 0x0000000D
    Minimize = 21, // 0x00000015
    Minimized = 22, // 0x00000016
    Maximized = 23, // 0x00000017
    CloseSplashScreen = 24, // 0x00000018
    SettingsUnits = 40, // 0x00000028
    RegisterClient = 50, // 0x00000032
  }
}
