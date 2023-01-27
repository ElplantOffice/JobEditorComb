using CustomControlLibrary;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace UserControls.Login
{
    public partial class LoginUserControl : UserControl
    {
        public static DependencyProperty UserSourceProperty;

        public static DependencyProperty CommandLogInProperty;

        public static DependencyProperty CommandCancelProperty;

        public static DependencyProperty InfoProperty;

        public static DependencyProperty PinCodeProperty;

        public static DependencyProperty LoggedInProperty;

        public static DependencyProperty LevelProperty;

        public ICommand CommandCancel
        {
            get
            {
                return (ICommand)base.GetValue(LoginUserControl.CommandCancelProperty);
            }
            set
            {
                base.SetCurrentValue(LoginUserControl.CommandCancelProperty, value);
            }
        }

        public ICommand CommandLogIn
        {
            get
            {
                return (ICommand)base.GetValue(LoginUserControl.CommandLogInProperty);
            }
            set
            {
                base.SetCurrentValue(LoginUserControl.CommandLogInProperty, value);
            }
        }

        public string Info
        {
            get
            {
                return (string)base.GetValue(LoginUserControl.InfoProperty);
            }
            set
            {
                base.SetCurrentValue(LoginUserControl.InfoProperty, value);
            }
        }

        public uint Level
        {
            get
            {
                return (uint)base.GetValue(LoginUserControl.LevelProperty);
            }
            set
            {
                base.SetCurrentValue(LoginUserControl.LevelProperty, value);
            }
        }

        public bool LoggedIn
        {
            get
            {
                return (bool)base.GetValue(LoginUserControl.LoggedInProperty);
            }
            set
            {
                base.SetCurrentValue(LoginUserControl.LoggedInProperty, value);
            }
        }

        public string PinCode
        {
            get
            {
                return (string)base.GetValue(LoginUserControl.PinCodeProperty);
            }
            set
            {
                base.SetCurrentValue(LoginUserControl.PinCodeProperty, value);
            }
        }

        public string UserSource
        {
            get
            {
                return (string)base.GetValue(LoginUserControl.UserSourceProperty);
            }
            set
            {
                base.SetCurrentValue(LoginUserControl.UserSourceProperty, value);
            }
        }

        static LoginUserControl()
        {
            LoginUserControl.UserSourceProperty = DependencyProperty.Register("UserSource", typeof(string), typeof(LoginUserControl), new PropertyMetadata(new PropertyChangedCallback(LoginUserControl.OnUserSourcePropertyChanged)));
            LoginUserControl.CommandLogInProperty = DependencyProperty.Register("CommandLogIn", typeof(ICommand), typeof(LoginUserControl));
            LoginUserControl.CommandCancelProperty = DependencyProperty.Register("CommandCancel", typeof(ICommand), typeof(LoginUserControl));
            LoginUserControl.InfoProperty = DependencyProperty.Register("Info", typeof(string), typeof(LoginUserControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(LoginUserControl.OnInfoPropertyChanged), new CoerceValueCallback(LoginUserControl.OnInfoPropertyValidate))
            {
                BindsTwoWayByDefault = true
            }, null);
            LoginUserControl.PinCodeProperty = DependencyProperty.Register("PinCode", typeof(string), typeof(LoginUserControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(LoginUserControl.OnPinCodePropertyChanged), new CoerceValueCallback(LoginUserControl.OnPinCodePropertyValidate))
            {
                BindsTwoWayByDefault = true
            }, null);
            LoginUserControl.LoggedInProperty = DependencyProperty.Register("LoggedIn", typeof(bool), typeof(LoginUserControl), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(LoginUserControl.OnLoggedInPropertyChanged), new CoerceValueCallback(LoginUserControl.OnLoggedInPropertyValidate))
            {
                BindsTwoWayByDefault = true
            }, null);
            LoginUserControl.LevelProperty = DependencyProperty.Register("Level", typeof(uint), typeof(LoginUserControl), new FrameworkPropertyMetadata((object)((uint)0), new PropertyChangedCallback(LoginUserControl.OnLevelPropertyChanged), new CoerceValueCallback(LoginUserControl.OnLevelPropertyValidate))
            {
                BindsTwoWayByDefault = true
            }, null);
        }

        public LoginUserControl()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.CommandCancel != null)
            {
                this.CommandCancel.Execute(null);
            }
            CustomControlLibrary.Keyboard.DisposeKeyboard();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Accounts account = Accounts.Load(this.UserSource);
                this.LoggedIn = false;
                this.Info = "";
                if (account == null || account.Users.Count == 0)
                {
                    this.Info = "Default User";
                    this.LoggedIn = true;
                    this.Level = 0;
                }
                else
                {
                    this.PinCode = this.TxtBoxPassWord.Password;
                    foreach (User user in account.Users)
                    {
                        if (user.PinCode != this.PinCode)
                        {
                            continue;
                        }
                        this.Info = user.Info;
                        this.LoggedIn = true;
                        this.Level = user.Level;
                        goto Label0;
                    }
                }
            Label0:
                if (this.LoggedIn)
                {
                    CustomControlLibrary.Keyboard.DisposeKeyboard();
                }
                else
                {
                    this.TxtBoxPassWord.Password = "";
                    this.TxtBoxPassWord.Focus();
                    CustomControlLibrary.Keyboard.StartKeyboard();
                }
                if (this.CommandLogIn != null)
                {
                    this.CommandLogIn.Execute(new LogInEventArgs(this.Info, this.PinCode, this.Level, this.LoggedIn));
                }
            }
            catch (Exception exception)
            {
                IDictionary data = exception.Data;
            }
        }

        private static void OnInfoPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
        }

        private static object OnInfoPropertyValidate(DependencyObject d, object value)
        {
            return (string)value;
        }

        private static void OnLevelPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            uint newValue = (uint)e.NewValue;
        }

        private static object OnLevelPropertyValidate(DependencyObject d, object value)
        {
            return (uint)value;
        }

        private static void OnLoggedInPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;
        }

        private static object OnLoggedInPropertyValidate(DependencyObject d, object value)
        {
            return (bool)value;
        }

        private static void OnPinCodePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
        }

        private static object OnPinCodePropertyValidate(DependencyObject d, object value)
        {
            return (string)value;
        }

        private static void OnUserSourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
        }

        private void TxtBoxPassWord_GotFocus(object sender, RoutedEventArgs e)
        {
            this.TxtBoxPassWord.Password = "";
            CustomControlLibrary.Keyboard.StartKeyboard();
        }

        private void usrLogin_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((LoginUserControl)sender).CommandLogIn != null && ((LoginUserControl)sender).CommandCancel != null && (bool)e.NewValue)
            {
                this.TxtBoxPassWord.Password = "";
                base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => {
                    this.TxtBoxPassWord.Focus();
                    System.Windows.Input.Keyboard.Focus(this.TxtBoxPassWord);
                }));
                CustomControlLibrary.Keyboard.StartKeyboard();
            }
        }
    }
}