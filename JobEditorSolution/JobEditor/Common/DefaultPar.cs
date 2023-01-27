using JobEditor.ViewModels;
using JobEditor.Views.ProductData;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Serialization;

namespace JobEditor.Common
{
	public class DefaultPar : ValidationViewBase, INotifyPropertyChanged
	{
		private int stepLapsNumberOfSteps_1;

		private int stepLapsNumberOfSteps_2;

		private int stepLapsNumberOfSteps_3;

		private int stepLapsNumberOfSame_1;

		private int stepLapsNumberOfSame_2;

		private int stepLapsNumberOfSame_3;

		private double stepLapsValue_1;

		private double stepLapsValue_2;

		private double stepLapsValue_3;

		private double tipCutsHeight_1;

		private double tipCutsHeight_2;

		private double tipCutsHeight_3;

		private double tipCutsOverCut_1;

		private double tipCutsOverCut_2;

		private double tipCutsOverCut_3;

		private double centersOverCut_1;

		private double centersOverCut_2;

		private double centersOverCut_3;

		private double materialThickness_1;

		private double materialThickness_2;

		private double materialThickness_3;

		private EditorViewModel viewModel;

		private bool valid;

		[XmlElement("CentersOverCut_1")]
		public double CentersOverCut_1
		{
			get
			{
				return this.centersOverCut_1;
			}
			set
			{
				this.centersOverCut_1 = value;
				this.RaisePropertyChanged("CentersOverCut_1", true);
			}
		}

		[XmlIgnore]
		public bool CentersOverCut_1_Valid
		{
			get
			{
				return base.GetValidState("CentersOverCut_1");
			}
			set
			{
				base.SetValidState("CentersOverCut_1", value);
			}
		}

		[XmlElement("CentersOverCut_2")]
		public double CentersOverCut_2
		{
			get
			{
				return this.centersOverCut_2;
			}
			set
			{
				this.centersOverCut_2 = value;
				this.RaisePropertyChanged("CentersOverCut_2", true);
			}
		}

		[XmlIgnore]
		public bool CentersOverCut_2_Valid
		{
			get
			{
				return base.GetValidState("CentersOverCut_2");
			}
			set
			{
				base.SetValidState("CentersOverCut_2", value);
			}
		}

		[XmlElement("CentersOverCut_3")]
		public double CentersOverCut_3
		{
			get
			{
				return this.centersOverCut_3;
			}
			set
			{
				this.centersOverCut_3 = value;
				this.RaisePropertyChanged("CentersOverCut_3", true);
			}
		}

		[XmlIgnore]
		public bool CentersOverCut_3_Valid
		{
			get
			{
				return base.GetValidState("CentersOverCut_3");
			}
			set
			{
				base.SetValidState("CentersOverCut_3", value);
			}
		}

		[XmlElement("MaterialThickness_1")]
		public double MaterialThickness_1
		{
			get
			{
				return this.materialThickness_1;
			}
			set
			{
				this.materialThickness_1 = value;
				this.RaisePropertyChanged("MaterialThickness_1", true);
			}
		}

		[XmlIgnore]
		public bool MaterialThickness_1_Valid
		{
			get
			{
				return base.GetValidState("MaterialThickness_1");
			}
			set
			{
				base.SetValidState("MaterialThickness_1", value);
			}
		}

		[XmlElement("MaterialThickness_2")]
		public double MaterialThickness_2
		{
			get
			{
				return this.materialThickness_2;
			}
			set
			{
				this.materialThickness_2 = value;
				this.RaisePropertyChanged("MaterialThickness_2", true);
			}
		}

		[XmlIgnore]
		public bool MaterialThickness_2_Valid
		{
			get
			{
				return base.GetValidState("MaterialThickness_2");
			}
			set
			{
				base.SetValidState("MaterialThickness_2", value);
			}
		}

		[XmlElement("MaterialThickness_3")]
		public double MaterialThickness_3
		{
			get
			{
				return this.materialThickness_3;
			}
			set
			{
				this.materialThickness_3 = value;
				this.RaisePropertyChanged("MaterialThickness_3", true);
			}
		}

