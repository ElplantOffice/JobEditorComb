using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class HolesDefaultView : ProductDataViewBase, ICloneable
	{
		private double offset;

		public ProductLib.HolesDefault HolesDefault
		{
			get
			{
				ProductLib.HolesDefault holesDefault = new ProductLib.HolesDefault();
				holesDefault.Offset = this.Offset;
				return holesDefault;
			}
			set
			{
				ProductLib.HolesDefault holesDefault = value;
				if (holesDefault == null)
				{
					this.Clear(0);
					return;
				}
				this.Offset = holesDefault.Offset;
			}
		}

		public double Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				double num = this.offset;
				this.offset = value;
				base.RaisePropertyChanged("Offset", true);
			}
		}

		public bool Offset_Valid
		{
			get
			{
				return base.GetValidState("Offset");
			}
			set
			{
				base.SetValidState("Offset", value);
			}
		}

		public HolesDefaultView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.HolesDefault holesDefault = null)
		{
			base.ProductView = productView;
			this.HolesDefault = holesDefault;
		}

		public HolesDefaultView(JobEditor.Views.ProductData.ProductView productView, double offset)
		{
			base.ProductView = productView;
			this.offset = offset;
		}

		public void Clear(double offset = 0)
		{
			this.Offset = offset;
		}

		public object Clone()
		{
			HolesDefaultView holesDefaultView = new HolesDefaultView(base.ProductView, null);
			holesDefaultView.CloneValidationDataFrom(this);
			holesDefaultView.offset = this.offset;
			return holesDefaultView;
		}

		public void CloneDataFrom(HolesDefaultView holesDefaultView)
		{
			this.Offset = holesDefaultView.Offset;
		}
	}
}