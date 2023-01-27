using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace JobEditor.Common
{
	public class StepLapsUCPar : INotifyPropertyChanged
	{
		private double stepLapsUserControlWidth;

		public double StepLapsUserControlWidth
		{
			get
			{
				return this.stepLapsUserControlWidth;
			}
			set
			{
				this.stepLapsUserControlWidth = value;
				this.RaisePropertyChanged("StepLapsUserControlWidth");
			}
		}

		public StepLapsUCPar()
		{
			this.stepLapsUserControlWidth = 216;
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