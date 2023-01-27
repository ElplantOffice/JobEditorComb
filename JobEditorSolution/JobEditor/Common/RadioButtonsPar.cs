using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace JobEditor.Common
{
	public class RadioButtonsPar : INotifyPropertyChanged
	{
		private bool stepLapsNumberOfSteps_1;

		private bool stepLapsNumberOfSteps_2;

		private bool stepLapsNumberOfSteps_3;

		private bool stepLapsNumberOfSteps_4;

		private bool stepLapsNumberOfSame_1;

		private bool stepLapsNumberOfSame_2;

		private bool stepLapsNumberOfSame_3;

		private bool stepLapsNumberOfSame_4;

		private bool stepLapsValue_1;

		private bool stepLapsValue_2;

		private bool stepLapsValue_3;

		private bool stepLapsValue_4;

		private bool tipCutsHeight_1;

		private bool tipCutsHeight_2;

		private bool tipCutsHeight_3;

		private bool tipCutsHeight_4;

		private bool tipCutsOverCut_1;

		private bool tipCutsOverCut_2;

		private bool tipCutsOverCut_3;

		private bool tipCutsOverCut_4;

		private bool tipCutsDoubleCut_1;

		private bool tipCutsDoubleCut_2;

		private bool slotsYOffset_1;

		private bool slotsYOffset_2;

		private bool slotsYOffset_3;

		private bool slotsYOffset_4;

		private bool centersOverCut_1;

		private bool centersOverCut_2;

		private bool centersOverCut_3;

		private bool centersOverCut_4;

		private bool centersDoubleCut_1;

		private bool centersDoubleCut_2;

		private bool heightRefType_1;

		private bool heightRefType_2;

		private bool heightMeasType_1;

		private bool heightMeasType_2;

		private bool layersMaterialThickness_1;

		private bool layersMaterialThickness_2;

		private bool layersMaterialThickness_3;

		private bool layersMaterialThickness_4;

		private bool heightCorreType_1;

		private bool heightCorreType_2;

		private bool heightCorreType_3;

		private bool heightCorreType_4;

		private bool heightCorreType_5;

		public bool CentersDoubleCut_1
		{
			get
			{
				return this.centersDoubleCut_1;
			}
			set
			{
				this.centersDoubleCut_1 = value;
				this.RaisePropertyChanged("CentersDoubleCut_1");
			}
		}

		public bool CentersDoubleCut_2
		{
			get
			{
				return this.centersDoubleCut_2;
			}
			set
			{
				this.centersDoubleCut_2 = value;
				this.RaisePropertyChanged("CentersDoubleCut_2");
			}
		}

		public bool CentersOverCut_1
		{
			get
			{
				return this.centersOverCut_1;
			}
			set
			{
				this.centersOverCut_1 = value;
				this.RaisePropertyChanged("CentersOverCut_1");
			}
		}

		public bool CentersOverCut_2
		{
			get
			{
				return this.centersOverCut_2;
			}
			set
			{
				this.centersOverCut_2 = value;
				this.RaisePropertyChanged("CentersOverCut_2");
			}
		}

		public bool CentersOverCut_3
		{
			get
			{
				return this.centersOverCut_3;
			}
			set
			{
				this.centersOverCut_3 = value;
				this.RaisePropertyChanged("CentersOverCut_3");
			}
		}

		public bool CentersOverCut_4
		{
			get
			{
				return this.centersOverCut_4;
			}
			set
			{
				this.centersOverCut_4 = value;
				this.RaisePropertyChanged("CentersOverCut_4");
			}
		}

		public bool HeightCorrType_1
		{
			get
			{
				return this.heightCorreType_1;
			}
			set
			{
				this.heightCorreType_1 = value;
				this.RaisePropertyChanged("HeightCorrType_1");
			}
		}

		public bool HeightCorrType_2
		{
			get
			{
				return this.heightCorreType_2;
			}
			set
			{
				this.heightCorreType_2 = value;
				this.RaisePropertyChanged("HeightCorrType_2");
			}
		}

		public bool HeightCorrType_3
		{
			get
			{
				return this.heightCorreType_3;
			}
			set
			{
				this.heightCorreType_3 = value;
				this.RaisePropertyChanged("HeightCorrType_3");
			}
		}

		public bool HeightCorrType_4
		{
			get
			{
				return this.heightCorreType_4;
			}
			set
			{
				this.heightCorreType_4 = value;
				this.RaisePropertyChanged("HeightCorrType_4");
			}
		}

		public bool HeightCorrType_5
		{
			get
			{
				return this.heightCorreType_5;
			}
			set
			{
				this.heightCorreType_5 = value;
				this.RaisePropertyChanged("HeightCorrType_5");
			}
		}

		public bool HeightMeasType_1
		{
			get
			{
				return this.heightMeasType_1;
			}
			set
			{
				this.heightMeasType_1 = value;
				this.RaisePropertyChanged("HeightMeasType_1");
			}
		}

		public bool HeightMeasType_2
		{
			get
			{
				return this.heightMeasType_2;
			}
			set
			{
				this.heightMeasType_2 = value;
				this.RaisePropertyChanged("HeightMeasType_2");
			}
		}

		public bool HeightRefType_1
		{
			get
			{
				return this.heightRefType_1;
			}
			set
			{
				this.heightRefType_1 = value;
				this.RaisePropertyChanged("HeightRefType_1");
			}
		}

		public bool HeightRefType_2
		{
			get
			{
				return this.heightRefType_2;
			}
			set
			{
				this.heightRefType_2 = value;
				this.RaisePropertyChanged("HeightRefType_2");
			}
		}

		public bool LayersMaterialThickness_1
		{
			get
			{
				return this.layersMaterialThickness_1;
			}
			set
			{
				this.layersMaterialThickness_1 = value;
				this.RaisePropertyChanged("LayersMaterialThickness_1");
			}
		}

		public bool LayersMaterialThickness_2
		{
			get
			{
				return this.layersMaterialThickness_2;
			}
			set
			{
				this.layersMaterialThickness_2 = value;
				this.RaisePropertyChanged("LayersMaterialThickness_2");
			}
		}

		public bool LayersMaterialThickness_3
		{
			get
			{
				return this.layersMaterialThickness_3;
			}
			set
			{
				this.layersMaterialThickness_3 = value;
				this.RaisePropertyChanged("LayersMaterialThickness_3");
			}
		}

		public bool LayersMaterialThickness_4
		{
			get
			{
				return this.layersMaterialThickness_4;
			}
			set
			{
				this.layersMaterialThickness_4 = value;
				this.RaisePropertyChanged("LayersMaterialThickness_4");
			}
		}

		public bool SlotsYOffset_1
		{
			get
			{
				return this.slotsYOffset_1;
			}
			set
			{
				this.slotsYOffset_1 = value;
				this.RaisePropertyChanged("SlotsYOffset_1");
			}
		}

		public bool SlotsYOffset_2
		{
			get
			{
				return this.slotsYOffset_2;
			}
			set
			{
				this.slotsYOffset_2 = value;
				this.RaisePropertyChanged("SlotsYOffset_2");
			}
		}

		public bool SlotsYOffset_3
		{
			get
			{
				return this.slotsYOffset_3;
			}
			set
			{
				this.slotsYOffset_3 = value;
				this.RaisePropertyChanged("SlotsYOffset_3");
			}
		}

		public bool SlotsYOffset_4
		{
			get
			{
				return this.slotsYOffset_4;
			}
			set
			{
				this.slotsYOffset_4 = value;
				this.RaisePropertyChanged("SlotsYOffset_4");
			}
		}

		public bool StepLapsNumberOfSame_1
		{
			get
			{
				return this.stepLapsNumberOfSame_1;
			}
			set
			{
				this.stepLapsNumberOfSame_1 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSame_1");
			}
		}

		public bool StepLapsNumberOfSame_2
		{
			get
			{
				return this.stepLapsNumberOfSame_2;
			}
			set
			{
				this.stepLapsNumberOfSame_2 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSame_2");
			}
		}

		public bool StepLapsNumberOfSame_3
		{
			get
			{
				return this.stepLapsNumberOfSame_3;
			}
			set
			{
				this.stepLapsNumberOfSame_3 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSame_3");
			}
		}

		public bool StepLapsNumberOfSame_4
		{
			get
			{
				return this.stepLapsNumberOfSame_4;
			}
			set
			{
				this.stepLapsNumberOfSame_4 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSame_4");
			}
		}

		public bool StepLapsNumberOfSteps_1
		{
			get
			{
				return this.stepLapsNumberOfSteps_1;
			}
			set
			{
				this.stepLapsNumberOfSteps_1 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSteps_1");
			}
		}

		public bool StepLapsNumberOfSteps_2
		{
			get
			{
				return this.stepLapsNumberOfSteps_2;
			}
			set
			{
				this.stepLapsNumberOfSteps_2 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSteps_2");
			}
		}

		public bool StepLapsNumberOfSteps_3
		{
			get
			{
				return this.stepLapsNumberOfSteps_3;
			}
			set
			{
				this.stepLapsNumberOfSteps_3 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSteps_3");
			}
		}

		public bool StepLapsNumberOfSteps_4
		{
			get
			{
				return this.stepLapsNumberOfSteps_4;
			}
			set
			{
				this.stepLapsNumberOfSteps_4 = value;
				this.RaisePropertyChanged("StepLapsNumberOfSteps_4");
			}
		}

		public bool StepLapsValue_1
		{
			get
			{
				return this.stepLapsValue_1;
			}
			set
			{
				this.stepLapsValue_1 = value;
				this.RaisePropertyChanged("StepLapsValue_1");
			}
		}

		public bool StepLapsValue_2
		{
			get
			{
				return this.stepLapsValue_2;
			}
			set
			{
				this.stepLapsValue_2 = value;
				this.RaisePropertyChanged("StepLapsValue_2");
			}
		}

		public bool StepLapsValue_3
		{
			get
			{
				return this.stepLapsValue_3;
			}
			set
			{
				this.stepLapsValue_3 = value;
				this.RaisePropertyChanged("StepLapsValue_3");
			}
		}

		public bool StepLapsValue_4
		{
			get
			{
				return this.stepLapsValue_4;
			}
			set
			{
				this.stepLapsValue_4 = value;
				this.RaisePropertyChanged("StepLapsValue_4");
			}
		}

		public bool TipCutsDoubleCut_1
		{
			get
			{
				return this.tipCutsDoubleCut_1;
			}
			set
			{
				this.tipCutsDoubleCut_1 = value;
				this.RaisePropertyChanged("TipCutsDoubleCut_1");
			}
		}

		public bool TipCutsDoubleCut_2
		{
			get
			{
				return this.tipCutsDoubleCut_2;
			}
			set
			{
				this.tipCutsDoubleCut_2 = value;
				this.RaisePropertyChanged("TipCutsDoubleCut_2");
			}
		}

		public bool TipCutsHeight_1
		{
			get
			{
				return this.tipCutsHeight_1;
			}
			set
			{
				this.tipCutsHeight_1 = value;
				this.RaisePropertyChanged("TipCutsHeight_1");
			}
		}

		public bool TipCutsHeight_2
		{
			get
			{
				return this.tipCutsHeight_2;
			}
			set
			{
				this.tipCutsHeight_2 = value;
				this.RaisePropertyChanged("TipCutsHeight_2");
			}
		}

		public bool TipCutsHeight_3
		{
			get
			{
				return this.tipCutsHeight_3;
			}
			set
			{
				this.tipCutsHeight_3 = value;
				this.RaisePropertyChanged("TipCutsHeight_3");
			}
		}

		public bool TipCutsHeight_4
		{
			get
			{
				return this.tipCutsHeight_4;
			}
			set
			{
				this.tipCutsHeight_4 = value;
				this.RaisePropertyChanged("TipCutsHeight_4");
			}
		}

		public bool TipCutsOverCut_1
		{
			get
			{
				return this.tipCutsOverCut_1;
			}
			set
			{
				this.tipCutsOverCut_1 = value;
				this.RaisePropertyChanged("TipCutsOverCut_1");
			}
		}

		public bool TipCutsOverCut_2
		{
			get
			{
				return this.tipCutsOverCut_2;
			}
			set
			{
				this.tipCutsOverCut_2 = value;
				this.RaisePropertyChanged("TipCutsOverCut_2");
			}
		}

		public bool TipCutsOverCut_3
		{
			get
			{
				return this.tipCutsOverCut_3;
			}
			set
			{
				this.tipCutsOverCut_3 = value;
				this.RaisePropertyChanged("TipCutsOverCut_3");
			}
		}

		public bool TipCutsOverCut_4
		{
			get
			{
				return this.tipCutsOverCut_4;
			}
			set
			{
				this.tipCutsOverCut_4 = value;
				this.RaisePropertyChanged("TipCutsOverCut_4");
			}
		}

		public RadioButtonsPar()
		{
		}

		private void RaisePropertyChanged(string prop)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}