
using System;

namespace UserControls.Login
{
  public class LogInEventArgs : EventArgs
  {
    public string Info { get; private set; }

    public string PinCode { get; private set; }

    public bool LoggedIn { get; private set; }

    public uint Level { get; private set; }

    public LogInEventArgs()
    {
    }

    public LogInEventArgs(string info, string pinCode, uint level, bool loggedIn)
    {
      this.Info = info;
      this.PinCode = pinCode;
      this.LoggedIn = loggedIn;
      this.Level = level;
    }
  }
}
