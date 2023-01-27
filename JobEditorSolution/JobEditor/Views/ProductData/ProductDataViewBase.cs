using JobEditor.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace JobEditor.Views.ProductData
{
	public class ProductDataViewBase : ValidationViewBase, INotifyPropertyChanged
	{
		public JobEditor.Views.ProductData.ProductView ProductView
		{
			get;
			protected set;
		}

		public ProductDataViewBase()
		{
		}

		protected void RaisePropertyChanged(string propertyName, bool callOnPropertyChanged = true)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
			if (callOnPropertyChanged && this.ProductView != null && this.ProductView.OnPropertyChanged != null && this.ProductView.EnableOnPropertyChanged)
			{
				this.ProductView.OnPropertyChanged(this, propertyName);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}