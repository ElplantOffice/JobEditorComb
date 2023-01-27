using JobEditor;
using JobEditor.Common;
using JobEditor.Helpers;
using JobEditor.Import;
using JobEditor.Model;
using JobEditor.Properties;
using JobEditor.ViewModels;
using JobEditor.Views.ProductData;
 
using Models;
using ProductLib;
using UserControls.FileSelector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using RelayCommand = JobEditor.Helpers.RelayCommand;
using Patterns.EventAggregator;

namespace JobEditor.ViewModels
{
	public class EditorViewModel : ViewModelBase1
	{
        // Fields
        private bool editEnaNumberOfStep;
        private bool editEnaNumberOfSame;
        private bool editEnaStepLapValue;
        private bool editEnaTipCutHeight;
        private bool editEnaTipCutOverCut;
        private bool editEnaHolesYOffset;
        private bool editEnaCentersOverCut;
        private bool editEnaCentersVOffset;
        private bool editEnaNumberOfLayers;
        private bool editEnaLayerSheetThickness;
        private bool cutSeqVisuEna;
        private bool stepLapVisuEna;
        private bool tipCutVisuEna;
        private bool holesVisuEna;
        private bool slotsVisuEna;
        private bool centersVisuEna;
        private bool stackerVisuEna;
        private bool layersVisuEna;
        private bool fileSelectorVisUEna;
        private bool logInVisUEna;
        private bool emptySpaceEna;
        private bool protectEditorPanel;
        private int nmbrLayersToAdd;
        private double emptySpaceWidth;
        private double emptySpaceHeight;
        private string productName = "";
        private MainWindow parrent;
        private UiLocalisation localisation;
        private LayerView selectedLayerView = new LayerView(null, null);
        private RadioButtonsPar rBPar = new RadioButtonsPar();
        private DefaultPar defaultPar;
        private List<CutSequenceItem> cutSequences;
        private SequenceMaker SeqMaker = new SequenceMaker();
        public EditorModel EditorModel = new EditorModel();
        public bool CutSeqSelected;
        private ProductView productView = new ProductView();
        private bool productFileValid;
        private Action updateAppBarButtonsState;
        private Action updateAppBarButtonsText;
        private int selectedCutSeqIndex;
        private LayerView copyOfLayerView;
        private JobEditorFileSelector fileSelector;
        private JobEditorLogin logIn;
        private SolidColorBrush editButtonBrushNumberOfStep;
        private SolidColorBrush editButtonBrushTipCutHeight;
        private SolidColorBrush editButtonBrushTipCutOverCut;
        private SolidColorBrush editButtonBrushNumberOfSame;
        private SolidColorBrush editButtonBrushStepLapValue;
        private SolidColorBrush editButtonBrushHolesYOffset;
        private SolidColorBrush editButtonBrushCentersOverCut;
        private SolidColorBrush editButtonBrushCentersVOffset;
        private SolidColorBrush editButtonBrushNumberOfLayers;
        private SolidColorBrush editButtonBrushLayersMaterialThickness;

        // Methods
        public EditorViewModel(UiLocalisation localisation, MainWindow parrent = null)
        {
            this.parrent = parrent;
            this.ProductView.OnPropertyChanged = new Action<object, string>(this.OnProductViewPropertyChanged);
            this.localisation = localisation;
            this.SeqMaker = this.EditorModel.SeqMaker;
            this.LoadDefaultPar();
            this.FileSelectorVisuEna = false;
            this.fileSelector = new JobEditorFileSelector(localisation);
            this.fileSelector.OnClose = new Action(this.OnCloseFileSelector);
            if (Settings.Default.LoginEnable)
            {
                this.ProtectEditorPanel = true;
            }
            else
            {
                this.ProtectEditorPanel = false;
            }
            this.LogInVisuEna = false;
            this.logIn = new JobEditorLogin(localisation);
            this.logIn.OnClose = new Action(this.OnCloseLogIn);
            this.LogIn.ImgCancel = "NumberOfSteps.png";
            this.LogIn.ImgLogIn = "Edit.png";
            this.cutSequences = new List<CutSequenceItem>();
            foreach (CutSequence sequence in this.SeqMaker.CutSequences)
            {
                this.cutSequences.Add(new CutSequenceItem(sequence.Name));
            }
            this.SelectCutSequenceConfirmCommand = new RelayCommand(new Action<object>(this.SelectCutSequenceConfirmation));
            this.EditNumberOfStepDefParCommand = new RelayCommand(new Action<object>(this.EditNumberOfStepDefPar));
            this.EditNumberOfSameDefParCommand = new RelayCommand(new Action<object>(this.EditNumberOfSameDefPar));
            this.EditStepLapValueDefParCommand = new RelayCommand(new Action<object>(this.EditStepLapValueDefPar));
            this.EditCentersOverCutDefParCommand = new RelayCommand(new Action<object>(this.EditCentersOverCutDefPar));
            this.EditTipCutHeightDefParCommand = new RelayCommand(new Action<object>(this.EditTipCutHeightDefPar));
            this.EditTipCutOverCutDefParCommand = new RelayCommand(new Action<object>(this.EditTipCutOverCutDefPar));
            this.EditLayersMaterialThicknessDefParCommand = new RelayCommand(new Action<object>(this.EditLayersMaterialThicknessDefPar));
            this.NumberOfStepsParCommand = new RelayCommand(new Action<object>(this.NumberOfStepsPar));
            this.NumberOfSameParCommand = new RelayCommand(new Action<object>(this.NumberOfSamePar));
            this.StepLapValueParCommand = new RelayCommand(new Action<object>(this.StepLapValuePar));
            this.CentersOverCutParCommand = new RelayCommand(new Action<object>(this.CentersOverCutPar));
            this.CentersDoubleCutParCommand = new RelayCommand(new Action<object>(this.CentersDoubleCutPar));
            this.NumberOfLayersParCommand = new RelayCommand(new Action<object>(this.NumberOfLayersPar));
            this.HeightRefTypeParCommand = new RelayCommand(new Action<object>(this.HeightRefTypePar));
            this.HeightMeasTypeParCommand = new RelayCommand(new Action<object>(this.HeightMeasTypePar));
            this.HeightCorrTypeParCommand = new RelayCommand(new Action<object>(this.HeightCorrTypePar));
            this.MaterialThicknessParCommand = new RelayCommand(new Action<object>(this.MaterialThicknessPar));
            this.TipCutHeightParCommand = new RelayCommand(new Action<object>(this.TipCutHeightPar));
            this.TipCutOverCutParCommand = new RelayCommand(new Action<object>(this.TipCutOverCutPar));
            this.TipCutsDoubleCutParCommand = new RelayCommand(new Action<object>(this.TipCutsDoubleCutPar));
            this.DPar = new DataPar(this);
            this.ProductFileFullPath = "";
            this.ProductFileValid = false;
            this.ImportFileFullPath = "";
            this.SetButtonEditNumberOfStepColor();
            this.SetButtonEditNumberOfSameColor();
            this.SetButtonEditStepLapValueColor();
            this.SetButtonEditCentersOverCutColor();
            this.SetButtonEditNumberLayersColor();
            this.SetButtonEditTipCutHeightColor();
            this.SetButtonEditTipCutOverCutColor();
            this.SetButtonEditLayersMaterialThicknessColor();
        }

        private void CentersDoubleCutPar(object parameter)
        {
            if (((string)parameter) == "ON")
            {
                this.ProductView.SelectedLayerView.CentersDefaultView.DoubleCut = true;
            }
            else
            {
                this.ProductView.SelectedLayerView.CentersDefaultView.DoubleCut = false;
            }
        }

        private void CentersOverCutPar(object parameter)
        {
            this.ProductView.SelectedLayerView.CentersDefaultView.OverCut = (double)parameter;
        }

        private LayerView CreateNewLayer(CutSequence cutSequence, int layerId)
        {
            LayerView layerView = new LayerView(this.ProductView, null)
            {
                Name = cutSequence.Name,
                Id = layerId
            };
            this.CreateShapeViews(cutSequence, layerView);
            return layerView;
        }

        public LayerView CreateNewLayer(string nameCutSequence, int layerId)
        {
            foreach (CutSequence sequence in this.SeqMaker.CutSequences)
            {
                if (sequence.Name == nameCutSequence)
                {
                    return this.CreateNewLayer(sequence, layerId);
                }
            }
            return null;
        }

        private void CreateShapeViews(CutSequence cutSequence, LayerView layerView)
        {
            foreach (Shape shape in cutSequence.Shapes)
            {
                for (int i = 0; i < shape.ShapeParts.Count; i++)
                {
                    EFeature feature;
                    shape.ShapeParts[i].Id = i;
                    if (i == 0)
                    {
                        switch (shape.ShapeParts[i].Feature)
                        {
                            case EFeature.Cut90:
                                {
                                    shape.ShapeParts[i].StepLap.Type = EStepLapType.Cut90Left;
                                    continue;
                                }
                            case EFeature.Cut45:
                                {
                                    shape.ShapeParts[i].StepLap.Type = EStepLapType.Cut45Left;
                                    continue;
                                }
                            case EFeature.Cut135:
                                {
                                    shape.ShapeParts[i].StepLap.Type = EStepLapType.Cut135Left;
                                    continue;
                                }
                            case EFeature.CLeft:
                                {
                                    shape.ShapeParts[i].StepLap.Type = EStepLapType.CTipLeft;
                                    continue;
                                }
                        }
                        shape.ShapeParts[i].StepLap.Type = EStepLapType.Cut90Left;
                        continue;
                    }
                    if (i == (shape.ShapeParts.Count - 1))
                    {
                        feature = shape.ShapeParts[i].Feature;
                        switch (feature)
                        {
                            case EFeature.Cut90:
                                {
                                    shape.ShapeParts[i].StepLap.Type = EStepLapType.Cut90Right;
                                    continue;
                                }
                            case EFeature.Cut45:
                                {
                                    shape.ShapeParts[i].StepLap.Type = EStepLapType.Cut45Right;
                                    continue;
                                }
                            case EFeature.Cut135:
                                {
                                    shape.ShapeParts[i].StepLap.Type = EStepLapType.Cut135Right;
                                    continue;
                                }
                            case EFeature.CRight:
                                {
                                    shape.ShapeParts[i].StepLap.Type = EStepLapType.CTipRight;
                                    continue;
                                }
                        }
                        shape.ShapeParts[i].StepLap.Type = EStepLapType.Cut90Right;
                    }
                    else
                    {
                        feature = shape.ShapeParts[i].Feature;
                        if (feature != EFeature.VTop)
                        {
                            if (feature == EFeature.VBottom)
                            {
                                goto Label_01F7;
                            }
                            goto Label_0211;
                        }
                        shape.ShapeParts[i].StepLap.Type = EStepLapType.VTop;
                    }
                    continue;
                Label_01F7:
                    shape.ShapeParts[i].StepLap.Type = EStepLapType.VBottom;
                    continue;
                Label_0211:
                    shape.ShapeParts[i].StepLap.Type = EStepLapType.VTop;
                }
                layerView.ShapeViews.Add(new ShapeView(this.ProductView, shape));
            }
        }

        private void EditCentersOverCutDefPar(object obj)
        {
            this.EditEnaCentersOverCut = !this.EditEnaCentersOverCut;
            this.SetButtonEditCentersOverCutColor();
            this.HandleSelectedAndChangedCentersDefaultPar();
            this.FillRadioButtons(false);
            this.SaveDefaultPar();
        }

        private void EditLayersMaterialThicknessDefPar(object obj)
        {
            this.EditEnaLayerSheetThickness = !this.EditEnaLayerSheetThickness;
            this.SetButtonEditLayersMaterialThicknessColor();
            this.HandleSelectedAndChangedLayersDefaultPar();
            this.FillRadioButtons(false);
            this.SaveDefaultPar();
        }

        private void EditNumberOfSameDefPar(object obj)
        {
            this.EditEnaNumberOfSame = !this.EditEnaNumberOfSame;
            this.SetButtonEditNumberOfSameColor();
            this.HandleSelectedAndChangedStepLapDefaultPar();
            this.FillRadioButtons(false);
            this.SaveDefaultPar();
        }

