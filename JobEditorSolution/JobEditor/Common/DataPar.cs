using JobEditor.ViewModels;
using JobEditor.Views.ProductData;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace JobEditor.Common
{
	public class DataPar : ValidationViewBase, INotifyPropertyChanged
	{
		public const int minNumberOfLayers = 1;

		public const int maxNumberOfLayers = 30;

		private double stepLapStepValue;

		private int stepLapNumberSame;

		private int stepLapNumberSteps;

		private double tipCutHeight;

		private double tipCutOverCut;

		private double centersOverCut;

		private int numberLayers = 1;

		private double materialThickness;

		private EditorViewModel viewModel;

		private bool valid;

		public double CentersOverCut
		{
			get
			{
				return this.centersOverCut;
			}
			set
			{
				this.centersOverCut = value;
				this.RaisePropertyChanged("CentersOverCut", true);
			}
		}

		public bool CentersOverCut_Valid
		{
			get
			{
				return base.GetValidState("CentersOverCut");
			}
			set
			{
				base.SetValidState("CentersOverCut", value);
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
				this.RaisePropertyChanged("MaterialThickness", true);
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

		public int NumberLayers
		{
			get
			{
				return this.numberLayers;
			}
			set
			{
				if (this.viewModel != null)
				{
					this.numberLayers = value;
					if (value < 1)
					{
						this.numberLayers = 1;
					}
					if (value > 30)
					{
						this.numberLayers = 30;
					}
					this.RaisePropertyChanged("NumberLayers", true);
					this.viewModel.NumberOfLayersPar(this.numberLayers);
				}
			}
		}

		public int StepLapNumberSame
		{
			get
			{
				return this.stepLapNumberSame;
			}
			set
			{
				this.stepLapNumberSame = value;
				this.RaisePropertyChanged("StepLapNumberSame", true);
			}
		}

		public bool StepLapNumberSame_Valid
		{
			get
			{
				return base.GetValidState("StepLapNumberSame");
			}
			set
			{
				base.SetValidState("StepLapNumberSame", value);
			}
		}

		public int StepLapNumberSteps
		{
			get
			{
				return this.stepLapNumberSteps;
			}
			set
			{
				this.stepLapNumberSteps = value;
				this.RaisePropertyChanged("StepLapNumberSteps", true);
			}
		}

		public bool StepLapNumberSteps_Valid
		{
			get
			{
				return base.GetValidState("StepLapNumberSteps");
			}
			set
			{
				base.SetValidState("StepLapNumberSteps", value);
			}
		}

		public double StepLapStepValue
		{
			get
			{
				return this.stepLapStepValue;
			}
			set
			{
				this.stepLapStepValue = value;
				this.RaisePropertyChanged("StepLapStepValue", true);
			}
		}

		public bool StepLapStepValue_Valid
		{
			get
			{
				return base.GetValidState("StepLapStepValue");
			}
			set
			{
				base.SetValidState("StepLapStepValue", value);
			}
		}

		public double TipCutHeight
		{
			get
			{
				return this.tipCutHeight;
			}
			set
			{
				this.tipCutHeight = value;
				this.RaisePropertyChanged("TipCutHeight", true);
			}
		}

		public double TipCutOverCut
		{
			get
			{
				return this.tipCutOverCut;
			}
			set
			{
				this.tipCutOverCut = value;
				this.RaisePropertyChanged("TipCutOverCut", true);
			}
		}

		public bool TipCutsHeight_Valid
		{
			get
			{
				return base.GetValidState("TipCutHeight");
			}
			set
			{
				base.SetValidState("TipCutHeight", value);
			}
		}

		public bool TipCutsOverCut_Valid
		{
			get
			{
				return base.GetValidState("TipCutOverCut");
			}
			set
			{
				base.SetValidState("TipCutOverCut", value);
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
				this.RaisePropertyChanged("Valid", false);
			}
		}

		public DataPar(EditorViewModel viewModel)
		{
			this.viewModel = viewModel;
		}

		public void Clear()
		{
			this.StepLapNumberSteps = 0;
			this.StepLapNumberSame = 0;
			this.StepLapStepValue = 0;
			this.CentersOverCut = 0;
			this.NumberLayers = 0;
		}

		public void OnPropertyChanged(string propertyName)
		{
			this.Validate();
		}

		private void RaisePropertyChanged(string propertyName, bool callOnPropertyChanged = true)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
			if (callOnPropertyChanged)
			{
				this.OnPropertyChanged(propertyName);
			}
		}

		public bool Validate()
		{
			if (this.viewModel == null || this.viewModel.RBPar == null)
			{
				return false;
			}
			RadioButtonsPar rBPar = this.viewModel.RBPar;
			LayerView selectedLayerView = this.viewModel.ProductView.SelectedLayerView;
			if (selectedLayerView == null)
			{
				return true;
			}
			bool flag = true;
			bool flag1 = true;
			if (rBPar.StepLapsNumberOfSteps_4)
			{
				bool numberOfStepsValid = selectedLayerView.StepLapsDefaultView.NumberOfSteps_Valid;
				flag1 = numberOfStepsValid;
				flag = numberOfStepsValid;
			}
			this.StepLapNumberSteps_Valid = flag1;
			bool flag2 = true;
			if (rBPar.StepLapsNumberOfSame_4)
			{
				bool numberOfSameValid = selectedLayerView.StepLapsDefaultView.NumberOfSame_Valid;
				flag2 = numberOfSameValid;
				flag = numberOfSameValid;
			}
			this.StepLapNumberSame_Valid = flag2;
			bool flag3 = true;
			if (rBPar.StepLapsValue_4)
			{
				bool valueValid = selectedLayerView.StepLapsDefaultView.Value_Valid;
				flag3 = valueValid;
				flag = valueValid;
			}
			this.StepLapStepValue_Valid = flag3;
			bool flag4 = true;
			if (rBPar.CentersOverCut_4)
			{
				bool overCutValid = selectedLayerView.CentersDefaultView.OverCut_Valid;
				flag4 = overCutValid;
				flag = overCutValid;
			}
			this.CentersOverCut_Valid = flag4;
			bool flag5 = true;
			if (rBPar.TipCutsHeight_4)
			{
				bool heightValid = selectedLayerView.TipCutsDefaultView.Height_Valid;
				flag5 = heightValid;
				flag = heightValid;
			}
			this.TipCutsHeight_Valid = flag5;
			bool flag6 = true;
			if (rBPar.TipCutsOverCut_4)
			{
				bool overCutValid1 = selectedLayerView.TipCutsDefaultView.OverCut_Valid;
				flag6 = overCutValid1;
				flag = overCutValid1;
			}
			this.TipCutsOverCut_Valid = flag6;
			bool flag7 = true;
			if (rBPar.LayersMaterialThickness_4)
			{
				bool materialThicknessValid = selectedLayerView.LayerDefaultView.MaterialThickness_Valid;
				flag7 = materialThicknessValid;
				flag = materialThicknessValid;
			}
			this.MaterialThickness_Valid = flag7;
			this.Valid = flag;
			return flag;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}