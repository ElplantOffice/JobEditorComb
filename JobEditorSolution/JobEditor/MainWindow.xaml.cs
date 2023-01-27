using JobEditor.ViewModels;
using JobEditor.Properties;
using JobEditor.Views.ProductData;
using JobEditor.CutSeqGenerator; 
using Communication.Pc;
using Communication.Plc.Shared;
using JobEditor.AppBar;
using JobEditor.Menu;
using Messages;
using Models;
using Patterns.EventAggregator;
using Services.TRL;
using UserControls.FileSelector;
using UserControls.Login;
using CustomControlLibrary;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using Expression = System.Linq.Expressions.Expression;
using Utils;
using ProductLib;
using TwinCAT.Ads;
using System.Runtime.InteropServices;
using JobEditor.DokTest;

namespace JobEditor
{
    public partial class MainWindow : CustomControlLibrary.Window
    {
        private object lockNavigate = new object();

        private UiLocalisation localisation = SingletonProvider<UiLocalisation>.Instance;

        private const string moduleNameClient = "JobEditor";

        private const string moduleNameServer = "Server";

        private MainWindow.JobEditorLocation location = MainWindow.JobEditorLocation.Products;

        private MainWindow.JobEditorLocation previousLocation = MainWindow.JobEditorLocation.Products;

        private ViewModelAdministrator viewModelAdministrator;

        private ModelAdministrator modelAdministrator;

        private Dictionary<string, Models.ModuleInfo> Modules = new Dictionary<string, Models.ModuleInfo>();

        private Messages.Address Address;

        private Dictionary<string, Action<Telegram>> modelAddressDict = new Dictionary<string, Action<Telegram>>();

        private List<short> treeLevelId = new List<short>();

        private CommAdministrator CommAdmin;

        private ICommChannel CommChannel;

        private ActivationClient activationClient;

        private bool connectedToServer;

        private bool hmiProcessIsRunning;

        private PlcControlDataRaw appBarButton1;

        private PlcControlDataRaw appBarButton2;

        private PlcControlDataRaw appBarButton3;

        private PlcControlDataRaw appBarButton4;

        private PlcControlDataRaw appBarButton5;

        private PlcControlDataRaw appBarButton6;

        private PlcControlDataRaw appBarButton7;

        private PlcControlDataRaw appBarButton8;

        private PlcControlDataRaw labelbutton1;

        private PlcControlDataRaw labelbutton2;

        private PlcControlDataRaw labelbutton3;

        private PlcControlDataRaw labelbutton4;

        private PlcControlDataRaw labelbutton5;

        private PlcControlDataRaw labelbutton6;

        private PlcControlDataRaw labelbutton8;

        private string appBarButton1Name;

        private string appBarButton2Name;

        private string appBarButton3Name;

        private string appBarButton4Name;

        private string appBarButton5Name;

        private string appBarButton6Name;

        private string appBarButton7Name;

        private string appBarButton8Name;

        private string appBarLabelButton1Name;

        private string appBarLabelButton2Name;

        private string appBarLabelButton3Name;

        private string appBarLabelButton4Name;

        private string appBarLabelButton5Name;

        private string appBarLabelButton6Name;

        private string appBarLabelButton8Name;

        private string appBarLabelButton1TextId;

        private string appBarLabelButton2TextId;

        private string appBarLabelButton3TextId;

        private string appBarLabelButton4TextId;

        private string appBarLabelButton5TextId;

        private string appBarLabelButton6TextId;

        private string appBarLabelButton8TextId;

        private const string appBarLabelButton8LoggedOn = "Log off";

        private JobEditorFileResources ResourceAssembly = new JobEditorFileResources();

        private EditorViewModel editorViewModel;

        private PlcTreeElementStringRaw? menuAfterNewJob;

        private List<PlcTreeElementStringRaw> menuItemsLoginEna = new List<PlcTreeElementStringRaw>();

        //private bool _contentLoaded;

        public MainWindow()
        {
            if (ProcessHelper.PreventStartupTwice(true))
            {
                return;
            }
            Serializer serializer = new Serializer(new Type[] { typeof(Messages.Address), typeof(Telegram), typeof(MessagePacket), typeof(UIProtoType), typeof(ModelProtoType), typeof(ViewModelProtoType), typeof(Assembly), typeof(List<ViewModelProtoType>), typeof(List<Telegram>), typeof(int), typeof(string) });

            this.InitializeComponent();
            this.Init();
            //this.ConnectToServer();
            this.connectedToServer = isConnectionToPlcAvailable();
            this.DataContext = editorViewModel;
        }

        private bool isConnectionToPlcAvailable()
        {
            bool toReturn = false;
            TcAdsClient tcClient = new TcAdsClient();

            if (Settings.Default.PLC_Active)
            {
                try
                {
                    tcClient.Connect(Settings.Default.PLC_Port);
                    toReturn = true;
                }
                catch (Exception err)
                {
                }
            }
           
            tcClient.Dispose();
            return toReturn;
        }

        private void AppBarCommands(Telegram telegram)
        {
            if (telegram.Command == 2)
            {
                this.BuildAppBar(telegram);
            }
        }

        private void BuildAppBar(Telegram telegram)
        {
            //SingletonProvider<EventAggregator>.Instance;
            IAddress address = new Messages.Address(this.Modules["JobEditor"].GetId(0), null, "", null);
            this.appBarButton1Name = "Button1";
            this.appBarButton1 = new PlcControlDataRaw();
            this.appBarLabelButton1Name = "LabelButton1";
            this.appBarLabelButton1TextId = "Desktop";
            this.labelbutton1 = new PlcControlDataRaw();
            PlcControlDataRaw plcControlDataRaw = new PlcControlDataRaw();
            this.appBarButton2Name = "Button2";
            this.appBarButton2 = new PlcControlDataRaw();
            this.appBarLabelButton2Name = "LabelButton2";
            this.appBarLabelButton2TextId = "New";
            this.labelbutton2 = new PlcControlDataRaw();
            PlcControlDataRaw plcControlDataRaw1 = new PlcControlDataRaw();
            this.appBarButton3Name = "Button3";
            this.appBarButton3 = new PlcControlDataRaw();
            this.appBarLabelButton3Name = "LabelButton3";
            this.appBarLabelButton3TextId = "Load";
            this.labelbutton3 = new PlcControlDataRaw();
            PlcControlDataRaw plcControlDataRaw2 = new PlcControlDataRaw();
            this.appBarButton4Name = "Button4";
            this.appBarButton4 = new PlcControlDataRaw();
            this.appBarLabelButton4Name = "LabelButton4";
            this.appBarLabelButton4TextId = "Save as";
            this.labelbutton4 = new PlcControlDataRaw();
            PlcControlDataRaw plcControlDataRaw3 = new PlcControlDataRaw();
            this.appBarButton5Name = "Button5";
            this.appBarButton5 = new PlcControlDataRaw();
            this.appBarLabelButton5Name = "LabelButton5";
            this.appBarLabelButton5TextId = "Import";
            this.labelbutton5 = new PlcControlDataRaw();
            PlcControlDataRaw plcControlDataRaw4 = new PlcControlDataRaw();
            this.appBarButton6Name = "Button6";
            this.appBarButton6 = new PlcControlDataRaw();
            this.appBarLabelButton6Name = "LabelButton6";
            this.appBarLabelButton6TextId = "Activate";
            this.labelbutton6 = new PlcControlDataRaw();
            PlcControlDataRaw plcControlDataRaw5 = new PlcControlDataRaw();
            this.appBarButton7Name = "Button7";
            this.appBarButton7 = new PlcControlDataRaw();
            PlcControlDataRaw plcControlDataRaw6 = new PlcControlDataRaw();
            this.appBarButton8Name = "Button8";
            this.appBarButton8 = new PlcControlDataRaw();
            this.appBarLabelButton8Name = "LabelButton8";
            this.appBarLabelButton8TextId = "Log on";
            this.labelbutton8 = new PlcControlDataRaw();
            PlcControlDataRaw plcControlDataRaw7 = new PlcControlDataRaw();
            this.InitAppBarControl(ref this.appBarButton1, true, address, "AppBarModel", this.appBarButton1Name, "", "", "", new Action<Telegram>(this.Button1Action));
            this.InitAppBarControl(ref this.labelbutton1, true, address, "AppBarModel", this.appBarLabelButton1Name, this.appBarLabelButton1TextId, "", "", null);
            this.InitAppBarControl(ref plcControlDataRaw, true, address, "AppBarModel", "ImageButton1", "", "", "Desktop.png", null);
            this.InitAppBarControl(ref this.labelbutton2, true, address, "AppBarModel", this.appBarLabelButton2Name, this.appBarLabelButton2TextId, "", "", null);
            this.InitAppBarControl(ref plcControlDataRaw1, true, address, "AppBarModel", "ImageButton2", "", "", "New.png", null);
            this.InitAppBarControl(ref this.appBarButton2, true, address, "AppBarModel", this.appBarButton2Name, "", "", "", new Action<Telegram>(this.Button2Action));
            this.InitAppBarControl(ref this.labelbutton3, true, address, "AppBarModel", this.appBarLabelButton3Name, this.appBarLabelButton3TextId, "", "", null);
            this.InitAppBarControl(ref plcControlDataRaw2, true, address, "AppBarModel", "ImageButton3", "", "", "Load.png", null);
            this.InitAppBarControl(ref this.appBarButton3, true, address, "AppBarModel", this.appBarButton3Name, "", "", "", new Action<Telegram>(this.Button3Action));
            this.InitAppBarControl(ref this.labelbutton4, true, address, "AppBarModel", this.appBarLabelButton4Name, this.appBarLabelButton4TextId, "", "", null);
            this.InitAppBarControl(ref plcControlDataRaw3, true, address, "AppBarModel", "ImageButton4", "", "", "Save.png", null);
            this.InitAppBarControl(ref this.appBarButton4, true, address, "AppBarModel", this.appBarButton4Name, "", "", "", new Action<Telegram>(this.Button4Action));
            this.InitAppBarControl(ref this.labelbutton5, true, address, "AppBarModel", this.appBarLabelButton5Name, this.appBarLabelButton5TextId, "", "", null);
            this.InitAppBarControl(ref plcControlDataRaw4, true, address, "AppBarModel", "ImageButton5", "", "", "Import.png", null);
            UiElementDataVisibility uiElementDataVisibility = 0;
            if (Settings.Default.ImportEnable)
            {
                uiElementDataVisibility = UiElementDataVisibility.Visible;
            }
            this.InitAppBarControl(ref this.appBarButton5, true, address, "AppBarModel", this.appBarButton5Name, "", "", "", new Action<Telegram>(this.Button5Action), uiElementDataVisibility);
            this.InitAppBarControl(ref this.labelbutton6, true, address, "AppBarModel", this.appBarLabelButton6Name, this.appBarLabelButton6TextId, "", "", null);
            this.InitAppBarControl(ref plcControlDataRaw5, true, address, "AppBarModel", "ImageButton6", "", "", "Apply.png", null);
            this.InitAppBarControl(ref this.appBarButton6, false, address, "AppBarModel", this.appBarButton6Name, "", "", "", new Action<Telegram>(this.Button6Action));
            this.InitAppBarControl(ref this.labelbutton8, true, address, "AppBarModel", this.appBarLabelButton8Name, this.appBarLabelButton8TextId, "", "", null);
            this.InitAppBarControl(ref plcControlDataRaw7, true, address, "AppBarModel", "ImageButton8", "", "", "Login.png", null);
            UiElementDataVisibility uiElementDataVisibility1 = 0;
            if (Settings.Default.LoginEnable)
            {
                uiElementDataVisibility1 = UiElementDataVisibility.Visible;
            }
            this.InitAppBarControl(ref this.appBarButton8, true, address, "AppBarModel", this.appBarButton8Name, "", "", "", new Action<Telegram>(this.Button8Action), uiElementDataVisibility1);
            this.InitAppBarControl(ref plcControlDataRaw6, true, address, "AppBarModel", "ImageButton7", "", "", "elplant.png", null);
            this.InitAppBarControl(ref this.appBarButton7, true, address, "AppBarModel", this.appBarButton7Name, "", "", "", new Action<Telegram>(this.Button7Action));
            this.UpdateAppBarButtonsState();
        }

