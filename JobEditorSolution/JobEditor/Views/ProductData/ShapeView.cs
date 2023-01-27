using JobEditor.Common;
using ProductLib;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace JobEditor.Views.ProductData
{
	public class ShapeView : ProductDataViewBase, ICloneable
	{
		private List<Hole> holes = new List<Hole>();

		private List<Slot> slots = new List<Slot>();

		private List<ShapePartView> shapePartViews = new List<ShapePartView>();

		private JobEditor.Views.ProductData.StackingView stackingView;

		private JobEditor.Views.ProductData.HolesView holesView;

		private JobEditor.Views.ProductData.SlotsView slotsView;

		private JobEditor.Views.ProductData.CentersView centersView;

		public JobEditor.Views.ProductData.CentersView CentersView
		{
			get
			{
				return this.centersView;
			}
			set
			{
				this.centersView = value;
				base.RaisePropertyChanged("CentersView", true);
			}
		}

		public string drawing
		{
			get;
			set;
		}

		public string Drawing
		{
			get
			{
				return this.drawing;
			}
			set
			{
				this.drawing = value;
				base.RaisePropertyChanged("Drawing", true);
			}
		}

		public bool Drawing_Valid
		{
			get
			{
				return base.GetValidState("Drawing");
			}
			set
			{
				base.SetValidState("Drawing", value);
			}
		}

		public JobEditor.Views.ProductData.HolesView HolesView
		{
			get
			{
				return this.holesView;
			}
			set
			{
				this.holesView = value;
				base.RaisePropertyChanged("HolesView", true);
			}
		}

		public int id
		{
			get;
			set;
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
				base.RaisePropertyChanged("Id", false);
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

		public ProductLib.Shape Shape
		{
			get
			{
				ProductLib.Shape shape = new ProductLib.Shape();
				shape.ShapeParts.Clear();
				if (this.ShapePartViews != null)
				{
					foreach (ShapePartView shapePartView in this.ShapePartViews)
					{
						ShapePart shapePart = shapePartView.ShapePart;
						shape.ShapeParts.Add(shapePart);
					}
				}
				shape.Stacking = this.StackingView.Stacking;
				shape.HolesInfo = this.HolesView.HolesInfo;
				shape.SlotsInfo = this.SlotsView.SlotsInfo;
				shape.CentersInfo = this.CentersView.CentersInfo;
				shape.Holes.Clear();
				if (this.holes != null)
				{
					foreach (Hole hole in this.holes)
					{
						shape.Holes.Add((Hole)hole.Clone());
					}
				}
				shape.Slots.Clear();
				if (this.slots != null)
				{
					foreach (Slot slot in this.slots)
					{
						shape.Slots.Add((Slot)slot.Clone());
					}
				}
				shape.Id = this.Id;
				shape.Drawing = this.Drawing;
				return shape;
			}
			set
			{
				ProductLib.Shape shape = value;
				if (shape == null)
				{
					this.Clear();
					return;
				}
				this.ShapePartViews.Clear();
				if (shape.ShapeParts != null)
				{
					foreach (ShapePart shapePart in shape.ShapeParts)
					{
						ShapePartView shapePartView = new ShapePartView(base.ProductView, shapePart);
						this.ShapePartViews.Add(shapePartView);
					}
				}
				this.StackingView.Stacking = shape.Stacking;
				this.HolesView.HolesInfo = shape.HolesInfo;
				this.SlotsView.SlotsInfo = shape.SlotsInfo;
				this.CentersView.CentersInfo = shape.CentersInfo;
				this.holes.Clear();
				if (shape.Holes != null)
				{
					foreach (Hole hole in shape.Holes)
					{
						this.holes.Add((Hole)hole.Clone());
					}
				}
				this.slots.Clear();
				if (shape.Slots != null)
				{
					foreach (Slot slot in shape.Slots)
					{
						this.slots.Add((Slot)slot.Clone());
					}
				}
				this.Id = shape.Id;
				this.Drawing = shape.Drawing;
			}
		}

		public List<ShapePartView> ShapePartViews
		{
			get
			{
				return this.shapePartViews;
			}
			set
			{
				this.shapePartViews = value;
				base.RaisePropertyChanged("ShapePartViews", true);
			}
		}

		public JobEditor.Views.ProductData.SlotsView SlotsView
		{
			get
			{
				return this.slotsView;
			}
			set
			{
				this.slotsView = value;
				base.RaisePropertyChanged("SlotsView", true);
			}
		}

		public JobEditor.Views.ProductData.StackingView StackingView
		{
			get
			{
				return this.stackingView;
			}
			set
			{
				this.stackingView = value;
				base.RaisePropertyChanged("StackingView", true);
			}
		}

		public ShapeView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.Shape shape = null)
		{
			base.ProductView = productView;
			this.StackingView = new JobEditor.Views.ProductData.StackingView(productView, null);
			this.HolesView = new JobEditor.Views.ProductData.HolesView(productView, null);
			this.SlotsView = new JobEditor.Views.ProductData.SlotsView(productView, null);
			this.CentersView = new JobEditor.Views.ProductData.CentersView(productView, null);
			this.Shape = shape;
		}

		public void Clear()
		{
			this.ShapePartViews.Clear();
			this.StackingView.Clear();
			this.HolesView.Clear();
			this.SlotsView.Clear();
			this.CentersView.Clear();
			this.holes.Clear();
			this.slots.Clear();
			this.Id = 0;
			this.Drawing = "";
		}

		public object Clone()
		{
			ShapeView shapeView = new ShapeView(base.ProductView, null);
			shapeView.CloneValidationDataFrom(this);
			shapeView.shapePartViews = new List<ShapePartView>();
			foreach (ShapePartView shapePartView in this.shapePartViews)
			{
				shapeView.shapePartViews.Add((ShapePartView)shapePartView.Clone());
			}
			shapeView.stackingView = (JobEditor.Views.ProductData.StackingView)this.stackingView.Clone();
			shapeView.holesView = (JobEditor.Views.ProductData.HolesView)this.holesView.Clone();
			shapeView.slotsView = (JobEditor.Views.ProductData.SlotsView)this.slotsView.Clone();
			shapeView.centersView = (JobEditor.Views.ProductData.CentersView)this.centersView.Clone();
			shapeView.holes = new List<Hole>();
			foreach (Hole hole in this.holes)
			{
				shapeView.holes.Add((Hole)hole.Clone());
			}
			shapeView.slots = new List<Slot>();
			foreach (Slot slot in this.slots)
			{
				shapeView.slots.Add((Slot)slot.Clone());
			}
			shapeView.id = this.id;
			shapeView.drawing = this.drawing;
			return shapeView;
		}

		public void CloneDataFrom(ShapeView shapeView)
		{
			if (shapeView.ShapePartViews == null)
			{
				this.ShapePartViews.Clear();
				this.ShapePartViews = null;
			}
			else
			{
				if (this.ShapePartViews == null)
				{
					this.ShapePartViews = new List<ShapePartView>();
				}
				for (int i = 0; i < shapeView.ShapePartViews.Count; i++)
				{
					if (this.ShapePartViews.Count <= i)
					{
						this.ShapePartViews.Add(new ShapePartView(base.ProductView, null));
					}
					ShapePartView item = shapeView.ShapePartViews[i];
					this.ShapePartViews[i].CloneDataFrom(item);
				}
				while (this.ShapePartViews.Count > shapeView.ShapePartViews.Count)
				{
					this.ShapePartViews.RemoveAt(this.ShapePartViews.Count - 1);
				}
			}
			this.StackingView.CloneDataFrom(shapeView.StackingView);
			this.HolesView.CloneDataFrom(shapeView.HolesView);
			this.SlotsView.CloneDataFrom(shapeView.SlotsView);
			this.CentersView.CloneDataFrom(shapeView.CentersView);
			this.holes = shapeView.holes;
			this.slots = shapeView.slots;
			this.Id = shapeView.Id;
			this.Drawing = shapeView.Drawing;
		}
	}
}