		[XmlIgnore]
		public bool MaterialThickness_3_Valid
		{
			get
			{
				return base.GetValidState("MaterialThickness_3");
			}
			set
			{
				base.SetValidState("MaterialThickness_3", value);
			}
		}

		[XmlElement("StepLapsNumberOfSame_1")]
		public int StepLapsNumberOfSame_1
		{
			get
			{
				return this.stepLapsNumberOfSame_1;
			}
			set
			{
				this.stepLapsNumberOfSame_1 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSame_1", true);
			}
		}

		[XmlIgnore]
		public bool StepLapsNumberOfSame_1_Valid
		{
			get
			{
				return base.GetValidState("StepLapsNumberOfSame_1");
			}
			set
			{
				base.SetValidState("StepLapsNumberOfSame_1", value);
			}
		}

		[XmlElement("StepLapsNumberOfSame_2")]
		public int StepLapsNumberOfSame_2
		{
			get
			{
				return this.stepLapsNumberOfSame_2;
			}
			set
			{
				this.stepLapsNumberOfSame_2 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSame_2", true);
			}
		}

		[XmlIgnore]
		public bool StepLapsNumberOfSame_2_Valid
		{
			get
			{
				return base.GetValidState("StepLapsNumberOfSame_2");
			}
			set
			{
				base.SetValidState("StepLapsNumberOfSame_2", value);
			}
		}

		[XmlElement("StepLapsNumberOfSame_3")]
		public int StepLapsNumberOfSame_3
		{
			get
			{
				return this.stepLapsNumberOfSame_3;
			}
			set
			{
				this.stepLapsNumberOfSame_3 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSame_3", true);
			}
		}

		[XmlIgnore]
		public bool StepLapsNumberOfSame_3_Valid
		{
			get
			{
				return base.GetValidState("StepLapsNumberOfSame_3");
			}
			set
			{
				base.SetValidState("StepLapsNumberOfSame_3", value);
			}
		}

		[XmlElement("StepLapsNumberOfSteps_1")]
		public int StepLapsNumberOfSteps_1
		{
			get
			{
				return this.stepLapsNumberOfSteps_1;
			}
			set
			{
				this.stepLapsNumberOfSteps_1 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSteps_1", true);
			}
		}

		[XmlIgnore]
		public bool StepLapsNumberOfSteps_1_Valid
		{
			get
			{
				return base.GetValidState("StepLapsNumberOfSteps_1");
			}
			set
			{
				base.SetValidState("StepLapsNumberOfSteps_1", value);
			}
		}

		[XmlElement("StepLapsNumberOfSteps_2")]
		public int StepLapsNumberOfSteps_2
		{
			get
			{
				return this.stepLapsNumberOfSteps_2;
			}
			set
			{
				this.stepLapsNumberOfSteps_2 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSteps_2", true);
			}
		}

		[XmlIgnore]
		public bool StepLapsNumberOfSteps_2_Valid
		{
			get
			{
				return base.GetValidState("StepLapsNumberOfSteps_2");
			}
			set
			{
				base.SetValidState("StepLapsNumberOfSteps_2", value);
			}
		}

		[XmlElement("StepLapsNumberOfSteps_3")]
		public int StepLapsNumberOfSteps_3
		{
			get
			{
				return this.stepLapsNumberOfSteps_3;
			}
			set
			{
				this.stepLapsNumberOfSteps_3 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSteps_3", true);
			}
		}

		[XmlIgnore]
		public bool StepLapsNumberOfSteps_3_Valid
		{
			get
			{
				return base.GetValidState("StepLapsNumberOfSteps_3");
			}
			set
			{
				base.SetValidState("StepLapsNumberOfSteps_3", value);
			}
		}

		[XmlElement("StepLapsValue_1")]
		public double StepLapsValue_1
		{
			get
			{
				return this.stepLapsValue_1;
			}
			set
			{
				this.stepLapsValue_1 = value;
				this.RaisePropertyChanged("StepLapsValue_1", true);
			}
		}

