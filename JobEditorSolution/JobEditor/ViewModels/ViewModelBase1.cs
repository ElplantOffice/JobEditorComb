using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace JobEditor.ViewModels
{
	public class ViewModelBase1 : INotifyPropertyChanged
	{
		private bool? _CloseWindowFlag;

		public bool? CloseWindowFlag
		{
			get
			{
				return this._CloseWindowFlag;
			}
			set
			{
				this._CloseWindowFlag = value;
				this.RaisePropertyChanged("CloseWindowFlag");
			}
		}

		public ViewModelBase1()
		{
		}

		public virtual void CloseWindow(bool? result = true)
		{
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				bool? nullable;
				bool? closeWindowFlag = this.CloseWindowFlag;
				if (!closeWindowFlag.HasValue)
				{
					nullable = new bool?(true);
				}
				else
				{
					closeWindowFlag = this.CloseWindowFlag;
					nullable = (closeWindowFlag.HasValue ? new bool?(!closeWindowFlag.GetValueOrDefault()) : null);
				}
				this.CloseWindowFlag = nullable;
			}));
		}

		internal void RaisePropertyChanged(string prop)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}