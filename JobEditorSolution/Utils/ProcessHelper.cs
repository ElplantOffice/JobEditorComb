
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;

namespace Utils
{
  public class ProcessHelper
  {
    private static string watchProcessFileName = "";
    private static ManagementEventWatcher watcherProcessStarted = (ManagementEventWatcher) null;
    private static ManagementEventWatcher watcherProcessEnded = (ManagementEventWatcher) null;
    private static Action<string> actionProcessStarted;
    private static Action<string> actionProcessEnded;

    [DllImport("User32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.DLL", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    public static void WatchProcess(string processFileName, Action<string> actionProcessStarted, Action<string> actionProcessEnded)
    {
      ProcessHelper.watchProcessFileName = Path.GetFullPath(processFileName);
      ProcessHelper.watcherProcessStarted = ProcessHelper.WatchForProcessStart(processFileName, actionProcessStarted);
      ProcessHelper.watcherProcessEnded = ProcessHelper.WatchForProcessEnd(processFileName, actionProcessEnded);
      Application.Current.MainWindow.Closed -= new EventHandler(ProcessHelper.OnMainWindow_Closed);
      Application.Current.MainWindow.Closed += new EventHandler(ProcessHelper.OnMainWindow_Closed);
    }

    private static void OnMainWindow_Closed(object sender, EventArgs e)
    {
      ProcessHelper.EndWatchProcess();
    }

    private static void EndWatchProcess()
    {
      if (ProcessHelper.watcherProcessStarted != null)
      {
        ProcessHelper.watcherProcessStarted.Stop();
        ProcessHelper.watcherProcessStarted.Dispose();
        ProcessHelper.watcherProcessStarted = (ManagementEventWatcher) null;
        ProcessHelper.actionProcessStarted = (Action<string>) null;
      }
      if (ProcessHelper.watcherProcessEnded != null)
      {
        ProcessHelper.watcherProcessEnded.Stop();
        ProcessHelper.watcherProcessEnded.Dispose();
        ProcessHelper.watcherProcessEnded = (ManagementEventWatcher) null;
        ProcessHelper.actionProcessEnded = (Action<string>) null;
      }
      ProcessHelper.watchProcessFileName = "";
    }

    private static ManagementEventWatcher WatchForProcessStart(string processFileName, Action<string> actionProcessStarted)
    {
      ManagementEventWatcher managementEventWatcher = new ManagementEventWatcher("\\\\.\\root\\CIMV2", "SELECT TargetInstance  FROM __InstanceCreationEvent WITHIN  10  WHERE TargetInstance ISA 'Win32_Process'    AND TargetInstance.Name = '" + Path.GetFileName(processFileName) + "'");
      managementEventWatcher.EventArrived += new EventArrivedEventHandler(ProcessHelper.ProcessStarted);
      managementEventWatcher.Start();
      ProcessHelper.actionProcessStarted = actionProcessStarted;
      return managementEventWatcher;
    }

    private static ManagementEventWatcher WatchForProcessEnd(string processFileName, Action<string> actionProcessEnded)
    {
      ManagementEventWatcher managementEventWatcher = new ManagementEventWatcher("\\\\.\\root\\CIMV2", "SELECT TargetInstance  FROM __InstanceDeletionEvent WITHIN  10  WHERE TargetInstance ISA 'Win32_Process'    AND TargetInstance.Name = '" + Path.GetFileName(processFileName) + "'");
      managementEventWatcher.EventArrived += new EventArrivedEventHandler(ProcessHelper.ProcessEnded);
      managementEventWatcher.Start();
      ProcessHelper.actionProcessEnded = actionProcessEnded;
      return managementEventWatcher;
    }

    private static void ProcessEnded(object sender, EventArrivedEventArgs e)
    {
      string strA = ((ManagementBaseObject) e.NewEvent.Properties["TargetInstance"].Value).Properties["ExecutablePath"].Value.ToString();
      if (string.Compare(strA, ProcessHelper.watchProcessFileName, true) != 0 || ProcessHelper.actionProcessEnded == null)
        return;
      ProcessHelper.actionProcessEnded(strA);
    }

    private static void ProcessStarted(object sender, EventArrivedEventArgs e)
    {
      string strA = ((ManagementBaseObject) e.NewEvent.Properties["TargetInstance"].Value).Properties["ExecutablePath"].Value.ToString();
      if (string.Compare(strA, ProcessHelper.watchProcessFileName, true) != 0 || ProcessHelper.actionProcessStarted == null)
        return;
      ProcessHelper.actionProcessStarted(strA);
    }

    public static Process FindProcess(string processFileName)
    {
      string fullPath = Path.GetFullPath(processFileName);
      string withoutExtension = Path.GetFileNameWithoutExtension(fullPath);
      Path.GetDirectoryName(fullPath);
      foreach (Process process in Process.GetProcessesByName(withoutExtension))
      {
        if (process.MainModule.ModuleName != null && string.Compare(process.MainModule.FileName, fullPath, true) == 0)
          return process;
      }
      return (Process) null;
    }

    public static void ShowProcess(string processFileName, ProcessHelper.ShowWindowCmd cmd = ProcessHelper.ShowWindowCmd.SW_SHOWNORMAL)
    {
      ProcessHelper.ShowProcess(ProcessHelper.FindProcess(processFileName), cmd);
    }

    public static void ShowProcess(Process process)
    {
      if (process == null || !(process.MainWindowHandle != IntPtr.Zero))
        return;
      ProcessHelper.ShowWindow(process.MainWindowHandle, 1);
      ProcessHelper.SetForegroundWindow(process.MainWindowHandle);
    }

    public static void ShowProcess(Process process, ProcessHelper.ShowWindowCmd cmd)
    {
      if (process == null || !(process.MainWindowHandle != IntPtr.Zero))
        return;
      ProcessHelper.ShowWindow(process.MainWindowHandle, (int) cmd);
      if (cmd != ProcessHelper.ShowWindowCmd.SW_SHOWNORMAL && cmd != ProcessHelper.ShowWindowCmd.SW_SHOWNOACTIVATE && (cmd != ProcessHelper.ShowWindowCmd.SW_SHOWMAXIMIZED && cmd != ProcessHelper.ShowWindowCmd.SW_SHOWDEFAULT))
        return;
      ProcessHelper.SetForegroundWindow(process.MainWindowHandle);
    }

    public static bool PreventStartupTwice(bool showRunningApplication)
    {
      Process currentProcess = Process.GetCurrentProcess();
      Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName.Replace(".vshost", ""));
      Process process1 = (Process) null;
      foreach (Process process2 in processesByName)
      {
        if (process2.Id != currentProcess.Id)
        {
          string moduleName = process2.MainModule.ModuleName;
          if (moduleName != null && moduleName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
          {
            process1 = process2;
            break;
          }
        }
      }
      if (process1 == null)
        return false;
      if (showRunningApplication)
      {
        IntPtr mainWindowHandle = process1.MainWindowHandle;
        if (mainWindowHandle != IntPtr.Zero)
        {
          if (ProcessHelper.IsIconic(mainWindowHandle))
            ProcessHelper.ShowWindowAsync(mainWindowHandle, 9);
          ProcessHelper.SetForegroundWindow(mainWindowHandle);
        }
      }
      Application.Current.Shutdown();
      return true;
    }

    public enum ShowWindowCmd
    {
      SW_HIDE = 0,
      SW_SHOWNORMAL = 1,
      SW_SHOWMINIMIZED = 2,
      SW_SHOWMAXIMIZED = 3,
      SW_SHOWNOACTIVATE = 4,
      SW_RESTORE = 9,
      SW_SHOWDEFAULT = 10, // 0x0000000A
    }
  }
}