        private void BuildCentersSubMenu(IEventAggregator aggregator, IAddress address, Telegram telegram, PlcTreeElementStringRaw parentElement, UITreeElementStringCreator creator, PlcTreeElementStringRaw previousMenu, PlcTreeElementStringRaw nextMenu)
        {
            PlcTreeElementStringRaw plcTreeElementStringRaw = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw, ref parentElement, creator.InitMenuData(true, "Job Editor", "JobEditor.png", 1024), false, 0, 0, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw1 = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw1, ref parentElement, creator.InitMenuData(true, "Back", "Back.png", 1024), false, -1, 1, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw2 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw2, ref parentElement, creator.InitMenuData(true, "Down", "MenuDown.png", 1024), address, (Telegram t) => this.MenuNavigateLayerDown(t), true, 2, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw3 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw3, ref parentElement, creator.InitMenuData(true, "Up", "MenuUp.png", 1024), address, (Telegram t) => this.MenuNavigateLayerUp(t), true, 2, 2);
            PlcTreeElementStringRaw plcTreeElementStringRaw4 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw4, ref parentElement, creator.InitMenuData(true, "Copy", "MenuCopy.png", 1024), address, (Telegram t) => this.MenuCopy(t), true, 3, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw4);
            PlcTreeElementStringRaw plcTreeElementStringRaw5 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw5, ref parentElement, creator.InitMenuData(true, "Paste", "MenuPaste.png", 1024), address, (Telegram t) => this.MenuPaste(t), true, 4, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw5);
            PlcTreeElementStringRaw plcTreeElementStringRaw6 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw6, ref parentElement, creator.InitMenuData(true, "Paste All", "MenuPasteAll.png", 1024), address, (Telegram t) => this.MenuPasteAll(t), true, 4, 2);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw6);
            PlcTreeElementStringRaw plcTreeElementStringRaw7 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw7, ref parentElement, creator.InitMenuData(true, "Slots", "Slots.png", 1024), previousMenu, address, (Telegram t) => this.MenuNavigateBackward(t), true, 5, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw8 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw8, ref parentElement, creator.InitMenuData(true, "Layers", "Layers.png", 1024), nextMenu, address, (Telegram t) => this.MenuNavigateForward(t), true, 5, 2);
        }

        private void BuildHolesSubMenu(IEventAggregator aggregator, IAddress address, Telegram telegram, PlcTreeElementStringRaw parentElement, UITreeElementStringCreator creator, PlcTreeElementStringRaw previousMenu, PlcTreeElementStringRaw nextMenu)
        {
            PlcTreeElementStringRaw plcTreeElementStringRaw = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw, ref parentElement, creator.InitMenuData(true, "Job Editor", "JobEditor.png", 1024), false, 0, 0, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw1 = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw1, ref parentElement, creator.InitMenuData(true, "Back", "Back.png", 1024), false, -1, 1, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw2 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw2, ref parentElement, creator.InitMenuData(true, "Down", "MenuDown.png", 1024), address, (Telegram t) => this.MenuNavigateLayerDown(t), true, 2, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw3 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw3, ref parentElement, creator.InitMenuData(true, "Up", "MenuUp.png", 1024), address, (Telegram t) => this.MenuNavigateLayerUp(t), true, 2, 2);
            PlcTreeElementStringRaw plcTreeElementStringRaw4 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw4, ref parentElement, creator.InitMenuData(true, "Copy", "MenuCopy.png", 1024), address, (Telegram t) => this.MenuCopy(t), true, 3, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw4);
            PlcTreeElementStringRaw plcTreeElementStringRaw5 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw5, ref parentElement, creator.InitMenuData(true, "Paste", "MenuPaste.png", 1024), address, (Telegram t) => this.MenuPaste(t), true, 4, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw5);
            PlcTreeElementStringRaw plcTreeElementStringRaw6 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw6, ref parentElement, creator.InitMenuData(true, "Paste All", "MenuPasteAll.png", 1024), address, (Telegram t) => this.MenuPasteAll(t), true, 4, 2);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw6);
            PlcTreeElementStringRaw plcTreeElementStringRaw7 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw7, ref parentElement, creator.InitMenuData(true, "StepLaps", "StepLaps.png", 1024), previousMenu, address, (Telegram t) => this.MenuNavigateBackward(t), true, 5, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw8 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw8, ref parentElement, creator.InitMenuData(true, "Tipcuts", "TipCuts.png", 1024), nextMenu, address, (Telegram t) => this.MenuNavigateForward(t), true, 5, 2);
        }

        private void BuildLayersSubMenu(IEventAggregator aggregator, IAddress address, Telegram telegram, PlcTreeElementStringRaw parentElement, UITreeElementStringCreator creator, PlcTreeElementStringRaw previousMenu, PlcTreeElementStringRaw nextMenu)
        {
            PlcTreeElementStringRaw plcTreeElementStringRaw = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw, ref parentElement, creator.InitMenuData(true, "Job Editor", "JobEditor.png", 1024), false, 0, 0, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw1 = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw1, ref parentElement, creator.InitMenuData(true, "Back", "Back.png", 1024), false, -1, 1, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw2 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw2, ref parentElement, creator.InitMenuData(true, "Down", "MenuDown.png", 1024), address, (Telegram t) => this.MenuNavigateLayerDown(t), true, 2, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw3 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw3, ref parentElement, creator.InitMenuData(true, "Up", "MenuUp.png", 1024), address, (Telegram t) => this.MenuNavigateLayerUp(t), true, 2, 2);
            PlcTreeElementStringRaw plcTreeElementStringRaw4 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw4, ref parentElement, creator.InitMenuData(true, "Copy", "MenuCopy.png", 1024), address, (Telegram t) => this.MenuCopy(t), true, 3, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw4);
            PlcTreeElementStringRaw plcTreeElementStringRaw5 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw5, ref parentElement, creator.InitMenuData(true, "Paste", "MenuPaste.png", 1024), address, (Telegram t) => this.MenuPaste(t), true, 4, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw5);
            PlcTreeElementStringRaw plcTreeElementStringRaw6 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw6, ref parentElement, creator.InitMenuData(true, "Paste All", "MenuPasteAll.png", 1024), address, (Telegram t) => this.MenuPasteAll(t), true, 4, 2);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw6);
            PlcTreeElementStringRaw plcTreeElementStringRaw7 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw7, ref parentElement, creator.InitMenuData(true, "Centers", "Centers.png", 1024), previousMenu, address, (Telegram t) => this.MenuNavigateBackward(t), true, 5, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw8 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw8, ref parentElement, creator.InitMenuData(true, "Steplaps", "Steplaps.png", 1024), nextMenu, address, (Telegram t) => this.MenuNavigateForward(t), true, 5, 2);
        }

        private void BuildMenu(Telegram telegram)
        {
            IEventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            List<object> objs = new List<object>();
            UITreeElementStringCreator uITreeElementStringCreator = new UITreeElementStringCreator(objs);
            IAddress address = new Messages.Address(this.Modules["JobEditor"].GetId(0), null, "", null);
            PlcTreeElementStringRaw plcTreeElementStringRaw = new PlcTreeElementStringRaw();
            uITreeElementStringCreator.RootMenu(ref plcTreeElementStringRaw, uITreeElementStringCreator.InitMenuData(true, "", "", 1024));
            PlcTreeElementStringRaw plcTreeElementStringRaw1 = new PlcTreeElementStringRaw();
            uITreeElementStringCreator.AddChildAction(ref plcTreeElementStringRaw1, ref plcTreeElementStringRaw, uITreeElementStringCreator.InitMenuData(true, "Job Editor", "JobEditor.png", 1024), null, null, false, 0, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw2 = new PlcTreeElementStringRaw();
            uITreeElementStringCreator.AddChildAction(ref plcTreeElementStringRaw2, ref plcTreeElementStringRaw, uITreeElementStringCreator.InitMenuData(true, "Products", "Products.png", 1024), address, (Telegram t) => this.CutSequencesMenuAction(t), true, 1, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw3 = new PlcTreeElementStringRaw();
            uITreeElementStringCreator.AddChildAction(ref plcTreeElementStringRaw3, ref plcTreeElementStringRaw, uITreeElementStringCreator.InitMenuData(true, "Layers", "Layers.png", 1024), address, (Telegram t) => this.LayersMenuAction(t), true, 1, 1, 2);
            PlcTreeElementStringRaw plcTreeElementStringRaw4 = new PlcTreeElementStringRaw();
            uITreeElementStringCreator.AddChildAction(ref plcTreeElementStringRaw4, ref plcTreeElementStringRaw, uITreeElementStringCreator.InitMenuData(true, "StepLaps", "StepLaps.png", 1024), address, (Telegram t) => this.StepLapsMenuAction(t), true, 1, 2, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw5 = new PlcTreeElementStringRaw();
            uITreeElementStringCreator.AddChildAction(ref plcTreeElementStringRaw5, ref plcTreeElementStringRaw, uITreeElementStringCreator.InitMenuData(true, "Holes", "Holes.png", 1024), address, (Telegram t) => this.HolesMenuAction(t), true, 1, 2, 2);
            PlcTreeElementStringRaw plcTreeElementStringRaw6 = new PlcTreeElementStringRaw();
            uITreeElementStringCreator.AddChildAction(ref plcTreeElementStringRaw6, ref plcTreeElementStringRaw, uITreeElementStringCreator.InitMenuData(true, "TipCuts", "TipCuts.png"), address, (Telegram t) => this.TipCutsMenuAction(t), true, 1, 3, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw7 = new PlcTreeElementStringRaw();
            uITreeElementStringCreator.AddChildAction(ref plcTreeElementStringRaw7, ref plcTreeElementStringRaw, uITreeElementStringCreator.InitMenuData(true, "Slots", "Slots.png"), address, (Telegram t) => this.SlotsMenuAction(t), true, 1, 3, 2);
            PlcTreeElementStringRaw plcTreeElementStringRaw8 = new PlcTreeElementStringRaw();
            uITreeElementStringCreator.AddChildAction(ref plcTreeElementStringRaw8, ref plcTreeElementStringRaw, uITreeElementStringCreator.InitMenuData(true, "Centers", "Centers.png", 1024), address, (Telegram t) => this.CentersMenuAction(t), true, 1, 4, 1);
            this.BuildLayersSubMenu(instance, address, telegram, plcTreeElementStringRaw3, uITreeElementStringCreator, plcTreeElementStringRaw8, plcTreeElementStringRaw4);
            this.BuildSteplapsSubMenu(instance, address, telegram, plcTreeElementStringRaw4, uITreeElementStringCreator, plcTreeElementStringRaw3, plcTreeElementStringRaw5);
            this.BuildHolesSubMenu(instance, address, telegram, plcTreeElementStringRaw5, uITreeElementStringCreator, plcTreeElementStringRaw4, plcTreeElementStringRaw6);
            this.BuildTipcutsSubMenu(instance, address, telegram, plcTreeElementStringRaw6, uITreeElementStringCreator, plcTreeElementStringRaw5, plcTreeElementStringRaw7);
            this.BuildSlotsSubMenu(instance, address, telegram, plcTreeElementStringRaw7, uITreeElementStringCreator, plcTreeElementStringRaw6, plcTreeElementStringRaw8);
            this.BuildCentersSubMenu(instance, address, telegram, plcTreeElementStringRaw8, uITreeElementStringCreator, plcTreeElementStringRaw7, plcTreeElementStringRaw3);
            this.menuAfterNewJob = new PlcTreeElementStringRaw?(plcTreeElementStringRaw);
            Telegram telegram1 = new Telegram(new Messages.Address(this.Modules["JobEditor"].GetId(0), string.Concat(this.Modules["JobEditor"].GetId(0), ".MenuModel"), "", "Tree"), 0, objs, null);
            string target = telegram1.Address.Target;
            instance.Publish<Telegram>(telegram1, target, false);
            if (Settings.Default.LoginEnable)
            {
                this.UpdateLoginMenuItemsVisibility(UiElementDataVisibility.Hidden, target);
            }
            this.SetPageText(this.GetCurrentJobEditorLocation().ToString());
        }

        private void BuildSlotsSubMenu(IEventAggregator aggregator, IAddress address, Telegram telegram, PlcTreeElementStringRaw parentElement, UITreeElementStringCreator creator, PlcTreeElementStringRaw previousMenu, PlcTreeElementStringRaw nextMenu)
        {
            PlcTreeElementStringRaw plcTreeElementStringRaw = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw, ref parentElement, creator.InitMenuData(true, "Job Editor", "JobEditor.png", 1024), false, 0, 0, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw1 = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw1, ref parentElement, creator.InitMenuData(true, "Back", "Back.png", 1024), false, -1, 1, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw2 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw2, ref parentElement, creator.InitMenuData(true, "Down", "MenuDown.png", 1024), address, (Telegram t) => this.MenuNavigateLayerDown(t), true, 2, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw3 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw3, ref parentElement, creator.InitMenuData(true, "Up", "MenuUp.png", 1024), address, (Telegram t) => this.MenuNavigateLayerUp(t), true, 2, 2);
            PlcTreeElementStringRaw plcTreeElementStringRaw4 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw4, ref parentElement, creator.InitMenuData(true, "Copy", "MenuCopy.png", 1024), address, (Telegram t) => this.MenuCopy(t), true, 3, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw4);
            PlcTreeElementStringRaw plcTreeElementStringRaw5 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw5, ref parentElement, creator.InitMenuData(true, "Paste", "MenuPaste.png", 1024), address, (Telegram t) => this.MenuPaste(t), true, 4, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw5);
            PlcTreeElementStringRaw plcTreeElementStringRaw6 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw6, ref parentElement, creator.InitMenuData(true, "Paste All", "MenuPasteAll.png", 1024), address, (Telegram t) => this.MenuPasteAll(t), true, 4, 2);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw6);
            PlcTreeElementStringRaw plcTreeElementStringRaw7 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw7, ref parentElement, creator.InitMenuData(true, "Tipcuts", "TipCuts.png", 1024), previousMenu, address, (Telegram t) => this.MenuNavigateBackward(t), true, 5, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw8 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw8, ref parentElement, creator.InitMenuData(true, "Centers", "Centers.png", 1024), nextMenu, address, (Telegram t) => this.MenuNavigateForward(t), true, 5, 2);
        }

        private void BuildSteplapsSubMenu(IEventAggregator aggregator, IAddress address, Telegram telegram, PlcTreeElementStringRaw parentElement, UITreeElementStringCreator creator, PlcTreeElementStringRaw previousMenu, PlcTreeElementStringRaw nextMenu)
        {
            PlcTreeElementStringRaw plcTreeElementStringRaw = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw, ref parentElement, creator.InitMenuData(true, "Job Editor", "JobEditor.png", 1024), false, 0, 0, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw1 = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw1, ref parentElement, creator.InitMenuData(true, "Back", "Back.png", 1024), false, -1, 1, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw2 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw2, ref parentElement, creator.InitMenuData(true, "Down", "MenuDown.png", 1024), address, (Telegram t) => this.MenuNavigateLayerDown(t), true, 2, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw3 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw3, ref parentElement, creator.InitMenuData(true, "Up", "MenuUp.png", 1024), address, (Telegram t) => this.MenuNavigateLayerUp(t), true, 2, 2);
            PlcTreeElementStringRaw plcTreeElementStringRaw4 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw4, ref parentElement, creator.InitMenuData(true, "Copy", "MenuCopy.png", 1024), address, (Telegram t) => this.MenuCopy(t), true, 3, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw4);
            PlcTreeElementStringRaw plcTreeElementStringRaw5 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw5, ref parentElement, creator.InitMenuData(true, "Paste", "MenuPaste.png", 1024), address, (Telegram t) => this.MenuPaste(t), true, 4, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw5);
            PlcTreeElementStringRaw plcTreeElementStringRaw6 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw6, ref parentElement, creator.InitMenuData(true, "Paste All", "MenuPasteAll.png", 1024), address, (Telegram t) => this.MenuPasteAll(t), true, 4, 2);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw6);
            PlcTreeElementStringRaw plcTreeElementStringRaw7 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw7, ref parentElement, creator.InitMenuData(true, "Layers", "Layers.png", 1024), previousMenu, address, (Telegram t) => this.MenuNavigateBackward(t), true, 5, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw8 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw8, ref parentElement, creator.InitMenuData(true, "Holes", "Holes.png", 1024), nextMenu, address, (Telegram t) => this.MenuNavigateForward(t), true, 5, 2);
        }

        private void BuildTipcutsSubMenu(IEventAggregator aggregator, IAddress address, Telegram telegram, PlcTreeElementStringRaw parentElement, UITreeElementStringCreator creator, PlcTreeElementStringRaw previousMenu, PlcTreeElementStringRaw nextMenu)
        {
            PlcTreeElementStringRaw plcTreeElementStringRaw = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw, ref parentElement, creator.InitMenuData(true, "Job Editor", "JobEditor.png", 1024), false, 0, 0, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw1 = new PlcTreeElementStringRaw();
            creator.AddChildToLevel(ref plcTreeElementStringRaw1, ref parentElement, creator.InitMenuData(true, "Back", "Back.png", 1024), false, -1, 1, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw2 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw2, ref parentElement, creator.InitMenuData(true, "Down", "MenuDown.png", 1024), address, (Telegram t) => this.MenuNavigateLayerDown(t), true, 2, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw3 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw3, ref parentElement, creator.InitMenuData(true, "Up", "MenuUp.png", 1024), address, (Telegram t) => this.MenuNavigateLayerUp(t), true, 2, 2);
            PlcTreeElementStringRaw plcTreeElementStringRaw4 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw4, ref parentElement, creator.InitMenuData(true, "Copy", "MenuCopy.png", 1024), address, (Telegram t) => this.MenuCopy(t), true, 3, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw4);
            PlcTreeElementStringRaw plcTreeElementStringRaw5 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw5, ref parentElement, creator.InitMenuData(true, "Paste", "MenuPaste.png", 1024), address, (Telegram t) => this.MenuPaste(t), true, 4, 1);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw5);
            PlcTreeElementStringRaw plcTreeElementStringRaw6 = new PlcTreeElementStringRaw();
            creator.AddChildAction(ref plcTreeElementStringRaw6, ref parentElement, creator.InitMenuData(true, "Paste All", "MenuPasteAll.png", 1024), address, (Telegram t) => this.MenuPasteAll(t), true, 4, 2);
            this.RegisterMenuToLoginEnaManipulation(ref plcTreeElementStringRaw6);
            PlcTreeElementStringRaw plcTreeElementStringRaw7 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw7, ref parentElement, creator.InitMenuData(true, "Holes", "Holes.png", 1024), previousMenu, address, (Telegram t) => this.MenuNavigateBackward(t), true, 5, 1);
            PlcTreeElementStringRaw plcTreeElementStringRaw8 = new PlcTreeElementStringRaw();
            creator.AddChildToNode(ref plcTreeElementStringRaw8, ref parentElement, creator.InitMenuData(true, "Slots", "Slots.png", 1024), nextMenu, address, (Telegram t) => this.MenuNavigateForward(t), true, 5, 2);
        }

        private void Button1Action(Telegram telegram)
        {
            this.OnMinimize(telegram);
            if (this.connectedToServer && this.hmiProcessIsRunning)
            {
                this.OnMinimizeHmi(telegram);
                this.ShowHmi(ProcessHelper.ShowWindowCmd.SW_SHOWMINIMIZED);
            }
        }

        private void Button2Action(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(() => this.editorViewModel.NewJob());
            if (this.menuAfterNewJob.HasValue)
            {
                EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
                Messages.Address address = new Messages.Address(this.Modules["JobEditor"].GetId(0), string.Concat(this.Modules["JobEditor"].GetId(0), ".MenuModel"), "", "Tree");
                Telegram telegram1 = new Telegram(address, 1, (object)this.menuAfterNewJob, null);
                instance.Publish<Telegram>(telegram1, address.Target, true);
            }
            this.editorViewModel.UpdateCutSequenceSelector();
        }

        private void Button3Action(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(() => this.editorViewModel.LoadJob(null, null));
        }

        private void Button4Action(Telegram telegram)
        {
            this.editorViewModel.SaveAsJob(null, null);
        }

        private void Button5Action(Telegram telegram)
        {
            this.editorViewModel.ImportJob(null, null);
        }

        private void Button6Action(Telegram telegram)
        {
            if (string.IsNullOrWhiteSpace(this.editorViewModel.ProductFileFullPath))
            {
                return;
            }
            this.editorViewModel.SaveAsJob(this.editorViewModel.ProductFileFullPath, null);
            //if (this.activationClient != null)
            //{
            //    this.activationClient.Send(this.editorViewModel.ProductFileFullPath);
            //}
            //Product p = this.editorViewModel.FinalizingProduct();
            //new Generator().generate(this.editorViewModel.ProductFileFullPath);
            OptimizedCutSeqGenerator.GenerateCutSequences(this.editorViewModel.ProductFileFullPath);
        }

        private void Button7Action(Telegram telegram)
        {
            if (this.connectedToServer && this.hmiProcessIsRunning)
            {
                this.ShowHmi(ProcessHelper.ShowWindowCmd.SW_SHOWNORMAL);
            }
        }

        private void Button8Action(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(() => this.editorViewModel.LogInOut());
        }

        public void CentersMenuAction(Telegram telegram)
        {
            this.SetJobEditorLocation(MainWindow.JobEditorLocation.Centers);
            this.SetPageText(this.GetCurrentJobEditorLocation().ToString());
            this.NotifyRedrawShape(MainWindow.JobEditorLocation.Centers);
            this.editorViewModel.CutSeqVisuEna = false;
            this.editorViewModel.StepLapVisuEna = false;
            this.editorViewModel.TipCutVisuEna = false;
            this.editorViewModel.HolesVisuEna = false;
            this.editorViewModel.SlotsVisuEna = false;
            if (this.editorViewModel.CutSeqSelected)
            {
                this.editorViewModel.CentersVisuEna = true;
            }
            this.editorViewModel.LayersVisuEna = false;
            this.editorViewModel.StepLapVisuEna1 = false;
            this.editorViewModel.TipCutVisuEna1 = false;
            this.editorViewModel.HolesVisuEna1 = false;
            this.editorViewModel.SlotsVisuEna1 = false;
            this.editorViewModel.CentersVisuEna1 = true;
            this.editorViewModel.LayersVisuEna1 = false;
            this.editorViewModel.EmptySpaceEna = true;
            this.editorViewModel.EmptySpaceHeight = 440;
        }

        private void ChangeTextAppBarButton8(string textId)
        {
            this.TranslateButton(ref this.appBarButton8, textId);
        }

        public long ConnectToServer()
        {
            lock (this.CommAdmin)
            {
                if (this.CommChannel != null)
                {
                    this.CommAdmin.RemoveChannel(this.CommChannel);
                    this.CommChannel = null;
                }
                string tcpIpServerAddress = Settings.Default.TcpIpServerAddress;
                int tcpIpServerPort = Settings.Default.TcpIpServerPort;
                int tcpIpConnectRetry = Settings.Default.TcpIpConnectRetry;
                int tcpIpKeepAliveSendInterval = Settings.Default.TcpIpKeepAliveSendInterval;
                int tcpIpKeepAliveTimeoutTime = Settings.Default.TcpIpKeepAliveTimeoutTime;
                SocketState socketState = new SocketState(this.Address);
                IPEndPoint pEndPoint = new IPEndPoint(IPAddress.Parse(tcpIpServerAddress), tcpIpServerPort);
                SocketSettingsBuffers socketSettingsBuffer = new SocketSettingsBuffers(this.Address, 1000000, 1000000, 0);
                SocketSettingsTimers socketSettingsTimer = new SocketSettingsTimers(this.Address, tcpIpKeepAliveSendInterval, tcpIpKeepAliveTimeoutTime, tcpIpConnectRetry);
                SocketSettings socketSetting = new SocketSettings(pEndPoint, this.Address, socketSettingsBuffer, socketSettingsTimer);
                this.CommChannel = new SocketChannel(socketSetting, socketState, this.Address);
                this.CommAdmin.AddChannel(this.CommChannel);
            }
            return (long)0;
        }

        private IAddress CreateAppBar()
        {
            IEventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            string str = "AppBarModel";
            string[] strArrays = new string[] { "AppBarViewModel" };
            Type type = typeof(AppBarModel);
            Type[] typeArray = new Type[] { typeof(AppBarViewModel) };
            string[] strArrays1 = new string[] { "JobEditor.AppBar.Views.AppBarViewModel.xaml" };
            string[] strArrays2 = new string[] { "AppBar" };
            TelegramCommand telegramCommand = TelegramCommand.Create;
            IAddress address = new Messages.Address(this.Modules["JobEditor"].GetId(0), "", null, "AppBar");
            instance.Subscribe<Telegram>(new Action<Telegram>(this.AppBarCommands), address.Owner);
            return this.ModelAction(str, strArrays, type, typeArray, strArrays1, strArrays2, telegramCommand, address);
        }

        private IAddress CreateMenu()
        {
            IEventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            string str = "MenuModel";
            string[] strArrays = new string[] { "MenuViewModel" };
            Type type = typeof(MenuModel);
            Type[] typeArray = new Type[] { typeof(MenuViewModel) };
            string[] strArrays1 = new string[] { "JobEditor.Menu.Views.MenuViewModel.xaml" };
            string[] strArrays2 = new string[] { "Menu" };
            TelegramCommand telegramCommand = TelegramCommand.Create;
            IAddress address = new Messages.Address(this.Modules["JobEditor"].GetId(0), "", null, "Menu");
            instance.Subscribe<Telegram>(new Action<Telegram>(this.MenuCommands), address.Owner);
            return this.ModelAction(str, strArrays, type, typeArray, strArrays1, strArrays2, telegramCommand, address);
        }

        public void CutSequencesMenuAction(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(() => this.editorViewModel.ShowCutSequenceSelector());
        }

        public long DisconnectFromServer()
        {
            if (this.CommChannel != null)
            {
                this.CommAdmin.RemoveChannel(this.CommChannel);
                this.CommChannel = null;
            }
            return (long)0;
        }

        private void EnableAppBarButton(ref PlcControlDataRaw control, bool enable, string controlName)
        {
            if (controlName == null)
            {
                return;
            }
            EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            IAddress address = new Messages.Address(this.Modules["JobEditor"].GetId(0), null, "", null);
            string str = "AppBarModel";
            control.IsEnabled = enable;
            Telegram telegram = new Telegram(new Messages.Address(this.Modules["JobEditor"].GetId(0), string.Concat(this.Modules["JobEditor"].GetId(0), ".", str), address.Owner, controlName), 0, (object)control, null);
            Telegram telegram1 = telegram;
            instance.Publish<Telegram>(telegram1, telegram1.Address.Target, false);
        }

        private void EnableAppBarButton1(bool enable)
        {
            this.EnableAppBarButton(ref this.appBarButton1, enable, this.appBarButton1Name);
        }

        private void EnableAppBarButton2(bool enable)
        {
            this.EnableAppBarButton(ref this.appBarButton2, enable, this.appBarButton2Name);
        }

        private void EnableAppBarButton3(bool enable)
        {
            this.EnableAppBarButton(ref this.appBarButton3, enable, this.appBarButton3Name);
        }

        private void EnableAppBarButton4(bool enable)
        {
            this.EnableAppBarButton(ref this.appBarButton4, enable, this.appBarButton4Name);
        }

        private void EnableAppBarButton5(bool enable)
        {
            this.EnableAppBarButton(ref this.appBarButton5, enable, this.appBarButton5Name);
        }

        private void EnableAppBarButton6(bool enable)
        {
            this.EnableAppBarButton(ref this.appBarButton6, enable, this.appBarButton6Name);
        }

        private void EnableAppBarButton8(bool enable)
        {
            this.EnableAppBarButton(ref this.appBarButton8, enable, this.appBarButton8Name);
        }

        public MainWindow.JobEditorLocation GetCurrentJobEditorLocation()
        {
            return this.location;
        }

        public MainWindow.JobEditorLocation GetPreviousJobEditorLocation()
        {
            return this.previousLocation;
        }

        private void HandleApplicationCommands(Telegram telegram)
        {
            int command = telegram.Command;
            if (command == 0)
            {
                this.OnCommAdminStateChanged(telegram);
                return;
            }
            if (command != 11)
            {
                return;
            }
            this.OnClientInitialize(telegram);
        }

        private void HmiHasEnded(string processFileName)
        {
            this.hmiProcessIsRunning = false;
            this.UpdateAppBarButtonsState();
        }

        private void HmiHasStarted(string processFileName)
        {
            this.hmiProcessIsRunning = true;
            this.UpdateAppBarButtonsState();
        }

        public void HolesMenuAction(Telegram telegram)
        {
            this.SetJobEditorLocation(MainWindow.JobEditorLocation.Holes);
            this.SetPageText(this.GetCurrentJobEditorLocation().ToString());
            this.NotifyRedrawShape(MainWindow.JobEditorLocation.Holes);
            this.editorViewModel.CutSeqVisuEna = false;
            this.editorViewModel.StepLapVisuEna = false;
            this.editorViewModel.TipCutVisuEna = false;
            if (this.editorViewModel.CutSeqSelected)
            {
                this.editorViewModel.HolesVisuEna = true;
            }
            this.editorViewModel.SlotsVisuEna = false;
            this.editorViewModel.CentersVisuEna = false;
            this.editorViewModel.LayersVisuEna = false;
            this.editorViewModel.StepLapVisuEna1 = false;
            this.editorViewModel.TipCutVisuEna1 = false;
            this.editorViewModel.HolesVisuEna1 = true;
            this.editorViewModel.SlotsVisuEna1 = false;
            this.editorViewModel.CentersVisuEna1 = false;
            this.editorViewModel.LayersVisuEna1 = false;
            this.editorViewModel.EmptySpaceEna = true;
            this.editorViewModel.EmptySpaceHeight = 440;
        }

        public long Init()
        {
            this.Modules.Add("Server", new Models.ModuleInfo("Server"));
            this.Modules.Add("JobEditor", new Models.ModuleInfo("JobEditor"));
            this.Address = new Messages.Address(this.Modules["JobEditor"].GetId(ModuleInfo.Types.Application), this.Modules["Server"].GetId(ModuleInfo.Types.Application), "", null);
            SingletonProvider<EventAggregator>.Instance.Subscribe<Telegram>(new Action<Telegram>(this.HandleApplicationCommands), this.Address.Owner);
            this.CommAdmin = new CommAdministrator(this.Address, Role.Client, null);
            this.viewModelAdministrator = new ViewModelAdministrator(new Messages.Address(this.Modules["JobEditor"].GetId(ModuleInfo.Types.ViewModelAdministrator), this.Modules["JobEditor"].GetId(ModuleInfo.Types.ModelAdministrator), "", null), "Xaml\\");
            this.modelAdministrator = new ModelAdministrator(new Messages.Address(this.Modules["JobEditor"].GetId(ModuleInfo.Types.ModelAdministrator), this.Modules["JobEditor"].GetId(ModuleInfo.Types.ViewModelAdministrator), "", null));
            UiLocalisation uiLocalisation = this.localisation;
            UiLocalisationSettings uiLocalisationSetting = new UiLocalisationSettings();
            uiLocalisationSetting.LoadFilename = Settings.Default.LocalisationLoadFile;
            uiLocalisationSetting.SaveFilename = Settings.Default.LocalisationSaveFile;
            uiLocalisationSetting.LocalisationId = Settings.Default.LocalisationDefaultId;
            uiLocalisation.Settings = uiLocalisationSetting;
            this.localisation.Load(true);
            ConverterDoubleToString.UnitInches = Settings.Default.UnitsInInches;
            this.menuItemsLoginEna = new List<PlcTreeElementStringRaw>();
            this.InitServices();
            this.InitJobEditorUI();
            this.SetAppBarLabel1Text(this.appBarLabelButton1TextId);
            this.SetAppBarLabel8Text(this.appBarLabelButton8TextId);
            if (ProcessHelper.FindProcess(Settings.Default.HmiProcessFilename) != null)
            {
                this.hmiProcessIsRunning = true;
                this.UpdateAppBarButtonsState();
            }
            ProcessHelper.WatchProcess(Settings.Default.HmiProcessFilename, new Action<string>(this.HmiHasStarted), new Action<string>(this.HmiHasEnded));
            return (long)0;
        }

        private void InitAppBarControl(ref PlcControlDataRaw control, bool enable, IAddress relayAddress, string modelName, string controlName, string contentText, string value, string contentImage, Action<Telegram> action = null, UiElementDataVisibility visibility = UiElementDataVisibility.Visible)
        {
            IEventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            control.Visibility = (short)visibility;
            control.IsEnabled = enable;
            control.ContentText = contentText;
            control.StringValue = value;
            control.ContentImage = contentImage;
            this.TranslateButton(ref control, contentText);
            if (action != null)
            {
                instance.Subscribe<Telegram>(action, string.Concat(this.Modules["JobEditor"].GetId(0), ".AppBar.", controlName));
            }
            Telegram telegram = new Telegram(new Messages.Address(this.Modules["JobEditor"].GetId(0), string.Concat(this.Modules["JobEditor"].GetId(0), ".", modelName), relayAddress.Owner, controlName), 0, (object)control, null);
            Telegram telegram1 = telegram;
            instance.Publish<Telegram>(telegram1, telegram1.Address.Target, false);
        }

        public long InitClientAfterServerInit()
        {
            return (long)0;
        }

        //[DebuggerNonUserCode]
        //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        //public void InitializeComponent()
        //{
        //    if (this._contentLoaded)
        //    {
        //        return;
        //    }
        //    this._contentLoaded = true;
        //    Application.LoadComponent(this, new Uri("/JobEditor;component/mainwindow.xaml", UriKind.Relative));
        //}

        private void InitJobEditorUI()
        {
            this.editorViewModel = new EditorViewModel(this.localisation, this);
            base.DataContext = this.editorViewModel;
            this.editorViewModel.UpdateAppBarButtonsState = new Action(this.UpdateAppBarButtonsState);
            this.editorViewModel.UpdateAppBarButtonsText = new Action(this.UpdateAppBarButtonTexts);
            this.CreateMenu();
            this.CreateAppBar();
            this.editorViewModel.ProductView.OnSelectedLayerViewChanged = new Action(this.OnSelectedLayerViewChanged);
            this.editorViewModel.NewJob();
        }

        public long InitServices()
        {
            this.activationClient = new ActivationClient(this.Address);
            this.activationClient.OnPlcStateChanged += new Action<ActivationClient.Commands>(this.OnPlcStateChangedActivationService);
            return (long)0;
        }

        private bool IsProductValidForActivation()
        {
            if (this.editorViewModel == null)
            {
                return false;
            }
            return this.editorViewModel.ProductValidForActivation;
        }

        public void LayersMenuAction(Telegram telegram)
        {
            this.SetJobEditorLocation(MainWindow.JobEditorLocation.Layers);
            this.SetPageText(this.GetCurrentJobEditorLocation().ToString());
            this.editorViewModel.CutSeqVisuEna = false;
            this.editorViewModel.StepLapVisuEna = false;
            this.editorViewModel.TipCutVisuEna = false;
            this.editorViewModel.HolesVisuEna = false;
            this.editorViewModel.SlotsVisuEna = false;
            this.editorViewModel.CentersVisuEna = false;
            this.editorViewModel.EmptySpaceEna = false;
            if (this.editorViewModel.CutSeqSelected)
            {
                this.editorViewModel.LayersVisuEna = true;
            }
            this.editorViewModel.StepLapVisuEna1 = false;
            this.editorViewModel.TipCutVisuEna1 = false;
            this.editorViewModel.HolesVisuEna1 = false;
            this.editorViewModel.SlotsVisuEna1 = false;
            this.editorViewModel.CentersVisuEna1 = false;
            this.editorViewModel.LayersVisuEna1 = true;
        }

        private void MenuCommands(Telegram telegram)
        {
            if (telegram.Command == 2)
            {
                this.BuildMenu(telegram);
            }
        }

        private void MenuCopy(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(() => {
                this.editorViewModel.CopyOfLayerView = new LayerView(null, null);
                this.editorViewModel.CopyOfLayerView.CloneDataFrom(this.editorViewModel.ProductView.SelectedLayerView);
            });
        }

        private void MenuNavigateBackward(Telegram telegram)
        {
            switch (this.location)
            {
                case MainWindow.JobEditorLocation.Layers:
                    {
                        this.CentersMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.StepLaps:
                    {
                        this.LayersMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.Holes:
                    {
                        this.StepLapsMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.TipCuts:
                    {
                        this.HolesMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.Slots:
                    {
                        this.TipCutsMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.Centers:
                    {
                        this.SlotsMenuAction(telegram);
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        private void MenuNavigateForward(Telegram telegram)
        {
            switch (this.location)
            {
                case MainWindow.JobEditorLocation.Layers:
                    {
                        this.StepLapsMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.StepLaps:
                    {
                        this.HolesMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.Holes:
                    {
                        this.TipCutsMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.TipCuts:
                    {
                        this.SlotsMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.Slots:
                    {
                        this.CentersMenuAction(telegram);
                        return;
                    }
                case MainWindow.JobEditorLocation.Centers:
                    {
                        this.LayersMenuAction(telegram);
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        private void MenuNavigateLayerDown(Telegram telegram)
        {
            lock (this.lockNavigate)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    if (this.editorViewModel.ProductView.ActLayerNum > 1)
                    {
                        ProductView productView = this.editorViewModel.ProductView;
                        productView.ActLayerNum = productView.ActLayerNum - 1;
                    }
                });
            }
        }

        private void MenuNavigateLayerUp(Telegram telegram)
        {
            lock (this.lockNavigate)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    int actLayerNum = this.editorViewModel.ProductView.ActLayerNum;
                    if (this.editorViewModel.ProductView.ActLayerNum < this.editorViewModel.ProductView.LayerViews.Count)
                    {
                        actLayerNum++;
                    }
                    if (this.editorViewModel.ProductView.ActLayerNum >= this.editorViewModel.ProductView.LayerViews.Count)
                    {
                        actLayerNum = this.editorViewModel.ProductView.LayerViews.Count;
                    }
                    this.editorViewModel.ProductView.ActLayerNum = actLayerNum;
                });
            }
        }

        private void MenuPaste(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(() => {
                if (this.editorViewModel.CopyOfLayerView != null)
                {
                    switch (this.location)
                    {
                        case MainWindow.JobEditorLocation.Layers:
                            {
                                this.editorViewModel.ProductView.SelectedLayerView.LayerDefaultView.CloneDataFrom(this.editorViewModel.CopyOfLayerView.LayerDefaultView);
                                this.editorViewModel.FillRadioButtons(false);
                                return;
                            }
                        case MainWindow.JobEditorLocation.StepLaps:
                            {
                                this.editorViewModel.ProductView.SelectedLayerView.CloneStepLapsDataFrom(this.editorViewModel.CopyOfLayerView);
                                return;
                            }
                        case MainWindow.JobEditorLocation.Holes:
                            {
                                this.editorViewModel.ProductView.SelectedLayerView.CloneHolesDataFrom(this.editorViewModel.CopyOfLayerView);
                                return;
                            }
                        case MainWindow.JobEditorLocation.TipCuts:
                            {
                                this.editorViewModel.ProductView.SelectedLayerView.CloneTipCutsDataFrom(this.editorViewModel.CopyOfLayerView);
                                return;
                            }
                        case MainWindow.JobEditorLocation.Slots:
                            {
                                this.editorViewModel.ProductView.SelectedLayerView.CloneSlotsDataFrom(this.editorViewModel.CopyOfLayerView);
                                return;
                            }
                        case MainWindow.JobEditorLocation.Centers:
                            {
                                this.editorViewModel.ProductView.SelectedLayerView.CloneCentersDataFrom(this.editorViewModel.CopyOfLayerView);
                                break;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
            });
        }

        private void MenuPasteAll(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(() => {
                if (this.editorViewModel.CopyOfLayerView != null)
                {
                    this.editorViewModel.ProductView.SelectedLayerView.CloneDataFrom(this.editorViewModel.CopyOfLayerView);
                }
            });
        }

        private IAddress ModelAction(string namemodel, string[] nameviewmodels, Type typeModel, Type[] typeViewModels, string[] xamls, string[] locations, TelegramCommand command, IAddress relayAddress = null)
        {
            IAddress address = null;
            if (nameviewmodels.Count<string>() != typeViewModels.Count<Type>() || typeViewModels.Count<Type>() != xamls.Count<string>() || xamls.Count<string>() != locations.Count<string>())
            {
                throw new ArgumentException("Different list counts encountered");
            }
            IEventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            foreach (KeyValuePair<string, Models.ModuleInfo> module in this.Modules)
            {
                if (module.Key != "JobEditor")
                {
                    continue;
                }
                string id = module.Value.GetId(ModuleInfo.Types.ModelAdministrator);
                string str = module.Value.GetId(ModuleInfo.Types.ViewModelAdministrator);
                string id1 = module.Value.GetId(0);
                Messages.Address address1 = new Messages.Address(id, str, "", null);
                string owner = "";
                if (relayAddress != null)
                {
                    owner = relayAddress.Owner;
                }
                string str1 = module.Value.GetId(0);
                Messages.Address address2 = new Messages.Address(str, id, "", null);
                List<ViewModelProtoType> viewModelProtoTypes = new List<ViewModelProtoType>();
                for (int i = 0; i < nameviewmodels.Count<string>(); i++)
                {
                    Messages.Address address3 = new Messages.Address(str1, id1, "", nameviewmodels[i], namemodel);
                    viewModelProtoTypes.Add(new ViewModelProtoType(address3, typeViewModels[i], xamls[i], locations[i]));
                }
                Messages.Address address4 = new Messages.Address(id1, str1, owner, namemodel, nameviewmodels[0]);
                ModelProtoType modelProtoType = new ModelProtoType(address4, typeModel);
                address = address4;
                UIProtoType uIProtoType = new UIProtoType(viewModelProtoTypes, modelProtoType);
                if (command != TelegramCommand.Create && command != TelegramCommand.Dispose)
                {
                    continue;
                }
                IEventMessage message = uIProtoType.GetMessage(new Messages.Address(id1, id, "", null), command);
                string owner1 = modelProtoType.Address.Owner;
                instance.Publish<Telegram>((Telegram)message, owner1, false);
            }
            return address;
        }

        private void NotifyRedrawShape(MainWindow.JobEditorLocation location)
        {
            string str = null;
            switch (location)
            {
                case MainWindow.JobEditorLocation.StepLaps:
                    {
                        str = "StepLaps";
                        break;
                    }
                case MainWindow.JobEditorLocation.Holes:
                    {
                        str = "Holes";
                        break;
                    }
                case MainWindow.JobEditorLocation.TipCuts:
                    {
                        str = "TipCuts";
                        break;
                    }
                case MainWindow.JobEditorLocation.Slots:
                    {
                        str = "Slots";
                        break;
                    }
                case MainWindow.JobEditorLocation.Centers:
                    {
                        str = "Centers";
                        break;
                    }
            }
            if (str == null)
            {
                return;
            }
            EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            Telegram telegram = new Telegram(new Messages.Address("JobEditor.MenuAction", string.Concat("JobEditor.UserControl.", str), "", null), 0, null, null);
            instance.Publish<Telegram>(telegram, false);
        }

        private void OnClientInitialize(Telegram telegram)
        {
            this.InitClientAfterServerInit();
            EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            Messages.Address address = new Messages.Address(telegram.Address.Target, telegram.Address.Owner, "", null);
            Telegram telegram1 = new Telegram(address, 12, null, null);
            instance.Publish<Telegram>(telegram1, address.Target, true);
        }

        private void OnCommAdminStateChanged(Telegram telegram)
        {
            IChannelState value = telegram.Value as IChannelState;
            //telegram.ChannelId;
            switch ((int)value.CommState)
            {
                case 1:
                    {
                        this.connectedToServer = false;
                        this.UpdateAppBarButtonsState();
                        if (!Settings.Default.ReconnectToServerAfterDisconnect)
                        {
                            return;
                        }
                        this.ConnectToServer();
                        return;
                    }
                case 2:
                case 3:
                case 4:
                    {
                        return;
                    }
                case 5:
                    {
                        this.connectedToServer = true;
                        this.UpdateAppBarButtonsState();
                        return;
                    }
                case 6:
                    {
                        this.connectedToServer = false;
                        this.UpdateAppBarButtonsState();
                        this.DisconnectFromServer();
                        return;
                    }
                case 7:
                    {
                        this.connectedToServer = false;
                        this.UpdateAppBarButtonsState();
                        this.DisconnectFromServer();
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        private void OnMinimize(Telegram telegram)
        {
            Application.Current.Dispatcher.Invoke(() => base.WindowState = WindowState.Minimized);
        }

        private void OnMinimizeHmi(Telegram telegram)
        {
            ProcessHelper.ShowProcess(ProcessHelper.FindProcess(Settings.Default.HmiProcessFilename), ProcessHelper.ShowWindowCmd.SW_SHOWMINIMIZED);
        }

        public void OnPlcStateChangedActivationService(ActivationClient.Commands plcState)
        {
            this.UpdateAppBarButtonsState();
        }

        public void OnSelectedLayerViewChanged()
        {
            this.NotifyRedrawShape(this.location);
            this.editorViewModel.FillRadioButtons(true);
        }

        private void RegisterMenuToLoginEnaManipulation(ref PlcTreeElementStringRaw menuTreeItem)
        {
            this.menuItemsLoginEna.Add(menuTreeItem);
        }

        private void SetAppBarLabel1Text(string contentText)
        {
            this.SetAppBarLabelText(ref this.labelbutton1, contentText, this.appBarLabelButton1Name);
        }

        private void SetAppBarLabel2Text(string contentText)
        {
            this.SetAppBarLabelText(ref this.labelbutton2, contentText, this.appBarLabelButton2Name);
        }

        private void SetAppBarLabel3Text(string contentText)
        {
            this.SetAppBarLabelText(ref this.labelbutton3, contentText, this.appBarLabelButton3Name);
        }

        private void SetAppBarLabel4Text(string contentText)
        {
            this.SetAppBarLabelText(ref this.labelbutton4, contentText, this.appBarLabelButton4Name);
        }

        private void SetAppBarLabel5Text(string contentText)
        {
            this.SetAppBarLabelText(ref this.labelbutton5, contentText, this.appBarLabelButton5Name);
        }

        private void SetAppBarLabel6Text(string contentText)
        {
            this.SetAppBarLabelText(ref this.labelbutton6, contentText, this.appBarLabelButton6Name);
        }

        private void SetAppBarLabel8Text(string contentText)
        {
            this.SetAppBarLabelText(ref this.labelbutton8, contentText, this.appBarLabelButton8Name);
        }

        private void SetAppBarLabelText(ref PlcControlDataRaw control, string text, string controlName)
        {
            if (controlName == null)
            {
                return;
            }
            EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            IAddress address = new Messages.Address(this.Modules["JobEditor"].GetId(0), null, "", null);
            string str = "AppBarModel";
            this.TranslateButton(ref control, text);
            Telegram telegram = new Telegram(new Messages.Address(this.Modules["JobEditor"].GetId(0), string.Concat(this.Modules["JobEditor"].GetId(0), ".", str), address.Owner, controlName), 0, (object)control, null);
            Telegram telegram1 = telegram;
            instance.Publish<Telegram>(telegram1, telegram1.Address.Target, false);
        }

        public void SetJobEditorLocation(MainWindow.JobEditorLocation location)
        {
            this.previousLocation = this.location;
            this.location = location;
        }

        public void SetMenuItemsVisibility(ref PlcTreeElementStringRaw item, UiElementDataVisibility visibility, string queId = "")
        {
            item.Data.Visibility = (short)visibility;
            EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            Telegram telegram = new Telegram(new Messages.Address(this.Modules["JobEditor"].GetId(0), string.Concat(this.Modules["JobEditor"].GetId(0), ".MenuModel"), "", "Tree"), 0, (object)item, null);
            instance.Publish<Telegram>(telegram, queId, true);
        }

        public void SetPageText(string text)
        {
            EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            Messages.Address address = new Messages.Address(this.Address.Owner, "JobEditor.MenuModel.UpdateMenuInfo", "", null);
            this.localisation.Translate(this.localisation.Settings.LocalisationId, ref text, null);
            instance.Publish<Telegram>(new Telegram(address, 0, text, null), true);
        }

        private void ShowHmi(ProcessHelper.ShowWindowCmd cmd = ProcessHelper.ShowWindowCmd.SW_SHOWNORMAL)
        {
            ProcessHelper.ShowProcess(Settings.Default.HmiProcessFilename, cmd);
        }

        public void SlotsMenuAction(Telegram telegram)
        {
            this.SetJobEditorLocation(MainWindow.JobEditorLocation.Slots);
            this.SetPageText(this.GetCurrentJobEditorLocation().ToString());
            this.NotifyRedrawShape(MainWindow.JobEditorLocation.Slots);
            this.editorViewModel.CutSeqVisuEna = false;
            this.editorViewModel.StepLapVisuEna = false;
            this.editorViewModel.TipCutVisuEna = false;
            this.editorViewModel.HolesVisuEna = false;
            if (this.editorViewModel.CutSeqSelected)
            {
                this.editorViewModel.SlotsVisuEna = true;
            }
            this.editorViewModel.CentersVisuEna = false;
            this.editorViewModel.LayersVisuEna = false;
            this.editorViewModel.StepLapVisuEna1 = false;
            this.editorViewModel.TipCutVisuEna1 = false;
            this.editorViewModel.HolesVisuEna1 = false;
            this.editorViewModel.SlotsVisuEna1 = true;
            this.editorViewModel.CentersVisuEna1 = false;
            this.editorViewModel.LayersVisuEna1 = false;
            this.editorViewModel.EmptySpaceEna = true;
            this.editorViewModel.EmptySpaceHeight = 440;
        }

        public void StepLapsMenuAction(Telegram telegram)
        {
            this.SetJobEditorLocation(MainWindow.JobEditorLocation.StepLaps);
            this.SetPageText(this.GetCurrentJobEditorLocation().ToString());
            this.NotifyRedrawShape(MainWindow.JobEditorLocation.StepLaps);
            this.editorViewModel.CutSeqVisuEna = false;
            if (this.editorViewModel.CutSeqSelected)
            {
                this.editorViewModel.StepLapVisuEna = true;
            }
            this.editorViewModel.TipCutVisuEna = false;
            this.editorViewModel.HolesVisuEna = false;
            this.editorViewModel.SlotsVisuEna = false;
            this.editorViewModel.CentersVisuEna = false;
            this.editorViewModel.LayersVisuEna = false;
            this.editorViewModel.StepLapVisuEna1 = true;
            this.editorViewModel.TipCutVisuEna1 = false;
            this.editorViewModel.HolesVisuEna1 = false;
            this.editorViewModel.SlotsVisuEna1 = false;
            this.editorViewModel.CentersVisuEna1 = false;
            this.editorViewModel.LayersVisuEna1 = false;
            this.editorViewModel.EmptySpaceEna = true;
            this.editorViewModel.EmptySpaceHeight = 440;
        }

        //[DebuggerNonUserCode]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        //void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //        case 1:
        //            {
        //                this.wndJobEditor = (MainWindow)target;
        //                this.wndJobEditor.StateChanged += new EventHandler(this.Window_StateChanged);
        //                this.wndJobEditor.Closed += new EventHandler(this.Window_Closed);
        //                return;
        //            }
        //        case 2:
        //            {
        //                this.MainEditor = (Grid)target;
        //                return;
        //            }
        //        case 3:
        //            {
        //                this.SwipeControl = (System.Windows.Controls.Label)target;
        //                return;
        //            }
        //        case 4:
        //            {
        //                this.Menu = (Grid)target;
        //                return;
        //            }
        //        case 5:
        //            {
        //                this.Info = (Grid)target;
        //                return;
        //            }
        //        case 6:
        //            {
        //                this.Editor = (Grid)target;
        //                return;
        //            }
        //        case 7:
        //            {
        //                this.CutSequences = (Grid)target;
        //                return;
        //            }
        //        case 8:
        //            {
        //                this.scvCutSeq = (CustomControlLibrary.ScrollViewer)target;
        //                return;
        //            }
        //        case 9:
        //            {
        //                this.CutSeqList = (ItemsControl)target;
        //                return;
        //            }
        //        case 10:
        //            {
        //                this.Shapes = (Grid)target;
        //                return;
        //            }
        //        case 11:
        //            {
        //                this.StepLapsPage = (Grid)target;
        //                return;
        //            }
        //        case 12:
        //            {
        //                this.StepLapsDefault = (Grid)target;
        //                return;
        //            }
        //        case 13:
        //            {
        //                this.NumberOfSteps = (StackPanel)target;
        //                return;
        //            }
        //        case 14:
        //            {
        //                this.EditButtonNumberOfStep = (AppButton)target;
        //                return;
        //            }
        //        case 15:
        //            {
        //                this.NumberOfSame = (StackPanel)target;
        //                return;
        //            }
        //        case 16:
        //            {
        //                this.EditButtonNumberOfSame = (AppButton)target;
        //                return;
        //            }
        //        case 17:
        //            {
        //                this.StapLapValue = (StackPanel)target;
        //                return;
        //            }
        //        case 18:
        //            {
        //                this.EditButtonStepLapValue = (AppButton)target;
        //                return;
        //            }
        //        case 19:
        //            {
        //                this.TipCutsPage = (Grid)target;
        //                return;
        //            }
        //        case 20:
        //            {
        //                this.TipCutsDefault = (Grid)target;
        //                return;
        //            }
        //        case 21:
        //            {
        //                this.TipCutsHeight = (StackPanel)target;
        //                return;
        //            }
        //        case 22:
        //            {
        //                this.EditButtonTipCutsHeight = (AppButton)target;
        //                return;
        //            }
        //        case 23:
        //            {
        //                this.TipCutsOverCut = (StackPanel)target;
        //                return;
        //            }
        //        case 24:
        //            {
        //                this.EditButtonTipCutsOverCut = (AppButton)target;
        //                return;
        //            }
        //        case 25:
        //            {
        //                this.TipCutDoubleCut = (StackPanel)target;
        //                return;
        //            }
        //        case 26:
        //            {
        //                this.HolesPage = (Grid)target;
        //                return;
        //            }
        //        case 27:
        //            {
        //                this.HolesDefault = (Grid)target;
        //                return;
        //            }
        //        case 28:
        //            {
        //                this.HolesYOffsetValue = (StackPanel)target;
        //                return;
        //            }
        //        case 29:
        //            {
        //                this.SlotsPage = (Grid)target;
        //                return;
        //            }
        //        case 30:
        //            {
        //                this.SlotsDefault = (Grid)target;
        //                return;
        //            }
        //        case 31:
        //            {
        //                this.SlotsYDistanceValue = (StackPanel)target;
        //                return;
        //            }
        //        case 32:
        //            {
        //                this.CentersPage = (Grid)target;
        //                return;
        //            }
        //        case 33:
        //            {
        //                this.CentersDefault = (Grid)target;
        //                return;
        //            }
        //        case 34:
        //            {
        //                this.CentersVOffset = (StackPanel)target;
        //                return;
        //            }
        //        case 35:
        //            {
        //                this.CentersOverCut = (StackPanel)target;
        //                return;
        //            }
        //        case 36:
        //            {
        //                this.EditButtonCentersOverCut = (AppButton)target;
        //                return;
        //            }
        //        case 37:
        //            {
        //                this.CentersDoubleCut = (StackPanel)target;
        //                return;
        //            }
        //        case 38:
        //            {
        //                this.LayersPage = (Grid)target;
        //                return;
        //            }
        //        case 39:
        //            {
        //                this.LayerDefRow1 = (StackPanel)target;
        //                return;
        //            }
        //        case 40:
        //            {
        //                this.NumberOfLayers = (StackPanel)target;
        //                return;
        //            }
        //        case 41:
        //            {
        //                this.LayerDefRow2 = (StackPanel)target;
        //                return;
        //            }
        //        case 42:
        //            {
        //                this.HeightRefType = (StackPanel)target;
        //                return;
        //            }
        //        case 43:
        //            {
        //                this.LayerHeightUnitMm = (SqRadioButton)target;
        //                return;
        //            }
        //        case 44:
        //            {
        //                this.LayerHeightUnitNmbr = (SqRadioButton)target;
        //                return;
        //            }
        //        case 45:
        //            {
        //                this.LayerHeightAbs = (SqRadioButton)target;
        //                return;
        //            }
        //        case 46:
        //            {
        //                this.LayerHeightRel = (SqRadioButton)target;
        //                return;
        //            }
        //        case 47:
        //            {
        //                this.LayersDefault = (Grid)target;
        //                return;
        //            }
        //        case 48:
        //            {
        //                this.SheetThickness = (StackPanel)target;
        //                return;
        //            }
        //        case 49:
        //            {
        //                this.EditButtonLayerThickness = (AppButton)target;
        //                return;
        //            }
        //        case 50:
        //            {
        //                this.LayerWidth = (StackPanel)target;
        //                return;
        //            }
        //        case 51:
        //            {
        //                this.LayerHeight = (StackPanel)target;
        //                return;
        //            }
        //        case 52:
        //            {
        //                this.HeightCorrection = (StackPanel)target;
        //                return;
        //            }
        //        case 53:
        //            {
        //                this.HeightCorrectionRadioButtons = (StackPanel)target;
        //                return;
        //            }
        //        case 54:
        //            {
        //                this.HeightCorrNone = (SqRadioButton)target;
        //                return;
        //            }
        //        case 55:
        //            {
        //                this.HeightCorrCCUp = (SqRadioButton)target;
        //                return;
        //            }
        //        case 56:
        //            {
        //                this.HeightCorrCCDown = (SqRadioButton)target;
        //                return;
        //            }
        //        case 57:
        //            {
        //                this.HeightCorrPUp = (SqRadioButton)target;
        //                return;
        //            }
        //        case 58:
        //            {
        //                this.HeightCorrPDown = (SqRadioButton)target;
        //                return;
        //            }
        //        case 59:
        //            {
        //                this.LayerInfo = (StackPanel)target;
        //                return;
        //            }
        //        case 60:
        //            {
        //                this.EmptySpace = (Grid)target;
        //                return;
        //            }
        //        case 61:
        //            {
        //                this.ProtectEditor = (Grid)target;
        //                return;
        //            }
        //        case 62:
        //            {
        //                this.AppBar = (Grid)target;
        //                return;
        //            }
        //        case 63:
        //            {
        //                this.FileSelector = (Grid)target;
        //                return;
        //            }
        //        case 64:
        //            {
        //                this.usrFileSelector = (FileSelectorUserControl)target;
        //                return;
        //            }
        //        case 65:
        //            {
        //                this.Login = (Grid)target;
        //                return;
        //            }
        //        case 66:
        //            {
        //                this.usrLogin = (LoginUserControl)target;
        //                return;
        //            }
        //    }
        //    this._contentLoaded = true;
        //}

        public void TipCutsMenuAction(Telegram telegram)
        {
            this.SetJobEditorLocation(MainWindow.JobEditorLocation.TipCuts);
            this.SetPageText(this.GetCurrentJobEditorLocation().ToString());
            this.NotifyRedrawShape(MainWindow.JobEditorLocation.TipCuts);
            this.editorViewModel.CutSeqVisuEna = false;
            this.editorViewModel.StepLapVisuEna = false;
            if (this.editorViewModel.CutSeqSelected)
            {
                this.editorViewModel.TipCutVisuEna = true;
            }
            this.editorViewModel.HolesVisuEna = false;
            this.editorViewModel.SlotsVisuEna = false;
            this.editorViewModel.CentersVisuEna = false;
            this.editorViewModel.LayersVisuEna = false;
            this.editorViewModel.StepLapVisuEna1 = false;
            this.editorViewModel.TipCutVisuEna1 = true;
            this.editorViewModel.HolesVisuEna1 = false;
            this.editorViewModel.SlotsVisuEna1 = false;
            this.editorViewModel.CentersVisuEna1 = false;
            this.editorViewModel.LayersVisuEna1 = false;
            this.editorViewModel.EmptySpaceEna = true;
            this.editorViewModel.EmptySpaceHeight = 440;
        }

        private void TranslateButton(ref PlcControlDataRaw control, string textId)
        {
            string str = "";
            str = textId;
            this.localisation.Translate(this.localisation.Settings.LocalisationId, ref str, null);
            control.ContentText = str;
            if (textId.Equals("Activate"))
                control.ContentText = str;
            control.Localisation = (short)this.localisation.Settings.LocalisationId;
        }

        private void UnRegisterMenuFromLoginEnaManipulation(ref PlcTreeElementStringRaw menuTreeItem)
        {
            this.menuItemsLoginEna.Remove(menuTreeItem);
        }

        public void UpdateAppBarButtonsState()
        {
            bool flag = false;
            if (this.IsProductValidForActivation() && (!Settings.Default.PLC_Active || this.connectedToServer))
                flag = true;  
            //if (!this.IsProductValidForActivation())
            //{
            //    flag = false;
            //}
            //if (!this.connectedToServer)
            //{
            //    flag = false;
            //}
            //if (flag && this.activationClient.PlcState != ActivationClient.Commands.PlcStateStopped)
            //{
            //    flag = false;
            //}
            this.EnableAppBarButton6(flag);
        }

        public void UpdateAppBarButtonTexts()
        {
            if (this.editorViewModel.LogIn.LoggedIn)
            {
                this.SetAppBarLabel8Text("Log off");
                return;
            }
            this.SetAppBarLabel8Text(this.appBarLabelButton8TextId);
        }

        public void UpdateLoginMenuItemsVisibility(UiElementDataVisibility visibility, string queId = "")
        {
            for (int i = 0; i < this.menuItemsLoginEna.Count; i++)
            {
                PlcTreeElementStringRaw item = this.menuItemsLoginEna[i];
                this.SetMenuItemsVisibility(ref item, visibility, queId);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.localisation.IsChanged)
            {
                this.localisation.Save(true);
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
        }

        public enum JobEditorLocation
        {
            Products = 1,
            Layers = 2,
            StepLaps = 3,
            Holes = 4,
            TipCuts = 5,
            Slots = 6,
            Centers = 7
        }
    }
}