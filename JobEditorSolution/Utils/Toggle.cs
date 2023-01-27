
using Models;
using System;
using System.Windows.Threading;

namespace Utils
{
  public class Toggle
  {
    private DispatcherTimer timer;
    private bool value;
    private ViewModelAttributedEventType<bool> control;

    public bool Value
    {
      get
      {
        return this.value;
      }
      set
      {
        this.value = value;
      }
    }

    public Toggle(long milliSeconds, ViewModelAttributedEventType<bool> control)
    {
      this.timer = new DispatcherTimer();
      this.timer.Interval = TimeSpan.FromMilliseconds((double) milliSeconds);
      this.timer.Tick += new EventHandler(this.timer_Tick);
      this.timer.Start();
      this.control = control;
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      this.timer.Stop();
      if (this.value)
      {
        this.value = false;
        this.control.Value = this.value;
      }
      else
      {
        this.value = true;
        this.control.Value = this.value;
      }
      this.timer.Start();
    }
  }
}
