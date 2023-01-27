using JobEditor.Common;
using ProductLib;
using System;

namespace JobEditor.Views.ProductData
{
	public class StepView : ProductDataViewBase, ICloneable
	{
		private int id;

		private double x;

		private double y;

		private int shapeId;

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

		public int ShapeId
		{
			get
			{
				return this.shapeId;
			}
			set
			{
				this.shapeId = value;
				base.RaisePropertyChanged("ShapeId", true);
			}
		}

		public bool ShapeId_Valid
		{
			get
			{
				return base.GetValidState("ShapeId");
			}
			set
			{
				base.SetValidState("ShapeId", value);
			}
		}

		public ProductLib.Step Step
		{
			get
			{
				ProductLib.Step step = new ProductLib.Step();
				step.Id = this.Id;
				step.X = this.X;
				step.Y = this.Y;
				step.ShapeId = this.ShapeId;
				return step;
			}
			set
			{
				ProductLib.Step step = value;
				if (step == null)
				{
					this.Clear();
					return;
				}
				this.Id = step.Id;
				this.X = step.X;
				this.Y = step.Y;
				this.ShapeId = step.ShapeId;
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

		public StepView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.Step step = null)
		{
			base.ProductView = productView;
			this.Step = step;
		}

		public void Clear()
		{
			this.Id = 0;
			this.X = 0;
			this.Y = 0;
			this.ShapeId = 0;
		}

		public object Clone()
		{
			StepView stepView = new StepView(base.ProductView, null);
			stepView.CloneValidationDataFrom(this);
			stepView.id = this.id;
			stepView.x = this.x;
			stepView.y = this.y;
			stepView.shapeId = this.shapeId;
			return stepView;
		}

		public void CloneDataFrom(StepView stepView)
		{
			this.Id = stepView.Id;
			this.X = stepView.X;
			this.Y = stepView.Y;
			this.ShapeId = stepView.ShapeId;
		}
	}
}