		[XmlIgnore]
		public bool StepLapsValue_1_Valid
		{
			get
			{
				return base.GetValidState("StepLapsValue_1");
			}
			set
			{
				base.SetValidState("StepLapsValue_1", value);
			}
		}

		[XmlElement("StepLapsValue_2")]
		public double StepLapsValue_2
		{
			get
			{
				return this.stepLapsValue_2;
			}
			set
			{
				this.stepLapsValue_2 = value;
				this.RaisePropertyChanged("StepLapsValue_2", true);
			}
		}

		[XmlIgnore]
		public bool StepLapsValue_2_Valid
		{
			get
			{
				return base.GetValidState("StepLapsValue_2");
			}
			set
			{
				base.SetValidState("StepLapsValue_2", value);
			}
		}

		[XmlElement("StepLapsValue_3")]
		public double StepLapsValue_3
		{
			get
			{
				return this.stepLapsValue_3;
			}
			set
			{
				this.stepLapsValue_3 = value;
				this.RaisePropertyChanged("StepLapsValue_3", true);
			}
		}

		[XmlIgnore]
		public bool StepLapsValue_3_Valid
		{
			get
			{
				return base.GetValidState("StepLapsValue_3");
			}
			set
			{
				base.SetValidState("StepLapsValue_3", value);
			}
		}

		[XmlElement("TipCutsHeight_1")]
		public double TipCutsHeight_1
		{
			get
			{
				return this.tipCutsHeight_1;
			}
			set
			{
				this.tipCutsHeight_1 = value;
				this.RaisePropertyChanged("TipCutsHeight_1", true);
			}
		}

		[XmlIgnore]
		public bool TipCutsHeight_1_Valid
		{
			get
			{
				return base.GetValidState("TipCutsHeight_1");
			}
			set
			{
				base.SetValidState("TipCutsHeight_1", value);
			}
		}

		[XmlElement("TipCutsHeight_2")]
		public double TipCutsHeight_2
		{
			get
			{
				return this.tipCutsHeight_2;
			}
			set
			{
				this.tipCutsHeight_2 = value;
				this.RaisePropertyChanged("TipCutsHeight_2", true);
			}
		}

		[XmlIgnore]
		public bool TipCutsHeight_2_Valid
		{
			get
			{
				return base.GetValidState("TipCutsHeight_2");
			}
			set
			{
				base.SetValidState("TipCutsHeight_2", value);
			}
		}

		[XmlElement("TipCutsHeight_3")]
		public double TipCutsHeight_3
		{
			get
			{
				return this.tipCutsHeight_3;
			}
			set
			{
				this.tipCutsHeight_3 = value;
				this.RaisePropertyChanged("TipCutsHeight_3", true);
			}
		}

		[XmlIgnore]
		public bool TipCutsHeight_3_Valid
		{
			get
			{
				return base.GetValidState("TipCutsHeight_3");
			}
			set
			{
				base.SetValidState("TipCutsHeight_3", value);
			}
		}

		[XmlElement("TipCutsOverCut_1")]
		public double TipCutsOverCut_1
		{
			get
			{
				return this.tipCutsOverCut_1;
			}
			set
			{
				this.tipCutsOverCut_1 = value;
				this.RaisePropertyChanged("TipCutsOverCut_1", true);
			}
		}

		[XmlIgnore]
		public bool TipCutsOverCut_1_Valid
		{
			get
			{
				return base.GetValidState("TipCutsOverCut_1");
			}
			set
			{
				base.SetValidState("TipCutsOverCut_1", value);
			}
		}

		[XmlElement("TipCutsOverCut_2")]
		public double TipCutsOverCut_2
		{
			get
			{
				return this.tipCutsOverCut_2;
			}
			set
			{
				this.tipCutsOverCut_2 = value;
				this.RaisePropertyChanged("TipCutsOverCut_2", true);
			}
		}

