using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class StackingView : ProductDataViewBase, ICloneable
	{
		private double altStepLapOffset;

		private double altStepLapNumOfSame;

		private double altSheetOffset;

		private double altSheetNumOfSame;

		public double AltSheetNumOfSame
		{
			get
			{
				return this.altSheetNumOfSame;
			}
			set
			{
				this.altSheetNumOfSame = value;
				base.RaisePropertyChanged("AltSheetNumOfSame", true);
			}
		}

		public bool AltSheetNumOfSame_Valid
		{
			get
			{
				return base.GetValidState("AltSheetNumOfSame");
			}
			set
			{
				base.SetValidState("AltSheetNumOfSame", value);
			}
		}

		public double AltSheetOffset
		{
			get
			{
				return this.altSheetOffset;
			}
			set
			{
				this.altSheetOffset = value;
				base.RaisePropertyChanged("AltSheetOffset", true);
			}
		}

		public bool AltSheetOffset_Valid
		{
			get
			{
				return base.GetValidState("AltSheetOffset");
			}
			set
			{
				base.SetValidState("AltSheetOffset", value);
			}
		}

		public double AltStepLapNumOfSame
		{
			get
			{
				return this.altStepLapNumOfSame;
			}
			set
			{
				this.altStepLapNumOfSame = value;
				base.RaisePropertyChanged("AltStepLapNumOfSame", true);
			}
		}

		public bool AltStepLapNumOfSame_Valid
		{
			get
			{
				return base.GetValidState("AltStepLapNumOfSame");
			}
			set
			{
				base.SetValidState("AltStepLapNumOfSame", value);
			}
		}

		public double AltStepLapOffset
		{
			get
			{
				return this.altStepLapOffset;
			}
			set
			{
				this.altStepLapOffset = value;
				base.RaisePropertyChanged("AltStepLapOffset", true);
			}
		}

		public bool AltStepLapOffset_Valid
		{
			get
			{
				return base.GetValidState("AltStepLapOffset");
			}
			set
			{
				base.SetValidState("AltStepLapOffset", value);
			}
		}

		public ProductLib.Stacking Stacking
		{
			get
			{
				ProductLib.Stacking stacking = new ProductLib.Stacking();
				stacking.AltStepLapOffset = this.AltStepLapOffset;
				stacking.AltStepLapNumOfSame = this.AltStepLapNumOfSame;
				stacking.AltSheetOffset = this.AltSheetOffset;
				stacking.AltSheetNumOfSame = this.AltSheetNumOfSame;
				return stacking;
			}
			set
			{
				ProductLib.Stacking stacking = value;
				if (stacking == null)
				{
					this.Clear();
					return;
				}
				this.AltStepLapOffset = stacking.AltStepLapOffset;
				this.AltStepLapNumOfSame = stacking.AltStepLapNumOfSame;
				this.AltSheetOffset = stacking.AltSheetOffset;
				this.AltSheetNumOfSame = stacking.AltSheetNumOfSame;
			}
		}

		public StackingView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.Stacking stacking = null)
		{
			base.ProductView = productView;
			this.Stacking = stacking;
		}

		public void Clear()
		{
			this.AltStepLapOffset = 0;
			this.AltStepLapNumOfSame = 0;
			this.AltSheetOffset = 0;
			this.AltSheetNumOfSame = 0;
		}

		public object Clone()
		{
			StackingView stackingView = new StackingView(base.ProductView, null);
			stackingView.CloneValidationDataFrom(this);
			stackingView.altStepLapOffset = this.altStepLapOffset;
			stackingView.altStepLapNumOfSame = this.altStepLapNumOfSame;
			stackingView.altSheetOffset = this.altSheetOffset;
			stackingView.altSheetNumOfSame = this.altSheetNumOfSame;
			return stackingView;
		}

		public void CloneDataFrom(StackingView stackingView)
		{
			this.AltStepLapOffset = stackingView.AltStepLapOffset;
			this.AltStepLapNumOfSame = stackingView.AltStepLapNumOfSame;
			this.AltSheetOffset = stackingView.AltSheetOffset;
			this.AltSheetNumOfSame = stackingView.AltSheetNumOfSame;
		}
	}
}