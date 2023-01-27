using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class CentersView : ProductDataViewBase, ICloneable
	{
		private EMeasuringType measuringType;

		private double length1;

		private double length2;

		private double length3;

		private double length4;

		public ProductLib.CentersInfo CentersInfo
		{
			get
			{
				ProductLib.CentersInfo centersInfo = new ProductLib.CentersInfo();
				centersInfo.MeasuringType = this.MeasuringType;
				centersInfo.Length1 = this.Length1;
				centersInfo.Length2 = this.Length2;
				centersInfo.Length3 = this.Length3;
				centersInfo.Length4 = this.Length4;
				return centersInfo;
			}
			set
			{
				ProductLib.CentersInfo centersInfo = value;
				if (centersInfo == null)
				{
					this.Clear();
					return;
				}
				this.MeasuringType = centersInfo.MeasuringType;
				this.Length1 = centersInfo.Length1;
				this.Length2 = centersInfo.Length2;
				this.Length3 = centersInfo.Length3;
				this.Length4 = centersInfo.Length4;
			}
		}

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

		public CentersView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.CentersInfo centersInfo = null)
		{
			base.ProductView = productView;
			this.CentersInfo = centersInfo;
		}

		public void Clear()
		{
			this.MeasuringType = 0;
			this.Length1 = 0;
			this.Length2 = 0;
			this.Length3 = 0;
			this.Length4 = 0;
		}

		public object Clone()
		{
			CentersView centersView = new CentersView(base.ProductView, null);
			centersView.CloneValidationDataFrom(this);
			centersView.measuringType = this.measuringType;
			centersView.length1 = this.length1;
			centersView.length2 = this.length2;
			centersView.length3 = this.length3;
			centersView.length4 = this.length4;
			return centersView;
		}

		public void CloneDataFrom(CentersView centersView)
		{
			this.MeasuringType = centersView.MeasuringType;
			this.Length1 = centersView.Length1;
			this.Length2 = centersView.Length2;
			this.Length3 = centersView.Length3;
			this.Length4 = centersView.Length4;
		}
	}
}