        private void EditNumberOfStepDefPar(object obj)
        {
            this.EditEnaNumberOfStep = !this.EditEnaNumberOfStep;
            this.SetButtonEditNumberOfStepColor();
            this.HandleSelectedAndChangedStepLapDefaultPar();
            this.FillRadioButtons(false);
            this.SaveDefaultPar();
        }

        private void EditStepLapValueDefPar(object obj)
        {
            this.EditEnaStepLapValue = !this.EditEnaStepLapValue;
            this.SetButtonEditStepLapValueColor();
            this.HandleSelectedAndChangedStepLapDefaultPar();
            this.FillRadioButtons(false);
            this.SaveDefaultPar();
        }

        private void EditTipCutHeightDefPar(object obj)
        {
            this.EditEnaTipCutHeight = !this.EditEnaTipCutHeight;
            this.SetButtonEditTipCutHeightColor();
            this.HandleSelectedAndChangedTipCutDefaultPar();
            this.FillRadioButtons(false);
            this.SaveDefaultPar();
        }

        private void EditTipCutOverCutDefPar(object obj)
        {
            this.EditEnaTipCutOverCut = !this.EditEnaTipCutOverCut;
            this.SetButtonEditTipCutOverCutColor();
            this.HandleSelectedAndChangedTipCutDefaultPar();
            this.FillRadioButtons(false);
            this.SaveDefaultPar();
        }

        public void EnterPinCode(Action<bool, string, string, uint> onLogInAction)
        {
            this.LogIn.OnOk = onLogInAction;
            this.LogInVisuEna = true;
        }

        private void FillProduct(int numberOfLayersToAdd)
        {
            this.ProductView.EnableOnPropertyChanged = false;
            int id = 0;
            if (this.ProductView.LayerViews.Count > 0)
            {
                id = Enumerable.Last<LayerView>(this.ProductView.LayerViews).Id;
            }
            for (int i = 0; i < numberOfLayersToAdd; i++)
            {
                this.ProductView.LayerViews.Add(this.CreateNewLayer(this.SeqMaker.CutSequences[this.selectedCutSeqIndex], (id + i) + 1));
            }
            if (numberOfLayersToAdd < 0)
            {
                numberOfLayersToAdd = Math.Abs(numberOfLayersToAdd);
                this.ProductView.LayerViews.RemoveRange(this.ProductView.LayerViews.Count - numberOfLayersToAdd, numberOfLayersToAdd);
            }
            this.ProductView.EnableOnPropertyChanged = true;
            this.ProductView.Validate();
            this.UpdateAppBarButtonsState();
        }

        public void FillRadioButtons(bool clearNotSelectedDParValue = false)
        {
            if (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps == this.DefaultPar.StepLapsNumberOfSteps_1)
            {
                this.RBPar.StepLapsNumberOfSteps_1 = true;
            }
            else
            {
                this.RBPar.StepLapsNumberOfSteps_1 = false;
            }
            if ((this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps == this.DefaultPar.StepLapsNumberOfSteps_2) && !this.RBPar.StepLapsNumberOfSteps_1)
            {
                this.RBPar.StepLapsNumberOfSteps_2 = true;
            }
            else
            {
                this.RBPar.StepLapsNumberOfSteps_2 = false;
            }
            if (((this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps == this.DefaultPar.StepLapsNumberOfSteps_3) && !this.RBPar.StepLapsNumberOfSteps_1) && !this.RBPar.StepLapsNumberOfSteps_2)
            {
                this.RBPar.StepLapsNumberOfSteps_3 = true;
            }
            else
            {
                this.RBPar.StepLapsNumberOfSteps_3 = false;
            }
            if ((!this.RBPar.StepLapsNumberOfSteps_1 && !this.RBPar.StepLapsNumberOfSteps_2) && !this.RBPar.StepLapsNumberOfSteps_3)
            {
                this.DPar.StepLapNumberSteps = this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps;
                this.RBPar.StepLapsNumberOfSteps_4 = true;
            }
            else
            {
                if (clearNotSelectedDParValue)
                {
                    this.DPar.StepLapNumberSteps = 0;
                }
                this.RBPar.StepLapsNumberOfSteps_4 = false;
            }
            if (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame == this.DefaultPar.StepLapsNumberOfSame_1)
            {
                this.RBPar.StepLapsNumberOfSame_1 = true;
            }
            else
            {
                this.RBPar.StepLapsNumberOfSame_1 = false;
            }
            if ((this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame == this.DefaultPar.StepLapsNumberOfSame_2) && !this.RBPar.StepLapsNumberOfSame_1)
            {
                this.RBPar.StepLapsNumberOfSame_2 = true;
            }
            else
            {
                this.RBPar.StepLapsNumberOfSame_2 = false;
            }
            if (((this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame == this.DefaultPar.StepLapsNumberOfSame_3) && !this.RBPar.StepLapsNumberOfSame_1) && !this.RBPar.StepLapsNumberOfSame_2)
            {
                this.RBPar.StepLapsNumberOfSame_3 = true;
            }
            else
            {
                this.RBPar.StepLapsNumberOfSame_3 = false;
            }
            if ((!this.RBPar.StepLapsNumberOfSame_1 && !this.RBPar.StepLapsNumberOfSame_2) && !this.RBPar.StepLapsNumberOfSame_3)
            {
                this.DPar.StepLapNumberSame = this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame;
                this.RBPar.StepLapsNumberOfSame_4 = true;
            }
            else
            {
                if (clearNotSelectedDParValue)
                {
                    this.DPar.StepLapNumberSame = 0;
                }
                this.RBPar.StepLapsNumberOfSame_4 = false;
            }
            if (this.ProductView.SelectedLayerView.StepLapsDefaultView.Value == this.DefaultPar.StepLapsValue_1)
            {
                this.RBPar.StepLapsValue_1 = true;
            }
            else
            {
                this.RBPar.StepLapsValue_1 = false;
            }
            if ((this.ProductView.SelectedLayerView.StepLapsDefaultView.Value == this.DefaultPar.StepLapsValue_2) && !this.RBPar.StepLapsValue_1)
            {
                this.RBPar.StepLapsValue_2 = true;
            }
            else
            {
                this.RBPar.StepLapsValue_2 = false;
            }
            if (((this.ProductView.SelectedLayerView.StepLapsDefaultView.Value == this.DefaultPar.StepLapsValue_3) && !this.RBPar.StepLapsValue_1) && !this.RBPar.StepLapsValue_2)
            {
                this.RBPar.StepLapsValue_3 = true;
            }
            else
            {
                this.RBPar.StepLapsValue_3 = false;
            }
            if ((!this.RBPar.StepLapsValue_1 && !this.RBPar.StepLapsValue_2) && !this.RBPar.StepLapsValue_3)
            {
                this.DPar.StepLapStepValue = this.ProductView.SelectedLayerView.StepLapsDefaultView.Value;
                this.RBPar.StepLapsValue_4 = true;
            }
            else
            {
                if (clearNotSelectedDParValue)
                {
                    this.DPar.StepLapStepValue = 0.0;
                }
                this.RBPar.StepLapsValue_4 = false;
            }
            if (this.ProductView.SelectedLayerView.TipCutsDefaultView.Height == this.DefaultPar.TipCutsHeight_1)
            {
                this.RBPar.TipCutsHeight_1 = true;
            }
            else
            {
                this.RBPar.TipCutsHeight_1 = false;
            }
            if ((this.ProductView.SelectedLayerView.TipCutsDefaultView.Height == this.DefaultPar.TipCutsHeight_2) && !this.RBPar.TipCutsHeight_1)
            {
                this.RBPar.TipCutsHeight_2 = true;
            }
            else
            {
                this.RBPar.TipCutsHeight_2 = false;
            }
            if (((this.ProductView.SelectedLayerView.TipCutsDefaultView.Height == this.DefaultPar.TipCutsHeight_3) && !this.RBPar.TipCutsHeight_1) && !this.RBPar.TipCutsHeight_2)
            {
                this.RBPar.TipCutsHeight_3 = true;
            }
            else
            {
                this.RBPar.TipCutsHeight_3 = false;
            }
            if ((!this.RBPar.TipCutsHeight_1 && !this.RBPar.TipCutsHeight_2) && !this.RBPar.TipCutsHeight_3)
            {
                this.DPar.TipCutHeight = this.ProductView.SelectedLayerView.TipCutsDefaultView.Height;
                this.RBPar.TipCutsHeight_4 = true;
            }
            else
            {
                if (clearNotSelectedDParValue)
                {
                    this.DPar.TipCutHeight = 0.0;
                }
                this.RBPar.TipCutsHeight_4 = false;
            }
            if (this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut == this.DefaultPar.TipCutsOverCut_1)
            {
                this.RBPar.TipCutsOverCut_1 = true;
            }
            else
            {
                this.RBPar.TipCutsOverCut_1 = false;
            }
            if ((this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut == this.DefaultPar.TipCutsOverCut_2) && !this.RBPar.TipCutsOverCut_1)
            {
                this.RBPar.TipCutsOverCut_2 = true;
            }
            else
            {
                this.RBPar.TipCutsOverCut_2 = false;
            }
            if (((this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut == this.DefaultPar.TipCutsOverCut_3) && !this.RBPar.TipCutsOverCut_1) && !this.RBPar.TipCutsOverCut_2)
            {
                this.RBPar.TipCutsOverCut_3 = true;
            }
            else
            {
                this.RBPar.TipCutsOverCut_3 = false;
            }
            if ((!this.RBPar.TipCutsOverCut_1 && !this.RBPar.TipCutsOverCut_2) && !this.RBPar.TipCutsOverCut_3)
            {
                this.DPar.TipCutOverCut = this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut;
                this.RBPar.TipCutsOverCut_4 = true;
            }
            else
            {
                if (clearNotSelectedDParValue)
                {
                    this.DPar.TipCutOverCut = 0.0;
                }
                this.RBPar.TipCutsOverCut_4 = false;
            }
            if (this.ProductView.SelectedLayerView.TipCutsDefaultView.DoubleCut)
            {
                this.RBPar.TipCutsDoubleCut_1 = true;
                this.RBPar.TipCutsDoubleCut_2 = false;
            }
            else
            {
                this.RBPar.TipCutsDoubleCut_1 = false;
                this.RBPar.TipCutsDoubleCut_2 = true;
            }
            if (this.ProductView.SelectedLayerView.CentersDefaultView.OverCut == this.DefaultPar.CentersOverCut_1)
            {
                this.RBPar.CentersOverCut_1 = true;
            }
            else
            {
                this.RBPar.CentersOverCut_1 = false;
            }
            if ((this.ProductView.SelectedLayerView.CentersDefaultView.OverCut == this.DefaultPar.CentersOverCut_2) && !this.RBPar.CentersOverCut_1)
            {
                this.RBPar.CentersOverCut_2 = true;
            }
            else
            {
                this.RBPar.CentersOverCut_2 = false;
            }
            if (((this.ProductView.SelectedLayerView.CentersDefaultView.OverCut == this.DefaultPar.CentersOverCut_3) && !this.RBPar.CentersOverCut_1) && !this.RBPar.CentersOverCut_2)
            {
                this.RBPar.CentersOverCut_3 = true;
            }
            else
            {
                this.RBPar.CentersOverCut_3 = false;
            }
            if ((!this.RBPar.CentersOverCut_1 && !this.RBPar.CentersOverCut_2) && !this.RBPar.CentersOverCut_3)
            {
                this.DPar.CentersOverCut = this.ProductView.SelectedLayerView.CentersDefaultView.OverCut;
                this.RBPar.CentersOverCut_4 = true;
            }
            else
            {
                if (clearNotSelectedDParValue)
                {
                    this.DPar.CentersOverCut = 0.0;
                }
                this.RBPar.CentersOverCut_4 = false;
            }
            if (this.ProductView.SelectedLayerView.CentersDefaultView.DoubleCut)
            {
                this.RBPar.CentersDoubleCut_1 = true;
                this.RBPar.CentersDoubleCut_2 = false;
            }
            else
            {
                this.RBPar.CentersDoubleCut_1 = false;
                this.RBPar.CentersDoubleCut_2 = true;
            }
            if (this.ProductView.HeightRefType == EHeightRefType.MM)
            {
                this.RBPar.HeightRefType_1 = true;
            }
            else
            {
                this.RBPar.HeightRefType_1 = false;
            }
            if (this.ProductView.HeightRefType == EHeightRefType.Number)
            {
                this.RBPar.HeightRefType_2 = true;
            }
            else
            {
                this.RBPar.HeightRefType_2 = false;
            }
            if (this.ProductView.HeightMeasType == EHeightMeasType.Absolute)
            {
                this.RBPar.HeightMeasType_1 = true;
            }
            else
            {
                this.RBPar.HeightMeasType_1 = false;
            }
            if (this.ProductView.HeightMeasType == EHeightMeasType.Relative)
            {
                this.RBPar.HeightMeasType_2 = true;
            }
            else
            {
                this.RBPar.HeightMeasType_2 = false;
            }
            if (this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness == this.DefaultPar.MaterialThickness_1)
            {
                this.RBPar.LayersMaterialThickness_1 = true;
            }
            else
            {
                this.RBPar.LayersMaterialThickness_1 = false;
            }
            if ((this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness == this.DefaultPar.MaterialThickness_2) && !this.RBPar.LayersMaterialThickness_1)
            {
                this.RBPar.LayersMaterialThickness_2 = true;
            }
            else
            {
                this.RBPar.LayersMaterialThickness_2 = false;
            }
            if (((this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness == this.DefaultPar.MaterialThickness_3) && !this.RBPar.LayersMaterialThickness_1) && !this.RBPar.LayersMaterialThickness_2)
            {
                this.RBPar.LayersMaterialThickness_3 = true;
            }
            else
            {
                this.RBPar.LayersMaterialThickness_3 = false;
            }
            if ((!this.RBPar.LayersMaterialThickness_1 && !this.RBPar.LayersMaterialThickness_2) && !this.RBPar.LayersMaterialThickness_3)
            {
                this.DPar.MaterialThickness = this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness;
                this.RBPar.LayersMaterialThickness_4 = true;
            }
            else
            {
                if (clearNotSelectedDParValue)
                {
                    this.DPar.MaterialThickness = 0.0;
                }
                this.RBPar.LayersMaterialThickness_4 = false;
            }
            if (this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType == EHeightCorrectionType.None)
            {
                this.RBPar.HeightCorrType_1 = true;
            }
            else
            {
                this.RBPar.HeightCorrType_1 = false;
            }
            if (this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType == EHeightCorrectionType.CompleteCycleUp)
            {
                this.RBPar.HeightCorrType_2 = true;
            }
            else
            {
                this.RBPar.HeightCorrType_2 = false;
            }
            if (this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType == EHeightCorrectionType.CompleteCycleDown)
            {
                this.RBPar.HeightCorrType_3 = true;
            }
            else
            {
                this.RBPar.HeightCorrType_3 = false;
            }
            if (this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType == EHeightCorrectionType.PreciseUp)
            {
                this.RBPar.HeightCorrType_4 = true;
            }
            else
            {
                this.RBPar.HeightCorrType_4 = false;
            }
            if (this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType == EHeightCorrectionType.PreciseDown)
            {
                this.RBPar.HeightCorrType_5 = true;
            }
            else
            {
                this.RBPar.HeightCorrType_5 = false;
            }
            this.DefaultPar.Validate();
            this.DPar.Validate();
        }

