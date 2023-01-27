using JobEditor.ViewModels;
using JobEditor.Properties;
using JobEditor.Views.ProductData;
using ProductLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace JobEditor.Import
{
	public class VTC_Import : IImport
	{
		public const double eps = 1E-07;

		public const double inch_mm = 25.4;

		private const int numberLinesLayer = 152;

		private const int lineYokeLegCenterLength = 4;

		private const int lineOutsideLegCenterLength = 5;

		private const int lineYokeLegHole1Distance = 6;

		private const int lineYokeLegHole2Distance = 7;

		private const int lineOutsideLegHole1Distance = 10;

		private const int lineOutsideLegHole2Distance = 11;

		private const int lineYokeOutsideLegSteplapType = 46;

		private const int lineYokeOutsideLegDisplacementUom = 72;

		private const int lineYokeOutsideLegCenterLegDisplacement = 73;

		private const int lineCenterLegCenterLength = 80;

		private const int lineCenterLegHole1Distance = 82;

		private const int lineCenterLegHole2Distance = 83;

		private const int lineCenterLegWidth = 90;

		private const int lineCenterLegNumberSheets = 100;

		private const int lineCenterLegSteplapType = 122;

		private const int lineCenterLegStepsPerBook = 145;

		private const int lineCenterLegStepIncrement = 146;

		private const int lineCenterLegSheetsPerStep = 147;

		private const int lineCenterLegDisplacementUom = 148;

		private const int lineCenterLegCenterLegDisplacement = 149;

		private const int lineCenterLegSteelGrade = 152;

		private const string cutSequenceName = "2004-004-2004-004-006";

		private List<string> fileLines = new List<string>();

		private EditorViewModel editorViewModel;

		private double defaultMaterialThickness;

		private string importFullFilename = "";

		private VTC_Config config;

		private int NumberLayers
		{
			get
			{
				return this.NumberLines / 152;
			}
		}

		private int NumberLines
		{
			get
			{
				int count = 0;
				if (this.fileLines != null)
				{
					count = this.fileLines.Count;
				}
				return count;
			}
		}

		public string ProductName
		{
			get
			{
				return JustDecompileGenerated_get_ProductName();
			}
			set
			{
				JustDecompileGenerated_set_ProductName(value);
			}
		}

		private string JustDecompileGenerated_ProductName_k__BackingField;

		public string JustDecompileGenerated_get_ProductName()
		{
			return this.JustDecompileGenerated_ProductName_k__BackingField;
		}

		private void JustDecompileGenerated_set_ProductName(string value)
		{
			this.JustDecompileGenerated_ProductName_k__BackingField = value;
		}

		public VTC_Import(string importFullFilename, EditorViewModel editorViewModel, double defaultMaterialThickness, VTC_Config config)
		{
			this.importFullFilename = importFullFilename;
			this.editorViewModel = editorViewModel;
			this.defaultMaterialThickness = defaultMaterialThickness;
			this.config = config;
			this.ProductName = Path.GetFileNameWithoutExtension(importFullFilename);
		}

		public bool Convert(ProductView productView)
		{
			if (productView == null)
			{
				return false;
			}
			productView.EnableOnSelectedLayerViewChanged(false);
			productView.EnableOnPropertyChanged = false;
			productView.Clear();
			productView.HeightMeasType = 0;
			productView.HeightRefType = EHeightRefType.Number;
			bool flag = true;
			List<int> list = (
				from index in Enumerable.Range(0, this.NumberLayers)
				orderby this.GetIntValue(index, 90)
				select index).ToList<int>();
			for (int i = 0; i < list.Count; i++)
			{
				int item = list[i];
				LayerView layerView = this.editorViewModel.CreateNewLayer("2004-004-2004-004-006", item);
				if (!this.FillInLayerShapes(layerView))
				{
					flag = false;
				}
				if (!this.FillInLayerDefaults(layerView))
				{
					flag = false;
				}
				if (!this.FillInCentersDefaults(layerView))
				{
					flag = false;
				}
				if (!this.FillInStepLapsDefaults(layerView))
				{
					flag = false;
				}
				productView.LayerViews.Add(layerView);
			}
			int count = productView.LayerViews.Count;
			int num = count - 1;
			for (int j = 0; j < num; j++)
			{
				int num1 = num - 1 - j;
				LayerView item1 = productView.LayerViews[num1];
				LayerView layerView1 = new LayerView(productView, null);
				layerView1.CloneDataFrom(item1);
				layerView1.Id = count + j;
				LayerDefaultView layerDefaultView = item1.LayerDefaultView;
				layerDefaultView.Height = layerDefaultView.Height / 2;
				LayerDefaultView height = layerView1.LayerDefaultView;
				height.Height = height.Height - item1.LayerDefaultView.Height;
				productView.LayerViews.Add(layerView1);
			}
			productView.EnableOnPropertyChanged = true;
			productView.Validate();
			if (productView.LayerViews.Count <= 0)
			{
				productView.ActLayerNum = 0;
			}
			else
			{
				productView.ActLayerNum = 1;
			}
			productView.EnableOnSelectedLayerViewChanged(true);
			productView.CallOnSelectedLayerViewChanged();
			return flag;
		}

		private bool FillInCentersDefaults(LayerView layerView)
		{
			if (layerView == null)
			{
				return false;
			}
			if (layerView.CentersDefaultView == null)
			{
				return false;
			}
			int id = layerView.Id;
			double overCut = 0;
			if (this.config != null)
			{
				overCut = this.config.OverCut;
			}
			layerView.CentersDefaultView.OverCut = overCut;
			return true;
		}

		private bool FillInLayerDefaults(LayerView layerView)
		{
			if (layerView == null)
			{
				return false;
			}
			if (layerView.LayerDefaultView == null)
			{
				return false;
			}
			int id = layerView.Id;
			double convertDoubleFactor = this.GetConvertDoubleFactor(id, 148);
			layerView.LayerDefaultView.Width = this.GetDoubleValue(id, 90, convertDoubleFactor);
			layerView.LayerDefaultView.Height = (double)this.GetIntValue(id, 100);
			EHeightCorrectionType eHeightCorrectionType = 0;
			if (this.config != null && this.config.EnableHeightMeasurement)
			{
				eHeightCorrectionType = EHeightCorrectionType.CompleteCycleUp;
			}
			layerView.LayerDefaultView.HeightCorrectionType = eHeightCorrectionType;
			double thickness = this.defaultMaterialThickness;
			string name = "";
			if (this.config != null)
			{
				int intValue = this.GetIntValue(id, 152);
				foreach (VTC_ConfigSteelGrade steelGrade in this.config.SteelGrades)
				{
					if (steelGrade.Value != intValue)
					{
						continue;
					}
					thickness = steelGrade.Thickness;
					name = steelGrade.Name;
					layerView.LayerDefaultView.MaterialThickness = thickness;
					layerView.Info = name;
					return true;
				}
			}
			layerView.LayerDefaultView.MaterialThickness = thickness;
			layerView.Info = name;
			return true;
		}

		private bool FillInLayerShapeCenterLeg(LayerView layerView, int shapeIndex)
		{
			if (layerView == null)
			{
				return false;
			}
			if (layerView.ShapeViews.Count <= shapeIndex)
			{
				return false;
			}
			ShapeView item = layerView.ShapeViews[shapeIndex];
			if (item == null)
			{
				return false;
			}
			bool flag = true;
			int id = layerView.Id;
			double convertDoubleFactor = this.GetConvertDoubleFactor(id, 148);
			if (item.CentersView == null)
			{
				flag = false;
			}
			else
			{
				double doubleValue = this.GetDoubleValue(id, 149, convertDoubleFactor);
				item.CentersView.Length1 = this.GetDoubleValue(id, 80, convertDoubleFactor);
				item.CentersView.Length3 = doubleValue;
				item.CentersView.Length4 = doubleValue;
				item.CentersView.MeasuringType = 0;
			}
			if (item.HolesView == null)
			{
				flag = false;
			}
			else
			{
				double num = this.GetDoubleValue(id, 82, convertDoubleFactor);
				double doubleValue1 = this.GetDoubleValue(id, 83, convertDoubleFactor);
				if (Math.Abs(num) < 1E-07)
				{
					num = doubleValue1;
					doubleValue1 = 0;
				}
				int num1 = 2;
				if (Math.Abs(doubleValue1) < 1E-07)
				{
					num1--;
				}
				if (Math.Abs(num) < 1E-07)
				{
					num1--;
				}
				item.HolesView.NumberOfHoles = num1;
				item.HolesView.Length1 = num;
				item.HolesView.Shape1 = 0;
				item.HolesView.Length2 = doubleValue1;
				item.HolesView.Shape2 = 0;
				item.HolesView.MeasuringType = 0;
			}
			StepLapView stepLapView = null;
			if (item.ShapePartViews.Count > 0 && item.ShapePartViews[0] != null)
			{
				stepLapView = item.ShapePartViews[0].StepLapView;
			}
			StepLapView stepLapView1 = null;
			if (item.ShapePartViews.Count > 1 && item.ShapePartViews[1] != null)
			{
				stepLapView1 = item.ShapePartViews[1].StepLapView;
			}
			if (stepLapView == null || stepLapView1 == null)
			{
				flag = false;
			}
			else if (this.GetIntValue(id, 122) == 2)
			{
				stepLapView.Type =  EStepLapType.CTipLeft_Left;
				stepLapView1.Type = EStepLapType.CTipRight_Right;
			}
			return flag;
		}

		private bool FillInLayerShapeOutsideLeg(LayerView layerView, int shapeIndex)
		{
			if (layerView == null)
			{
				return false;
			}
			if (layerView.ShapeViews.Count <= shapeIndex)
			{
				return false;
			}
			ShapeView item = layerView.ShapeViews[shapeIndex];
			if (item == null)
			{
				return false;
			}
			bool flag = true;
			int id = layerView.Id;
			double convertDoubleFactor = this.GetConvertDoubleFactor(id, 72);
			if (item.CentersView == null)
			{
				flag = false;
			}
			else
			{
				item.CentersView.Length1 = this.GetDoubleValue(id, 5, convertDoubleFactor);
				item.CentersView.MeasuringType = 0;
			}
			if (item.HolesView == null)
			{
				flag = false;
			}
			else
			{
				double doubleValue = this.GetDoubleValue(id, 10, convertDoubleFactor);
				double num = this.GetDoubleValue(id, 11, convertDoubleFactor);
				if (Math.Abs(doubleValue) < 1E-07)
				{
					doubleValue = num;
					num = 0;
				}
				int num1 = 2;
				if (Math.Abs(num) < 1E-07)
				{
					num1--;
				}
				if (Math.Abs(doubleValue) < 1E-07)
				{
					num1--;
				}
				item.HolesView.NumberOfHoles = num1;
				item.HolesView.Length1 = doubleValue;
				item.HolesView.Shape1 = 0;
				item.HolesView.Length2 = num;
				item.HolesView.Shape2 = 0;
				item.HolesView.MeasuringType = 0;
			}
			StepLapView stepLapView = null;
			if (item.ShapePartViews.Count > 0 && item.ShapePartViews[0] != null)
			{
				stepLapView = item.ShapePartViews[0].StepLapView;
			}
			StepLapView stepLapView1 = null;
			if (item.ShapePartViews.Count > 1 && item.ShapePartViews[1] != null)
			{
				stepLapView1 = item.ShapePartViews[1].StepLapView;
			}
			if (stepLapView == null || stepLapView1 == null)
			{
				flag = false;
			}
			else if (this.GetIntValue(id, 46) == 2)
			{
				stepLapView.Type = EStepLapType.Cut135Left_Left;
				stepLapView1.Type = EStepLapType.Cut45Right_Right;
			}
			return flag;
		}

		private bool FillInLayerShapes(LayerView layerView)
		{
			bool flag = true;
			if (!this.FillInLayerShapeYoke(layerView, 0))
			{
				flag = false;
			}
			if (!this.FillInLayerShapeOutsideLeg(layerView, 1))
			{
				flag = false;
			}
			if (!this.FillInLayerShapeYoke(layerView, 2))
			{
				flag = false;
			}
			if (!this.FillInLayerShapeOutsideLeg(layerView, 3))
			{
				flag = false;
			}
			if (!this.FillInLayerShapeCenterLeg(layerView, 4))
			{
				flag = false;
			}
			return flag;
		}

		private bool FillInLayerShapeYoke(LayerView layerView, int shapeIndex)
		{
			if (layerView == null)
			{
				return false;
			}
			if (layerView.ShapeViews.Count <= shapeIndex)
			{
				return false;
			}
			ShapeView item = layerView.ShapeViews[shapeIndex];
			if (item == null)
			{
				return false;
			}
			bool flag = true;
			int id = layerView.Id;
			double convertDoubleFactor = this.GetConvertDoubleFactor(id, 72);
			if (item.CentersView == null)
			{
				flag = false;
			}
			else
			{
				double doubleValue = this.GetDoubleValue(id, 73, convertDoubleFactor);
				double num = this.GetDoubleValue(id, 4, convertDoubleFactor);
				item.CentersView.Length1 = num / 2 + doubleValue;
				item.CentersView.Length2 = num;
				item.CentersView.MeasuringType = 0;
			}
			if (item.HolesView == null)
			{
				flag = false;
			}
			else
			{
				double doubleValue1 = this.GetDoubleValue(id, 6, convertDoubleFactor);
				double num1 = this.GetDoubleValue(id, 7, convertDoubleFactor);
				if (Math.Abs(doubleValue1) < 1E-07)
				{
					doubleValue1 = num1;
					num1 = 0;
				}
				int num2 = 2;
				if (Math.Abs(num1) < 1E-07)
				{
					num2--;
				}
				if (Math.Abs(doubleValue1) < 1E-07)
				{
					num2--;
				}
				item.HolesView.NumberOfHoles = num2;
				item.HolesView.Length1 = doubleValue1;
				item.HolesView.Shape1 = 0;
				item.HolesView.Length2 = num1;
				item.HolesView.Shape2 = 0;
				item.HolesView.MeasuringType = 0;
			}
			StepLapView stepLapView = null;
			if (item.ShapePartViews.Count > 0 && item.ShapePartViews[0] != null)
			{
				stepLapView = item.ShapePartViews[0].StepLapView;
			}
			StepLapView stepLapView1 = null;
			if (item.ShapePartViews.Count > 1 && item.ShapePartViews[1] != null)
			{
				stepLapView1 = item.ShapePartViews[1].StepLapView;
			}
			StepLapView stepLapView2 = null;
			if (item.ShapePartViews.Count > 2 && item.ShapePartViews[2] != null)
			{
				stepLapView2 = item.ShapePartViews[2].StepLapView;
			}
			if (stepLapView == null || stepLapView1 == null || stepLapView2 == null)
			{
				flag = false;
			}
			else if (this.GetIntValue(id, 46) == 2)
			{
				stepLapView.Type = EStepLapType.Cut45Left_Right;
				stepLapView1.Type = EStepLapType.VTop_Down;
				stepLapView2.Type = EStepLapType.Cut135Right_Left;
			}
			return flag;
		}

		private bool FillInStepLapsDefaults(LayerView layerView)
		{
			if (layerView == null)
			{
				return false;
			}
			if (layerView.StepLapsDefaultView == null)
			{
				return false;
			}
			int id = layerView.Id;
			double convertDoubleFactor = this.GetConvertDoubleFactor(id, 148);
			switch (this.GetIntValue(id, 122))
			{
				case 0:
				case 1:
				{
					layerView.StepLapsDefaultView.NumberOfSteps = 1;
					layerView.StepLapsDefaultView.NumberOfSame = 1;
					layerView.StepLapsDefaultView.Value = 0;
					break;
				}
				case 2:
				{
					layerView.StepLapsDefaultView.NumberOfSteps = this.GetIntValue(id, 145);
					layerView.StepLapsDefaultView.NumberOfSame = this.GetIntValue(id, 147);
					layerView.StepLapsDefaultView.Value = this.GetDoubleValue(id, 146, convertDoubleFactor);
					break;
				}
			}
			return true;
		}

		private double GetConvertDoubleFactor(int layerIndex, int lineNrUnitsOfMeasurement)
		{
			double num = 1;
			if (this.GetIntValue(layerIndex, lineNrUnitsOfMeasurement) == 1)
			{
				num = (!Settings.Default.UnitsInInches ? 0.0254 : 0.001);
			}
			else if (Settings.Default.UnitsInInches)
			{
				num = 0.0393700787401575;
			}
			return num;
		}

		private double GetDoubleValue(int layerIndex, int lineNr, double convertDoubleFactor = 1)
		{
			double num = 0;
			string stringValue = this.GetStringValue(layerIndex, lineNr);
			if (!string.IsNullOrWhiteSpace(stringValue))
			{
				num = double.Parse(stringValue) * convertDoubleFactor;
			}
			return num;
		}

		private int GetIntValue(int layerIndex, int lineNr)
		{
			int num = 0;
			string stringValue = this.GetStringValue(layerIndex, lineNr);
			if (!string.IsNullOrWhiteSpace(stringValue))
			{
				num = int.Parse(stringValue);
			}
			return num;
		}

		private string GetStringValue(int layerIndex, int lineNr)
		{
			string item = null;
			int num = layerIndex * 152 + lineNr - 1;
			if (num >= 0 && num < this.NumberLines)
			{
				item = this.fileLines[num];
			}
			return item;
		}

		public bool ReadFile()
		{
			bool flag;
			this.fileLines.Clear();
			try
			{
				StreamReader streamReader = new StreamReader(this.importFullFilename);
				while (true)
				{
					string str = streamReader.ReadLine();
					string str1 = str;
					if (str == null)
					{
						break;
					}
					if (!string.IsNullOrEmpty(str1))
					{
						this.fileLines.Add(str1);
					}
				}
				return true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				MessageBox.Show(string.Concat("Could not read import file!\nException: ", exception.ToString()));
				flag = false;
			}
			return flag;
		}

		private enum EVTCMeasurementUnit
		{
			MilliInch = 1
		}

		private enum EVTCStepLapType
		{
			NotUsed,
			NoStepLap,
			CVLAPplus
		}
	}
}