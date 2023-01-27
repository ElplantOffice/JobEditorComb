using JobEditor.Common;
using JobEditor.Properties;
using ProductLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JobEditor.Views.ProductData
{
	public class ProductView : ProductDataViewBase
	{
		public const double eps = 1E-07;

		public const double inch_mm = 25.4;

		public const int minNumberOfLayers = 1;

		public const int maxNumberOfLayers = 30;

		public const double maxShapeLength_mm = 7000;

		public const double maxShapeLength_inch = 275.590551181102;

		public const double maxCentersLength_mm = 7000;

		public const double maxCentersLength_inch = 275.590551181102;

		public const double minTipOffset_mm = 0;

		public const double minTipOffset_inch = 0;

		public const double minCentersOverCut_mm = 0;

		public const double minCentersOverCut_inch = 0;

		public const double maxCentersOverCut_mm = 10;

		public const double maxCentersOverCut_inch = 0.393700787401575;

		public const double minHoleXPosition_mm = 0;

		public const double minHoleXPosition_inch = 0;

		public const double minHoleXDistance_mm = 0.01;

		public const double minHoleXDistance_inch = 0.0001;

		public const double minHoleDiameter_mm = 2;

		public const double minHoleDiameter_inch = 0.08;

		public const double minSlotXPosition_mm = 0;

		public const double minSlotXPosition_inch = 0;

		public const double minSlotXLength_mm = 0.01;

		public const double minSlotXLength_inch = 0.0001;

		private double minSlotDistanceY;

		public const int minNumberSteps = 1;

		public const int maxNumberSteps = 10;

		public const int minNumberSame = 1;

		public const int maxNumberSame = 10;

		public const double minStepLapValue_mm = 0;

		public const double minStepLapValue_inch = 0;

		public const double maxStepLapValue_mm = 20;

		public const double maxStepLapValue_inch = 0.78740157480315;

		public const double minTipCutsOverCut_mm = 0;

		public const double minTipCutsOverCut_inch = 0;

		public const double maxTipCutsOverCut_mm = 10;

		public const double maxTipCutsOverCut_inch = 0.393700787401575;

		public const double minTipCutsHeight_mm = 0;

		public const double minTipCutsHeight_inch = 0;

		public const double minMaterialThickness_mm = 0.1;

		public const double minMaterialThickness_inch = 0.00393700787401575;

		public const double maxMaterialThickness_mm = 1;

		public const double maxMaterialThickness_inch = 0.0393700787401575;

		public const double minWidth_mm = 10;

		public const double minWidth_inch = 0.393700787401575;

		public const double maxWidth_mm = 1200;

		public const double maxWidth_inch = 47.244094488189;

		public const double minHeightNumber = 1;

		public const double maxHeightNumber = 99999;

		private int actLayerNum;

		private LayerView selectedLayerView = new LayerView(null, null);

		private List<LayerView> layerViews = new List<LayerView>();

		private EHeightMeasType heightMeasType = EHeightMeasType.Relative;

		private EHeightRefType heightRefType;

		private int countDisablingOnSelectedLayerViewChanged;

		private bool valid;

		public int ActLayerNum
		{
			get
			{
				return this.actLayerNum;
			}
			set
			{
				this.actLayerNum = value;
				this.EnableOnSelectedLayerViewChanged(false);
				this.EnableOnPropertyChanged = false;
				if (this.actLayerNum < 1 || this.actLayerNum > this.LayerViews.Count)
				{
					this.SelectedLayerView = new LayerView(null, null);
				}
				else
				{
					bool flag = false;
					if (this.actLayerNum < 1 || this.actLayerNum > this.LayerViews.Count)
					{
						flag = true;
					}
					if (this.selectedLayerView.Name != this.LayerViews[this.actLayerNum - 1].Name)
					{
						flag = true;
					}
					if (flag)
					{
						this.SelectedLayerView = new LayerView(base.ProductView, null);
					}
					for (int i = 0; i < this.LayerViews.Count; i++)
					{
						LayerView item = this.LayerViews[i];
						if (item == this.SelectedLayerView)
						{
							LayerView layerView = new LayerView(base.ProductView, null);
							layerView.CloneDataFrom(item);
							this.LayerViews[i] = layerView;
						}
					}
					this.SelectedLayerView.CloneDataFrom(this.LayerViews[this.actLayerNum - 1]);
					this.LayerViews[this.actLayerNum - 1] = this.SelectedLayerView;
				}
				this.EnableOnPropertyChanged = true;
				this.Validate();
				base.RaisePropertyChanged("ActLayerNum", true);
				this.EnableOnSelectedLayerViewChanged(true);
				this.CallOnSelectedLayerViewChanged();
			}
		}

		public bool EnableOnPropertyChanged
		{
			get;
			set;
		}

		public EHeightMeasType HeightMeasType
		{
			get
			{
				return this.heightMeasType;
			}
			set
			{
				this.heightMeasType = value;
				base.RaisePropertyChanged("HeightMeasType", true);
			}
		}

		public bool HeightMeasType_Valid
		{
			get
			{
				return base.GetValidState("HeightMeasType");
			}
			set
			{
				base.SetValidState("HeightMeasType", value);
			}
		}

		public EHeightRefType HeightRefType
		{
			get
			{
				return this.heightRefType;
			}
			set
			{
				this.heightRefType = value;
				base.RaisePropertyChanged("HeightRefType", true);
			}
		}

		public bool HeightRefType_Valid
		{
			get
			{
				return base.GetValidState("HeightRefType");
			}
			set
			{
				base.SetValidState("HeightRefType", value);
			}
		}

		public bool IsEnabledOnSelectedLayerViewChanged
		{
			get
			{
				bool flag = false;
				if (this.countDisablingOnSelectedLayerViewChanged == 0)
				{
					flag = true;
				}
				return flag;
			}
		}

		public List<LayerView> LayerViews
		{
			get
			{
				return this.layerViews;
			}
			set
			{
				this.layerViews = value;
				base.RaisePropertyChanged("LayerViews", true);
			}
		}

		public Action<object, string> OnPropertyChanged
		{
			get;
			set;
		}

		public Action OnSelectedLayerViewChanged
		{
			get;
			set;
		}

		public ProductLib.Product Product
		{
			get
			{
				ProductLib.Product product = new ProductLib.Product();
				product.HeightRefType = this.HeightRefType;
				product.NumberOfLayers = this.LayerViews.Count;
				product.HeightMeasType = this.HeightMeasType;
				product.UnitsInInches = Settings.Default.UnitsInInches;
				product.Layers.Clear();
				if (this.LayerViews != null)
				{
					foreach (LayerView layerView in this.LayerViews)
					{
						Layer layer = layerView.Layer;
						product.Layers.Add(layer);
					}
				}
				return product;
			}
			set
			{
				ProductLib.Product product = value;
				if (product == null)
				{
					this.Clear();
					return;
				}
				this.EnableOnSelectedLayerViewChanged(false);
				this.EnableOnPropertyChanged = false;
				this.HeightRefType = product.HeightRefType;
				this.HeightMeasType = product.HeightMeasType;
				this.LayerViews.Clear();
				if (product.Layers != null)
				{
					foreach (Layer layer in product.Layers)
					{
						LayerView layerView = new LayerView(base.ProductView, layer);
						this.LayerViews.Add(layerView);
					}
				}
				this.EnableOnPropertyChanged = true;
				this.Validate();
				if (this.LayerViews.Count <= 0)
				{
					this.ActLayerNum = 0;
				}
				else
				{
					this.ActLayerNum = 1;
				}
				this.EnableOnSelectedLayerViewChanged(true);
				this.CallOnSelectedLayerViewChanged();
			}
		}

		public LayerView SelectedLayerView
		{
			get
			{
				return this.selectedLayerView;
			}
			private set
			{
				this.selectedLayerView = value;
				base.RaisePropertyChanged("SelectedLayerView", true);
			}
		}

		public bool Valid
		{
			get
			{
				return this.valid;
			}
			private set
			{
				this.valid = value;
				base.RaisePropertyChanged("Valid", false);
			}
		}

		public ProductView()
		{
			base.ProductView = this;
			this.OnPropertyChanged = new Action<object, string>(this.OnDefaultPropertyChanged);
			this.EnableOnPropertyChanged = true;
			this.OnSelectedLayerViewChanged = null;
			this.countDisablingOnSelectedLayerViewChanged = 0;
			this.Clear();
		}

		public bool CallOnSelectedLayerViewChanged()
		{
			if (!this.IsEnabledOnSelectedLayerViewChanged)
			{
				return false;
			}
			if (this.OnSelectedLayerViewChanged == null)
			{
				return false;
			}
			this.OnSelectedLayerViewChanged();
			return true;
		}

		public void Clear()
		{
			this.EnableOnSelectedLayerViewChanged(false);
			bool enableOnPropertyChanged = this.EnableOnPropertyChanged;
			this.EnableOnPropertyChanged = false;
			this.HeightRefType = 0;
			this.HeightMeasType = EHeightMeasType.Relative;
			this.LayerViews.Clear();
			this.ActLayerNum = 0;
			this.EnableOnPropertyChanged = enableOnPropertyChanged;
			if (enableOnPropertyChanged)
			{
				this.Validate();
			}
			this.EnableOnSelectedLayerViewChanged(true);
			this.CallOnSelectedLayerViewChanged();
		}

		public void EnableOnSelectedLayerViewChanged(bool enable)
		{
			if (!enable)
			{
				this.countDisablingOnSelectedLayerViewChanged++;
			}
			else
			{
				this.countDisablingOnSelectedLayerViewChanged--;
				if (this.countDisablingOnSelectedLayerViewChanged < 0)
				{
					this.countDisablingOnSelectedLayerViewChanged = 0;
					return;
				}
			}
		}

		public double GetMaxHoleViewX(ShapeView shapeView)
		{
			double num = 0;
			if (shapeView == null)
			{
				return num;
			}
			if (shapeView.HolesView == null)
			{
				return num;
			}
			HolesView holesView = shapeView.HolesView;
			double length1 = holesView.Length1;
			double length2 = holesView.Length2;
			double length3 = holesView.Length3;
			double length4 = holesView.Length4;
			double length5 = holesView.Length5;
			double length6 = holesView.Length6;
			double length7 = holesView.Length7;
			double length8 = holesView.Length8;
			double length9 = holesView.Length9;
			double length10 = holesView.Length10;
			if (holesView.MeasuringType == EMeasuringType.Absolute)
			{
				switch (holesView.NumberOfHoles)
				{
					case 1:
					{
						num = length1;
						break;
					}
					case 2:
					{
						num = length2;
						break;
					}
					case 3:
					{
						num = length3;
						break;
					}
					case 4:
					{
						num = length4;
						break;
					}
					case 5:
					{
						num = length5;
						break;
					}
					case 6:
					{
						num = length6;
						break;
					}
					case 7:
					{
						num = length7;
						break;
					}
					case 8:
					{
						num = length8;
						break;
					}
					case 9:
					{
						num = length9;
						break;
					}
					case 10:
					{
						num = length10;
						break;
					}
				}
			}
			if (holesView.MeasuringType == EMeasuringType.Relative)
			{
				switch (holesView.NumberOfHoles)
				{
					case 1:
					{
						num = length1;
						break;
					}
					case 2:
					{
						num = length1 + length2;
						break;
					}
					case 3:
					{
						num = length1 + length2 + length3;
						break;
					}
					case 4:
					{
						num = length1 + length2 + length3 + length4;
						break;
					}
					case 5:
					{
						num = length1 + length2 + length3 + length4 + length5;
						break;
					}
					case 6:
					{
						num = length1 + length2 + length3 + length4 + length5 + length6;
						break;
					}
					case 7:
					{
						num = length1 + length2 + length3 + length4 + length5 + length6 + length7;
						break;
					}
					case 8:
					{
						num = length1 + length2 + length3 + length4 + length5 + length6 + length7 + length8;
						break;
					}
					case 9:
					{
						num = length1 + length2 + length3 + length4 + length5 + length6 + length7 + length8 + length9;
						break;
					}
					case 10:
					{
						num = length1 + length2 + length3 + length4 + length5 + length6 + length7 + length8 + length9 + length10;
						break;
					}
				}
			}
			if (holesView.MeasuringType == EMeasuringType.Simetric)
			{
				double shapeViewLength = this.GetShapeViewLength(shapeView);
				switch (holesView.NumberOfHoles)
				{
					case 1:
					{
						num = shapeViewLength / 2;
						break;
					}
					case 2:
					{
						num = (shapeViewLength - length1) / 2 + length1;
						break;
					}
					case 3:
					{
						num = (shapeViewLength - length1 - length2) / 2 + length1 + length2;
						break;
					}
					case 4:
					{
						num = (shapeViewLength - length1 - length2 - length3) / 2 + length1 + length2 + length3;
						break;
					}
					case 5:
					{
						num = (shapeViewLength - length1 - length2 - length3 - length4) / 2 + length1 + length2 + length3 + length4;
						break;
					}
					case 6:
					{
						num = (shapeViewLength - length1 - length2 - length3 - length4 - length5) / 2 + length1 + length2 + length3 + length4 + length5;
						break;
					}
					case 7:
					{
						num = (shapeViewLength - length1 - length2 - length3 - length4 - length5 - length6) / 2 + length1 + length2 + length3 + length4 + length5 + length6;
						break;
					}
					case 8:
					{
						num = (shapeViewLength - length1 - length2 - length3 - length4 - length5 - length6 - length7) / 2 + length1 + length2 + length3 + length4 + length5 + length6 + length7;
						break;
					}
					case 9:
					{
						num = (shapeViewLength - length1 - length2 - length3 - length4 - length5 - length6 - length7 - length8) / 2 + length1 + length2 + length3 + length4 + length5 + length6 + length7 + length8;
						break;
					}
					case 10:
					{
						num = (shapeViewLength - length1 - length2 - length3 - length4 - length5 - length6 - length7 - length8 - length9) / 2 + length1 + length2 + length3 + length4 + length5 + length6 + length7 + length8 + length9;
						break;
					}
				}
			}
			return num;
		}

		public int GetMaxNumberSlotsShapes(LayerView layerView)
		{
			if (layerView == null)
			{
				return 0;
			}
			int numberOfSlots = 0;
			foreach (ShapeView shapeView in layerView.ShapeViews)
			{
				SlotsView slotsView = shapeView.SlotsView;
				if (slotsView.NumberOfSlots <= numberOfSlots)
				{
					continue;
				}
				numberOfSlots = slotsView.NumberOfSlots;
			}
			return numberOfSlots;
		}

		public double GetMaxSlotViewX(ShapeView shapeView)
		{
			double length2 = 0;
			if (shapeView == null)
			{
				return length2;
			}
			if (shapeView.SlotsView == null)
			{
				return length2;
			}
			SlotsView slotsView = shapeView.SlotsView;
			switch ((int)slotsView.MeasuringType)
			{
				case 0:
				{
					length2 = slotsView.Length2;
					break;
				}
				case 1:
				{
					length2 = slotsView.Length1 + slotsView.Length2;
					break;
				}
				case 2:
				{
					length2 = (this.GetShapeViewLength(shapeView) - slotsView.Length1) / 2 + slotsView.Length1;
					break;
				}
				default:
				{
					length2 = 0;
					break;
				}
			}
			return length2;
		}

		public double GetShapeViewLength(ShapeView shapeView)
		{
			double length1 = 0;
			if (shapeView.CentersView.MeasuringType == EMeasuringType.Absolute)
			{
				switch ((int)shapeView.ShapePartViews.Count)
				{
					case 0:
					case 1:
					case 2:
					{
						length1 = shapeView.CentersView.Length1;
						break;
					}
					case 3:
					{
						length1 = shapeView.CentersView.Length2;
						break;
					}
					case 4:
					{
						length1 = shapeView.CentersView.Length3;
						break;
					}
					case 5:
					{
						length1 = shapeView.CentersView.Length4;
						break;
					}
				}
			}
			if (shapeView.CentersView.MeasuringType == EMeasuringType.Relative)
			{
				switch (shapeView.ShapePartViews.Count)
				{
					case 0:
					case 1:
					case 2:
					{
						length1 = shapeView.CentersView.Length1;
						break;
					}
					case 3:
					{
						length1 = shapeView.CentersView.Length1 + shapeView.CentersView.Length2;
						break;
					}
					case 4:
					{
						length1 = shapeView.CentersView.Length1 + shapeView.CentersView.Length2 + shapeView.CentersView.Length3;
						break;
					}
					case 5:
					{
						length1 = shapeView.CentersView.Length1 + shapeView.CentersView.Length2 + shapeView.CentersView.Length3 + shapeView.CentersView.Length4;
						break;
					}
				}
			}
			if (shapeView.CentersView.MeasuringType == EMeasuringType.Simetric)
			{
				switch (shapeView.ShapePartViews.Count)
				{
					case 0:
					case 1:
					case 2:
					{
						length1 = shapeView.CentersView.Length1;
						break;
					}
					case 3:
					{
						length1 = shapeView.CentersView.Length1;
						break;
					}
					case 4:
					{
						length1 = shapeView.CentersView.Length3;
						break;
					}
					case 5:
					{
						length1 = shapeView.CentersView.Length4;
						break;
					}
				}
			}
			return length1;
		}

		public void OnDefaultPropertyChanged(object propertyParent, string propertyName)
		{
			this.Validate();
		}

		public bool Validate()
		{
			bool flag = this.ValidateCentersViews();
			bool flag1 = this.ValidateHolesViews();
			bool flag2 = this.ValidateSlotsViews();
			bool flag3 = this.ValidateLayerViews();
			bool flag4 = this.ValidateStepLapsViews();
			bool flag5 = this.ValidateTipCutsViews();
			bool flag6 = true;
			if (!flag)
			{
				flag6 = false;
			}
			if (!flag1)
			{
				flag6 = false;
			}
			if (!flag2)
			{
				flag6 = false;
			}
			if (!flag3)
			{
				flag6 = false;
			}
			if (!flag4)
			{
				flag6 = false;
			}
			if (!flag5)
			{
				flag6 = false;
			}
			this.Valid = flag6;
			return flag6;
		}

		public bool ValidateCentersDefaultView(LayerView layerView)
		{
			if (layerView == null)
			{
				return true;
			}
			if (layerView.CentersDefaultView == null)
			{
				return true;
			}
			bool flag = true;
			CentersDefaultView centersDefaultView = layerView.CentersDefaultView;
			double width = -layerView.LayerDefaultView.Width / 2;
			double num = layerView.LayerDefaultView.Width / 2;
			bool flag1 = true;
			if (centersDefaultView.VOffset + 1E-07 < width)
			{
				flag1 = false;
				flag = false;
			}
			if (centersDefaultView.VOffset - 1E-07 > num)
			{
				flag1 = false;
				flag = false;
			}
			centersDefaultView.VOffset_Valid = flag1;
			double num3 = (Settings.Default.UnitsInInches ? 0 : 0);
			double num4 = (Settings.Default.UnitsInInches ? 0.393700787401575 : 10);
			bool flag2 = true;
			if (centersDefaultView.OverCut + 1E-07 < num3)
			{
				flag2 = false;
				flag = false;
			}
			if (centersDefaultView.OverCut - 1E-07 > num4)
			{
				flag2 = false;
				flag = false;
			}
			centersDefaultView.OverCut_Valid = flag2;
			return flag;
		}

		public bool ValidateCentersViews()
		{
			bool flag;
			if (this.LayerViews == null)
			{
				return true;
			}
			bool flag1 = true;
			double num = (Settings.Default.UnitsInInches ? 275.590551181102 : 7000);
			double num1 = (Settings.Default.UnitsInInches ? 0 : 0);
			List<LayerView>.Enumerator enumerator = this.LayerViews.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LayerView current = enumerator.Current;
					if (current.ShapeViews != null)
					{
						double width = current.LayerDefaultView.Width;
						double width1 = current.LayerDefaultView.Width / 2;
						using (IEnumerator<ShapeView> enumerator1 = current.ShapeViews.GetEnumerator())
						{
							while (enumerator1.MoveNext())
							{
								ShapeView shapeView = enumerator1.Current;
								bool flag2 = true;
								bool flag3 = true;
								bool flag4 = true;
								bool flag5 = true;
								double num2 = width;
								double num3 = width;
								double num4 = width;
								double num5 = width;
								CentersView centersView = shapeView.CentersView;
								int count = shapeView.ShapePartViews.Count;
								if (centersView.MeasuringType == EMeasuringType.Absolute)
								{
									if (count > 1)
									{
										if (centersView.Length1 + 1E-07 < num2)
										{
											flag2 = false;
										}
										if (centersView.Length1 - 1E-07 > num)
										{
											flag2 = false;
										}
									}
									if (count > 2)
									{
										if (centersView.Length2 + 1E-07 < centersView.Length1 + num3)
										{
											flag2 = false;
											flag3 = false;
										}
										if (centersView.Length2 - 1E-07 > num)
										{
											flag3 = false;
										}
									}
									if (count > 3)
									{
										if (centersView.Length3 + 1E-07 < centersView.Length1 + num3 + num4)
										{
											flag2 = false;
											flag4 = false;
										}
										if (centersView.Length3 + 1E-07 < centersView.Length2 + num4)
										{
											flag3 = false;
											flag4 = false;
										}
										if (centersView.Length3 - 1E-07 > num)
										{
											flag4 = false;
										}
									}
									if (count > 4)
									{
										if (centersView.Length4 + 1E-07 < centersView.Length1 + num3 + num4 + num5)
										{
											flag2 = false;
											flag5 = false;
										}
										if (centersView.Length4 + 1E-07 < centersView.Length2 + num4 + num5)
										{
											flag3 = false;
											flag5 = false;
										}
										if (centersView.Length4 + 1E-07 < centersView.Length3 + num5)
										{
											flag4 = false;
											flag5 = false;
										}
										if (centersView.Length4 - 1E-07 > num)
										{
											flag5 = false;
										}
									}
								}
								if (centersView.MeasuringType == EMeasuringType.Relative)
								{
									if (count > 1)
									{
										if (centersView.Length1 + 1E-07 < num2)
										{
											flag2 = false;
										}
										if (centersView.Length1 - 1E-07 > num)
										{
											flag2 = false;
										}
									}
									if (count > 2)
									{
										if (centersView.Length2 + 1E-07 < num3)
										{
											flag3 = false;
										}
										if (centersView.Length2 - 1E-07 > num - centersView.Length1)
										{
											flag3 = false;
										}
									}
									if (count > 3)
									{
										if (centersView.Length3 + 1E-07 < num4)
										{
											flag4 = false;
										}
										if (centersView.Length3 - 1E-07 > num - centersView.Length1 - centersView.Length2)
										{
											flag4 = false;
										}
									}
									if (count > 4)
									{
										if (centersView.Length4 + 1E-07 < num5)
										{
											flag5 = false;
										}
										if (centersView.Length4 - 1E-07 > num - centersView.Length1 - centersView.Length2 - centersView.Length3)
										{
											flag5 = false;
										}
									}
								}
								if (centersView.MeasuringType == EMeasuringType.Simetric)
								{
									if (count <= 2)
									{
										if (centersView.Length1 + 1E-07 < num2)
										{
											flag2 = false;
										}
										if (centersView.Length1 - 1E-07 > num)
										{
											flag2 = false;
										}
									}
									if (count == 3)
									{
										if (centersView.Length1 + 1E-07 < num2 + num3)
										{
											flag2 = false;
										}
										if (centersView.Length1 - 1E-07 > num)
										{
											flag2 = false;
										}
									}
									if (count == 4)
									{
										if (centersView.Length1 + 1E-07 < num3)
										{
											flag2 = false;
										}
										if (centersView.Length1 - 1E-07 > num - num2 - num4)
										{
											flag2 = false;
										}
										if (centersView.Length3 + 1E-07 < num2 + centersView.Length1 + num4)
										{
											flag2 = false;
											flag4 = false;
										}
										if (centersView.Length3 - 1E-07 > num)
										{
											flag4 = false;
										}
									}
									if (count == 5)
									{
										if (centersView.Length1 + 1E-07 < num3)
										{
											flag2 = false;
										}
										if (centersView.Length1 - 1E-07 > num - num2 - num4 - num5)
										{
											flag2 = false;
										}
										if (centersView.Length2 + 1E-07 < num4)
										{
											flag3 = false;
										}
										if (centersView.Length2 - 1E-07 > num - num2 - num3 - num5)
										{
											flag3 = false;
										}
										if (centersView.Length4 + 1E-07 < num2 + centersView.Length1 + centersView.Length2 + num5)
										{
											flag2 = false;
											flag3 = false;
											flag5 = false;
										}
										if (centersView.Length4 - 1E-07 > num)
										{
											flag5 = false;
										}
									}
								}
								double shapeViewLength = this.GetShapeViewLength(shapeView);
								if (this.GetMaxHoleViewX(shapeView) > shapeViewLength)
								{
									switch (count)
									{
										case 2:
										{
											flag2 = false;
											break;
										}
										case 3:
										{
											if (centersView.MeasuringType != EMeasuringType.Simetric)
											{
												flag3 = false;
												break;
											}
											else
											{
												flag2 = false;
												break;
											}
										}
										case 4:
										{
											flag4 = false;
											break;
										}
										case 5:
										{
											flag5 = false;
											break;
										}
									}
								}
								shapeViewLength = this.GetShapeViewLength(shapeView);
								if (this.GetMaxSlotViewX(shapeView) > shapeViewLength)
								{
									switch (count)
									{
										case 2:
										{
											flag2 = false;
											break;
										}
										case 3:
										{
											if (centersView.MeasuringType != EMeasuringType.Simetric)
											{
												flag3 = false;
												break;
											}
											else
											{
												flag2 = false;
												break;
											}
										}
										case 4:
										{
											flag4 = false;
											break;
										}
										case 5:
										{
											flag5 = false;
											break;
										}
									}
								}
								if (count == 2 || count == 3)
								{
									string[] strArrays = shapeView.Drawing.Split(new char[] { '|' });
									if (strArrays.First<string>() == "CB" || strArrays.First<string>() == "CF")
									{
										if (centersView.Length3 + 1E-07 < num1)
										{
											flag4 = false;
										}
										if (centersView.Length3 - 1E-07 > width1)
										{
											flag4 = false;
										}
									}
									if (strArrays.Last<string>() == "CB" || strArrays.Last<string>() == "CF")
									{
										if (centersView.Length4 + 1E-07 < num1)
										{
											flag5 = false;
										}
										if (centersView.Length4 - 1E-07 > width1)
										{
											flag5 = false;
										}
									}
								}
								centersView.Length1_Valid = flag2;
								centersView.Length2_Valid = flag3;
								centersView.Length3_Valid = flag4;
								centersView.Length4_Valid = flag5;
								if (!flag2)
								{
									flag1 = false;
								}
								if (!flag3)
								{
									flag1 = false;
								}
								if (!flag4)
								{
									flag1 = false;
								}
								if (flag5)
								{
									continue;
								}
								flag1 = false;
							}
						}
						if (this.ValidateCentersDefaultView(current))
						{
							continue;
						}
						flag1 = false;
					}
					else
					{
						flag = true;
						return flag;
					}
				}
				return flag1;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		public bool ValidateHolesDefaultView(LayerView layerView)
		{
			if (layerView == null)
			{
				return true;
			}
			if (layerView.HolesDefaultView == null)
			{
				return true;
			}
			bool flag = true;
			HolesDefaultView holesDefaultView = layerView.HolesDefaultView;
			double width = -layerView.LayerDefaultView.Width / 2;
			double num = layerView.LayerDefaultView.Width / 2;
			bool flag1 = true;
			if (holesDefaultView.Offset + 1E-07 < width)
			{
				flag1 = false;
				flag = false;
			}
			if (holesDefaultView.Offset - 1E-07 > num)
			{
				flag1 = false;
				flag = false;
			}
			holesDefaultView.Offset_Valid = flag1;
			return flag;
		}

		public bool ValidateHolesViews()
		{
			bool flag;
			if (this.LayerViews == null)
			{
				return true;
			}
			bool flag1 = true;
			double num = (Settings.Default.UnitsInInches ? 0 : 0);
			double num1 = (Settings.Default.UnitsInInches ? 0.0001 : 0.01);
			double num2 = (Settings.Default.UnitsInInches ? 0.08 : 2);
			num = num + num2 / 2;
			num1 += num2;
			List<LayerView>.Enumerator enumerator = this.LayerViews.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LayerView current = enumerator.Current;
					if (current.ShapeViews != null)
					{
						using (IEnumerator<ShapeView> enumerator1 = current.ShapeViews.GetEnumerator())
						{
							while (enumerator1.MoveNext())
							{
								ShapeView shapeView = enumerator1.Current;
								bool flag2 = true;
								bool flag3 = true;
								bool flag4 = true;
								bool flag5 = true;
								bool flag6 = true;
								bool flag7 = true;
								bool flag8 = true;
								bool flag9 = true;
								bool flag10 = true;
								bool flag11 = true;
								double shapeViewLength = this.GetShapeViewLength(shapeView) - num2 / 2;
								HolesView holesView = shapeView.HolesView;
								int numberOfHoles = holesView.NumberOfHoles;
								if (holesView.MeasuringType == EMeasuringType.Absolute)
								{
									if (numberOfHoles >= 1)
									{
										if (holesView.Length1 + 1E-07 < num)
										{
											flag2 = false;
										}
										if (holesView.Length1 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
										}
									}
									if (numberOfHoles >= 2)
									{
										if (holesView.Length2 + 1E-07 < holesView.Length1 + num1)
										{
											flag2 = false;
											flag3 = false;
										}
										if (holesView.Length2 - 1E-07 > shapeViewLength)
										{
											flag3 = false;
										}
									}
									if (numberOfHoles >= 3)
									{
										if (holesView.Length3 + 1E-07 < holesView.Length1 + num1 * 2)
										{
											flag2 = false;
											flag4 = false;
										}
										if (holesView.Length3 + 1E-07 < holesView.Length2 + num1)
										{
											flag3 = false;
											flag4 = false;
										}
										if (holesView.Length3 - 1E-07 > shapeViewLength)
										{
											flag4 = false;
										}
									}
									if (numberOfHoles >= 4)
									{
										if (holesView.Length4 + 1E-07 < holesView.Length1 + num1 * 3)
										{
											flag2 = false;
											flag5 = false;
										}
										if (holesView.Length4 + 1E-07 < holesView.Length2 + num1 * 2)
										{
											flag3 = false;
											flag5 = false;
										}
										if (holesView.Length4 + 1E-07 < holesView.Length3 + num1)
										{
											flag4 = false;
											flag5 = false;
										}
										if (holesView.Length4 - 1E-07 > shapeViewLength)
										{
											flag5 = false;
										}
									}
									if (numberOfHoles >= 5)
									{
										if (holesView.Length5 + 1E-07 < holesView.Length1 + num1 * 4)
										{
											flag2 = false;
											flag6 = false;
										}
										if (holesView.Length5 + 1E-07 < holesView.Length2 + num1 * 3)
										{
											flag3 = false;
											flag6 = false;
										}
										if (holesView.Length5 + 1E-07 < holesView.Length3 + num1 * 2)
										{
											flag4 = false;
											flag6 = false;
										}
										if (holesView.Length5 + 1E-07 < holesView.Length4 + num1)
										{
											flag5 = false;
											flag6 = false;
										}
										if (holesView.Length5 - 1E-07 > shapeViewLength)
										{
											flag6 = false;
										}
									}
									if (numberOfHoles >= 6)
									{
										if (holesView.Length6 + 1E-07 < holesView.Length1 + num1 * 5)
										{
											flag2 = false;
											flag7 = false;
										}
										if (holesView.Length6 + 1E-07 < holesView.Length2 + num1 * 4)
										{
											flag3 = false;
											flag7 = false;
										}
										if (holesView.Length6 + 1E-07 < holesView.Length3 + num1 * 3)
										{
											flag4 = false;
											flag7 = false;
										}
										if (holesView.Length6 + 1E-07 < holesView.Length4 + num1 * 2)
										{
											flag5 = false;
											flag7 = false;
										}
										if (holesView.Length6 + 1E-07 < holesView.Length5 + num1)
										{
											flag6 = false;
											flag7 = false;
										}
										if (holesView.Length6 - 1E-07 > shapeViewLength)
										{
											flag7 = false;
										}
									}
									if (numberOfHoles >= 7)
									{
										if (holesView.Length7 + 1E-07 < holesView.Length1 + num1 * 6)
										{
											flag2 = false;
											flag8 = false;
										}
										if (holesView.Length7 + 1E-07 < holesView.Length2 + num1 * 5)
										{
											flag3 = false;
											flag8 = false;
										}
										if (holesView.Length7 + 1E-07 < holesView.Length3 + num1 * 4)
										{
											flag4 = false;
											flag8 = false;
										}
										if (holesView.Length7 + 1E-07 < holesView.Length4 + num1 * 3)
										{
											flag5 = false;
											flag8 = false;
										}
										if (holesView.Length7 + 1E-07 < holesView.Length5 + num1 * 2)
										{
											flag6 = false;
											flag8 = false;
										}
										if (holesView.Length7 + 1E-07 < holesView.Length6 + num1)
										{
											flag7 = false;
											flag8 = false;
										}
										if (holesView.Length7 - 1E-07 > shapeViewLength)
										{
											flag8 = false;
										}
									}
									if (numberOfHoles >= 8)
									{
										if (holesView.Length8 + 1E-07 < holesView.Length1 + num1 * 7)
										{
											flag2 = false;
											flag9 = false;
										}
										if (holesView.Length8 + 1E-07 < holesView.Length2 + num1 * 6)
										{
											flag3 = false;
											flag9 = false;
										}
										if (holesView.Length8 + 1E-07 < holesView.Length3 + num1 * 5)
										{
											flag4 = false;
											flag9 = false;
										}
										if (holesView.Length8 + 1E-07 < holesView.Length4 + num1 * 4)
										{
											flag5 = false;
											flag9 = false;
										}
										if (holesView.Length8 + 1E-07 < holesView.Length5 + num1 * 3)
										{
											flag6 = false;
											flag9 = false;
										}
										if (holesView.Length8 + 1E-07 < holesView.Length6 + num1 * 2)
										{
											flag7 = false;
											flag9 = false;
										}
										if (holesView.Length8 + 1E-07 < holesView.Length7 + num1)
										{
											flag8 = false;
											flag9 = false;
										}
										if (holesView.Length8 - 1E-07 > shapeViewLength)
										{
											flag9 = false;
										}
									}
									if (numberOfHoles >= 9)
									{
										if (holesView.Length9 + 1E-07 < holesView.Length1 + num1 * 8)
										{
											flag2 = false;
											flag10 = false;
										}
										if (holesView.Length9 + 1E-07 < holesView.Length2 + num1 * 7)
										{
											flag3 = false;
											flag10 = false;
										}
										if (holesView.Length9 + 1E-07 < holesView.Length3 + num1 * 6)
										{
											flag4 = false;
											flag10 = false;
										}
										if (holesView.Length9 + 1E-07 < holesView.Length4 + num1 * 5)
										{
											flag5 = false;
											flag10 = false;
										}
										if (holesView.Length9 + 1E-07 < holesView.Length5 + num1 * 4)
										{
											flag6 = false;
											flag10 = false;
										}
										if (holesView.Length9 + 1E-07 < holesView.Length6 + num1 * 3)
										{
											flag7 = false;
											flag10 = false;
										}
										if (holesView.Length9 + 1E-07 < holesView.Length7 + num1 * 2)
										{
											flag8 = false;
											flag10 = false;
										}
										if (holesView.Length9 + 1E-07 < holesView.Length8 + num1)
										{
											flag9 = false;
											flag10 = false;
										}
										if (holesView.Length9 - 1E-07 > shapeViewLength)
										{
											flag10 = false;
										}
									}
									if (numberOfHoles >= 10)
									{
										if (holesView.Length10 + 1E-07 < holesView.Length1 + num1 * 9)
										{
											flag2 = false;
											flag11 = false;
										}
										if (holesView.Length10 + 1E-07 < holesView.Length2 + num1 * 8)
										{
											flag3 = false;
											flag11 = false;
										}
										if (holesView.Length10 + 1E-07 < holesView.Length3 + num1 * 7)
										{
											flag4 = false;
											flag11 = false;
										}
										if (holesView.Length10 + 1E-07 < holesView.Length4 + num1 * 6)
										{
											flag5 = false;
											flag11 = false;
										}
										if (holesView.Length10 + 1E-07 < holesView.Length5 + num1 * 5)
										{
											flag6 = false;
											flag11 = false;
										}
										if (holesView.Length10 + 1E-07 < holesView.Length6 + num1 * 4)
										{
											flag7 = false;
											flag11 = false;
										}
										if (holesView.Length10 + 1E-07 < holesView.Length7 + num1 * 3)
										{
											flag8 = false;
											flag11 = false;
										}
										if (holesView.Length10 + 1E-07 < holesView.Length8 + num1 * 2)
										{
											flag9 = false;
											flag11 = false;
										}
										if (holesView.Length10 + 1E-07 < holesView.Length9 + num1)
										{
											flag10 = false;
											flag11 = false;
										}
										if (holesView.Length10 - 1E-07 > shapeViewLength)
										{
											flag11 = false;
										}
									}
								}
								if (holesView.MeasuringType == EMeasuringType.Relative)
								{
									if (numberOfHoles >= 1)
									{
										if (holesView.Length1 + 1E-07 < num)
										{
											flag2 = false;
										}
										if (holesView.Length1 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
										}
									}
									if (numberOfHoles >= 2)
									{
										if (holesView.Length2 + 1E-07 < num1)
										{
											flag3 = false;
										}
										if (holesView.Length2 - 1E-07 > shapeViewLength - holesView.Length1)
										{
											flag3 = false;
										}
									}
									if (numberOfHoles >= 3)
									{
										if (holesView.Length3 + 1E-07 < num1)
										{
											flag4 = false;
										}
										if (holesView.Length3 - 1E-07 > shapeViewLength - holesView.Length1 - holesView.Length2)
										{
											flag4 = false;
										}
									}
									if (numberOfHoles >= 4)
									{
										if (holesView.Length4 + 1E-07 < num1)
										{
											flag5 = false;
										}
										if (holesView.Length4 - 1E-07 > shapeViewLength - holesView.Length1 - holesView.Length2 - holesView.Length3)
										{
											flag5 = false;
										}
									}
									if (numberOfHoles >= 5)
									{
										if (holesView.Length5 + 1E-07 < num1)
										{
											flag6 = false;
										}
										if (holesView.Length5 - 1E-07 > shapeViewLength - holesView.Length1 - holesView.Length2 - holesView.Length3 - holesView.Length4)
										{
											flag6 = false;
										}
									}
									if (numberOfHoles >= 6)
									{
										if (holesView.Length6 + 1E-07 < num1)
										{
											flag7 = false;
										}
										if (holesView.Length6 - 1E-07 > shapeViewLength - holesView.Length1 - holesView.Length2 - holesView.Length3 - holesView.Length4 - holesView.Length5)
										{
											flag7 = false;
										}
									}
									if (numberOfHoles >= 7)
									{
										if (holesView.Length7 + 1E-07 < num1)
										{
											flag8 = false;
										}
										if (holesView.Length7 - 1E-07 > shapeViewLength - holesView.Length1 - holesView.Length2 - holesView.Length3 - holesView.Length4 - holesView.Length5 - holesView.Length6)
										{
											flag8 = false;
										}
									}
									if (numberOfHoles >= 8)
									{
										if (holesView.Length8 + 1E-07 < num1)
										{
											flag9 = false;
										}
										if (holesView.Length8 - 1E-07 > shapeViewLength - holesView.Length1 - holesView.Length2 - holesView.Length3 - holesView.Length4 - holesView.Length5 - holesView.Length6 - holesView.Length7)
										{
											flag9 = false;
										}
									}
									if (numberOfHoles >= 9)
									{
										if (holesView.Length9 + 1E-07 < num1)
										{
											flag10 = false;
										}
										if (holesView.Length9 - 1E-07 > shapeViewLength - holesView.Length1 - holesView.Length2 - holesView.Length3 - holesView.Length4 - holesView.Length5 - holesView.Length6 - holesView.Length7 - holesView.Length8)
										{
											flag10 = false;
										}
									}
									if (numberOfHoles >= 10)
									{
										if (holesView.Length10 + 1E-07 < num1)
										{
											flag11 = false;
										}
										if (holesView.Length10 - 1E-07 > shapeViewLength - holesView.Length1 - holesView.Length2 - holesView.Length3 - holesView.Length4 - holesView.Length5 - holesView.Length6 - holesView.Length7 - holesView.Length8 - holesView.Length9)
										{
											flag11 = false;
										}
									}
								}
								if (holesView.MeasuringType == EMeasuringType.Simetric)
								{
									if (numberOfHoles >= 1)
									{
										if (holesView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (holesView.Length1 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
										}
									}
									if (numberOfHoles == 3)
									{
										if (holesView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (holesView.Length2 + 1E-07 < num1)
										{
											flag3 = false;
										}
										if (holesView.Length1 + holesView.Length2 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
											flag3 = false;
										}
									}
									if (numberOfHoles == 4)
									{
										if (holesView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (holesView.Length2 + 1E-07 < num1)
										{
											flag3 = false;
										}
										if (holesView.Length3 + 1E-07 < num1)
										{
											flag4 = false;
										}
										if (holesView.Length1 + holesView.Length2 + holesView.Length3 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
											flag3 = false;
											flag4 = false;
										}
									}
									if (numberOfHoles == 5)
									{
										if (holesView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (holesView.Length2 + 1E-07 < num1)
										{
											flag3 = false;
										}
										if (holesView.Length3 + 1E-07 < num1)
										{
											flag4 = false;
										}
										if (holesView.Length4 + 1E-07 < num1)
										{
											flag5 = false;
										}
										if (holesView.Length1 + holesView.Length2 + holesView.Length3 + holesView.Length4 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
											flag3 = false;
											flag4 = false;
											flag5 = false;
										}
									}
									if (numberOfHoles == 6)
									{
										if (holesView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (holesView.Length2 + 1E-07 < num1)
										{
											flag3 = false;
										}
										if (holesView.Length3 + 1E-07 < num1)
										{
											flag4 = false;
										}
										if (holesView.Length4 + 1E-07 < num1)
										{
											flag5 = false;
										}
										if (holesView.Length5 + 1E-07 < num1)
										{
											flag6 = false;
										}
										if (holesView.Length1 + holesView.Length2 + holesView.Length3 + holesView.Length4 + holesView.Length5 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
											flag3 = false;
											flag4 = false;
											flag5 = false;
											flag6 = false;
										}
									}
									if (numberOfHoles == 7)
									{
										if (holesView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (holesView.Length2 + 1E-07 < num1)
										{
											flag3 = false;
										}
										if (holesView.Length3 + 1E-07 < num1)
										{
											flag4 = false;
										}
										if (holesView.Length4 + 1E-07 < num1)
										{
											flag5 = false;
										}
										if (holesView.Length5 + 1E-07 < num1)
										{
											flag6 = false;
										}
										if (holesView.Length6 + 1E-07 < num1)
										{
											flag7 = false;
										}
										if (holesView.Length1 + holesView.Length2 + holesView.Length3 + holesView.Length4 + holesView.Length5 + holesView.Length6 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
											flag3 = false;
											flag4 = false;
											flag5 = false;
											flag6 = false;
											flag7 = false;
										}
									}
									if (numberOfHoles == 8)
									{
										if (holesView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (holesView.Length2 + 1E-07 < num1)
										{
											flag3 = false;
										}
										if (holesView.Length3 + 1E-07 < num1)
										{
											flag4 = false;
										}
										if (holesView.Length4 + 1E-07 < num1)
										{
											flag5 = false;
										}
										if (holesView.Length5 + 1E-07 < num1)
										{
											flag6 = false;
										}
										if (holesView.Length6 + 1E-07 < num1)
										{
											flag7 = false;
										}
										if (holesView.Length7 + 1E-07 < num1)
										{
											flag8 = false;
										}
										if (holesView.Length1 + holesView.Length2 + holesView.Length3 + holesView.Length4 + holesView.Length5 + holesView.Length6 + holesView.Length7 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
											flag3 = false;
											flag4 = false;
											flag5 = false;
											flag6 = false;
											flag7 = false;
											flag8 = false;
										}
									}
									if (numberOfHoles == 9)
									{
										if (holesView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (holesView.Length2 + 1E-07 < num1)
										{
											flag3 = false;
										}
										if (holesView.Length3 + 1E-07 < num1)
										{
											flag4 = false;
										}
										if (holesView.Length4 + 1E-07 < num1)
										{
											flag5 = false;
										}
										if (holesView.Length5 + 1E-07 < num1)
										{
											flag6 = false;
										}
										if (holesView.Length6 + 1E-07 < num1)
										{
											flag7 = false;
										}
										if (holesView.Length7 + 1E-07 < num1)
										{
											flag8 = false;
										}
										if (holesView.Length8 + 1E-07 < num1)
										{
											flag9 = false;
										}
										if (holesView.Length1 + holesView.Length2 + holesView.Length3 + holesView.Length4 + holesView.Length5 + holesView.Length6 + holesView.Length7 + holesView.Length8 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
											flag3 = false;
											flag4 = false;
											flag5 = false;
											flag6 = false;
											flag7 = false;
											flag8 = false;
											flag9 = false;
										}
									}
									if (numberOfHoles == 10)
									{
										if (holesView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (holesView.Length2 + 1E-07 < num1)
										{
											flag3 = false;
										}
										if (holesView.Length3 + 1E-07 < num1)
										{
											flag4 = false;
										}
										if (holesView.Length4 + 1E-07 < num1)
										{
											flag5 = false;
										}
										if (holesView.Length5 + 1E-07 < num1)
										{
											flag6 = false;
										}
										if (holesView.Length6 + 1E-07 < num1)
										{
											flag7 = false;
										}
										if (holesView.Length7 + 1E-07 < num1)
										{
											flag8 = false;
										}
										if (holesView.Length8 + 1E-07 < num1)
										{
											flag9 = false;
										}
										if (holesView.Length9 + 1E-07 < num1)
										{
											flag10 = false;
										}
										if (holesView.Length1 + holesView.Length2 + holesView.Length3 + holesView.Length4 + holesView.Length5 + holesView.Length6 + holesView.Length7 + holesView.Length8 + holesView.Length9 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
											flag3 = false;
											flag4 = false;
											flag5 = false;
											flag6 = false;
											flag7 = false;
											flag8 = false;
											flag9 = false;
										}
									}
								}
								holesView.Length1_Valid = flag2;
								holesView.Length2_Valid = flag3;
								holesView.Length3_Valid = flag4;
								holesView.Length4_Valid = flag5;
								holesView.Length5_Valid = flag6;
								holesView.Length6_Valid = flag7;
								holesView.Length7_Valid = flag8;
								holesView.Length8_Valid = flag9;
								holesView.Length9_Valid = flag10;
								holesView.Length10_Valid = flag11;
								if (!flag2)
								{
									flag1 = false;
								}
								if (!flag3)
								{
									flag1 = false;
								}
								if (!flag4)
								{
									flag1 = false;
								}
								if (!flag5)
								{
									flag1 = false;
								}
								if (!flag6)
								{
									flag1 = false;
								}
								if (!flag7)
								{
									flag1 = false;
								}
								if (!flag8)
								{
									flag1 = false;
								}
								if (!flag9)
								{
									flag1 = false;
								}
								if (!flag10)
								{
									flag1 = false;
								}
								if (flag11)
								{
									continue;
								}
								flag1 = false;
							}
						}
						if (this.ValidateHolesDefaultView(current))
						{
							continue;
						}
						flag1 = false;
					}
					else
					{
						flag = true;
						return flag;
					}
				}
				return flag1;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		public bool ValidateLayerDefaultView(LayerView layerView)
		{
			if (layerView == null)
			{
				return true;
			}
			if (layerView.LayerDefaultView == null)
			{
				return true;
			}
			bool flag = true;
			LayerDefaultView layerDefaultView = layerView.LayerDefaultView;
			double num = (Settings.Default.UnitsInInches ? 0.393700787401575 : 10);
			double num1 = (Settings.Default.UnitsInInches ? 47.244094488189 : 1200);
			double num2 = (Settings.Default.UnitsInInches ? 0.00393700787401575 : 0.1);
			double num3 = (Settings.Default.UnitsInInches ? 0.0393700787401575 : 1);
			bool flag1 = true;
			if (layerDefaultView.Width + 1E-07 < num)
			{
				flag1 = false;
				flag = false;
			}
			if (layerDefaultView.Width - 1E-07 > num1)
			{
				flag1 = false;
				flag = false;
			}
			layerDefaultView.Width_Valid = flag1;
			bool flag2 = true;
			if (this.HeightRefType == EHeightRefType.MM)
			{
				double materialThickness = layerDefaultView.MaterialThickness;
				if (materialThickness < num2)
				{
					materialThickness = num2;
				}
				double num6 = 1 * materialThickness;
				double num7 = 99999 * materialThickness;
				if (layerDefaultView.Height + 1E-07 < num6)
				{
					flag2 = false;
					flag = false;
				}
				if (layerDefaultView.Height - 1E-07 > num7)
				{
					flag2 = false;
					flag = false;
				}
			}
			if (this.HeightRefType == EHeightRefType.Number)
			{
				if (layerDefaultView.Height + 1E-07 < 1)
				{
					flag2 = false;
					flag = false;
				}
				if (layerDefaultView.Height - 1E-07 > 99999)
				{
					flag2 = false;
					flag = false;
				}
				double num12 = Math.Round(layerDefaultView.Height);
				if (Math.Abs(layerDefaultView.Height - num12) > 1E-07)
				{
					flag2 = false;
					flag = false;
				}
			}
			bool flag3 = true;
			if (layerDefaultView.MaterialThickness + 1E-07 < num2)
			{
				flag3 = false;
				flag = false;
			}
			if (layerDefaultView.MaterialThickness - 1E-07 > num3)
			{
				flag3 = false;
				flag = false;
			}
			if (this.HeightRefType == EHeightRefType.Number)
			{
				if (!flag3)
				{
					flag2 = false;
					flag = false;
				}
				if (!flag2 && layerDefaultView.Height - 1E-07 > 1)
				{
					flag3 = false;
					flag = false;
				}
			}
			layerDefaultView.Height_Valid = flag2;
			layerDefaultView.MaterialThickness_Valid = flag3;
			return flag;
		}

		public bool ValidateLayerViews()
		{
			if (this.LayerViews == null)
			{
				return true;
			}
			bool flag = true;
			foreach (LayerView layerView in this.LayerViews)
			{
				if (this.ValidateLayerDefaultView(layerView))
				{
					continue;
				}
				flag = false;
			}
			return flag;
		}

		public bool ValidateSlotsDefaultView(LayerView layerView)
		{
			if (layerView == null)
			{
				return true;
			}
			if (layerView.SlotsDefaultView == null)
			{
				return true;
			}
			bool flag = true;
			SlotsDefaultView slotsDefaultView = layerView.SlotsDefaultView;
			int maxNumberSlotsShapes = this.GetMaxNumberSlotsShapes(layerView);
			bool flag1 = true;
			if (maxNumberSlotsShapes > 1)
			{
				double width = layerView.LayerDefaultView.Width / (double)(maxNumberSlotsShapes - 1);
				if (slotsDefaultView.DistanceY + 1E-07 < this.minSlotDistanceY)
				{
					flag1 = false;
					flag = false;
				}
				if (slotsDefaultView.DistanceY - 1E-07 > width)
				{
					flag1 = false;
					flag = false;
				}
			}
			slotsDefaultView.DistanceY_Valid = flag1;
			return flag;
		}

		public bool ValidateSlotsViews()
		{
			bool flag;
			if (this.LayerViews == null)
			{
				return true;
			}
			bool flag1 = true;
			double num = (Settings.Default.UnitsInInches ? 0 : 0);
			double num1 = (Settings.Default.UnitsInInches ? 0.0001 : 0.01);
			List<LayerView>.Enumerator enumerator = this.LayerViews.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LayerView current = enumerator.Current;
					if (current.ShapeViews != null)
					{
						using (IEnumerator<ShapeView> enumerator1 = current.ShapeViews.GetEnumerator())
						{
							while (enumerator1.MoveNext())
							{
								ShapeView shapeView = enumerator1.Current;
								bool flag2 = true;
								bool flag3 = true;
								double shapeViewLength = this.GetShapeViewLength(shapeView);
								SlotsView slotsView = shapeView.SlotsView;
								if (slotsView.NumberOfSlots >= 1)
								{
									if (slotsView.MeasuringType == EMeasuringType.Absolute)
									{
										if (slotsView.Length1 + 1E-07 < num)
										{
											flag2 = false;
										}
										if (slotsView.Length1 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
										}
										if (slotsView.Length2 + 1E-07 < slotsView.Length1 + num1)
										{
											flag2 = false;
											flag3 = false;
										}
										if (slotsView.Length2 - 1E-07 > shapeViewLength)
										{
											flag3 = false;
										}
									}
									if (slotsView.MeasuringType == EMeasuringType.Relative)
									{
										if (slotsView.Length1 + 1E-07 < num)
										{
											flag2 = false;
										}
										if (slotsView.Length1 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
										}
										if (slotsView.Length2 + 1E-07 < num)
										{
											flag3 = false;
										}
										if (slotsView.Length2 - 1E-07 > shapeViewLength - slotsView.Length1)
										{
											flag3 = false;
										}
									}
									if (slotsView.MeasuringType == EMeasuringType.Simetric)
									{
										if (slotsView.Length1 + 1E-07 < num1)
										{
											flag2 = false;
										}
										if (slotsView.Length1 - 1E-07 > shapeViewLength)
										{
											flag2 = false;
										}
									}
								}
								slotsView.Length1_Valid = flag2;
								slotsView.Length2_Valid = flag3;
								if (!flag2)
								{
									flag1 = false;
								}
								if (flag3)
								{
									continue;
								}
								flag1 = false;
							}
						}
						if (this.ValidateSlotsDefaultView(current))
						{
							continue;
						}
						flag1 = false;
					}
					else
					{
						flag = true;
						return flag;
					}
				}
				return flag1;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		public bool ValidateStepLapsDefaultView(LayerView layerView)
		{
			if (layerView == null)
			{
				return true;
			}
			if (layerView.StepLapsDefaultView == null)
			{
				return true;
			}
			bool flag = true;
			StepLapsDefaultView stepLapsDefaultView = layerView.StepLapsDefaultView;
			bool flag1 = true;
			if (stepLapsDefaultView.NumberOfSteps < 1)
			{
				flag1 = false;
				flag = false;
			}
			if (stepLapsDefaultView.NumberOfSteps > 10)
			{
				flag1 = false;
				flag = false;
			}
			stepLapsDefaultView.NumberOfSteps_Valid = flag1;
			bool flag2 = true;
			if (stepLapsDefaultView.NumberOfSame < 1)
			{
				flag2 = false;
				flag = false;
			}
			if (stepLapsDefaultView.NumberOfSame > 10)
			{
				flag2 = false;
				flag = false;
			}
			stepLapsDefaultView.NumberOfSame_Valid = flag2;
			bool flag3 = true;
			double num4 = (Settings.Default.UnitsInInches ? 0 : 0);
			double num5 = (Settings.Default.UnitsInInches ? 0.78740157480315 : 20);
			if (stepLapsDefaultView.Value + 1E-07 < num4)
			{
				flag3 = false;
				flag = false;
			}
			if (stepLapsDefaultView.Value - 1E-07 > num5)
			{
				flag3 = false;
				flag = false;
			}
			stepLapsDefaultView.Value_Valid = flag3;
			return flag;
		}

		public bool ValidateStepLapsViews()
		{
			if (this.LayerViews == null)
			{
				return true;
			}
			bool flag = true;
			foreach (LayerView layerView in this.LayerViews)
			{
				if (this.ValidateStepLapsDefaultView(layerView))
				{
					continue;
				}
				flag = false;
			}
			return flag;
		}

		public bool ValidateTipCutsDefaultView(LayerView layerView)
		{
			if (layerView == null)
			{
				return true;
			}
			if (layerView.TipCutsDefaultView == null)
			{
				return true;
			}
			TipCutsDefaultView tipCutsDefaultView = layerView.TipCutsDefaultView;
			bool flag = true;
			bool flag1 = true;
			double num = (Settings.Default.UnitsInInches ? 0 : 0);
			double width = layerView.LayerDefaultView.Width / 2;
			if (tipCutsDefaultView.Height < num)
			{
				flag1 = false;
				flag = false;
			}
			if (tipCutsDefaultView.Height > width)
			{
				flag1 = false;
				flag = false;
			}
			tipCutsDefaultView.Height_Valid = flag1;
			bool flag2 = true;
			double num3 = (Settings.Default.UnitsInInches ? 0 : 0);
			double num4 = (Settings.Default.UnitsInInches ? 0.393700787401575 : 10);
			if (tipCutsDefaultView.OverCut < num3)
			{
				flag2 = false;
				flag = false;
			}
			if (tipCutsDefaultView.OverCut > num4)
			{
				flag2 = false;
				flag = false;
			}
			tipCutsDefaultView.OverCut_Valid = flag2;
			return flag;
		}

		public bool ValidateTipCutsViews()
		{
			if (this.LayerViews == null)
			{
				return true;
			}
			bool flag = true;
			foreach (LayerView layerView in this.LayerViews)
			{
				if (this.ValidateTipCutsDefaultView(layerView))
				{
					continue;
				}
				flag = false;
			}
			return flag;
		}
	}
}