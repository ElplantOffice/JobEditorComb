using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class HolesView : ProductDataViewBase, ICloneable
	{
		private int numberOfHoles;

		private EMeasuringType measuringType;

		private EHoleShape shape1;

		private EHoleShape shape2;

		private EHoleShape shape3;

		private EHoleShape shape4;

		private EHoleShape shape5;

		private EHoleShape shape6;

		private EHoleShape shape7;

		private EHoleShape shape8;

		private EHoleShape shape9;

		private EHoleShape shape10;

		private double length1;

		private double length2;

		private double length3;

		private double length4;

		private double length5;

		private double length6;

		private double length7;

		private double length8;

		private double length9;

		private double length10;

		public ProductLib.HolesInfo HolesInfo
		{
			get
			{
				ProductLib.HolesInfo holesInfo = new ProductLib.HolesInfo();
				holesInfo.NumberOfHoles = this.NumberOfHoles;
				holesInfo.MeasuringType = this.MeasuringType;
				holesInfo.Shape1 = this.Shape1;
				holesInfo.Shape2 = this.Shape2;
				holesInfo.Shape3 = this.Shape3;
				holesInfo.Shape4 = this.Shape4;
				holesInfo.Shape5 = this.Shape5;
				holesInfo.Shape6 = this.Shape6;
				holesInfo.Shape7 = this.Shape7;
				holesInfo.Shape8 = this.Shape8;
				holesInfo.Shape9 = this.Shape9;
				holesInfo.Shape10 = this.Shape10;
				holesInfo.Length1 = this.Length1;
				holesInfo.Length2 = this.Length2;
				holesInfo.Length3 = this.Length3;
				holesInfo.Length4 = this.Length4;
				holesInfo.Length5 = this.Length5;
				holesInfo.Length6 = this.Length6;
				holesInfo.Length7 = this.Length7;
				holesInfo.Length8 = this.Length8;
				holesInfo.Length9 = this.Length9;
				holesInfo.Length10 = this.Length10;
				return holesInfo;
			}
			set
			{
				ProductLib.HolesInfo holesInfo = value;
				if (holesInfo == null)
				{
					this.Clear();
					return;
				}
				this.NumberOfHoles = holesInfo.NumberOfHoles;
				this.MeasuringType = holesInfo.MeasuringType;
				this.Shape1 = holesInfo.Shape1;
				this.Shape2 = holesInfo.Shape2;
				this.Shape3 = holesInfo.Shape3;
				this.Shape4 = holesInfo.Shape4;
				this.Shape5 = holesInfo.Shape5;
				this.Shape6 = holesInfo.Shape6;
				this.Shape7 = holesInfo.Shape7;
				this.Shape8 = holesInfo.Shape8;
				this.Shape9 = holesInfo.Shape9;
				this.Shape10 = holesInfo.Shape10;
				this.Length1 = holesInfo.Length1;
				this.Length2 = holesInfo.Length2;
				this.Length3 = holesInfo.Length3;
				this.Length4 = holesInfo.Length4;
				this.Length5 = holesInfo.Length5;
				this.Length6 = holesInfo.Length6;
				this.Length7 = holesInfo.Length7;
				this.Length8 = holesInfo.Length8;
				this.Length9 = holesInfo.Length9;
				this.Length10 = holesInfo.Length10;
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

		public double Length10
		{
			get
			{
				return this.length10;
			}
			set
			{
				this.length10 = value;
				base.RaisePropertyChanged("Length10", true);
			}
		}

		public bool Length10_Valid
		{
			get
			{
				return base.GetValidState("Length10");
			}
			set
			{
				base.SetValidState("Length10", value);
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

		public double Length5
		{
			get
			{
				return this.length5;
			}
			set
			{
				this.length5 = value;
				base.RaisePropertyChanged("Length5", true);
			}
		}

		public bool Length5_Valid
		{
			get
			{
				return base.GetValidState("Length5");
			}
			set
			{
				base.SetValidState("Length5", value);
			}
		}

		public double Length6
		{
			get
			{
				return this.length6;
			}
			set
			{
				this.length6 = value;
				base.RaisePropertyChanged("Length6", true);
			}
		}

		public bool Length6_Valid
		{
			get
			{
				return base.GetValidState("Length6");
			}
			set
			{
				base.SetValidState("Length6", value);
			}
		}

		public double Length7
		{
			get
			{
				return this.length7;
			}
			set
			{
				this.length7 = value;
				base.RaisePropertyChanged("Length7", true);
			}
		}

		public bool Length7_Valid
		{
			get
			{
				return base.GetValidState("Length7");
			}
			set
			{
				base.SetValidState("Length7", value);
			}
		}

		public double Length8
		{
			get
			{
				return this.length8;
			}
			set
			{
				this.length8 = value;
				base.RaisePropertyChanged("Length8", true);
			}
		}

		public bool Length8_Valid
		{
			get
			{
				return base.GetValidState("Length84");
			}
			set
			{
				base.SetValidState("Length8", value);
			}
		}

		public double Length9
		{
			get
			{
				return this.length9;
			}
			set
			{
				this.length9 = value;
				base.RaisePropertyChanged("Length9", true);
			}
		}

		public bool Length9_Valid
		{
			get
			{
				return base.GetValidState("Length9");
			}
			set
			{
				base.SetValidState("Length9", value);
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

		public int NumberOfHoles
		{
			get
			{
				return this.numberOfHoles;
			}
			set
			{
				this.numberOfHoles = value;
				base.RaisePropertyChanged("NumberOfHoles", true);
			}
		}

		public bool NumberOfHoles_Valid
		{
			get
			{
				return base.GetValidState("NumberOfHoles");
			}
			set
			{
				base.SetValidState("NumberOfHoles", value);
			}
		}

		public EHoleShape Shape1
		{
			get
			{
				return this.shape1;
			}
			set
			{
				this.shape1 = value;
				base.RaisePropertyChanged("Shape1", true);
			}
		}

		public bool Shape1_Valid
		{
			get
			{
				return base.GetValidState("Shape1");
			}
			set
			{
				base.SetValidState("Shape1", value);
			}
		}

		public EHoleShape Shape10
		{
			get
			{
				return this.shape10;
			}
			set
			{
				this.shape10 = value;
				base.RaisePropertyChanged("Shape10", true);
			}
		}

		public bool Shape10_Valid
		{
			get
			{
				return base.GetValidState("Shape10");
			}
			set
			{
				base.SetValidState("Shape10", value);
			}
		}

		public EHoleShape Shape2
		{
			get
			{
				return this.shape2;
			}
			set
			{
				this.shape2 = value;
				base.RaisePropertyChanged("Shape2", true);
			}
		}

		public bool Shape2_Valid
		{
			get
			{
				return base.GetValidState("Shape2");
			}
			set
			{
				base.SetValidState("Shape2", value);
			}
		}

		public EHoleShape Shape3
		{
			get
			{
				return this.shape3;
			}
			set
			{
				this.shape3 = value;
				base.RaisePropertyChanged("Shape3", true);
			}
		}

		public bool Shape3_Valid
		{
			get
			{
				return base.GetValidState("Shape3");
			}
			set
			{
				base.SetValidState("Shape3", value);
			}
		}

		public EHoleShape Shape4
		{
			get
			{
				return this.shape4;
			}
			set
			{
				this.shape4 = value;
				base.RaisePropertyChanged("Shape4", true);
			}
		}

		public bool Shape4_Valid
		{
			get
			{
				return base.GetValidState("Shape4");
			}
			set
			{
				base.SetValidState("Shape4", value);
			}
		}

		public EHoleShape Shape5
		{
			get
			{
				return this.shape5;
			}
			set
			{
				this.shape5 = value;
				base.RaisePropertyChanged("Shape5", true);
			}
		}

		public bool Shape5_Valid
		{
			get
			{
				return base.GetValidState("Shape5");
			}
			set
			{
				base.SetValidState("Shape5", value);
			}
		}

		public EHoleShape Shape6
		{
			get
			{
				return this.shape6;
			}
			set
			{
				this.shape6 = value;
				base.RaisePropertyChanged("Shape6", true);
			}
		}

		public bool Shape6_Valid
		{
			get
			{
				return base.GetValidState("Shape6");
			}
			set
			{
				base.SetValidState("Shape6", value);
			}
		}

		public EHoleShape Shape7
		{
			get
			{
				return this.shape7;
			}
			set
			{
				this.shape7 = value;
				base.RaisePropertyChanged("Shape7", true);
			}
		}

		public bool Shape7_Valid
		{
			get
			{
				return base.GetValidState("Shape7");
			}
			set
			{
				base.SetValidState("Shape7", value);
			}
		}

		public EHoleShape Shape8
		{
			get
			{
				return this.shape8;
			}
			set
			{
				this.shape8 = value;
				base.RaisePropertyChanged("Shape8", true);
			}
		}

		public bool Shape8_Valid
		{
			get
			{
				return base.GetValidState("Shape8");
			}
			set
			{
				base.SetValidState("Shape8", value);
			}
		}

		public EHoleShape Shape9
		{
			get
			{
				return this.shape9;
			}
			set
			{
				this.shape9 = value;
				base.RaisePropertyChanged("Shape9", true);
			}
		}

		public bool Shape9_Valid
		{
			get
			{
				return base.GetValidState("Shape9");
			}
			set
			{
				base.SetValidState("Shape9", value);
			}
		}

		public HolesView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.HolesInfo holesInfo = null)
		{
			base.ProductView = productView;
			this.HolesInfo = holesInfo;
		}

		public void Clear()
		{
			this.NumberOfHoles = 0;
			this.MeasuringType = 0;
			this.Shape1 = 0;
			this.Shape2 = 0;
			this.Shape3 = 0;
			this.Shape4 = 0;
			this.Shape5 = 0;
			this.Shape6 = 0;
			this.Shape7 = 0;
			this.Shape8 = 0;
			this.Shape9 = 0;
			this.Shape10 = 0;
			this.Length1 = 0;
			this.Length2 = 0;
			this.Length3 = 0;
			this.Length4 = 0;
			this.Length5 = 0;
			this.Length6 = 0;
			this.Length7 = 0;
			this.Length8 = 0;
			this.Length9 = 0;
			this.Length10 = 0;
		}

		public object Clone()
		{
			HolesView holesView = new HolesView(base.ProductView, null);
			holesView.CloneValidationDataFrom(this);
			holesView.numberOfHoles = this.numberOfHoles;
			holesView.measuringType = this.measuringType;
			holesView.shape1 = this.shape1;
			holesView.shape2 = this.shape2;
			holesView.shape3 = this.shape3;
			holesView.shape4 = this.shape4;
			holesView.shape5 = this.shape5;
			holesView.shape6 = this.shape6;
			holesView.shape7 = this.shape7;
			holesView.shape8 = this.shape8;
			holesView.shape9 = this.shape9;
			holesView.shape10 = this.shape10;
			holesView.length1 = this.length1;
			holesView.length2 = this.length2;
			holesView.length3 = this.length3;
			holesView.length4 = this.length4;
			holesView.length5 = this.length5;
			holesView.length6 = this.length6;
			holesView.length7 = this.length7;
			holesView.length8 = this.length8;
			holesView.length9 = this.length9;
			holesView.length10 = this.length10;
			return holesView;
		}

		public void CloneDataFrom(HolesView holesView)
		{
			this.NumberOfHoles = holesView.NumberOfHoles;
			this.MeasuringType = holesView.MeasuringType;
			this.Shape1 = holesView.Shape1;
			this.Shape2 = holesView.Shape2;
			this.Shape3 = holesView.Shape3;
			this.Shape4 = holesView.Shape4;
			this.Shape5 = holesView.Shape5;
			this.Shape6 = holesView.Shape6;
			this.Shape7 = holesView.Shape7;
			this.Shape8 = holesView.Shape8;
			this.Shape9 = holesView.Shape9;
			this.Shape10 = holesView.Shape10;
			this.Length1 = holesView.Length1;
			this.Length2 = holesView.Length2;
			this.Length3 = holesView.Length3;
			this.Length4 = holesView.Length4;
			this.Length5 = holesView.Length5;
			this.Length6 = holesView.Length6;
			this.Length7 = holesView.Length7;
			this.Length8 = holesView.Length8;
			this.Length9 = holesView.Length9;
			this.Length10 = holesView.Length10;
		}
	}
}