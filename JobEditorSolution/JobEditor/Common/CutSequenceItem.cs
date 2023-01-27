using JobEditor.Properties;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace JobEditor.Common
{
	public class CutSequenceItem : INotifyPropertyChanged
	{
		private string shape;

		private bool used;

		public int Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Shape
		{
			get
			{
				return string.Concat(this.Name, ".png");
			}
			private set
			{
				this.shape = value;
			}
		}

		public string ShapePath
		{
			get
			{
				return Settings.Default.SequenceMakerShapesPath;
			}
		}

		public bool Used
		{
			get
			{
				return this.used;
			}
			set
			{
				this.used = value;
				this.RaisePropertyChanged("Used");
			}
		}

		public CutSequenceItem()
		{
		}

		public CutSequenceItem(string name)
		{
			this.Name = name;
		}

		protected void RaisePropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}