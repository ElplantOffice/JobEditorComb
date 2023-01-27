using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class LayerDefaultView : ProductDataViewBase, ICloneable
	{
		private double height;

		private double width;

		private double materialThickness;

		private EHeightCorrectionType heightCorrectionType;

		public double Height
		{
			get
			{
				double num = this.height;
				if (base.ProductView != null && base.ProductView.HeightRefType == EHeightRefType.Number)
				{
					num = Math.Round(this.height);
				}
				return num;
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

		public EHeightCorrectionType HeightCorrectionType
		{
			get
			{
				return this.heightCorrectionType;
			}
			set
			{
				this.heightCorrectionType = value;
				base.RaisePropertyChanged("HeightCorrectionType", true);
			}
		}

		public ProductLib.LayerDefault LayerDefault
		{
			get
			{
				ProductLib.LayerDefault layerDefault = new ProductLib.LayerDefault();
				layerDefault.Height = this.Height;
				layerDefault.Width = this.Width;
				layerDefault.MaterialThickness = this.MaterialThickness;
				layerDefault.HeightCorrType = this.HeightCorrectionType;
				return layerDefault;
			}
			set
			{
				ProductLib.LayerDefault layerDefault = value;
				if (layerDefault == null)
				{
					this.Clear();
					return;
				}
				this.Height = layerDefault.Height;
				this.Width = layerDefault.Width;
				this.MaterialThickness = layerDefault.MaterialThickness;
				this.HeightCorrectionType = layerDefault.HeightCorrType;
			}
		}

		public double MaterialThickness
		{
			get
			{
				return this.materialThickness;
			}
			set
			{
				this.materialThickness = value;
				base.RaisePropertyChanged("MaterialThickness", true);
			}
		}

		public bool MaterialThickness_Valid
		{
			get
			{
				return base.GetValidState("MaterialThickness");
			}
			set
			{
				base.SetValidState("MaterialThickness", value);
			}
		}

		public double Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
				base.RaisePropertyChanged("Width", true);
			}
		}

		public bool Width_Valid
		{
			get
			{
				return base.GetValidState("Width");
			}
			set
			{
				base.SetValidState("Width", value);
			}
		}

		public LayerDefaultView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.LayerDefault layerDefault = null)
		{
			base.ProductView = productView;
			this.LayerDefault = layerDefault;
		}

		public LayerDefaultView(JobEditor.Views.ProductData.ProductView productView, double height, double width, double materialThickness, EHeightCorrectionType heightCorrType)
		{
			base.ProductView = productView;
			this.height = height;
			this.width = width;
			this.materialThickness = materialThickness;
			this.heightCorrectionType = heightCorrType;
		}

		public void Clear()
		{
			this.Height = 0;
			this.Width = 0;
			this.MaterialThickness = 0;
			this.heightCorrectionType = 0;
		}

		public object Clone()
		{
			LayerDefaultView layerDefaultView = new LayerDefaultView(base.ProductView, null);
			layerDefaultView.CloneValidationDataFrom(this);
			layerDefaultView.height = this.height;
			layerDefaultView.width = this.width;
			layerDefaultView.materialThickness = this.materialThickness;
			layerDefaultView.HeightCorrectionType = this.heightCorrectionType;
			return layerDefaultView;
		}

		public void CloneDataFrom(LayerDefaultView layerDefaultView)
		{
			this.Height = layerDefaultView.Height;
			this.Width = layerDefaultView.Width;
			this.MaterialThickness = layerDefaultView.MaterialThickness;
			this.HeightCorrectionType = layerDefaultView.HeightCorrectionType;
		}
	}
}