
using Models;
using UserControls.FileSelector;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using RelayCommand = JobEditor.Helpers.RelayCommand;

namespace JobEditor
{
	public class JobEditorFileSelector : INotifyPropertyChanged
	{
		private string textFiles;

		private string textGoUp;

		private string textSortByName;

		private string textSortByDate;

		private string textCancel;

		private string textOK;

		private string rootPath;

		private string currentPath;

		private string currentFile;

		private bool showFileExtension;

		private string defaultFileExtension;

		private string fileSearchPattern;

		private FileSelectorTypes type = FileSelectorTypes.Load;

		private bool forceRescanFiles;

		private string font = "Segoe UI";

		private UiLocalisation localisation;

		private const string textcancel = "Cancel";

		private const string textok = "OK";

		private const string textfiles = "Files";

		private const string textgoup = "Go up";

		private const string textsortbydate = "Sort by date";

		private const string textsortbyname = "Sort by name";

		public RelayCommand CommandCancel
		{
			get;
			set;
		}

		public RelayCommand CommandOK
		{
			get;
			set;
		}

		public string CurrentFile
		{
			get
			{
				return this.currentFile;
			}
			set
			{
				this.currentFile = value;
				this.RaisePropertyChanged("CurrentFile");
			}
		}

		public string CurrentPath
		{
			get
			{
				return this.currentPath;
			}
			set
			{
				this.currentPath = value;
				this.RaisePropertyChanged("CurrentPath");
			}
		}

		public string DefaultFileExtension
		{
			get
			{
				return this.defaultFileExtension;
			}
			set
			{
				this.defaultFileExtension = value;
				this.RaisePropertyChanged("DefaultFileExtension");
			}
		}

		public string FileSearchPattern
		{
			get
			{
				return this.fileSearchPattern;
			}
			set
			{
				this.fileSearchPattern = value;
				this.RaisePropertyChanged("FileSearchPattern");
			}
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

		public bool ForceRescanFiles
		{
			get
			{
				return this.forceRescanFiles;
			}
			set
			{
				this.forceRescanFiles = value;
				this.RaisePropertyChanged("ForceRescanFiles");
			}
		}

		public Action OnClose
		{
			get;
			set;
		}

		public Action<string, string> OnOk
		{
			get;
			set;
		}

		public string RootPath
		{
			get
			{
				return this.rootPath;
			}
			set
			{
				this.rootPath = value;
				this.RaisePropertyChanged("RootPath");
			}
		}

		public bool ShowFileExtension
		{
			get
			{
				return this.showFileExtension;
			}
			set
			{
				this.showFileExtension = value;
				this.RaisePropertyChanged("ShowFileExtension");
			}
		}

		public string TextCancel
		{
			get
			{
				return this.textCancel;
			}
			set
			{
				this.textCancel = value;
				this.RaisePropertyChanged("TextCancel");
			}
		}

		public string TextFiles
		{
			get
			{
				return this.textFiles;
			}
			set
			{
				this.textFiles = value;
				this.RaisePropertyChanged("TextFiles");
			}
		}

		public string TextGoUp
		{
			get
			{
				return this.textGoUp;
			}
			set
			{
				this.textGoUp = value;
				this.RaisePropertyChanged("TextGoUp");
			}
		}

		public string TextOK
		{
			get
			{
				return this.textOK;
			}
			set
			{
				this.textOK = value;
				this.RaisePropertyChanged("TextOK");
			}
		}

		public string TextSortByDate
		{
			get
			{
				return this.textSortByDate;
			}
			set
			{
				this.textSortByDate = value;
				this.RaisePropertyChanged("TextSortByDate");
			}
		}

		public string TextSortByName
		{
			get
			{
				return this.textSortByName;
			}
			set
			{
				this.textSortByName = value;
				this.RaisePropertyChanged("TextSortByName");
			}
		}

		public FileSelectorTypes Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				this.RaisePropertyChanged("Type");
			}
		}

		public JobEditorFileSelector(UiLocalisation localisation)
		{
			this.localisation = localisation;
			this.CommandOK = new RelayCommand(new Action<object>(this.FileSelectorOnOK));
			this.CommandCancel = new RelayCommand(new Action<object>(this.FileSelectorOnCancel));
			this.Translate();
			this.Font = SystemFonts.MenuFont.FontFamily.Name;
		}

		private void FileSelectorOnCancel(object parameter)
		{
			if (this.OnClose != null)
			{
				this.OnClose();
			}
		}

		private void FileSelectorOnOK(object parameter)
		{
			if (this.OnClose != null)
			{
				this.OnClose();
			}
			if (!string.IsNullOrWhiteSpace(this.CurrentFile) && this.OnOk != null)
			{
				this.OnOk(this.CurrentFile, this.CurrentPath);
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
			this.TextCancel = this.TranslateItem("Cancel");
			this.TextOK = this.TranslateItem("OK");
			this.TextFiles = this.TranslateItem("Files");
			this.TextGoUp = this.TranslateItem("Go up");
			this.TextSortByDate = this.TranslateItem("Sort by date");
			this.TextSortByName = this.TranslateItem("Sort by name");
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