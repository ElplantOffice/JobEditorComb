using JobEditor.Common;
using ProductLib;
using System;
using System.Collections.Generic;

namespace JobEditor.Views.ProductData
{
	public class StepLapView : ProductDataViewBase, ICloneable
	{
		private List<StepView> stepViews = new List<StepView>();

		private EStepLapType type;

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

		public ProductLib.StepLap StepLap
		{
			get
			{
				ProductLib.StepLap stepLap = new ProductLib.StepLap();
				stepLap.Type = this.Type;
				stepLap.NumberOfSteps = this.NumberOfSteps;
				stepLap.NumberOfSame = this.NumberOfSame;
				stepLap.Value = this.Value;
				stepLap.Steps.Clear();
				if (this.StepViews != null)
				{
					foreach (StepView stepView in this.StepViews)
					{
						Step step = stepView.Step;
						stepLap.Steps.Add(step);
					}
				}
				return stepLap;
			}
			set
			{
				ProductLib.StepLap stepLap = value;
				if (stepLap == null)
				{
					this.Clear();
					return;
				}
				this.Type = stepLap.Type;
				this.NumberOfSteps = stepLap.NumberOfSteps;
				this.NumberOfSame = stepLap.NumberOfSame;
				this.Value = stepLap.Value;
				this.StepViews.Clear();
				if (stepLap.Steps != null)
				{
					foreach (Step step in stepLap.Steps)
					{
						StepView stepView = new StepView(base.ProductView, step);
						this.StepViews.Add(stepView);
					}
				}
			}
		}

		public List<StepView> StepViews
		{
			get
			{
				return this.stepViews;
			}
			set
			{
				this.stepViews = value;
				base.RaisePropertyChanged("StepViews", true);
			}
		}

		public EStepLapType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				base.RaisePropertyChanged("Type", true);
			}
		}

		public bool Type_Valid
		{
			get
			{
				return base.GetValidState("Type");
			}
			set
			{
				base.SetValidState("Type", value);
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

		public StepLapView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.StepLap stepLap = null)
		{
			base.ProductView = productView;
			this.StepLap = stepLap;
		}

		public void Clear()
		{
			this.Type = EStepLapType.CTipLeft_Down;
			this.NumberOfSteps = 0;
			this.NumberOfSame = 0;
			this.Value = 0;
			this.StepViews.Clear();
		}

		public object Clone()
		{
			StepLapView stepLapView = new StepLapView(base.ProductView, null);
			stepLapView.CloneValidationDataFrom(this);
			stepLapView.type = this.type;
			stepLapView.numberOfSteps = this.numberOfSteps;
			stepLapView.numberOfSame = this.numberOfSame;
			stepLapView.@value = this.@value;
			return stepLapView;
		}

		public void CloneDataFrom(StepLapView stepLapView)
		{
			if (stepLapView.StepViews == null)
			{
				this.StepViews.Clear();
				this.StepViews = null;
			}
			else
			{
				if (this.StepViews == null)
				{
					this.StepViews = new List<StepView>();
				}
				for (int i = 0; i < stepLapView.StepViews.Count; i++)
				{
					if (this.StepViews.Count <= i)
					{
						this.StepViews.Add(new StepView(base.ProductView, null));
					}
					StepView item = stepLapView.StepViews[i];
					this.StepViews[i].CloneDataFrom(item);
				}
				while (this.StepViews.Count > stepLapView.StepViews.Count)
				{
					this.StepViews.RemoveAt(this.StepViews.Count - 1);
				}
			}
			this.Type = stepLapView.Type;
			this.NumberOfSteps = stepLapView.NumberOfSteps;
			this.NumberOfSame = stepLapView.NumberOfSame;
			this.Value = stepLapView.Value;
		}
	}
}