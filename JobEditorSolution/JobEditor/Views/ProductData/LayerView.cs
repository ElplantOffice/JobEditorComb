using JobEditor.Common;
using ProductLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JobEditor.Views.ProductData
{
	public class LayerView : ProductDataViewBase, ICloneable
	{
		private ObservableCollection<ShapeView> shapeViews = new ObservableCollection<ShapeView>();

		private JobEditor.Views.ProductData.StepLapsDefaultView stepLapsDefaultView;

		private JobEditor.Views.ProductData.TipCutsDefaultView tipCutsDefaultView;

		private JobEditor.Views.ProductData.HolesDefaultView holesDefaultView;

		private JobEditor.Views.ProductData.SlotsDefaultView slotsDefaultView;

		private JobEditor.Views.ProductData.CentersDefaultView centersDefaultView;

		private JobEditor.Views.ProductData.LayerDefaultView layerDefaultView;

		private int id;

		private string name = "";

		private string info = "";

		public JobEditor.Views.ProductData.CentersDefaultView CentersDefaultView
		{
			get
			{
				return this.centersDefaultView;
			}
			set
			{
				this.centersDefaultView = value;
				base.RaisePropertyChanged("CentersDefaultView", true);
			}
		}

		public JobEditor.Views.ProductData.HolesDefaultView HolesDefaultView
		{
			get
			{
				return this.holesDefaultView;
			}
			set
			{
				this.holesDefaultView = value;
				base.RaisePropertyChanged("HolesDefaultView", true);
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

		public string Info
		{
			get
			{
				return this.info;
			}
			set
			{
				this.info = value;
				base.RaisePropertyChanged("Info", true);
			}
		}

		public bool Info_Valid
		{
			get
			{
				return base.GetValidState("Info");
			}
			set
			{
				base.SetValidState("Info", value);
			}
		}

		public ProductLib.Layer Layer
		{
			get
			{
				ProductLib.Layer layer = new ProductLib.Layer();
				layer.Shapes.Clear();
				if (this.shapeViews != null)
				{
					foreach (ShapeView shapeView in this.shapeViews)
					{
						Shape shape = shapeView.Shape;
						layer.Shapes.Add(shape);
					}
				}
				layer.LayerDefault = this.layerDefaultView.LayerDefault;
				layer.StepLapsDefault = this.stepLapsDefaultView.StepLapsDefault;
				layer.HolesDefault = this.holesDefaultView.HolesDefault;
				layer.TipCutsDefault = this.tipCutsDefaultView.TipCutsDefault;
				layer.SlotsDefault = this.slotsDefaultView.SlotsDefault;
				layer.CentersDefault = this.centersDefaultView.CentersDefault;
				layer.Id = this.Id;
				layer.Name = this.Name;
				layer.Info = this.Info;
				return layer;
			}
			set
			{
				ProductLib.Layer layer = value;
				if (layer == null)
				{
					this.Clear();
					return;
				}
				this.EnableOnSelectedLayerViewChanged(false);
				this.shapeViews.Clear();
				if (layer != null && layer.Shapes != null)
				{
					foreach (Shape shape in layer.Shapes)
					{
						ShapeView shapeView = new ShapeView(base.ProductView, shape);
						this.shapeViews.Add(shapeView);
					}
				}
				this.layerDefaultView.LayerDefault = layer.LayerDefault;
				this.stepLapsDefaultView.StepLapsDefault = layer.StepLapsDefault;
				this.holesDefaultView.HolesDefault = layer.HolesDefault;
				this.tipCutsDefaultView.TipCutsDefault = layer.TipCutsDefault;
				this.slotsDefaultView.SlotsDefault = layer.SlotsDefault;
				this.centersDefaultView.CentersDefault = layer.CentersDefault;
				this.Id = layer.Id;
				this.Name = layer.Name;
				this.Info = layer.Info;
				this.EnableOnSelectedLayerViewChanged(true);
				this.CallOnSelectedLayerViewChanged();
			}
		}

		public JobEditor.Views.ProductData.LayerDefaultView LayerDefaultView
		{
			get
			{
				return this.layerDefaultView;
			}
			set
			{
				this.layerDefaultView = value;
				base.RaisePropertyChanged("LayerDefaultView", true);
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				base.RaisePropertyChanged("Name", true);
			}
		}

		public bool Name_Valid
		{
			get
			{
				return base.GetValidState("Name");
			}
			set
			{
				base.SetValidState("Name", value);
			}
		}

		public ObservableCollection<ShapeView> ShapeViews
		{
			get
			{
				return this.shapeViews;
			}
			set
			{
				this.shapeViews = value;
				base.RaisePropertyChanged("ShapeViews", true);
			}
		}

		public JobEditor.Views.ProductData.SlotsDefaultView SlotsDefaultView
		{
			get
			{
				return this.slotsDefaultView;
			}
			set
			{
				this.slotsDefaultView = value;
				base.RaisePropertyChanged("SlotsDefaultView", true);
			}
		}

		public JobEditor.Views.ProductData.StepLapsDefaultView StepLapsDefaultView
		{
			get
			{
				return this.stepLapsDefaultView;
			}
			set
			{
				this.stepLapsDefaultView = value;
				base.RaisePropertyChanged("StepLapsDefaultView", true);
			}
		}

		public JobEditor.Views.ProductData.TipCutsDefaultView TipCutsDefaultView
		{
			get
			{
				return this.tipCutsDefaultView;
			}
			set
			{
				this.tipCutsDefaultView = value;
				base.RaisePropertyChanged("TipCutsDefaultView", true);
			}
		}

		public LayerView(JobEditor.Views.ProductData.ProductView productView = null, ProductLib.Layer layer = null)
		{
			base.ProductView = productView;
			this.layerDefaultView = new JobEditor.Views.ProductData.LayerDefaultView(productView, null);
			this.stepLapsDefaultView = new JobEditor.Views.ProductData.StepLapsDefaultView(productView, null);
			this.holesDefaultView = new JobEditor.Views.ProductData.HolesDefaultView(productView, null);
			this.tipCutsDefaultView = new JobEditor.Views.ProductData.TipCutsDefaultView(productView, null);
			this.slotsDefaultView = new JobEditor.Views.ProductData.SlotsDefaultView(productView, null);
			this.centersDefaultView = new JobEditor.Views.ProductData.CentersDefaultView(productView, null);
			this.Layer = layer;
		}

		public bool CallOnSelectedLayerViewChanged()
		{
			if (base.ProductView == null)
			{
				return false;
			}
			if (base.ProductView.SelectedLayerView != this)
			{
				return false;
			}
			return base.ProductView.CallOnSelectedLayerViewChanged();
		}

		public void Clear()
		{
			this.EnableOnSelectedLayerViewChanged(false);
			this.ShapeViews.Clear();
			this.LayerDefaultView.Clear();
			this.StepLapsDefaultView.Clear(0, 0, 0);
			this.HolesDefaultView.Clear(0);
			this.TipCutsDefaultView.Clear();
			this.SlotsDefaultView.Clear();
			this.CentersDefaultView.Clear(0, false, 0, 0);
			this.Id = 0;
			this.Name = "";
			this.Info = "";
			this.EnableOnSelectedLayerViewChanged(true);
			this.CallOnSelectedLayerViewChanged();
		}

		public object Clone()
		{
			LayerView layerView = new LayerView(base.ProductView, null);
			layerView.CloneValidationDataFrom(this);
			layerView.ShapeViews.Clear();
			if (this.ShapeViews != null)
			{
				foreach (ShapeView shapeView in this.ShapeViews)
				{
					layerView.ShapeViews.Add(shapeView);
				}
			}
			layerView.layerDefaultView = (JobEditor.Views.ProductData.LayerDefaultView)this.LayerDefaultView.Clone();
			layerView.stepLapsDefaultView = (JobEditor.Views.ProductData.StepLapsDefaultView)this.StepLapsDefaultView.Clone();
			layerView.holesDefaultView = (JobEditor.Views.ProductData.HolesDefaultView)this.HolesDefaultView.Clone();
			layerView.tipCutsDefaultView = (JobEditor.Views.ProductData.TipCutsDefaultView)this.TipCutsDefaultView.Clone();
			layerView.slotsDefaultView = (JobEditor.Views.ProductData.SlotsDefaultView)this.SlotsDefaultView.Clone();
			layerView.centersDefaultView = (JobEditor.Views.ProductData.CentersDefaultView)this.CentersDefaultView.Clone();
			layerView.id = this.id;
			layerView.name = this.name;
			layerView.info = this.info;
			return layerView;
		}

		public void CloneCentersDataFrom(LayerView layerView)
		{
			this.EnableOnSelectedLayerViewChanged(false);
			this.CentersDefaultView.CloneDataFrom(layerView.CentersDefaultView);
			for (int i = 0; i < layerView.ShapeViews.Count; i++)
			{
				ShapeView item = layerView.ShapeViews[i];
				if (layerView.ShapeViews.Count != this.ShapeViews.Count)
				{
					break;
				}
				this.ShapeViews[i].CentersView.CloneDataFrom(item.CentersView);
			}
			this.EnableOnSelectedLayerViewChanged(true);
			this.CallOnSelectedLayerViewChanged();
		}

		public void CloneDataFrom(LayerView layerView)
		{
			this.EnableOnSelectedLayerViewChanged(false);
			if (layerView.ShapeViews == null)
			{
				this.ShapeViews.Clear();
				this.ShapeViews = null;
			}
			else
			{
				if (this.ShapeViews == null)
				{
					this.ShapeViews = new ObservableCollection<ShapeView>();
				}
				for (int i = 0; i < layerView.ShapeViews.Count; i++)
				{
					if (this.ShapeViews.Count <= i)
					{
						this.ShapeViews.Add(new ShapeView(base.ProductView, null));
					}
					ShapeView item = layerView.ShapeViews[i];
					this.ShapeViews[i].CloneDataFrom(item);
				}
				while (this.ShapeViews.Count > layerView.ShapeViews.Count)
				{
					this.ShapeViews.RemoveAt(this.ShapeViews.Count - 1);
				}
			}
			this.Id = layerView.Id;
			this.Name = layerView.Name;
			this.Info = layerView.Info;
			this.LayerDefaultView.CloneDataFrom(layerView.LayerDefaultView);
			this.StepLapsDefaultView.CloneDataFrom(layerView.StepLapsDefaultView);
			this.HolesDefaultView.CloneDataFrom(layerView.HolesDefaultView);
			this.TipCutsDefaultView.CloneDataFrom(layerView.TipCutsDefaultView);
			this.SlotsDefaultView.CloneDataFrom(layerView.SlotsDefaultView);
			this.CentersDefaultView.CloneDataFrom(layerView.CentersDefaultView);
			this.EnableOnSelectedLayerViewChanged(true);
			this.CallOnSelectedLayerViewChanged();
		}

		public void CloneHolesDataFrom(LayerView layerView)
		{
			this.EnableOnSelectedLayerViewChanged(false);
			this.HolesDefaultView.CloneDataFrom(layerView.HolesDefaultView);
			for (int i = 0; i < layerView.ShapeViews.Count; i++)
			{
				ShapeView item = layerView.ShapeViews[i];
				if (layerView.ShapeViews.Count != this.ShapeViews.Count)
				{
					break;
				}
				this.ShapeViews[i].HolesView.CloneDataFrom(item.HolesView);
			}
			this.EnableOnSelectedLayerViewChanged(true);
			this.CallOnSelectedLayerViewChanged();
		}

		public void CloneSlotsDataFrom(LayerView layerView)
		{
			this.EnableOnSelectedLayerViewChanged(false);
			this.SlotsDefaultView.CloneDataFrom(layerView.SlotsDefaultView);
			for (int i = 0; i < layerView.ShapeViews.Count; i++)
			{
				ShapeView item = layerView.ShapeViews[i];
				if (layerView.ShapeViews.Count != this.ShapeViews.Count)
				{
					break;
				}
				this.ShapeViews[i].SlotsView.CloneDataFrom(item.SlotsView);
			}
			this.EnableOnSelectedLayerViewChanged(true);
			this.CallOnSelectedLayerViewChanged();
		}

		public void CloneStepLapsDataFrom(LayerView layerView)
		{
			this.EnableOnSelectedLayerViewChanged(false);
			this.StepLapsDefaultView.CloneDataFrom(layerView.StepLapsDefaultView);
			for (int i = 0; i < layerView.ShapeViews.Count; i++)
			{
				ShapeView item = layerView.ShapeViews[i];
				if (layerView.ShapeViews.Count != this.ShapeViews.Count)
				{
					break;
				}
				for (int j = 0; j < item.ShapePartViews.Count; j++)
				{
					ShapePartView shapePartView = item.ShapePartViews[j];
					if (item.ShapePartViews.Count != this.ShapeViews[i].ShapePartViews.Count)
					{
						break;
					}
					this.ShapeViews[i].ShapePartViews[j].StepLapView.CloneDataFrom(shapePartView.StepLapView);
				}
			}
			this.EnableOnSelectedLayerViewChanged(true);
			this.CallOnSelectedLayerViewChanged();
		}

		public void CloneTipCutsDataFrom(LayerView layerView)
		{
			this.EnableOnSelectedLayerViewChanged(false);
			this.TipCutsDefaultView.CloneDataFrom(layerView.TipCutsDefaultView);
			for (int i = 0; i < layerView.ShapeViews.Count; i++)
			{
				ShapeView item = layerView.ShapeViews[i];
				if (layerView.ShapeViews.Count != this.ShapeViews.Count)
				{
					break;
				}
				for (int j = 0; j < item.ShapePartViews.Count; j++)
				{
					ShapePartView shapePartView = item.ShapePartViews[j];
					if (item.ShapePartViews.Count != this.ShapeViews[i].ShapePartViews.Count)
					{
						break;
					}
					this.ShapeViews[i].ShapePartViews[j].TipCutView.CloneDataFrom(shapePartView.TipCutView);
				}
			}
			this.EnableOnSelectedLayerViewChanged(true);
			this.CallOnSelectedLayerViewChanged();
		}

		public void EnableOnSelectedLayerViewChanged(bool enable)
		{
			if (base.ProductView == null)
			{
				return;
			}
			if (base.ProductView.SelectedLayerView != this)
			{
				return;
			}
			base.ProductView.EnableOnSelectedLayerViewChanged(enable);
		}
	}
}