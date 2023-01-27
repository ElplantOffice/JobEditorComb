
using Microsoft.Win32;
using System;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControlLibrary
{
  public class Keyboard
  {
    private const string keyboardDetectFilter = "Select * from Win32_Keyboard";
    private const string taskBarWindowName = "Shell_TrayWnd";
    private const string taskbarChildWindowName = "ReBarWindow32";
    private const string tipBandWindowName = "TIPBand";
    private const string virtualKeyboardWindowName = "IPTip_Main_Window";
    private const string tapTipregistryPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\TabletTip\\1.7";
    private const string tapTipDockedParameter = "EdgeTargetDockedState";
    private const string editablePropertyName = "IsEditable";
    private const string userDllName = "user32.dll";
    private static bool bridgeHandleKeyboard;
    private double screenResHeight;
    private const double keyBoardHeight = 278.0;
    private const double topScreenSpare = 50.0;
    private Keyboard.Detecting keyBoardDetect;
    private Point originalPosition;
    private double cumulativeYPos;
    private double oldCumulativeYPos;
    private bool keyBoardVisible;

    public bool AnimationBusy { get; set; }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

    static Keyboard()
    {
      EventManager.RegisterClassHandler(typeof (Window), UIElement.TouchUpEvent, (Delegate) new EventHandler<TouchEventArgs>(Keyboard.ControlTouchUp));
      EventManager.RegisterClassHandler(typeof (Window), TextBox.TapToKeyboardEvent, (Delegate) new RoutedEventHandler(Keyboard.ControlTouchUp));
      EventManager.RegisterClassHandler(typeof (Window), ComboBox.TapToKeyboardEvent, (Delegate) new RoutedEventHandler(Keyboard.ControlTouchUp));
    }

    public void InitKeyboard(Window window, Keyboard.Detecting keyBoardDetect)
    {
      this.screenResHeight = SystemParameters.PrimaryScreenHeight;
      this.keyBoardDetect = keyBoardDetect;
    }

    private static void ControlTouchUp(object sender, RoutedEventArgs e)
    {
      try
      {
        if (!(sender is Window))
          return;
        Window window = sender as Window;
        window.virtualKeyBoard.HandleKeyboard(window, (UIElement) e.OriginalSource);
      }
      catch (InvalidCastException ex)
      {
      }
    }

    private bool IsKeyboardConnected()
    {
      if (this.keyBoardDetect != Keyboard.Detecting.Yes)
        return false;
      ManagementObjectCollection objectCollection;
      using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_Keyboard"))
        objectCollection = managementObjectSearcher.Get();
      return objectCollection.Count >= 1;
    }

    private IntPtr GetTipBand()
    {
      return Keyboard.FindWindowEx(Keyboard.FindWindowEx(Keyboard.FindWindow("Shell_TrayWnd", (string) null), IntPtr.Zero, "ReBarWindow32", (string) null), IntPtr.Zero, "TIPBand", (string) null);
    }

    private void StartVirtualKeyboard()
    {
      IntPtr tipBand = this.GetTipBand();
      Keyboard.SendMessage(tipBand, 513, 1U, 1376285U);
      Keyboard.SendMessage(tipBand, 514, 0U, 1376285U);
    }

    public static void StartKeyboard()
    {
      Keyboard keyboard = new Keyboard();
      Keyboard.bridgeHandleKeyboard = true;
      if (keyboard.IsKeyboardConnected())
        return;
      keyboard.StartVirtualKeyboard();
    }

    public void DisposeVirtualKeyboard()
    {
      Keyboard.PostMessage(Keyboard.FindWindow("IPTip_Main_Window", (string) null), 274U, 61536, 0);
    }

    public static void DisposeKeyboard()
    {
      Keyboard keyboard = new Keyboard();
      Keyboard.bridgeHandleKeyboard = false;
      if (keyboard.IsKeyboardConnected())
        return;
      keyboard.DisposeVirtualKeyboard();
    }

    private bool KeyboardDocked()
    {
      return (int) Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\TabletTip\\1.7", "EdgeTargetDockedState", (object) 0) != 0;
    }

    private void HandleKeyboard(Window window, UIElement control)
    {
      if (this.AnimationBusy || Keyboard.bridgeHandleKeyboard)
        return;
      if (window == null)
        throw new ArgumentNullException(nameof (window));
      if (window == null)
        throw new ArgumentNullException(nameof (control));
      if (this.IsKeyboardConnected())
        return;
      if (control.IsEnabled && this.IsControlEditable(control))
      {
        Console.WriteLine("Keyboard Visible = {0}", (object) this.keyBoardVisible);
        if (!this.keyBoardVisible)
        {
          Console.WriteLine("Getting Original Y position {0}", (object) window.Top);
          this.originalPosition = new Point(window.Left, window.Top);
        }
        this.keyBoardVisible = true;
        if (this.KeyboardDocked())
          this.PositioningWindow(window, control);
        this.StartVirtualKeyboard();
      }
      else
      {
        this.DisposeVirtualKeyboard();
        this.PositioningWindow(window, (UIElement) null);
        this.keyBoardVisible = false;
      }
    }

    private double GetHeight(UIElement control)
    {
      if (!(control is FrameworkElement))
        return 0.0;
      return ((FrameworkElement) control).ActualHeight;
    }

    private PropertyInfo HasProperty(object objectToCheck, string propertyname)
    {
      return objectToCheck.GetType().GetProperty(propertyname);
    }

    private bool IsControlEditable(UIElement control)
    {
      if (control == null)
        throw new ArgumentNullException(nameof (control));
      bool flag = false;
      if (control.GetType() == typeof (TextBox) || control.GetType() == typeof (PasswordBox))
      {
        flag = true;
      }
      else
      {
        PropertyInfo propertyInfo = this.HasProperty((object) control, "IsEditable");
        if (propertyInfo != (PropertyInfo) null && (bool) propertyInfo.GetValue((object) control))
          flag = true;
      }
      return flag;
    }

    private Point TranslatePoints(Window window, Point point)
    {
      return PresentationSource.FromVisual((Visual) window).CompositionTarget.TransformFromDevice.Transform(point);
    }

    private Keyboard.Position CalcWindowPos(double referencePosition, double yPosControl, double yPosWindow)
    {
      Keyboard.Position position;
      position.Shift = 0.0;
      position.YPos = yPosWindow;
      if (yPosControl < 50.0)
      {
        position.Shift = 50.0 - yPosControl;
        position.YPos = yPosWindow + position.Shift;
      }
      if (yPosControl > referencePosition)
      {
        position.Shift = referencePosition - yPosControl;
        position.YPos = yPosWindow + position.Shift;
      }
      return position;
    }

    private Keyboard.Position HandleWindowBoundaries(double referencePosition, double yPosControl, double yPosWindow, double heightWindow, Keyboard.Position newWindowMovement)
    {
      Keyboard.Position position;
      position.YPos = newWindowMovement.YPos;
      position.Shift = newWindowMovement.Shift;
      double num = this.screenResHeight - 278.0;
      if (yPosControl + position.Shift > referencePosition && position.YPos + heightWindow < num)
      {
        position.YPos = num - heightWindow;
        ref Keyboard.Position local = ref position;
        local.Shift = local.YPos - yPosWindow;
      }
      if (yPosControl + position.Shift < 50.0 && newWindowMovement.YPos > this.originalPosition.Y)
      {
        position.YPos = this.originalPosition.Y;
        ref Keyboard.Position local = ref position;
        local.Shift = local.YPos - yPosWindow;
      }
      if (position.YPos + heightWindow < num)
      {
        position.Shift = -278.0;
        position.YPos = this.originalPosition.Y - 278.0;
      }
      return position;
    }

    private void PositioningWindow(Window window, UIElement control)
    {
      if (window == null)
        throw new ArgumentNullException(nameof (window));
      if (control != null)
      {
        Point point = this.TranslatePoints(window, control.PointToScreen(new Point(0.0, 0.0)));
        double referencePosition = Math.Round((this.screenResHeight - 278.0) * 2.0 / 3.0);
        Keyboard.Position newWindowMovement = this.CalcWindowPos(referencePosition, point.Y, window.YPos);
        Keyboard.Position position = this.HandleWindowBoundaries(referencePosition, point.Y, window.YPos, window.Height, newWindowMovement);
        if (Math.Abs(window.YPos - position.YPos) <= 1E-05)
          return;
        window.YPos = position.YPos;
        this.cumulativeYPos += position.Shift;
      }
      else
      {
        window.YPos = 0.0;
        this.cumulativeYPos = 0.0;
      }
    }

    public enum Detecting
    {
      Yes,
      No,
    }

    private struct Position
    {
      public double YPos;
      public double Shift;
    }
  }
}
