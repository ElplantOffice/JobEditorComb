using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class CentersDefaultView : ProductDataViewBase, ICloneable
	{
		private double overCut;

		private bool doubleCut;

		private double vOffset;

		private double cTipOffset;

		public ProductLib.CentersDefault CentersDefault
		{
			get
			{
				ProductLib.CentersDefault centersDefault = new ProductLib.CentersDefault();
				centersDefault.OverCut = this.OverCut;
				centersDefault.DoubleCut = this.DoubleCut;
				centersDefault.VOffset = this.VOffset;
				centersDefault.CTipOffset = this.CTipOffset;
				return centersDefault;
			}
			set
			{
				ProductLib.CentersDefault centersDefault = value;
				if (centersDefault == null)
				{
					this.Clear(0, false, 0, 0);
					return;
				}
				this.OverCut = centersDefault.OverCut;
				this.DoubleCut = centersDefault.DoubleCut;
				this.VOffset = centersDefault.VOffset;
				this.CTipOffset = centersDefault.CTipOffset;
			}
		}

		public double CTipOffset
		{
			get
			{
				return this.cTipOffset;
			}
			set
			{
				this.cTipOffset = value;
				base.RaisePropertyChanged("CTipOffset", true);
			}
		}

		public bool CTipOffset_Valid
		{
			get
			{
				return base.GetValidState("CTipOffset");
			}
			set
			{
				base.SetValidState("CTipOffset", value);
			}
		}

		public bool DoubleCut
		{
			get
			{
				return this.doubleCut;
			}
			set
			{
				this.doubleCut = value;
				base.RaisePropertyChanged("DoubleCut", true);
			}
		}

		public bool DoubleCut_Valid
		{
			get
			{
				return base.GetValidState("DoubleCut");
			}
			set
			{
				base.SetValidState("DoubleCut", value);
			}
		}

		public double OverCut
		{
			get
			{
				return this.overCut;
			}
			set
			{
				this.overCut = value;
				base.RaisePropertyChanged("OverCut", true);
			}
		}

		public bool OverCut_Valid
		{
			get
			{
				return base.GetValidState("OverCut");
			}
			set
			{
				base.SetValidState("OverCut", value);
			}
		}

		public double VOffset
		{
			get
			{
				return this.vOffset;
			}
			set
			{
				double num = this.vOffset;
				this.vOffset = value;
				base.RaisePropertyChanged("VOffset", true);
			}
		}

		public bool VOffset_Valid
		{
			get
			{
				return base.GetValidState("VOffset");
			}
			set
			{
				base.SetValidState("VOffset", value);
			}
		}

		public CentersDefaultView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.CentersDefault centersDefault = null)
		{
			base.ProductView = productView;
			this.CentersDefault = centersDefault;
		}

		public CentersDefaultView(JobEditor.Views.ProductData.ProductView productView, double overCut, bool doubleCut, double vOffset, double cTipOffset)
		{
			base.ProductView = productView;
			this.overCut = overCut;
			this.doubleCut = doubleCut;
			this.vOffset = vOffset;
			this.cTipOffset = cTipOffset;
		}

		public void Clear(double overCut = 0, bool doubleCut = false, double vOffset = 0, double cTipOffset = 0)
		{
			this.overCut = overCut;
			this.doubleCut = doubleCut;
			this.vOffset = vOffset;
			this.cTipOffset = cTipOffset;
		}

		public object Clone()
		{
			CentersDefaultView centersDefaultView = new CentersDefaultView(base.ProductView, null);
			centersDefaultView.CloneValidationDataFrom(this);
			centersDefaultView.overCut = this.overCut;
			centersDefaultView.doubleCut = this.doubleCut;
			centersDefaultView.vOffset = this.vOffset;
			centersDefaultView.cTipOffset = this.cTipOffset;
			return centersDefaultView;
		}

		public void CloneDataFrom(CentersDefaultView centersDefaultView)
		{
			this.OverCut = centersDefaultView.OverCut;
			this.DoubleCut = centersDefaultView.DoubleCut;
			this.VOffset = centersDefaultView.VOffset;
			this.CTipOffset = centersDefaultView.CTipOffset;
		}
	}
}