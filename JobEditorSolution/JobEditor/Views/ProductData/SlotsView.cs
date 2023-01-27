using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class SlotsView : ProductDataViewBase, ICloneable
	{
		private int numberOfSlots;

		private EMeasuringType measuringType;

		private double length1;

		private double length2;

		private double length3;

		private double length4;

		public double Length1
		{
			get
			{
				return this.length1;
			}
			set
			{
				this.length1 = value;
				base.RaisePropertyChanged("Length1", true);
			}
		}

		public bool Length1_Valid
		{
			get
			{
				return base.GetValidState("Length1");
			}
			set
			{
				base.SetValidState("Length1", value);
			}
		}

		public double Length2
		{
			get
			{
				return this.length2;
			}
			set
			{
				this.length2 = value;
				base.RaisePropertyChanged("Length2", true);
			}
		}

		public bool Length2_Valid
		{
			get
			{
				return base.GetValidState("Length2");
			}
			set
			{
				base.SetValidState("Length2", value);
			}
		}

		public double Length3
		{
			get
			{
				return this.length3;
			}
			set
			{
				this.length3 = value;
				base.RaisePropertyChanged("Length3", true);
			}
		}

		public bool Length3_Valid
		{
			get
			{
				return base.GetValidState("Length3");
			}
			set
			{
				base.SetValidState("Length3", value);
			}
		}

		public double Length4
		{
			get
			{
				return this.length4;
			}
			set
			{
				this.length4 = value;
				base.RaisePropertyChanged("Length4", true);
			}
		}

		public bool Length4_Valid
		{
			get
			{
				return base.GetValidState("Length4");
			}
			set
			{
				base.SetValidState("Length4", value);
			}
		}

		public EMeasuringType MeasuringType
		{
			get
			{
				return this.measuringType;
			}
			set
			{
				this.measuringType = value;
				base.RaisePropertyChanged("MeasuringType", true);
			}
		}

		public bool MeasuringType_Valid
		{
			get
			{
				return base.GetValidState("MeasuringType");
			}
			set
			{
				base.SetValidState("MeasuringType", value);
			}
		}

		public int NumberOfSlots
		{
			get
			{
				return this.numberOfSlots;
			}
			set
			{
				this.numberOfSlots = value;
				base.RaisePropertyChanged("NumberOfSlots", true);
			}
		}

		public bool NumberOfSlots_Valid
		{
			get
			{
				return base.GetValidState("NumberOfSlots");
			}
			set
			{
				base.SetValidState("NumberOfSlots", value);
			}
		}

		public ProductLib.SlotsInfo SlotsInfo
		{
			get
			{
				ProductLib.SlotsInfo slotsInfo = new ProductLib.SlotsInfo();
				slotsInfo.NumberOfSlots = this.NumberOfSlots;
				slotsInfo.MeasuringType = this.MeasuringType;
				slotsInfo.Length1 = this.Length1;
				slotsInfo.Length2 = this.Length2;
				slotsInfo.Length3 = this.Length3;
				slotsInfo.Length4 = this.Length4;
				return slotsInfo;
			}
			set
			{
				ProductLib.SlotsInfo slotsInfo = value;
				if (slotsInfo == null)
				{
					this.Clear();
					return;
				}
				this.NumberOfSlots = slotsInfo.NumberOfSlots;
				this.MeasuringType = slotsInfo.MeasuringType;
				this.Length1 = slotsInfo.Length1;
				this.Length2 = slotsInfo.Length2;
				this.Length3 = slotsInfo.Length3;
				this.Length4 = slotsInfo.Length4;
			}
		}

		public SlotsView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.SlotsInfo slotsInfo = null)
		{
			base.ProductView = productView;
			this.SlotsInfo = slotsInfo;
		}

		public void Clear()
		{
			this.NumberOfSlots = 0;
			this.MeasuringType = 0;
			this.Length1 = 0;
			this.Length2 = 0;
			this.Length3 = 0;
			this.Length4 = 0;
		}

		public object Clone()
		{
			SlotsView slotsView = new SlotsView(base.ProductView, null);
			slotsView.CloneValidationDataFrom(this);
			slotsView.numberOfSlots = this.numberOfSlots;
			slotsView.measuringType = this.measuringType;
			slotsView.length1 = this.length1;
			slotsView.length2 = this.length2;
			slotsView.length3 = this.length3;
			slotsView.length4 = this.length4;
			return slotsView;
		}

		public void CloneDataFrom(SlotsView slotsView)
		{
			this.NumberOfSlots = slotsView.NumberOfSlots;
			this.MeasuringType = slotsView.MeasuringType;
			this.Length1 = slotsView.Length1;
			this.Length2 = slotsView.Length2;
			this.Length3 = slotsView.Length3;
			this.Length4 = slotsView.Length4;
		}
	}
}