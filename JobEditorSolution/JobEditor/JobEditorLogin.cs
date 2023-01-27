
using Models;
using UserControls.Login;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using RelayCommand = JobEditor.Helpers.RelayCommand;

namespace JobEditor
{
	public class JobEditorLogin : INotifyPropertyChanged
	{
		private string imgLogIn;

		private string imgCancel;

		private string loggedInInfo;

		private string loggedInPinCode;

		private uint loggedInLevel;

		private bool loggedIn;

		private string font = "Segoe UI";

		private UiLocalisation localisation;

		private const string textcancel = "Cancel";

		private const string textok = "OK";

		public RelayCommand CommandCancel
		{
			get;
			set;
		}

		public RelayCommand CommandLogIn
		{
			get;
			set;
		}

		public string Font
		{
			get
			{
				return this.font;
			}
			set
			{
				this.font = value;
				this.RaisePropertyChanged("Font");
			}
		}

		public string ImgCancel
		{
			get
			{
				return this.imgCancel;
			}
			set
			{
				this.imgCancel = value;
				this.RaisePropertyChanged("ImgCancel");
			}
		}

		public string ImgLogIn
		{
			get
			{
				return this.imgLogIn;
			}
			set
			{
				this.imgLogIn = value;
				this.RaisePropertyChanged("ImgLogIn");
			}
		}

		public bool LoggedIn
		{
			get
			{
				return this.loggedIn;
			}
			set
			{
				this.loggedIn = value;
			}
		}

		public string LoggedInInfo
		{
			get
			{
				return this.loggedInInfo;
			}
			set
			{
				this.loggedInInfo = value;
			}
		}

		public uint LoggedInLevel
		{
			get
			{
				return this.loggedInLevel;
			}
			set
			{
				this.loggedInLevel = value;
			}
		}

		public string LoggedInPinCode
		{
			get
			{
				return this.loggedInPinCode;
			}
			set
			{
				this.loggedInPinCode = value;
			}
		}

		public Action OnClose
		{
			get;
			set;
		}

		public Action<bool, string, string, uint> OnOk
		{
			get;
			set;
		}

		public JobEditorLogin(UiLocalisation localisation)
		{
			this.localisation = localisation;
			this.CommandLogIn = new RelayCommand(new Action<object>(this.LogInOnOK));
			this.CommandCancel = new RelayCommand(new Action<object>(this.LogInOnCancel));
			this.Translate();
			this.Font = SystemFonts.MenuFont.FontFamily.Name;
		}

		public void ClearLoggedInData()
		{
			this.LoggedIn = false;
			this.LoggedInInfo = "";
			this.LoggedInPinCode = "";
		}

		private void LogInOnCancel(object parameter)
		{
			if (this.OnClose != null)
			{
				this.OnClose();
			}
		}

		private void LogInOnOK(object parameter)
		{
			LogInEventArgs logInEventArg = (LogInEventArgs)parameter;
			if (logInEventArg.LoggedIn)
			{
				if (this.OnClose != null)
				{
					this.OnClose();
				}
				if (this.OnOk != null)
				{
					this.OnOk(logInEventArg.LoggedIn, logInEventArg.Info, logInEventArg.PinCode, logInEventArg.Level);
				}
			}
		}

		internal void RaisePropertyChanged(string prop)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		public void Translate()
		{
		}

		public string TranslateItem(string textId)
		{
			string str = textId;
			this.localisation.Translate(this.localisation.Settings.LocalisationId, ref str, null);
			return str;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}