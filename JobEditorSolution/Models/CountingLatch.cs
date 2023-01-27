
using System;
using System.Threading;

namespace Models
{
  public class CountingLatch
  {
    private readonly object _counterLock = new object();

    public int LatchCounter { get; private set; }

    public bool IsLatched
    {
      get
      {
        return this.LatchCounter > 0;
      }
    }

    public void RunLatched(Action action)
    {
            try
            {
                lock (this._counterLock)
                {
                    this.LatchCounter = this.LatchCounter + 1;
                }
                action();
            }
            finally
            {
                lock (this._counterLock)
                {
                    this.LatchCounter = this.LatchCounter - 1;
                }
            }
        }

    public void RunIfNotLatched(Action action)
    {
      if (this.IsLatched)
        return;
      this.RunLatched(action);
    }
  }
}
