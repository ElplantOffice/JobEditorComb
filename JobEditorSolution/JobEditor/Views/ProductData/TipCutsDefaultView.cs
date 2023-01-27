using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class TipCutsDefaultView : ProductDataViewBase, ICloneable
	{
		private double height;

		private double overCut;

		private bool doubleCut;

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

		public double Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
				base.RaisePropertyChanged("Height", true);
			}
		}

		public bool Height_Valid
		{
			get
			{
				return base.GetValidState("Height");
			}
			set
			{
				base.SetValidState("Height", value);
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

		public ProductLib.TipCutsDefault TipCutsDefault
		{
			get
			{
				ProductLib.TipCutsDefault tipCutsDefault = new ProductLib.TipCutsDefault();
				tipCutsDefault.Height = this.Height;
				tipCutsDefault.OverCut = this.OverCut;
				tipCutsDefault.DoubleCut = this.DoubleCut;
				return tipCutsDefault;
			}
			set
			{
				ProductLib.TipCutsDefault tipCutsDefault = value;
				if (tipCutsDefault == null)
				{
					this.Clear();
					return;
				}
				this.Height = tipCutsDefault.Height;
				this.OverCut = tipCutsDefault.OverCut;
				this.DoubleCut = tipCutsDefault.DoubleCut;
			}
		}

		public TipCutsDefaultView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.TipCutsDefault tipCutsDefault = null)
		{
			base.ProductView = productView;
			this.TipCutsDefault = tipCutsDefault;
		}

		public void Clear()
		{
			this.Height = 0;
			this.OverCut = 0;
			this.DoubleCut = false;
		}

		public object Clone()
		{
			TipCutsDefaultView tipCutsDefaultView = new TipCutsDefaultView(base.ProductView, null);
			tipCutsDefaultView.CloneValidationDataFrom(this);
			tipCutsDefaultView.height = this.height;
			tipCutsDefaultView.overCut = this.overCut;
			tipCutsDefaultView.doubleCut = this.doubleCut;
			return tipCutsDefaultView;
		}

		public void CloneDataFrom(TipCutsDefaultView tipCutsDefaultView)
		{
			this.Height = tipCutsDefaultView.Height;
			this.OverCut = tipCutsDefaultView.OverCut;
			this.DoubleCut = tipCutsDefaultView.DoubleCut;
		}
	}
}