using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Windows.Automation;
using System.Windows.Interop;



namespace CustomControlLibrary
{
    public class Window : System.Windows.Window
    {
        /// <summary>
        /// Name of the window defined in Xaml
        /// </summary>
        private const string windowName = "CustomWindow";

        /// <summary>
        /// Name of the focusable grid defined in Xaml
        /// </summary>
        private const string focusGrid = "FocusGrid";

        /// <summary>
        /// Name of the property to know if a control is editable
        /// </summary>
        private const string editablePropertyName = "IsEditable";

        /// <summary>
        /// virtualKeyBoard field to handle the virtual keyboard...
        /// </summary>
        internal CustomControlLibrary.Keyboard virtualKeyBoard { get; private set; }
        /// <summary>
        /// Internal enum object to pass it true the Keyboard Init Methode
        /// </summary>
        private CustomControlLibrary.Keyboard.Detecting keyBoardDetect;

        /// <summary>
        /// Field for making the transition animation
        /// </summary>
        private DoubleAnimation doubleAnimation = new DoubleAnimation();

        [DefaultValue(true)]
        public bool KeyboardDetect
        {
            get
            {
                if (keyBoardDetect == Keyboard.Detecting.Yes) return true;



                else return false;
            }
            set
            {
                if (value) keyBoardDetect = Keyboard.Detecting.Yes;




                else keyBoardDetect = Keyboard.Detecting.No;
            }
        }


        /// <summary>
        /// Property YPos to set the YPos of the window with transition, use always this property to set the YPosition of the window in stead
        /// of the Top property.
        /// </summary>
        public double YPos
        {


            get { return this.Top; }
            set { if (virtualKeyBoard != null) virtualKeyBoard.AnimationBusy = true; this.BeginAnimation(Window.TopProperty, CreateTransition(this.Top, value)); }
        }


        /// <summary>
        /// Constructor of the inherrited window class to initialize the styles.
        /// </summary>
        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
        }


        /// <summary>
        /// Cleanup Window
        /// </summary>
        ~Window()
        {


        }


        /// <summary>
        /// Overide function OnApplyTemplate() the attach events and initiate a virtual keyboard object.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //Initiate delegates for handling the associated events....
            //this.MouseDown += Window_MouseDown;
            this.TouchDown += Window_TouchDown;
            //this.TouchDown -= Window_TouchDown;
            this.Closed += Window_Closed;
            doubleAnimation.Completed += AnimationCompleted;

            //Initiate the virtualKeyBoard object the Keyboard constructor is a static constructor
            //that causes that we cannot pass arguments by constructor
            virtualKeyBoard = new CustomControlLibrary.Keyboard();
            virtualKeyBoard.AnimationBusy = false;
            //Because we cannot pass arguments by constructor we have to initialize the keyboard
            //with a InitKeyboard methode...
            virtualKeyBoard.InitKeyboard(this, keyBoardDetect);
            //Registre a name for this window because we need this name for creating transition and storyboard
            this.RegisterName(windowName, this);

            // Set Base color of Window
            Application.Current.Resources["BaseBackgroundThemeBrush"] = Background;

            AddHandler(MouseDownEvent, new RoutedEventHandler(Window_MouseDown));

        }

        /// <summary>
        /// To cleanup some internal fields, delegates, ...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            this.MouseDown -= Window_MouseDown;
            this.TouchDown -= Window_TouchDown;
            doubleAnimation.Completed -= AnimationCompleted;
        }

        /// <summary>
        /// when the animation is completed then cleanup the animation...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimationCompleted(object sender, EventArgs e)
        {
            this.BeginAnimation(Window.TopProperty, null);
            if (virtualKeyBoard != null) virtualKeyBoard.AnimationBusy = false;



        }

        /// <summary>
        /// Initialize the window transition animation
        /// </summary>
        /// <param name="from">double: Starting point of transition</param>
        /// <param name="to">double: End point of transition</param>
        private DoubleAnimation CreateTransition(double from, double to)
        {
            doubleAnimation.From = from;
            doubleAnimation.To = to;

            Console.WriteLine("Trans from {0} to {1}", from, to);

            //If the window should move up then set the time for transition otherwise set time to 0
            if (from > to)
            {


                doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(Math.Abs(to / 1000.0)));
                doubleAnimation.BeginTime = new TimeSpan(0, 0, 0, 0, 150);
            }
            else
            {

                doubleAnimation.BeginTime = new TimeSpan(0, 0, 0, 0, 100);
                doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0));
            }



































            return doubleAnimation;
        }

        /// <summary>
        /// Handles the MouseDown event to set the focus on internal grid
        /// that has to be done because we need to remove the focus of the
        /// editable control when a MouseDown is happend outside the focused editable control
        /// </summary>
        /// <param name="sender">object: Sender of the Event</param>
        /// <param name="e">System.Windows.Input.MouseButtonEventArgs: Information about the Event</param>
        private void Window_MouseDown(object sender, RoutedEventArgs e)
        {
            //((Grid)(this.GetTemplateChild(focusGrid))).Focus();
        }


        /// <summary>
        /// Handles the TouchDown event to set the focus on internal grid
        /// that has to be done because we need to remove the focus of the
        /// editable control when a TouchDown is happend outside the focused editable control
        /// </summary>
        /// <param name="sender">object: Sender of the Event</param>
        /// <param name="e">System.Windows.Input.TouchEventArgs: Information about the Event</param>
        private void Window_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (virtualKeyBoard != null)
                if (virtualKeyBoard.AnimationBusy) return;









            ((Grid)(this.GetTemplateChild(focusGrid))).Focus();
        }
    }
}