		[XmlIgnore]
		public bool TipCutsOverCut_2_Valid
		{
			get
			{
				return base.GetValidState("TipCutsOverCut_2");
			}
			set
			{
				base.SetValidState("TipCutsOverCut_2", value);
			}
		}

		[XmlElement("TipCutsOverCut_3")]
		public double TipCutsOverCut_3
		{
			get
			{
				return this.tipCutsOverCut_3;
			}
			set
			{
				this.tipCutsOverCut_3 = value;
				this.RaisePropertyChanged("TipCutsOverCut_3", true);
			}
		}

		[XmlIgnore]
		public bool TipCutsOverCut_3_Valid
		{
			get
			{
				return base.GetValidState("TipCutsOverCut_3");
			}
			set
			{
				base.SetValidState("TipCutsOverCut_3", value);
			}
		}

		[XmlIgnore]
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

		[XmlIgnore]
		public EditorViewModel ViewModel
		{
			get
			{
				return this.viewModel;
			}
			set
			{
				this.viewModel = value;
			}
		}

		public DefaultPar()
		{
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
			bool flag2 = true;
			bool flag3 = true;
			if (rBPar.StepLapsNumberOfSteps_1)
			{
				bool numberOfStepsValid = selectedLayerView.StepLapsDefaultView.NumberOfSteps_Valid;
				flag1 = numberOfStepsValid;
				flag = numberOfStepsValid;
			}
			if (rBPar.StepLapsNumberOfSteps_2)
			{
				bool numberOfStepsValid1 = selectedLayerView.StepLapsDefaultView.NumberOfSteps_Valid;
				flag2 = numberOfStepsValid1;
				flag = numberOfStepsValid1;
			}
			if (rBPar.StepLapsNumberOfSteps_3)
			{
				bool numberOfStepsValid2 = selectedLayerView.StepLapsDefaultView.NumberOfSteps_Valid;
				flag3 = numberOfStepsValid2;
				flag = numberOfStepsValid2;
			}
			this.StepLapsNumberOfSteps_1_Valid = flag1;
			this.StepLapsNumberOfSteps_2_Valid = flag2;
			this.StepLapsNumberOfSteps_3_Valid = flag3;
			bool flag4 = true;
			bool flag5 = true;
			bool flag6 = true;
			if (rBPar.StepLapsNumberOfSame_1)
			{
				bool numberOfSameValid = selectedLayerView.StepLapsDefaultView.NumberOfSame_Valid;
				flag4 = numberOfSameValid;
				flag = numberOfSameValid;
			}
			if (rBPar.StepLapsNumberOfSame_2)
			{
				bool numberOfSameValid1 = selectedLayerView.StepLapsDefaultView.NumberOfSame_Valid;
				flag5 = numberOfSameValid1;
				flag = numberOfSameValid1;
			}
			if (rBPar.StepLapsNumberOfSame_3)
			{
				bool numberOfSameValid2 = selectedLayerView.StepLapsDefaultView.NumberOfSame_Valid;
				flag6 = numberOfSameValid2;
				flag = numberOfSameValid2;
			}
			this.StepLapsNumberOfSame_1_Valid = flag4;
			this.StepLapsNumberOfSame_2_Valid = flag5;
			this.StepLapsNumberOfSame_3_Valid = flag6;
			bool flag7 = true;
			bool flag8 = true;
			bool flag9 = true;
			if (rBPar.StepLapsValue_1)
			{
				bool valueValid = selectedLayerView.StepLapsDefaultView.Value_Valid;
				flag7 = valueValid;
				flag = valueValid;
			}
			if (rBPar.StepLapsValue_2)
			{
				bool valueValid1 = selectedLayerView.StepLapsDefaultView.Value_Valid;
				flag8 = valueValid1;
				flag = valueValid1;
			}
			if (rBPar.StepLapsValue_3)
			{
				bool valueValid2 = selectedLayerView.StepLapsDefaultView.Value_Valid;
				flag9 = valueValid2;
				flag = valueValid2;
			}
			this.StepLapsValue_1_Valid = flag7;
			this.StepLapsValue_2_Valid = flag8;
			this.StepLapsValue_3_Valid = flag9;
			bool flag10 = true;
			bool flag11 = true;
			bool flag12 = true;
			if (rBPar.CentersOverCut_1)
			{
				bool overCutValid = selectedLayerView.CentersDefaultView.OverCut_Valid;
				flag10 = overCutValid;
				flag = overCutValid;
			}
			if (rBPar.CentersOverCut_2)
			{
				bool overCutValid1 = selectedLayerView.CentersDefaultView.OverCut_Valid;
				flag11 = overCutValid1;
				flag = overCutValid1;
			}
			if (rBPar.CentersOverCut_3)
			{
				bool overCutValid2 = selectedLayerView.CentersDefaultView.OverCut_Valid;
				flag12 = overCutValid2;
				flag = overCutValid2;
			}
			this.CentersOverCut_1_Valid = flag10;
			this.CentersOverCut_2_Valid = flag11;
			this.CentersOverCut_3_Valid = flag12;
			bool flag13 = true;
			bool flag14 = true;
			bool flag15 = true;
			if (rBPar.TipCutsHeight_1)
			{
				bool heightValid = selectedLayerView.TipCutsDefaultView.Height_Valid;
				flag13 = heightValid;
				flag = heightValid;
			}
			if (rBPar.TipCutsHeight_2)
			{
				bool heightValid1 = selectedLayerView.TipCutsDefaultView.Height_Valid;
				flag14 = heightValid1;
				flag = heightValid1;
			}
			if (rBPar.TipCutsHeight_3)
			{
				bool heightValid2 = selectedLayerView.TipCutsDefaultView.Height_Valid;
				flag15 = heightValid2;
				flag = heightValid2;
			}
			this.TipCutsHeight_1_Valid = flag13;
			this.TipCutsHeight_2_Valid = flag14;
			this.TipCutsHeight_3_Valid = flag15;
			bool flag16 = true;
			bool flag17 = true;
			bool flag18 = true;
			if (rBPar.TipCutsOverCut_1)
			{
				bool overCutValid3 = selectedLayerView.TipCutsDefaultView.OverCut_Valid;
				flag16 = overCutValid3;
				flag = overCutValid3;
			}
			if (rBPar.TipCutsOverCut_2)
			{
				bool overCutValid4 = selectedLayerView.TipCutsDefaultView.OverCut_Valid;
				flag17 = overCutValid4;
				flag = overCutValid4;
			}
			if (rBPar.TipCutsOverCut_3)
			{
				bool overCutValid5 = selectedLayerView.TipCutsDefaultView.OverCut_Valid;
				flag18 = overCutValid5;
				flag = overCutValid5;
			}
			this.TipCutsOverCut_1_Valid = flag16;
			this.TipCutsOverCut_2_Valid = flag17;
			this.TipCutsOverCut_3_Valid = flag18;
			bool flag19 = true;
			bool flag20 = true;
			bool flag21 = true;
			if (rBPar.LayersMaterialThickness_1)
			{
				bool materialThicknessValid = selectedLayerView.LayerDefaultView.MaterialThickness_Valid;
				flag19 = materialThicknessValid;
				flag = materialThicknessValid;
			}
			if (rBPar.LayersMaterialThickness_2)
			{
				bool materialThicknessValid1 = selectedLayerView.LayerDefaultView.MaterialThickness_Valid;
				flag20 = materialThicknessValid1;
				flag = materialThicknessValid1;
			}
			if (rBPar.LayersMaterialThickness_3)
			{
				bool materialThicknessValid2 = selectedLayerView.LayerDefaultView.MaterialThickness_Valid;
				flag21 = materialThicknessValid2;
				flag = materialThicknessValid2;
			}
			this.MaterialThickness_1_Valid = flag19;
			this.MaterialThickness_2_Valid = flag20;
			this.MaterialThickness_3_Valid = flag21;
			this.Valid = flag;
			return flag;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}