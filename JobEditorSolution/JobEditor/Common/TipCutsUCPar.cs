using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace JobEditor.Common
{
	public class TipCutsUCPar : INotifyPropertyChanged
	{
		private double tipCutsUserControlWidth;

		public double TipCutsUserControlWidth
		{
			get
			{
				return this.tipCutsUserControlWidth;
			}
			set
			{
				this.tipCutsUserControlWidth = value;
				this.RaisePropertyChanged("TipCutsUserControlWidth");
			}
		}

		public TipCutsUCPar()
		{
			this.tipCutsUserControlWidth = 216;
		}

		private void RaisePropertyChanged(string prop)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}