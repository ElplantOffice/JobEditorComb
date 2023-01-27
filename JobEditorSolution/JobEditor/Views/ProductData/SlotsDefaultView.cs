using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class SlotsDefaultView : ProductDataViewBase, ICloneable
	{
		private double distanceY;

		public double DistanceY
		{
			get
			{
				return this.distanceY;
			}
			set
			{
				double num = this.distanceY;
				this.distanceY = value;
				base.RaisePropertyChanged("DistanceY", true);
			}
		}

		public bool DistanceY_Valid
		{
			get
			{
				return base.GetValidState("DistanceY");
			}
			set
			{
				base.SetValidState("DistanceY", value);
			}
		}

		public ProductLib.SlotsDefault SlotsDefault
		{
			get
			{
				ProductLib.SlotsDefault slotsDefault = new ProductLib.SlotsDefault();
				slotsDefault.DistanceY = this.DistanceY;
				return slotsDefault;
			}
			set
			{
				ProductLib.SlotsDefault slotsDefault = value;
				if (slotsDefault == null)
				{
					this.Clear();
					return;
				}
				this.DistanceY = slotsDefault.DistanceY;
			}
		}

		public SlotsDefaultView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.SlotsDefault slotsDefault = null)
		{
			base.ProductView = productView;
			this.SlotsDefault = slotsDefault;
		}

		public void Clear()
		{
			this.DistanceY = 0;
		}

		public object Clone()
		{
			SlotsDefaultView slotsDefaultView = new SlotsDefaultView(base.ProductView, null);
			slotsDefaultView.CloneValidationDataFrom(this);
			slotsDefaultView.distanceY = this.distanceY;
			return slotsDefaultView;
		}

		public void CloneDataFrom(SlotsDefaultView slotsDefaultView)
		{
			this.DistanceY = slotsDefaultView.DistanceY;
		}
	}
}