using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class ShapePartView : ProductDataViewBase, ICloneable
	{
		private JobEditor.Views.ProductData.TipCutView tipCutView = new JobEditor.Views.ProductData.TipCutView(null, null);

		private JobEditor.Views.ProductData.StepLapView stepLapView = new JobEditor.Views.ProductData.StepLapView(null, null);

		private int id;

		private double x;

		private double y;

		private EFeature feature;

		private double overCut;

		private bool doubleCut;

		private bool last;

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

		public EFeature Feature
		{
			get
			{
				return this.feature;
			}
			set
			{
				this.feature = value;
				base.RaisePropertyChanged("Feature", true);
			}
		}

		public bool Feature_Valid
		{
			get
			{
				return base.GetValidState("Feature");
			}
			set
			{
				base.SetValidState("Feature", value);
			}
		}

		public int Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
				base.RaisePropertyChanged("Id", true);
			}
		}

		public bool Id_Valid
		{
			get
			{
				return base.GetValidState("Id");
			}
			set
			{
				base.SetValidState("Id", value);
			}
		}

		public bool Last
		{
			get
			{
				return this.last;
			}
			set
			{
				this.last = value;
				base.RaisePropertyChanged("Last", true);
			}
		}

		public bool Last_Valid
		{
			get
			{
				return base.GetValidState("Last");
			}
			set
			{
				base.SetValidState("Last", value);
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

		public ProductLib.ShapePart ShapePart
		{
			get
			{
				ProductLib.ShapePart shapePart = new ProductLib.ShapePart();
				shapePart.TipCut = this.TipCutView.TipCut;
				shapePart.StepLap = this.StepLapView.StepLap;
				shapePart.Id = this.Id;
				shapePart.X = this.X;
				shapePart.Y = this.Y;
				shapePart.Feature = this.Feature;
				shapePart.OverCut = this.OverCut;
				shapePart.DoubleCut = this.DoubleCut;
				shapePart.Last = this.Last;
				return shapePart;
			}
			set
			{
				ProductLib.ShapePart shapePart = value;
				if (shapePart == null)
				{
					this.Clear();
					return;
				}
				this.TipCutView.TipCut = shapePart.TipCut;
				this.StepLapView.StepLap = shapePart.StepLap;
				this.Id = shapePart.Id;
				this.X = shapePart.X;
				this.Y = shapePart.Y;
				this.Feature = shapePart.Feature;
				this.OverCut = shapePart.OverCut;
				this.DoubleCut = shapePart.DoubleCut;
				this.Last = shapePart.Last;
			}
		}

		public JobEditor.Views.ProductData.StepLapView StepLapView
		{
			get
			{
				return this.stepLapView;
			}
			set
			{
				this.stepLapView = value;
				base.RaisePropertyChanged("StepLapView", true);
			}
		}

		public JobEditor.Views.ProductData.TipCutView TipCutView
		{
			get
			{
				return this.tipCutView;
			}
			set
			{
				this.tipCutView = value;
				base.RaisePropertyChanged("TipCutView", true);
			}
		}

		public double X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
				base.RaisePropertyChanged("X", true);
			}
		}

		public bool X_Valid
		{
			get
			{
				return base.GetValidState("X");
			}
			set
			{
				base.SetValidState("X", value);
			}
		}

		public double Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
				base.RaisePropertyChanged("Y", true);
			}
		}

		public bool Y_Valid
		{
			get
			{
				return base.GetValidState("Y");
			}
			set
			{
				base.SetValidState("Y", value);
			}
		}

		public ShapePartView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.ShapePart shapePart = null)
		{
			base.ProductView = productView;
			this.tipCutView = new JobEditor.Views.ProductData.TipCutView(productView, null);
			this.StepLapView = new JobEditor.Views.ProductData.StepLapView(productView, null);
			this.ShapePart = shapePart;
		}

		public void Clear()
		{
			this.TipCutView.Clear();
			this.StepLapView.Clear();
			this.Id = 0;
			this.X = 0;
			this.Y = 0;
			this.Feature = EFeature.CLeft;
			this.OverCut = 0;
			this.DoubleCut = false;
			this.Last = false;
		}

		public object Clone()
		{
			ShapePartView shapePartView = new ShapePartView(base.ProductView, null);
			shapePartView.CloneValidationDataFrom(this);
			shapePartView.tipCutView = (JobEditor.Views.ProductData.TipCutView)this.tipCutView.Clone();
			shapePartView.stepLapView = (JobEditor.Views.ProductData.StepLapView)this.stepLapView.Clone();
			shapePartView.id = this.id;
			shapePartView.x = this.x;
			shapePartView.y = this.y;
			shapePartView.feature = this.feature;
			shapePartView.overCut = this.overCut;
			shapePartView.doubleCut = this.doubleCut;
			shapePartView.last = this.last;
			return shapePartView;
		}

		public void CloneDataFrom(ShapePartView shapePartView)
		{
			this.TipCutView.CloneDataFrom(shapePartView.TipCutView);
			this.StepLapView.CloneDataFrom(shapePartView.StepLapView);
			this.Id = shapePartView.Id;
			this.X = shapePartView.X;
			this.Y = shapePartView.Y;
			this.Feature = shapePartView.Feature;
			this.OverCut = shapePartView.OverCut;
			this.DoubleCut = shapePartView.DoubleCut;
			this.Last = shapePartView.Last;
		}
	}
}