        public Product FinalizingProduct()
        {
            Product product = this.ProductView.Product;
            for (int i = 0; i < product.Layers.Count; i++)
            {
                Layer layer = product.Layers[i];
                layer.Id = i;
                layer.LayerDefault.NumberOfSteps = layer.StepLapsDefault.NumberOfSteps;
                layer.LayerDefault.NumberOfSame = layer.StepLapsDefault.NumberOfSame;
                layer.LayerDefault.StepLapValue = layer.StepLapsDefault.Value;
                layer.LayerDefault.DistanceY = layer.SlotsDefault.DistanceY;
                layer.LayerDefault.VOffset = layer.CentersDefault.VOffset;
                layer.LayerDefault.YOffset = layer.HolesDefault.Offset;
                for (int j = 0; j < layer.Shapes.Count; j++)
                {
                    double num3 = this.ShapeLength(layer.Shapes[j]);
                    layer.Shapes[j].Holes.Clear();
                    for (int k = 0; k < layer.Shapes[j].HolesInfo.NumberOfHoles; k++)
                    {
                        Hole item = new Hole
                        {
                            Id = k
                        };
                        switch (k)
                        {
                            case 0:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape1;
                                break;

                            case 1:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape2;
                                break;

                            case 2:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape3;
                                break;

                            case 3:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape4;
                                break;

                            case 4:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape5;
                                break;

                            case 5:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape6;
                                break;

                            case 6:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape7;
                                break;

                            case 7:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape8;
                                break;

                            case 8:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape9;
                                break;

                            case 9:
                                item.Shape = layer.Shapes[j].HolesInfo.Shape10;
                                break;

                            default:
                                item.Shape = EHoleShape.Round;
                                break;
                        }
                        item.Y = layer.HolesDefault.Offset;
                        double num6 = layer.Shapes[j].HolesInfo.Length1;
                        double num7 = layer.Shapes[j].HolesInfo.Length2;
                        double num8 = layer.Shapes[j].HolesInfo.Length3;
                        double num9 = layer.Shapes[j].HolesInfo.Length4;
                        double num10 = layer.Shapes[j].HolesInfo.Length5;
                        double num11 = layer.Shapes[j].HolesInfo.Length6;
                        double num12 = layer.Shapes[j].HolesInfo.Length7;
                        double num13 = layer.Shapes[j].HolesInfo.Length8;
                        double num14 = layer.Shapes[j].HolesInfo.Length9;
                        double num15 = layer.Shapes[j].HolesInfo.Length10;
                        if (layer.Shapes[j].HolesInfo.MeasuringType == EMeasuringType.Absolute)
                        {
                            switch (k)
                            {
                                case 0:
                                    item.X = num6;
                                    goto Label_044C;

                                case 1:
                                    item.X = num7;
                                    goto Label_044C;

                                case 2:
                                    item.X = num8;
                                    goto Label_044C;

                                case 3:
                                    item.X = num9;
                                    goto Label_044C;

                                case 4:
                                    item.X = num10;
                                    goto Label_044C;

                                case 5:
                                    item.X = num11;
                                    goto Label_044C;

                                case 6:
                                    item.X = num12;
                                    goto Label_044C;

                                case 7:
                                    item.X = num13;
                                    goto Label_044C;

                                case 8:
                                    item.X = num14;
                                    goto Label_044C;

                                case 9:
                                    item.X = num15;
                                    goto Label_044C;
                            }
                            item.X = 0.0;
                        }
                    Label_044C:
                        if (layer.Shapes[j].HolesInfo.MeasuringType == EMeasuringType.Relative)
                        {
                            switch (k)
                            {
                                case 0:
                                    item.X = num6;
                                    goto Label_05B4;

                                case 1:
                                    item.X = num6 + num7;
                                    goto Label_05B4;

                                case 2:
                                    item.X = (num6 + num7) + num8;
                                    goto Label_05B4;

                                case 3:
                                    item.X = ((num6 + num7) + num8) + num9;
                                    goto Label_05B4;

                                case 4:
                                    item.X = (((num6 + num7) + num8) + num9) + num10;
                                    goto Label_05B4;

                                case 5:
                                    item.X = ((((num6 + num7) + num8) + num9) + num10) + num11;
                                    goto Label_05B4;

                                case 6:
                                    item.X = (((((num6 + num7) + num8) + num9) + num10) + num11) + num12;
                                    goto Label_05B4;

                                case 7:
                                    item.X = ((((((num6 + num7) + num8) + num9) + num10) + num11) + num12) + num13;
                                    goto Label_05B4;

                                case 8:
                                    item.X = (((((((num6 + num7) + num8) + num9) + num10) + num11) + num12) + num13) + num14;
                                    goto Label_05B4;

                                case 9:
                                    item.X = ((((((((num6 + num7) + num8) + num9) + num10) + num11) + num12) + num13) + num14) + num15;
                                    goto Label_05B4;
                            }
                            item.X = 0.0;
                        }
                    Label_05B4:
                        if (layer.Shapes[j].HolesInfo.MeasuringType == EMeasuringType.Simetric)
                        {
                            if (layer.Shapes[j].HolesInfo.NumberOfHoles == 1)
                            {
                                item.X = num3 / 2.0;
                            }
                            else if (layer.Shapes[j].HolesInfo.NumberOfHoles == 2)
                            {
                                switch (k)
                                {
                                    case 0:
                                        item.X = (num3 - num6) / 2.0;
                                        break;

                                    case 1:
                                        item.X = ((num3 - num6) / 2.0) + num6;
                                        break;
                                }
                            }
                            else if (layer.Shapes[j].HolesInfo.NumberOfHoles == 3)
                            {
                                switch (k)
                                {
                                    case 0:
                                        item.X = ((num3 - num6) - num7) / 2.0;
                                        break;

                                    case 1:
                                        item.X = (((num3 - num6) - num7) / 2.0) + num6;
                                        break;

                                    case 2:
                                        item.X = ((((num3 - num6) - num7) / 2.0) + num6) + num7;
                                        break;
                                }
                            }
                            else if (layer.Shapes[j].HolesInfo.NumberOfHoles == 4)
                            {
                                switch (k)
                                {
                                    case 0:
                                        item.X = (((num3 - num6) - num7) - num8) / 2.0;
                                        break;

                                    case 1:
                                        item.X = ((((num3 - num6) - num7) - num8) / 2.0) + num6;
                                        break;

                                    case 2:
                                        item.X = (((((num3 - num6) - num7) - num8) / 2.0) + num6) + num7;
                                        break;

                                    case 3:
                                        item.X = ((((((num3 - num6) - num7) - num8) / 2.0) + num6) + num7) + num8;
                                        break;
                                }
                            }
                            else if (layer.Shapes[j].HolesInfo.NumberOfHoles == 5)
                            {
                                switch (k)
                                {
                                    case 0:
                                        item.X = ((((num3 - num6) - num7) - num8) - num9) / 2.0;
                                        break;

                                    case 1:
                                        item.X = (((((num3 - num6) - num7) - num8) - num9) / 2.0) + num6;
                                        break;

                                    case 2:
                                        item.X = ((((((num3 - num6) - num7) - num8) - num9) / 2.0) + num6) + num7;
                                        break;

                                    case 3:
                                        item.X = (((((((num3 - num6) - num7) - num8) - num9) / 2.0) + num6) + num7) + num8;
                                        break;

                                    case 4:
                                        item.X = ((((((((num3 - num6) - num7) - num8) - num9) / 2.0) + num6) + num7) + num8) + num9;
                                        break;
                                }
                            }
                            else if (layer.Shapes[j].HolesInfo.NumberOfHoles == 6)
                            {
                                switch (k)
                                {
                                    case 0:
                                        item.X = (((((num3 - num6) - num7) - num8) - num9) - num10) / 2.0;
                                        break;

                                    case 1:
                                        item.X = ((((((num3 - num6) - num7) - num8) - num9) - num10) / 2.0) + num6;
                                        break;

                                    case 2:
                                        item.X = (((((((num3 - num6) - num7) - num8) - num9) - num10) / 2.0) + num6) + num7;
                                        break;

                                    case 3:
                                        item.X = ((((((((num3 - num6) - num7) - num8) - num9) - num10) / 2.0) + num6) + num7) + num8;
                                        break;

                                    case 4:
                                        item.X = (((((((((num3 - num6) - num7) - num8) - num9) - num10) / 2.0) + num6) + num7) + num8) + num9;
                                        break;

                                    case 5:
                                        item.X = ((((((((((num3 - num6) - num7) - num8) - num9) - num10) / 2.0) + num6) + num7) + num8) + num9) + num10;
                                        break;
                                }
                            }
                            else if (layer.Shapes[j].HolesInfo.NumberOfHoles == 7)
                            {
                                switch (k)
                                {
                                    case 0:
                                        item.X = ((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) / 2.0;
                                        break;

                                    case 1:
                                        item.X = (((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) / 2.0) + num6;
                                        break;

                                    case 2:
                                        item.X = ((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) / 2.0) + num6) + num7;
                                        break;

                                    case 3:
                                        item.X = (((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) / 2.0) + num6) + num7) + num8;
                                        break;

                                    case 4:
                                        item.X = ((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) / 2.0) + num6) + num7) + num8) + num9;
                                        break;

                                    case 5:
                                        item.X = (((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) / 2.0) + num6) + num7) + num8) + num9) + num10;
                                        break;

                                    case 6:
                                        item.X = ((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11;
                                        break;
                                }
                            }
                            else if (layer.Shapes[j].HolesInfo.NumberOfHoles == 8)
                            {
                                switch (k)
                                {
                                    case 0:
                                        item.X = (((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) / 2.0;
                                        break;

                                    case 1:
                                        item.X = ((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) / 2.0) + num6;
                                        break;

                                    case 2:
                                        item.X = (((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) / 2.0) + num6) + num7;
                                        break;

                                    case 3:
                                        item.X = ((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) / 2.0) + num6) + num7) + num8;
                                        break;

                                    case 4:
                                        item.X = (((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) / 2.0) + num6) + num7) + num8) + num9;
                                        break;

                                    case 5:
                                        item.X = ((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) / 2.0) + num6) + num7) + num8) + num9) + num10;
                                        break;

                                    case 6:
                                        item.X = (((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11;
                                        break;

                                    case 7:
                                        item.X = ((((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11) + num12;
                                        break;
                                }
                            }
                            else if (layer.Shapes[j].HolesInfo.NumberOfHoles == 9)
                            {
                                switch (k)
                                {
                                    case 0:
                                        item.X = ((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) / 2.0;
                                        break;

                                    case 1:
                                        item.X = (((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) / 2.0) + num6;
                                        break;

                                    case 2:
                                        item.X = ((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) / 2.0) + num6) + num7;
                                        break;

                                    case 3:
                                        item.X = (((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) / 2.0) + num6) + num7) + num8;
                                        break;

                                    case 4:
                                        item.X = ((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) / 2.0) + num6) + num7) + num8) + num9;
                                        break;

                                    case 5:
                                        item.X = (((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) / 2.0) + num6) + num7) + num8) + num9) + num10;
                                        break;

                                    case 6:
                                        item.X = ((((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11;
                                        break;

                                    case 7:
                                        item.X = (((((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11) + num12;
                                        break;

                                    case 8:
                                        item.X = ((((((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11) + num12) + num13;
                                        break;
                                }
                            }
                            else if (layer.Shapes[j].HolesInfo.NumberOfHoles == 10)
                            {
                                switch (k)
                                {
                                    case 0:
                                        item.X = (((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0;
                                        break;

                                    case 1:
                                        item.X = ((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0) + num6;
                                        break;

                                    case 2:
                                        item.X = (((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0) + num6) + num7;
                                        break;

                                    case 3:
                                        item.X = ((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0) + num6) + num7) + num8;
                                        break;

                                    case 4:
                                        item.X = (((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0) + num6) + num7) + num8) + num9;
                                        break;

                                    case 5:
                                        item.X = ((((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0) + num6) + num7) + num8) + num9) + num10;
                                        break;

                                    case 6:
                                        item.X = (((((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11;
                                        break;

                                    case 7:
                                        item.X = ((((((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11) + num12;
                                        break;

                                    case 8:
                                        item.X = (((((((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11) + num12) + num13;
                                        break;

                                    case 9:
                                        item.X = ((((((((((((((((((num3 - num6) - num7) - num8) - num9) - num10) - num11) - num12) - num13) - num14) / 2.0) + num6) + num7) + num8) + num9) + num10) + num11) + num12) + num13) + num14;
                                        break;
                                }
                            }
                            else
                            {
                                item.X = 0.0;
                            }
                        }
                        layer.Shapes[j].Holes.Add(item);
                    }
                    layer.Shapes[j].Slots.Clear();
                    int numberOfSlots = layer.Shapes[j].SlotsInfo.NumberOfSlots;
                    bool flag = (numberOfSlots % 2) == 0;
                    for (int m = 0; m < numberOfSlots; m++)
                    {
                        Slot item = new Slot
                        {
                            Id = m
                        };
                        if (flag)
                        {
                            item.Y = (((1 - numberOfSlots) + (m * 2.0)) / 2.0) * layer.SlotsDefault.DistanceY;
                        }
                        else
                        {
                            item.Y = ((((double)(1 - numberOfSlots)) / 2.0) + m) * layer.SlotsDefault.DistanceY;
                        }
                        switch (layer.Shapes[j].SlotsInfo.MeasuringType)
                        {
                            case EMeasuringType.Absolute:
                                item.X = layer.Shapes[j].SlotsInfo.Length1;
                                item.Length = layer.Shapes[j].SlotsInfo.Length2 - layer.Shapes[j].SlotsInfo.Length1;
                                break;

                            case EMeasuringType.Relative:
                                item.X = layer.Shapes[j].SlotsInfo.Length1;
                                item.Length = layer.Shapes[j].SlotsInfo.Length2;
                                break;

                            case EMeasuringType.Simetric:
                                item.X = (num3 / 2.0) - (layer.Shapes[j].SlotsInfo.Length1 / 2.0);
                                item.Length = layer.Shapes[j].SlotsInfo.Length1;
                                break;

                            default:
                                item.X = 0.0;
                                item.Length = 0.0;
                                break;
                        }
                        layer.Shapes[j].Slots.Add(item);
                    }
                    int count = layer.Shapes[j].ShapePartList.Count;
                    for (int n = 0; n < layer.Shapes[j].ShapePartList.Count; n++)
                    {
                        layer.Shapes[j].ShapePartList[n].Id = n;
                        layer.Shapes[j].ShapePartList[n].OverCut = layer.CentersDefault.OverCut;
                        layer.Shapes[j].ShapePartList[n].DoubleCut = layer.CentersDefault.DoubleCut;
                        if (n == 0)
                        {
                            layer.Shapes[j].ShapePartList[n].X = 0.0;
                            if (layer.Shapes[j].ShapePartList[n].Feature == EFeature.CLeft)
                            {
                                layer.Shapes[j].ShapePartList[n].Y = layer.CentersDefault.CTipOffset;
                            }
                            else
                            {
                                layer.Shapes[j].ShapePartList[n].Y = 0.0;
                            }
                            goto Label_1AB6;
                        }
                        if (n >= (layer.Shapes[j].ShapePartList.Count - 1))
                        {
                            goto Label_1A26;
                        }
                        if (layer.Shapes[j].CentersInfo.MeasuringType == EMeasuringType.Absolute)
                        {
                            switch (n)
                            {
                                case 1:
                                    layer.Shapes[j].ShapePartList[n].X = layer.Shapes[j].CentersInfo.Length1;
                                    goto Label_19AB;

                                case 2:
                                    layer.Shapes[j].ShapePartList[n].X = layer.Shapes[j].CentersInfo.Length2;
                                    goto Label_19AB;

                                case 3:
                                    layer.Shapes[j].ShapePartList[n].X = layer.Shapes[j].CentersInfo.Length3;
                                    goto Label_19AB;
                            }
                            layer.Shapes[j].ShapePartList[n].X = 0.0;
                        }
                        else if (layer.Shapes[j].CentersInfo.MeasuringType == EMeasuringType.Relative)
                        {
                            switch (n)
                            {
                                case 1:
                                    layer.Shapes[j].ShapePartList[n].X = layer.Shapes[j].CentersInfo.Length1;
                                    goto Label_19AB;

                                case 2:
                                    layer.Shapes[j].ShapePartList[n].X = layer.Shapes[j].CentersInfo.Length1 + layer.Shapes[j].CentersInfo.Length2;
                                    goto Label_19AB;

                                case 3:
                                    layer.Shapes[j].ShapePartList[n].X = (layer.Shapes[j].CentersInfo.Length1 + layer.Shapes[j].CentersInfo.Length2) + layer.Shapes[j].CentersInfo.Length3;
                                    goto Label_19AB;
                            }
                            layer.Shapes[j].ShapePartList[n].X = 0.0;
                        }
                        else if (layer.Shapes[j].ShapePartList.Count == 3)
                        {
                            layer.Shapes[j].ShapePartList[n].X = (n * num3) / 2.0;
                        }
                        else if (layer.Shapes[j].ShapePartList.Count == 4)
                        {
                            layer.Shapes[j].ShapePartList[n].X = (n * num3) / 3.0;
                        }
                        else if (layer.Shapes[j].ShapePartList.Count == 5)
                        {
                            layer.Shapes[j].ShapePartList[n].X = (n * num3) / 4.0;
                        }
                    Label_19AB:
                        if (layer.Shapes[j].ShapePartList[n].Feature == EFeature.VTop)
                        {
                            layer.Shapes[j].ShapePartList[n].Y = layer.CentersDefault.VOffset;
                        }
                        else
                        {
                            layer.Shapes[j].ShapePartList[n].Y = -layer.CentersDefault.VOffset;
                        }
                        goto Label_1AB6;
                    Label_1A26:
                        layer.Shapes[j].ShapePartList[n].X = num3;
                        if (layer.Shapes[j].ShapePartList[n].Feature == EFeature.CRight)
                        {
                            layer.Shapes[j].ShapePartList[n].Y = -layer.CentersDefault.CTipOffset;
                        }
                        else
                        {
                            layer.Shapes[j].ShapePartList[n].Y = 0.0;
                        }
                    Label_1AB6:
                        layer.Shapes[j].ShapePartList[n].TipCut.Height = layer.TipCutsDefault.Height;
                        layer.Shapes[j].ShapePartList[n].TipCut.OverCut = layer.TipCutsDefault.OverCut;
                        layer.Shapes[j].ShapePartList[n].TipCut.DoubleCut = layer.TipCutsDefault.DoubleCut;
                        layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSteps = layer.StepLapsDefault.NumberOfSteps;
                        layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSame = layer.StepLapsDefault.NumberOfSame;
                        layer.Shapes[j].ShapePartList[n].StepLap.Value = layer.StepLapsDefault.Value;
                        layer.Shapes[j].ShapePartList[n].StepLap.Steps.Clear();
                        int num18 = -1;
                        for (int num19 = 0; num19 < layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSteps; num19++)
                        {
                            Step step = new Step
                            {
                                ShapeId = j
                            };
                            switch (layer.Shapes[j].ShapePartList[n].StepLap.Type)
                            {
                                case EStepLapType.Cut90Left:
                                case EStepLapType.Cut90Right:
                                case EStepLapType.Cut45Left:
                                case EStepLapType.Cut45Right:
                                case EStepLapType.Cut135Left:
                                case EStepLapType.Cut135Right:
                                case EStepLapType.CTipLeft:
                                case EStepLapType.CTipRight:
                                case EStepLapType.VTop:
                                case EStepLapType.VBottom:
                                    step.X = 0.0;
                                    step.Y = 0.0;
                                    break;

                                case EStepLapType.Cut90Left_Right:
                                case EStepLapType.Cut45Left_Right:
                                case EStepLapType.Cut135Left_Right:
                                case EStepLapType.CTipLeft_Right:
                                case EStepLapType.VTop_Right:
                                case EStepLapType.VBottom_Right:
                                    step.X = layer.Shapes[j].ShapePartList[n].StepLap.Value * (num19 - (((double)(layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSteps - 1)) / 2.0));
                                    step.Y = 0.0;
                                    break;

                                case EStepLapType.Cut90Left_Left:
                                case EStepLapType.Cut45Left_Left:
                                case EStepLapType.Cut135Left_Left:
                                case EStepLapType.CTipLeft_Left:
                                case EStepLapType.VTop_Left:
                                case EStepLapType.VBottom_Left:
                                    step.X = layer.Shapes[j].ShapePartList[n].StepLap.Value * ((((double)(layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSteps - 1)) / 2.0) - num19);
                                    step.Y = 0.0;
                                    break;

                                case EStepLapType.Cut90Right_Left:
                                case EStepLapType.Cut45Right_Left:
                                case EStepLapType.Cut135Right_Left:
                                case EStepLapType.CTipRight_Left:
                                    step.X = layer.Shapes[j].ShapePartList[n].StepLap.Value * ((((double)(layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSteps - 1)) / 2.0) - num19);
                                    step.Y = 0.0;
                                    break;

                                case EStepLapType.Cut90Right_Right:
                                case EStepLapType.Cut45Right_Right:
                                case EStepLapType.Cut135Right_Right:
                                case EStepLapType.CTipRight_Right:
                                    step.X = layer.Shapes[j].ShapePartList[n].StepLap.Value * (num19 - (((double)(layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSteps - 1)) / 2.0));
                                    step.Y = 0.0;
                                    break;

                                case EStepLapType.CTipLeft_Up:
                                case EStepLapType.CTipRight_Up:
                                case EStepLapType.VTop_Up:
                                case EStepLapType.VBottom_Up:
                                    step.X = 0.0;
                                    step.Y = layer.Shapes[j].ShapePartList[n].StepLap.Value * (num19 - (((double)(layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSteps - 1)) / 2.0));
                                    break;

                                case EStepLapType.CTipLeft_Down:
                                case EStepLapType.CTipRight_Down:
                                case EStepLapType.VTop_Down:
                                case EStepLapType.VBottom_Down:
                                    step.X = 0.0;
                                    step.Y = layer.Shapes[j].ShapePartList[n].StepLap.Value * ((((double)(layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSteps - 1)) / 2.0) - num19);
                                    break;
                            }
                            for (int num20 = 0; num20 < layer.Shapes[j].ShapePartList[n].StepLap.NumberOfSame; num20++)
                            {
                                num18++;
                                step.Id = num18;
                                layer.Shapes[j].ShapePartList[n].StepLap.Steps.Add((Step)step.Clone());
                            }
                        }
                    }
                    char[] separator = new char[] { '|' };
                    string[] textArray1 = layer.Shapes[j].Drawing.Split(separator);
                    if (Enumerable.First<string>(textArray1) == "CB")
                    {
                        layer.Shapes[j].ShapePartList[0].Y += layer.Shapes[j].CentersInfo.Length3;
                    }
                    if (Enumerable.First<string>(textArray1) == "CF")
                    {
                        layer.Shapes[j].ShapePartList[0].Y -= layer.Shapes[j].CentersInfo.Length3;
                    }
                    if (Enumerable.Last<string>(textArray1) == "CB")
                    {
                        layer.Shapes[j].ShapePartList[layer.Shapes[j].ShapePartList.Count - 1].Y += layer.Shapes[j].CentersInfo.Length4;
                    }
                    if (Enumerable.Last<string>(textArray1) == "CF")
                    {
                        layer.Shapes[j].ShapePartList[layer.Shapes[j].ShapePartList.Count - 1].Y -= layer.Shapes[j].CentersInfo.Length4;
                    }
                }
            }
            return product;
        }

        private CutSequenceItem GetCutSquenceItem(string name)
        {
            List<CutSequenceItem> list = Enumerable.ToList<CutSequenceItem>((IEnumerable<CutSequenceItem>)(from x in this.CutSequences
                                                                                                           where x.Name == name
                                                                                                           select x));
            if ((list != null) && (Enumerable.Count<CutSequenceItem>(list) > 0))
            {
                return list[0];
            }
            return null;
        }

        private void HandleSelectedAndChangedCentersDefaultPar()
        {
            if (this.RBPar.CentersOverCut_1 && (this.ProductView.SelectedLayerView.CentersDefaultView.OverCut != this.DefaultPar.CentersOverCut_1))
            {
                this.ProductView.SelectedLayerView.CentersDefaultView.OverCut = this.DefaultPar.CentersOverCut_1;
            }
            if (this.RBPar.CentersOverCut_2 && (this.ProductView.SelectedLayerView.CentersDefaultView.OverCut != this.DefaultPar.CentersOverCut_2))
            {
                this.ProductView.SelectedLayerView.CentersDefaultView.OverCut = this.DefaultPar.CentersOverCut_2;
            }
            if (this.RBPar.CentersOverCut_3 && (this.ProductView.SelectedLayerView.CentersDefaultView.OverCut != this.DefaultPar.CentersOverCut_3))
            {
                this.ProductView.SelectedLayerView.CentersDefaultView.OverCut = this.DefaultPar.CentersOverCut_3;
            }
            if (this.RBPar.CentersOverCut_4 && (this.ProductView.SelectedLayerView.CentersDefaultView.OverCut != this.DPar.CentersOverCut))
            {
                this.ProductView.SelectedLayerView.CentersDefaultView.OverCut = this.DPar.CentersOverCut;
            }
        }

        private void HandleSelectedAndChangedLayersDefaultPar()
        {
            if (this.RBPar.LayersMaterialThickness_1 && (this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness != this.DefaultPar.MaterialThickness_1))
            {
                this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness = this.DefaultPar.MaterialThickness_1;
            }
            if (this.RBPar.LayersMaterialThickness_2 && (this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness != this.DefaultPar.MaterialThickness_2))
            {
                this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness = this.DefaultPar.MaterialThickness_2;
            }
            if (this.RBPar.LayersMaterialThickness_3 && (this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness != this.DefaultPar.MaterialThickness_3))
            {
                this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness = this.DefaultPar.MaterialThickness_3;
            }
            if (this.RBPar.LayersMaterialThickness_4 && (this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness != this.DPar.MaterialThickness))
            {
                this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness = this.DPar.MaterialThickness;
            }
            if (this.RBPar.HeightCorrType_1)
            {
                this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType = EHeightCorrectionType.None;
            }
            if (this.RBPar.HeightCorrType_2)
            {
                this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType = EHeightCorrectionType.CompleteCycleUp;
            }
            if (this.RBPar.HeightCorrType_3)
            {
                this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType = EHeightCorrectionType.CompleteCycleDown;
            }
            if (this.RBPar.HeightCorrType_4)
            {
                this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType = EHeightCorrectionType.PreciseUp;
            }
            if (this.RBPar.HeightCorrType_5)
            {
                this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType = EHeightCorrectionType.PreciseDown;
            }
        }

        private void HandleSelectedAndChangedStepLapDefaultPar()
        {
            if (this.RBPar.StepLapsNumberOfSteps_1 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps != this.DefaultPar.StepLapsNumberOfSteps_1))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps = this.DefaultPar.StepLapsNumberOfSteps_1;
            }
            if (this.RBPar.StepLapsNumberOfSteps_2 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps != this.DefaultPar.StepLapsNumberOfSteps_2))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps = this.DefaultPar.StepLapsNumberOfSteps_2;
            }
            if (this.RBPar.StepLapsNumberOfSteps_3 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps != this.DefaultPar.StepLapsNumberOfSteps_3))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps = this.DefaultPar.StepLapsNumberOfSteps_3;
            }
            if (this.RBPar.StepLapsNumberOfSteps_4 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps != this.DPar.StepLapNumberSteps))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps = this.DPar.StepLapNumberSteps;
            }
            if (this.RBPar.StepLapsNumberOfSame_1 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame != this.DefaultPar.StepLapsNumberOfSame_1))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame = this.DefaultPar.StepLapsNumberOfSame_1;
            }
            if (this.RBPar.StepLapsNumberOfSame_2 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame != this.DefaultPar.StepLapsNumberOfSame_2))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame = this.DefaultPar.StepLapsNumberOfSame_2;
            }
            if (this.RBPar.StepLapsNumberOfSame_3 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame != this.DefaultPar.StepLapsNumberOfSame_3))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame = this.DefaultPar.StepLapsNumberOfSame_3;
            }
            if (this.RBPar.StepLapsNumberOfSame_4 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame != this.DPar.StepLapNumberSame))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame = this.DPar.StepLapNumberSame;
            }
            if (this.RBPar.StepLapsValue_1 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.Value != this.DefaultPar.StepLapsValue_1))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.Value = this.DefaultPar.StepLapsValue_1;
            }
            if (this.RBPar.StepLapsValue_2 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.Value != this.DefaultPar.StepLapsValue_2))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.Value = this.DefaultPar.StepLapsValue_2;
            }
            if (this.RBPar.StepLapsValue_3 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.Value != this.DefaultPar.StepLapsValue_3))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.Value = this.DefaultPar.StepLapsValue_3;
            }
            if (this.RBPar.StepLapsValue_4 && (this.ProductView.SelectedLayerView.StepLapsDefaultView.Value != this.DPar.StepLapStepValue))
            {
                this.ProductView.SelectedLayerView.StepLapsDefaultView.Value = this.DPar.StepLapStepValue;
            }
        }

        private void HandleSelectedAndChangedTipCutDefaultPar()
        {
            if (this.RBPar.TipCutsHeight_1 && (this.ProductView.SelectedLayerView.TipCutsDefaultView.Height != this.DefaultPar.TipCutsHeight_1))
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.Height = this.DefaultPar.TipCutsHeight_1;
            }
            if (this.RBPar.TipCutsHeight_2 && (this.ProductView.SelectedLayerView.TipCutsDefaultView.Height != this.DefaultPar.TipCutsHeight_2))
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.Height = this.DefaultPar.TipCutsHeight_2;
            }
            if (this.RBPar.TipCutsHeight_3 && (this.ProductView.SelectedLayerView.TipCutsDefaultView.Height != this.DefaultPar.TipCutsHeight_3))
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.Height = this.DefaultPar.TipCutsHeight_3;
            }
            if (this.RBPar.TipCutsHeight_4 && (this.ProductView.SelectedLayerView.TipCutsDefaultView.Height != this.DPar.TipCutHeight))
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.Height = this.DPar.TipCutHeight;
            }
            if (this.RBPar.TipCutsOverCut_1 && (this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut != this.DefaultPar.TipCutsOverCut_1))
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut = this.DefaultPar.TipCutsOverCut_1;
            }
            if (this.RBPar.TipCutsOverCut_2 && (this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut != this.DefaultPar.TipCutsOverCut_2))
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut = this.DefaultPar.TipCutsOverCut_2;
            }
            if (this.RBPar.TipCutsOverCut_3 && (this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut != this.DefaultPar.TipCutsOverCut_3))
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut = this.DefaultPar.TipCutsOverCut_3;
            }
            if (this.RBPar.TipCutsOverCut_4 && (this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut != this.DPar.TipCutOverCut))
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut = this.DPar.TipCutOverCut;
            }
        }

        private void HeightCorrTypePar(object parameter)
        {
            this.ProductView.SelectedLayerView.LayerDefaultView.HeightCorrectionType = (EHeightCorrectionType)parameter;
        }

        private void HeightMeasTypePar(object parameter)
        {
            if (((string)parameter) == "Absolute")
            {
                this.ProductView.HeightMeasType = EHeightMeasType.Absolute;
            }
            else
            {
                this.ProductView.HeightMeasType = EHeightMeasType.Relative;
            }
        }

        private void HeightRefTypePar(object parameter)
        {
            if (((string)parameter) == "mm")
            {
                this.ProductView.HeightRefType = EHeightRefType.MM;
            }
            else
            {
                this.ProductView.HeightRefType = EHeightRefType.Number;
            }
        }

        public void ImportJob(string importJobFileName = null, string importJobFilePath = null)
        {
            if (importJobFileName == null)
            {
                this.SelectImportFile(FileSelectorTypes.Load, new Action<string, string>(this.ImportJob));
            }
            else
            {
                string importFullFilename = importJobFileName;
                if (!string.IsNullOrWhiteSpace(importJobFilePath))
                {
                    importFullFilename = Path.Combine(importJobFilePath, importJobFileName);
                }
                if (importFullFilename != "")
                {
                    IImport import = null;
                    VTC_Config config = VTC_Config.Load("JobEditor.Convert.xml");
                    import = new VTC_Import(importFullFilename, this, this.DefaultPar.MaterialThickness_1, config);
                    if (import.ReadFile())
                    {
                        import.Convert(this.ProductView);
                        this.ProductName = import.ProductName;
                        string str2 = import.ProductName + ".xml";
                        string path = Path.Combine(Settings.Default.PathProducts, str2);
                        this.ProductFileFullPath = Path.GetFullPath(path);
                        this.ProductFileValid = FileSelectorFileInfo.IsValidFileName(str2);
                        this.UpdateProductAndCutSequenceVisuals();
                        this.CopyOfLayerView = null;
                    }
                }
            }
        }

        private void InitProductWithFirstDefaultParameters()
        {
            foreach (LayerView local1 in this.ProductView.LayerViews)
            {
                local1.CentersDefaultView.OverCut = this.DefaultPar.CentersOverCut_1;
                local1.LayerDefaultView.MaterialThickness = this.DefaultPar.MaterialThickness_1;
                local1.StepLapsDefaultView.NumberOfSteps = this.DefaultPar.StepLapsNumberOfSteps_1;
                local1.StepLapsDefaultView.NumberOfSame = this.DefaultPar.StepLapsNumberOfSame_1;
                local1.StepLapsDefaultView.Value = this.DefaultPar.StepLapsValue_1;
                local1.TipCutsDefaultView.Height = this.DefaultPar.TipCutsHeight_1;
                local1.TipCutsDefaultView.OverCut = this.DefaultPar.TipCutsOverCut_1;
            }
        }

        public void LoadDefaultPar()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DefaultPar));
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(Settings.Default.DefaultParametersFile);
                this.DefaultPar = (DefaultPar)serializer.Deserialize(reader);
                this.DefaultPar.ViewModel = this;
            }
            catch (IOException)
            {
                char[] separator = new char[] { '\\' };
                string name = Enumerable.Last<string>(Settings.Default.DefaultParametersFile.Split(separator));
                Stream stream = new FileResources().Data.Get<Stream>(name);
                this.DefaultPar = (DefaultPar)serializer.Deserialize(stream);
                this.DefaultPar.ViewModel = this;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void LoadJob(string jobFileName = null, string jobFilePath = null)
        {
            if (jobFileName == null)
            {
                this.SelectFile(FileSelectorTypes.Load, new Action<string, string>(this.LoadJob));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(jobFilePath))
                {
                    this.ProductFileFullPath = jobFileName;
                }
                else
                {
                    this.ProductFileFullPath = Path.Combine(jobFilePath, jobFileName);
                }
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jobFileName);
                Product product = null;
                if (this.ProductFileFullPath != "")
                {
                    StreamReader reader = null;
                    try
                    {
                        reader = new StreamReader(this.ProductFileFullPath);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Could not open file!\nException: " + exception.ToString());
                        return;
                    }
                    try
                    {
                        product = (Product)new XmlSerializer(typeof(Product)).Deserialize(reader);
                    }
                    catch (Exception exception2)
                    {
                        product = null;
                        MessageBox.Show("Could not load file!\nException: " + exception2.ToString());
                        return;
                    }
                    reader.Close();
                }
                this.ProductName = fileNameWithoutExtension;
                if (product == null)
                {
                    this.ProductFileValid = false;
                }
                else
                {
                    this.ProductView.Product = product;
                    this.ProductFileValid = true;
                    this.UpdateProductAndCutSequenceVisuals();
                    this.CopyOfLayerView = null;
                }
            }
        }

        public void LogInAction(bool loggedIn, string info, string pinCode, uint level)
        {
            this.LogIn.LoggedIn = loggedIn;
            this.ProtectEditorPanel = !this.LogIn.LoggedIn;
            this.LogIn.LoggedInInfo = info;
            this.LogIn.LoggedInPinCode = pinCode;
            this.LogIn.LoggedInLevel = level;
            if (loggedIn)
            {
                this.UpdateAppBarButtonsText();
                this.LogInVisuEna = false;
                if (this.parrent != null)
                {
                    this.parrent.UpdateLoginMenuItemsVisibility(UiElementDataVisibility.Visible, "");
                }
            }
            else if (this.parrent != null)
            {
                this.parrent.UpdateLoginMenuItemsVisibility(UiElementDataVisibility.Hidden, "");
            }
        }

        public void LogInOut()
        {
            if (!this.LogIn.LoggedIn)
            {
                this.EnterPinCode(new Action<bool, string, string, uint>(this.LogInAction));
            }
            else
            {
                this.LogIn.ClearLoggedInData();
                this.ProtectEditorPanel = !this.LogIn.LoggedIn;
                this.UpdateAppBarButtonsText();
                if (this.parrent != null)
                {
                    this.parrent.UpdateLoginMenuItemsVisibility(UiElementDataVisibility.Hidden, "");
                }
                this.UpdateCutSequenceSelector();
            }
        }

        private void MaterialThicknessPar(object parameter)
        {
            this.ProductView.SelectedLayerView.LayerDefaultView.MaterialThickness = (double)parameter;
        }

        public void NewJob()
        {
            this.ProductView.EnableOnSelectedLayerViewChanged(false);
            this.ProductView.EnableOnPropertyChanged = false;
            SequenceMaker seqMaker = new SequenceMaker();
            seqMaker = this.EditorModel.SeqMaker;
            this.SeqMaker = seqMaker;
            this.ProductName = "";
            this.ProductFileFullPath = "";
            this.ProductFileValid = false;
            this.ProductView.Clear();
            this.DPar.Clear();
            this.ProductView.ActLayerNum = 1;
            this.StepLapVisuEna = false;
            this.TipCutVisuEna = false;
            this.HolesVisuEna = false;
            this.SlotsVisuEna = false;
            this.CentersVisuEna = false;
            this.StackerVisuEna = false;
            this.LayersVisuEna = false;
            this.EmptySpaceEna = false;
            this.EmptySpaceWidth = 0.0;
            this.EmptySpaceHeight = 0.0;
            this.SetEditorViewModelDefaultScreen();
            this.CopyOfLayerView = null;
            this.InitProductWithFirstDefaultParameters();
            this.ProductView.EnableOnPropertyChanged = true;
            this.ProductView.Validate();
            this.ProductView.EnableOnSelectedLayerViewChanged(true);
            this.ProductView.CallOnSelectedLayerViewChanged();
        }

        public void NumberOfLayersPar(object parameter)
        {
            int num = (int)parameter;
            int count = this.ProductView.LayerViews.Count;
            this.nmbrLayersToAdd = num - count;
            this.FillProduct(this.nmbrLayersToAdd);
            if (this.ProductView.ActLayerNum > num)
            {
                this.ProductView.ActLayerNum = num;
            }
        }

        private void NumberOfSamePar(object parameter)
        {
            this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSame = (int)parameter;
        }

        private void NumberOfStepsPar(object parameter)
        {
            this.ProductView.SelectedLayerView.StepLapsDefaultView.NumberOfSteps = (int)parameter;
        }

        public void OnCloseFileSelector()
        {
            this.FileSelectorVisuEna = false;
        }

        public void OnCloseLogIn()
        {
            this.LogInVisuEna = false;
            this.UpdateAppBarButtonsText();
        }

        public void OnProductViewPropertyChanged(object propertyParent, string propertyName)
        {
            this.Validate();
            if (this.UpdateAppBarButtonsState != null)
            {
                this.UpdateAppBarButtonsState();
            }
        }

        public void SaveAsJob(string jobFileName = null, string jobFilePath = null)
        {
            Product product = this.FinalizingProduct();
            try
            {
                if (jobFileName == null)
                {
                    this.SelectFile(FileSelectorTypes.Save, new Action<string, string>(this.SaveAsJob));
                    return;
                }
                if (string.IsNullOrWhiteSpace(jobFilePath))
                {
                    this.ProductFileFullPath = jobFileName;
                }
                else
                {
                    this.ProductFileFullPath = Path.Combine(jobFilePath, jobFileName);
                }
                this.ProductName = Path.GetFileNameWithoutExtension(jobFileName);
                if (this.ProductFileFullPath != "")
                {
                    StreamWriter writer = new StreamWriter(this.ProductFileFullPath);
                    new XmlSerializer(typeof(Product)).Serialize((TextWriter)writer, product);
                    writer.Close();
                    this.ProductFileValid = true;
                }
            }
            catch (Exception exception)
            {
                this.ProductFileValid = false;
                MessageBox.Show("Error during saving: " + exception.Message);
            }
            char[] separator = new char[] { '\\' };
            char[] chArray2 = new char[] { '.' };
            this.ProductName = Enumerable.Last<string>(this.ProductFileFullPath.Split(separator)).Split(chArray2)[0];
        }

        public void SaveDefaultPar()
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(Settings.Default.DefaultParametersFile, false);
                new XmlSerializer(typeof(DefaultPar)).Serialize((TextWriter)writer, this.DefaultPar);
                writer.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error during saving: " + exception.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private void SelectCutSequence(CutSequenceItem item)
        {
            this.ProductView.EnableOnSelectedLayerViewChanged(false);
            this.ProductView.EnableOnPropertyChanged = false;
            this.selectedCutSeqIndex = this.cutSequences.IndexOf(item);
            if ((this.selectedCutSeqIndex >= 0) && (this.selectedCutSeqIndex < this.cutSequences.Count))
            {
                LayerView view = this.CreateNewLayer(this.SeqMaker.CutSequences[this.selectedCutSeqIndex], 0);
                this.ProductView.ActLayerNum = 0;
                this.ProductView.LayerViews.Clear();
                this.ProductView.LayerViews.Add(view);
                this.ProductView.ActLayerNum = 1;
                if (this.SeqMaker.CutSequences[this.selectedCutSeqIndex].Shapes.Count < 5)
                {
                    this.EmptySpaceWidth = 0x447;
                    this.EmptySpaceHeight = 440.0;
                    double num = 0.0;
                    foreach (Shape shape in this.SeqMaker.CutSequences[this.selectedCutSeqIndex].Shapes)
                    {
                        if (shape.ShapePartList.Count < 4)
                        {
                            this.EmptySpaceWidth -= 220.0;
                            num += 220.0;
                        }
                        if (shape.ShapePartList.Count >= 4)
                        {
                            this.EmptySpaceWidth -= 220 + (0x48 * (shape.ShapePartList.Count - 3));
                            num += 220 + (0x48 * (shape.ShapePartList.Count - 3));
                        }
                    }
                    if ((this.EmptySpaceWidth < 0.0) || (num > 0x445))
                    {
                        this.EmptySpaceWidth = 0.0;
                        this.EmptySpaceEna = false;
                    }
                }
                else
                {
                    this.EmptySpaceWidth = 0.0;
                    this.EmptySpaceEna = false;
                }
                this.SetCutSeqUsed(item, true);
                this.CutSeqSelected = true;
                this.DPar.NumberLayers = 1;
                this.InitProductWithFirstDefaultParameters();
                this.ProductName = "";
                this.ProductFileFullPath = "";
                this.ProductFileValid = false;
            }
            this.DPar.Clear();
            this.ProductView.EnableOnPropertyChanged = true;
            this.Validate();
            this.ProductView.EnableOnSelectedLayerViewChanged(true);
            this.ProductView.CallOnSelectedLayerViewChanged();
        }

        private void SelectCutSequenceConfirmation(object parameter)
        {
            if (parameter is CutSequenceItem)
            {
                this.SelectCutSequence((CutSequenceItem)parameter);
                if (this.parrent != null)
                {
                    this.parrent.LayersMenuAction(null);
                }
                this.CopyOfLayerView = null;
            }
        }

        private void SelectFile(FileSelectorTypes selectorType, Action<string, string> onOkAction)
        {
            this.FileSelector.OnOk = onOkAction;
            this.FileSelector.Type = selectorType;
            this.FileSelector.ShowFileExtension = false;
            this.FileSelector.DefaultFileExtension = ".xml";
            this.FileSelector.FileSearchPattern = null;
            if (!string.IsNullOrWhiteSpace(this.ProductFileFullPath))
            {
                this.FileSelector.CurrentPath = Path.GetDirectoryName(this.ProductFileFullPath);
                this.FileSelector.CurrentFile = Path.GetFileName(this.ProductFileFullPath);
            }
            else
            {
                this.FileSelector.CurrentPath = Path.GetFullPath(Settings.Default.PathProducts);
                this.FileSelector.CurrentFile = "";
            }
            this.FileSelector.ForceRescanFiles = true;
            this.FileSelectorVisuEna = true;
        }

        private void SelectImportFile(FileSelectorTypes selectorType, Action<string, string> onOkAction)
        {
            this.FileSelector.OnOk = onOkAction;
            this.FileSelector.Type = selectorType;
            this.FileSelector.ShowFileExtension = false;
            this.FileSelector.DefaultFileExtension = Settings.Default.ImportFileExtension;
            this.FileSelector.FileSearchPattern = null;
            if (!string.IsNullOrWhiteSpace(this.ImportFileFullPath))
            {
                this.FileSelector.CurrentPath = Path.GetDirectoryName(this.ImportFileFullPath);
                this.FileSelector.CurrentFile = Path.GetFileName(this.ImportFileFullPath);
            }
            else
            {
                this.FileSelector.CurrentPath = Path.GetFullPath(Settings.Default.ImportPath);
                this.FileSelector.CurrentFile = "";
            }
            this.FileSelector.ForceRescanFiles = true;
            this.FileSelectorVisuEna = true;
        }

        private void SetButtonEditCentersOverCutColor()
        {
            if (this.EditEnaCentersOverCut)
            {
                this.EditButtonBrushCentersOverCut = (SolidColorBrush)Application.Current.TryFindResource("Dark2BaseBackgroundThemeBrush");
            }
            else
            {
                this.EditButtonBrushCentersOverCut = (SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush");
            }
        }

        private void SetButtonEditLayersMaterialThicknessColor()
        {
            if (this.EditEnaLayerSheetThickness)
            {
                this.EditButtonBrushLayersMaterialThickness = (SolidColorBrush)Application.Current.TryFindResource("Dark2BaseBackgroundThemeBrush");
            }
            else
            {
                this.EditButtonBrushLayersMaterialThickness = (SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush");
            }
        }

        private void SetButtonEditNumberLayersColor()
        {
            if (this.EditEnaNumberOfLayers)
            {
                this.EditButtonBrushNumberOfLayers = (SolidColorBrush)Application.Current.TryFindResource("Dark2BaseBackgroundThemeBrush");
            }
            else
            {
                this.EditButtonBrushNumberOfLayers = (SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush");
            }
        }

        private void SetButtonEditNumberOfSameColor()
        {
            if (this.EditEnaNumberOfSame)
            {
                this.EditButtonBrushNumberOfSame = (SolidColorBrush)Application.Current.TryFindResource("Dark2BaseBackgroundThemeBrush");
            }
            else
            {
                this.EditButtonBrushNumberOfSame = (SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush");
            }
        }

        private void SetButtonEditNumberOfStepColor()
        {
            if (this.EditEnaNumberOfStep)
            {
                this.EditButtonBrushNumberOfStep = (SolidColorBrush)Application.Current.TryFindResource("Dark2BaseBackgroundThemeBrush");
            }
            else
            {
                this.EditButtonBrushNumberOfStep = (SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush");
            }
        }

        private void SetButtonEditStepLapValueColor()
        {
            if (this.EditEnaStepLapValue)
            {
                this.EditButtonBrushStepLapValue = (SolidColorBrush)Application.Current.TryFindResource("Dark2BaseBackgroundThemeBrush");
            }
            else
            {
                this.EditButtonBrushStepLapValue = (SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush");
            }
        }

        private void SetButtonEditTipCutHeightColor()
        {
            if (this.EditEnaTipCutHeight)
            {
                this.EditButtonBrushTipCutHeight = (SolidColorBrush)Application.Current.TryFindResource("Dark2BaseBackgroundThemeBrush");
            }
            else
            {
                this.EditButtonBrushTipCutHeight = (SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush");
            }
        }

        private void SetButtonEditTipCutOverCutColor()
        {
            if (this.EditEnaTipCutOverCut)
            {
                this.EditButtonBrushTipCutOverCut = (SolidColorBrush)Application.Current.TryFindResource("Dark2BaseBackgroundThemeBrush");
            }
            else
            {
                this.EditButtonBrushTipCutOverCut = (SolidColorBrush)Application.Current.TryFindResource("Dark1BaseBackgroundThemeBrush");
            }
        }

        private void SetCutSeqUsed(CutSequenceItem item, bool confirmed)
        {
            for (int i = 0; i < this.CutSequences.Count; i++)
            {
                this.CutSequences[i].Used = false;
            }
            if (item != null)
            {
                item.Used = confirmed;
            }
            this.SelectedCutSeqIndex = this.CutSequences.IndexOf(item);
        }

        public void SetEditorViewModelDefaultScreen()
        {
            if (this.CutSequences.Count > 0)
            {
                this.SelectCutSequence(this.CutSequences[0]);
            }
            this.ShowCutSequenceSelector();
        }

        private void SetEmptySpace(ObservableCollection<ShapeView> shapeViews)
        {
            this.EmptySpaceEna = true;
            if (shapeViews.Count < 5)
            {
                this.EmptySpaceWidth = 0x445;
                this.EmptySpaceHeight = 440.0;
                double num = 0.0;
                foreach (ShapeView view in shapeViews)
                {
                    if (view.ShapePartViews.Count < 4)
                    {
                        this.EmptySpaceWidth -= 220.0;
                        num += 220.0;
                    }
                    if (view.ShapePartViews.Count >= 4)
                    {
                        this.EmptySpaceWidth -= 220 + (0x48 * (view.ShapePartViews.Count - 3));
                        num += 220 + (0x48 * (view.ShapePartViews.Count - 3));
                    }
                }
                if ((this.EmptySpaceWidth < 0.0) || (num > 0x445))
                {
                    this.EmptySpaceWidth = 0.0;
                    this.EmptySpaceEna = false;
                }
            }
            else
            {
                this.EmptySpaceWidth = 0.0;
                this.EmptySpaceEna = false;
            }
            if (this.LayersVisuEna)
            {
                this.EmptySpaceHeight = 0x171;
            }
            else
            {
                this.EmptySpaceHeight = 440.0;
            }
        }

        public double ShapeLength(Shape s)
        {
            double num = 0.0;
            if (s.CentersInfo.MeasuringType == EMeasuringType.Absolute)
            {
                if (s.ShapePartList.Count < 3)
                {
                    num = s.CentersInfo.Length1;
                }
                else if (s.ShapePartList.Count == 3)
                {
                    num = s.CentersInfo.Length2;
                }
                else if (s.ShapePartList.Count == 4)
                {
                    num = s.CentersInfo.Length3;
                }
                else if (s.ShapePartList.Count == 5)
                {
                    num = s.CentersInfo.Length4;
                }
                else
                {
                    num = 0.0;
                }
            }
            if (s.CentersInfo.MeasuringType == EMeasuringType.Relative)
            {
                if (s.ShapePartList.Count < 3)
                {
                    num = s.CentersInfo.Length1;
                }
                else if (s.ShapePartList.Count == 3)
                {
                    num = s.CentersInfo.Length1 + s.CentersInfo.Length2;
                }
                else if (s.ShapePartList.Count == 4)
                {
                    num = (s.CentersInfo.Length1 + s.CentersInfo.Length2) + s.CentersInfo.Length3;
                }
                else if (s.ShapePartList.Count == 5)
                {
                    num = ((s.CentersInfo.Length1 + s.CentersInfo.Length2) + s.CentersInfo.Length3) + s.CentersInfo.Length4;
                }
                else
                {
                    num = 0.0;
                }
            }
            if (s.CentersInfo.MeasuringType != EMeasuringType.Simetric)
            {
                return num;
            }
            if (s.ShapePartList.Count < 3)
            {
                return s.CentersInfo.Length1;
            }
            if (s.ShapePartList.Count == 3)
            {
                return s.CentersInfo.Length1;
            }
            if (s.ShapePartList.Count == 4)
            {
                return s.CentersInfo.Length3;
            }
            if (s.ShapePartList.Count == 5)
            {
                return s.CentersInfo.Length4;
            }
            return 0.0;
        }

        public void ShowCutSequenceSelector()
        {
            if (this.parrent != null)
            {
                this.parrent.SetJobEditorLocation(MainWindow.JobEditorLocation.Products);
                this.parrent.SetPageText(this.parrent.GetCurrentJobEditorLocation().ToString());
            }
            this.CutSeqVisuEna = true;
            this.StepLapVisuEna = false;
            this.TipCutVisuEna = false;
            this.HolesVisuEna = false;
            this.SlotsVisuEna = false;
            this.CentersVisuEna = false;
            this.LayersVisuEna = false;
            this.EmptySpaceEna = false;
            this.EmptySpaceHeight = 440.0;
            List<CutSequenceItem> list = Enumerable.ToList<CutSequenceItem>((IEnumerable<CutSequenceItem>)(from x in this.CutSequences
                                                                                                           where x.Used
                                                                                                           select x));
            if ((list != null) && (list.Count > 0))
            {
                if (this.parrent != null)
                {
                    this.parrent.CutSeqList.ScrollIntoView(list[0]);
                }
                list[0].Used = !list[0].Used;
                list[0].Used = !list[0].Used;
            }
        }

        private void StepLapValuePar(object parameter)
        {
            this.ProductView.SelectedLayerView.StepLapsDefaultView.Value = (double)parameter;
        }

        private void TipCutHeightPar(object parameter)
        {
            this.ProductView.SelectedLayerView.TipCutsDefaultView.Height = (double)parameter;
        }

        private void TipCutOverCutPar(object parameter)
        {
            this.ProductView.SelectedLayerView.TipCutsDefaultView.OverCut = (double)parameter;
        }

        private void TipCutsDoubleCutPar(object parameter)
        {
            if (((string)parameter) == "ON")
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.DoubleCut = true;
            }
            else
            {
                this.ProductView.SelectedLayerView.TipCutsDefaultView.DoubleCut = false;
            }
        }

        public void UpdateCutSequenceSelector()
        {
            List<CutSequenceItem> list = Enumerable.ToList<CutSequenceItem>((IEnumerable<CutSequenceItem>)(from x in this.CutSequences
                                                                                                           where x.Used
                                                                                                           select x));
            if (((list != null) && (list.Count > 0)) && (this.parrent != null))
            {
                this.parrent.CutSeqList.ScrollIntoView(list[0]);
            }
        }

        private void UpdateProductAndCutSequenceVisuals()
        {
            if (this.ProductView.LayerViews.Count > 0)
            {
                this.SetCutSeqUsed(this.GetCutSquenceItem(this.ProductView.LayerViews[0].Name), true);
            }
            this.UpdateCutSequenceSelector();
            this.DPar.NumberLayers = this.ProductView.LayerViews.Count;
            this.CutSeqSelected = true;
            if (!this.CutSeqVisuEna)
            {
                if (this.StepLapVisuEna1)
                {
                    this.StepLapVisuEna = true;
                }
                if (this.TipCutVisuEna1)
                {
                    this.TipCutVisuEna = true;
                }
                if (this.HolesVisuEna1)
                {
                    this.HolesVisuEna = true;
                }
                if (this.SlotsVisuEna1)
                {
                    this.SlotsVisuEna = true;
                }
                if (this.CentersVisuEna1)
                {
                    this.CentersVisuEna = true;
                }
                if (this.LayersVisuEna1)
                {
                    this.LayersVisuEna = true;
                }
            }
            this.SetEmptySpace(this.ProductView.SelectedLayerView.ShapeViews);
            if (((!this.StepLapVisuEna && !this.TipCutVisuEna) && (!this.HolesVisuEna && !this.SlotsVisuEna)) && !this.CentersVisuEna)
            {
                this.EmptySpaceEna = false;
            }
        }

        public void Validate()
        {
            this.ProductView.Validate();
            this.DefaultPar.Validate();
            this.DPar.Validate();
        }

        // Properties
        public string ProductFileFullPath { get; private set; }

        public string ImportFileFullPath { get; private set; }

        public bool ProductFileValid
        {
            get =>
                this.productFileValid;
            private set
            {
                this.productFileValid = value;
                if (this.UpdateAppBarButtonsState != null)
                {
                    this.UpdateAppBarButtonsState();
                }
            }
        }

        public bool ProductValidForActivation
        {
            get
            {
                bool flag = true;
                if (!this.ProductFileValid)
                {
                    flag = false;
                }
                if (!this.ProductView.Valid)
                {
                    flag = false;
                }
                return flag;
            }
        }

        public Action UpdateAppBarButtonsState
        {
            get =>
                this.updateAppBarButtonsState;
            set =>
                this.updateAppBarButtonsState = value;
        }

        public Action UpdateAppBarButtonsText
        {
            get =>
                this.updateAppBarButtonsText;
            set =>
                this.updateAppBarButtonsText = value;
        }

        public ProductView ProductView
        {
            get =>
                this.productView;
            set
            {
                this.productView = value;
                base.RaisePropertyChanged("ProductView");
            }
        }

        public bool StepLapVisuEna1 { get; set; }

        public bool TipCutVisuEna1 { get; set; }

        public bool HolesVisuEna1 { get; set; }

        public bool SlotsVisuEna1 { get; set; }

        public bool CentersVisuEna1 { get; set; }

        public bool StackerVisuEna1 { get; set; }

        public bool LayersVisuEna1 { get; set; }

        public int SelectedCutSeqIndex
        {
            get =>
                this.selectedCutSeqIndex;
            set =>
                this.selectedCutSeqIndex = value;
        }

        public LayerView CopyOfLayerView
        {
            get =>
                this.copyOfLayerView;
            set =>
                this.copyOfLayerView = value;
        }

        public DefaultPar DefaultPar
        {
            get =>
                this.defaultPar;
            set =>
                this.defaultPar = value;
        }

        public JobEditorFileSelector FileSelector =>
            this.fileSelector;

        public JobEditorLogin LogIn =>
            this.logIn;

        public SolidColorBrush EditButtonBrushNumberOfStep
        {
            get =>
                this.editButtonBrushNumberOfStep;
            set
            {
                this.editButtonBrushNumberOfStep = value;
                base.RaisePropertyChanged("EditButtonBrushNumberOfStep");
            }
        }

        public SolidColorBrush EditButtonBrushTipCutHeight
        {
            get =>
                this.editButtonBrushTipCutHeight;
            set
            {
                this.editButtonBrushTipCutHeight = value;
                base.RaisePropertyChanged("EditButtonBrushTipCutHeight");
            }
        }

        public SolidColorBrush EditButtonBrushTipCutOverCut
        {
            get =>
                this.editButtonBrushTipCutOverCut;
            set
            {
                this.editButtonBrushTipCutOverCut = value;
                base.RaisePropertyChanged("EditButtonBrushTipCutOverCut");
            }
        }

        public SolidColorBrush EditButtonBrushNumberOfSame
        {
            get =>
                this.editButtonBrushNumberOfSame;
            set
            {
                this.editButtonBrushNumberOfSame = value;
                base.RaisePropertyChanged("EditButtonBrushNumberOfSame");
            }
        }

        public SolidColorBrush EditButtonBrushStepLapValue
        {
            get =>
                this.editButtonBrushStepLapValue;
            set
            {
                this.editButtonBrushStepLapValue = value;
                base.RaisePropertyChanged("EditButtonBrushStepLapValue");
            }
        }

        public SolidColorBrush EditButtonBrushHolesYOffset
        {
            get =>
                this.editButtonBrushHolesYOffset;
            set
            {
                this.editButtonBrushHolesYOffset = value;
                base.RaisePropertyChanged("EditButtonBrushHolesYOffset");
            }
        }

        public SolidColorBrush EditButtonBrushCentersOverCut
        {
            get =>
                this.editButtonBrushCentersOverCut;
            set
            {
                this.editButtonBrushCentersOverCut = value;
                base.RaisePropertyChanged("EditButtonBrushCentersOverCut");
            }
        }

        public SolidColorBrush EditButtonBrushCentersVOffset
        {
            get =>
                this.editButtonBrushCentersVOffset;
            set
            {
                this.editButtonBrushCentersVOffset = value;
                base.RaisePropertyChanged("EditButtonBrushCentersVOffset");
            }
        }

        public SolidColorBrush EditButtonBrushNumberOfLayers
        {
            get =>
                this.editButtonBrushNumberOfLayers;
            set
            {
                this.editButtonBrushNumberOfLayers = value;
                base.RaisePropertyChanged("EditButtonBrushNumberOfLayers");
            }
        }

        public SolidColorBrush EditButtonBrushLayersMaterialThickness
        {
            get =>
                this.editButtonBrushLayersMaterialThickness;
            set
            {
                this.editButtonBrushLayersMaterialThickness = value;
                base.RaisePropertyChanged("EditButtonBrushLayersMaterialThickness");
            }
        }

        public List<CutSequenceItem> CutSequences =>
            this.cutSequences;

        public bool ProtectEditorPanel
        {
            get =>
                this.protectEditorPanel;
            set
            {
                this.protectEditorPanel = value;
                base.RaisePropertyChanged("ProtectEditorPanel");
            }
        }

        public bool CutSeqVisuEna
        {
            get =>
                this.cutSeqVisuEna;
            set
            {
                this.cutSeqVisuEna = value;
                base.RaisePropertyChanged("CutSeqVisuEna");
            }
        }

        public bool StepLapVisuEna
        {
            get =>
                this.stepLapVisuEna;
            set
            {
                this.stepLapVisuEna = value;
                base.RaisePropertyChanged("StepLapVisuEna");
            }
        }

        public bool TipCutVisuEna
        {
            get =>
                this.tipCutVisuEna;
            set
            {
                this.tipCutVisuEna = value;
                base.RaisePropertyChanged("TipCutVisuEna");
            }
        }

        public bool HolesVisuEna
        {
            get =>
                this.holesVisuEna;
            set
            {
                this.holesVisuEna = value;
                base.RaisePropertyChanged("HolesVisuEna");
            }
        }

        public bool SlotsVisuEna
        {
            get =>
                this.slotsVisuEna;
            set
            {
                this.slotsVisuEna = value;
                base.RaisePropertyChanged("SlotsVisuEna");
            }
        }

        public bool CentersVisuEna
        {
            get =>
                this.centersVisuEna;
            set
            {
                this.centersVisuEna = value;
                base.RaisePropertyChanged("CentersVisuEna");
            }
        }

        public bool StackerVisuEna
        {
            get =>
                this.stackerVisuEna;
            set
            {
                this.stackerVisuEna = value;
                base.RaisePropertyChanged("StackerVisuEna");
            }
        }

        public bool LayersVisuEna
        {
            get =>
                this.layersVisuEna;
            set
            {
                this.layersVisuEna = value;
                base.RaisePropertyChanged("LayersVisuEna");
            }
        }

        public bool EditEnaTipCutHeight
        {
            get =>
                this.editEnaTipCutHeight;
            set
            {
                this.editEnaTipCutHeight = value;
                base.RaisePropertyChanged("EditEnaTipCutHeight");
            }
        }

        public bool EditEnaTipCutOverCut
        {
            get =>
                this.editEnaTipCutOverCut;
            set
            {
                this.editEnaTipCutOverCut = value;
                base.RaisePropertyChanged("EditEnaTipCutOverCut");
            }
        }

        public bool EditEnaNumberOfStep
        {
            get =>
                this.editEnaNumberOfStep;
            set
            {
                this.editEnaNumberOfStep = value;
                base.RaisePropertyChanged("EditEnaNumberOfStep");
            }
        }

        public bool EditEnaNumberOfSame
        {
            get =>
                this.editEnaNumberOfSame;
            set
            {
                this.editEnaNumberOfSame = value;
                base.RaisePropertyChanged("EditEnaNumberOfSame");
            }
        }

        public bool EditEnaStepLapValue
        {
            get =>
                this.editEnaStepLapValue;
            set
            {
                this.editEnaStepLapValue = value;
                base.RaisePropertyChanged("EditEnaStepLapValue");
            }
        }

        public bool EditEnaHolesYOffset
        {
            get =>
                this.editEnaHolesYOffset;
            set
            {
                this.editEnaHolesYOffset = value;
                base.RaisePropertyChanged("EditEnaHolesYOffset");
            }
        }

        public bool EditEnaCentersOverCut
        {
            get =>
                this.editEnaCentersOverCut;
            set
            {
                this.editEnaCentersOverCut = value;
                base.RaisePropertyChanged("EditEnaCentersOverCut");
            }
        }

        public bool EditEnaCentersVOffset
        {
            get =>
                this.editEnaCentersVOffset;
            set
            {
                this.editEnaCentersVOffset = value;
                base.RaisePropertyChanged("EditEnaCentersVOffset");
            }
        }

        public bool EditEnaNumberOfLayers
        {
            get =>
                this.editEnaNumberOfLayers;
            set
            {
                this.editEnaNumberOfLayers = value;
                base.RaisePropertyChanged("EditEnaNumberOfLayers");
            }
        }

        public bool EditEnaLayerSheetThickness
        {
            get =>
                this.editEnaLayerSheetThickness;
            set
            {
                this.editEnaLayerSheetThickness = value;
                base.RaisePropertyChanged("EditEnaLayerSheetThickness");
            }
        }

        public double EmptySpaceWidth
        {
            get =>
                this.emptySpaceWidth;
            set
            {
                this.emptySpaceWidth = value;
                base.RaisePropertyChanged("EmptySpaceWidth");
            }
        }

        public double EmptySpaceHeight
        {
            get =>
                this.emptySpaceHeight;
            set
            {
                this.emptySpaceHeight = value;
                base.RaisePropertyChanged("EmptySpaceHeight");
            }
        }

        public bool EmptySpaceEna
        {
            get =>
                this.emptySpaceEna;
            set
            {
                this.emptySpaceEna = value;
                base.RaisePropertyChanged("EmptySpaceEna");
            }
        }

        public RadioButtonsPar RBPar
        {
            get =>
                this.rBPar;
            set =>
                this.rBPar = value;
        }

        public DataPar DPar { get; set; }

        public string ProductName
        {
            get =>
                this.productName;
            set
            {
                this.productName = value;
                base.RaisePropertyChanged("ProductName");
            }
        }

        public string UnitInchMMText
        {
            get
            {
                if (!Settings.Default.UnitsInInches)
                {
                    return "mm";
                }
                return "inch";
            }
        }

        public bool JobEditorVisuEna =>
            (!this.fileSelectorVisUEna && !this.logInVisUEna);

        public bool FileSelectorVisuEna
        {
            get
            {
                if (this.logInVisUEna)
                {
                    this.fileSelectorVisUEna = false;
                }
                return this.fileSelectorVisUEna;
            }
            set
            {
                this.fileSelectorVisUEna = value;
                if (value)
                {
                    base.RaisePropertyChanged("JobEditorVisuEna");
                    base.RaisePropertyChanged("LogInVisuEna");
                    base.RaisePropertyChanged("FileSelectorVisuEna");
                }
                else
                {
                    base.RaisePropertyChanged("FileSelectorVisuEna");
                    base.RaisePropertyChanged("LogInVisuEna");
                    base.RaisePropertyChanged("JobEditorVisuEna");
                }
            }
        }

        public bool LogInVisuEna
        {
            get
            {
                if (this.fileSelectorVisUEna)
                {
                    this.logInVisUEna = false;
                }
                return this.logInVisUEna;
            }
            set
            {
                this.logInVisUEna = value;
                if (value)
                {
                    base.RaisePropertyChanged("JobEditorVisuEna");
                    base.RaisePropertyChanged("FileSelectorVisuEna");
                    base.RaisePropertyChanged("LogInVisuEna");
                }
                else
                {
                    base.RaisePropertyChanged("LogInVisuEna");
                    base.RaisePropertyChanged("FileSelectorVisuEna");
                    base.RaisePropertyChanged("JobEditorVisuEna");
                }
            }
        }

        public RelayCommand EditNumberOfStepDefParCommand { get; set; }

        public RelayCommand EditNumberOfSameDefParCommand { get; set; }

        public RelayCommand EditStepLapValueDefParCommand { get; set; }

        public RelayCommand EditCentersOverCutDefParCommand { get; set; }

        public RelayCommand EditNumberOfLayersDefParCommand { get; set; }

        public RelayCommand EditLayersMaterialThicknessDefParCommand { get; set; }

        public RelayCommand EditTipCutHeightDefParCommand { get; set; }

        public RelayCommand EditTipCutOverCutDefParCommand { get; set; }

        public RelayCommand ChooseCutSequenceCommand { get; set; }

        public RelayCommand SelectCutSequenceConfirmCommand { get; set; }

        public RelayCommand NumberOfStepsParCommand { get; set; }

        public RelayCommand NumberOfSameParCommand { get; set; }

        public RelayCommand StepLapValueParCommand { get; set; }

        public RelayCommand CentersOverCutParCommand { get; set; }

        public RelayCommand CentersDoubleCutParCommand { get; set; }

        public RelayCommand NumberOfLayersParCommand { get; set; }

        public RelayCommand HeightRefTypeParCommand { get; set; }

        public RelayCommand HeightMeasTypeParCommand { get; set; }

        public RelayCommand HeightCorrTypeParCommand { get; set; }

        public RelayCommand MaterialThicknessParCommand { get; set; }

        public RelayCommand TipCutHeightParCommand { get; set; }

        public RelayCommand TipCutOverCutParCommand { get; set; }

        public RelayCommand TipCutsDoubleCutParCommand { get; set; }

    }
}