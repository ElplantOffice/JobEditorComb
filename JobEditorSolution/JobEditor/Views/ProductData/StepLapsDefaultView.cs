using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class StepLapsDefaultView : ProductDataViewBase, ICloneable
	{
		private int numberOfSteps;

		private int numberOfSame;

		private double @value;

		public int NumberOfSame
		{
			get
			{
				return this.numberOfSame;
			}
			set
			{
				int num = this.numberOfSame;
				this.numberOfSame = value;
				base.RaisePropertyChanged("NumberOfSame", true);
			}
		}

		public bool NumberOfSame_Valid
		{
			get
			{
				return base.GetValidState("NumberOfSame");
			}
			set
			{
				base.SetValidState("NumberOfSame", value);
			}
		}

		public int NumberOfSteps
		{
			get
			{
				return this.numberOfSteps;
			}
			set
			{
				int num = this.numberOfSteps;
				this.numberOfSteps = value;
				base.RaisePropertyChanged("NumberOfSteps", true);
			}
		}

		public bool NumberOfSteps_Valid
		{
			get
			{
				return base.GetValidState("NumberOfSteps");
			}
			set
			{
				base.SetValidState("NumberOfSteps", value);
			}
		}

		public ProductLib.StepLapsDefault StepLapsDefault
		{
			get
			{
				ProductLib.StepLapsDefault stepLapsDefault = new ProductLib.StepLapsDefault();
				stepLapsDefault.NumberOfSteps = this.NumberOfSteps;
				stepLapsDefault.NumberOfSame = this.NumberOfSame;
				stepLapsDefault.Value = this.Value;
				return stepLapsDefault;
			}
			set
			{
				ProductLib.StepLapsDefault stepLapsDefault = value;
				if (stepLapsDefault == null)
				{
					this.Clear(0, 0, 0);
					return;
				}
				this.NumberOfSteps = stepLapsDefault.NumberOfSteps;
				this.NumberOfSame = stepLapsDefault.NumberOfSame;
				this.Value = stepLapsDefault.Value;
			}
		}

		public double Value
		{
			get
			{
				return this.@value;
			}
			set
			{
				double num = this.@value;
				this.@value = value;
				base.RaisePropertyChanged("Value", true);
			}
		}

		public bool Value_Valid
		{
			get
			{
				return base.GetValidState("Value");
			}
			set
			{
				base.SetValidState("Value", value);
			}
		}

		public StepLapsDefaultView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.StepLapsDefault stepLapsDefault = null)
		{
			base.ProductView = productView;
			this.StepLapsDefault = stepLapsDefault;
		}

		public StepLapsDefaultView(JobEditor.Views.ProductData.ProductView productView, int nmbrSteps, int nmbrSame, double stepValue)
		{
			base.ProductView = productView;
			this.numberOfSteps = nmbrSteps;
			this.numberOfSame = nmbrSame;
			this.@value = stepValue;
		}

		public void Clear(int nmbrSteps = 0, int nmbrSame = 0, double stepValue = 0)
		{
			this.numberOfSteps = nmbrSteps;
			this.numberOfSame = nmbrSame;
			this.@value = stepValue;
		}

		public object Clone()
		{
			StepLapsDefaultView stepLapsDefaultView = new StepLapsDefaultView(base.ProductView, null);
			stepLapsDefaultView.CloneValidationDataFrom(this);
			stepLapsDefaultView.numberOfSteps = this.numberOfSteps;
			stepLapsDefaultView.numberOfSame = this.numberOfSame;
			stepLapsDefaultView.@value = this.@value;
			return stepLapsDefaultView;
		}

		public void CloneDataFrom(StepLapsDefaultView stepLapsDefaultView)
		{
			this.NumberOfSteps = stepLapsDefaultView.NumberOfSteps;
			this.NumberOfSame = stepLapsDefaultView.NumberOfSame;
			this.Value = stepLapsDefaultView.Value;
		}
	}
}