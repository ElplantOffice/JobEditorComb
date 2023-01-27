
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace UserControls.FileSelector
{
    public partial class FileSelectorUserControl : UserControl
    {
        private string initCurrentPath;

        private string initCurrentFile;

        public static DependencyProperty FileSelectorTypeProperty;

        public static DependencyProperty TextFilesProperty;

        public static DependencyProperty TextOKProperty;

        public static DependencyProperty TextCancelProperty;

        public static DependencyProperty TextGoUpProperty;

        public static DependencyProperty TextSortByNameProperty;

        public static DependencyProperty TextSortByDateProperty;

        public static DependencyProperty DefaultFileExtensionProperty;

        public static DependencyProperty RootPathProperty;

        public static DependencyProperty CurrentPathProperty;

        public static DependencyProperty CurrentFileProperty;

        public static DependencyProperty ShowFileExtensionProperty;

        public static DependencyProperty FileSearchPatternProperty;

        public static DependencyProperty DataProperty;

        public static DependencyProperty CommandOKProperty;

        public static DependencyProperty CommandCancelProperty;

        public static DependencyProperty ForceRescanFilesProperty;

        public ICommand CommandCancel
        {
            get
            {
                return (ICommand)base.GetValue(FileSelectorUserControl.CommandCancelProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.CommandCancelProperty, value);
            }
        }

        public ICommand CommandOK
        {
            get
            {
                return (ICommand)base.GetValue(FileSelectorUserControl.CommandOKProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.CommandOKProperty, value);
            }
        }

        public string CurrentFile
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.CurrentFileProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.CurrentFileProperty, value);
            }
        }

        private string CurrentFileView
        {
            get
            {
                string str = "";
                if (this.CurrentFile != null)
                {
                    str = (this.ShowFileExtension || string.IsNullOrWhiteSpace(this.DefaultFileExtension) ? this.CurrentFile : Path.GetFileNameWithoutExtension(this.CurrentFile));
                }
                return str;
            }
            set
            {
                string str = value;
                if (this.ShowFileExtension || string.IsNullOrWhiteSpace(this.DefaultFileExtension))
                {
                    this.CurrentFile = str;
                    return;
                }
                this.CurrentFile = string.Format("{0}{1}", str, this.DefaultFileExtension);
            }
        }

        public string CurrentPath
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.CurrentPathProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.CurrentPathProperty, value);
            }
        }

        public IFileSelectorData Data
        {
            get
            {
                return (IFileSelectorData)base.GetValue(FileSelectorUserControl.DataProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.DataProperty, value);
            }
        }

        public string DefaultFileExtension
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.DefaultFileExtensionProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.DefaultFileExtensionProperty, value);
            }
        }

        public string FileSearchPattern
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.FileSearchPatternProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.FileSearchPatternProperty, value);
            }
        }

        public FileSelectorTypes FileSelectorType
        {
            get
            {
                return (FileSelectorTypes)base.GetValue(FileSelectorUserControl.FileSelectorTypeProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.FileSelectorTypeProperty, value);
            }
        }

        public bool ForceRescanFiles
        {
            get
            {
                return (bool)base.GetValue(FileSelectorUserControl.ForceRescanFilesProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.ForceRescanFilesProperty, value);
            }
        }

        public string RootPath
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.RootPathProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.RootPathProperty, value);
            }
        }

        public bool ShowFileExtension
        {
            get
            {
                return (bool)base.GetValue(FileSelectorUserControl.ShowFileExtensionProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.ShowFileExtensionProperty, value);
            }
        }

        public string TextCancel
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.TextCancelProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.TextCancelProperty, value);
            }
        }

        public string TextFiles
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.TextFilesProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.TextFilesProperty, value);
            }
        }

        public string TextGoUp
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.TextGoUpProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.TextGoUpProperty, value);
            }
        }

        public string TextOK
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.TextOKProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.TextOKProperty, value);
            }
        }

        public string TextSortByDate
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.TextSortByDateProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.TextSortByDateProperty, value);
            }
        }

        public string TextSortByName
        {
            get
            {
                return (string)base.GetValue(FileSelectorUserControl.TextSortByNameProperty);
            }
            set
            {
                base.SetCurrentValue(FileSelectorUserControl.TextSortByNameProperty, value);
            }
        }

        static FileSelectorUserControl()
        {
            FileSelectorUserControl.FileSelectorTypeProperty = DependencyProperty.Register("FileSelectorType", typeof(FileSelectorTypes), typeof(FileSelectorUserControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnFileSelectorTypePropertyChanged))
            {
                BindsTwoWayByDefault = true
            });
            FileSelectorUserControl.TextFilesProperty = DependencyProperty.Register("TextFiles", typeof(string), typeof(FileSelectorUserControl), new PropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnTextFilesPropertyChanged)));
            FileSelectorUserControl.TextOKProperty = DependencyProperty.Register("TextOK", typeof(string), typeof(FileSelectorUserControl), new PropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnTextOKPropertyChanged)));
            FileSelectorUserControl.TextCancelProperty = DependencyProperty.Register("TextCancel", typeof(string), typeof(FileSelectorUserControl), new PropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnTextCancelPropertyChanged)));
            FileSelectorUserControl.TextGoUpProperty = DependencyProperty.Register("TextGoUp", typeof(string), typeof(FileSelectorUserControl), new PropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnTextGoUpPropertyChanged)));
            FileSelectorUserControl.TextSortByNameProperty = DependencyProperty.Register("TextSortByName", typeof(string), typeof(FileSelectorUserControl), new PropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnTextSortByNamePropertyChanged)));
            FileSelectorUserControl.TextSortByDateProperty = DependencyProperty.Register("TextSortByDate", typeof(string), typeof(FileSelectorUserControl), new PropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnTextSortByDatePropertyChanged)));
            FileSelectorUserControl.DefaultFileExtensionProperty = DependencyProperty.Register("DefaultFileExtension", typeof(string), typeof(FileSelectorUserControl), new PropertyMetadata(null, new PropertyChangedCallback(FileSelectorUserControl.OnDefaultFileExtensionPropertyChanged), new CoerceValueCallback(FileSelectorUserControl.OnDefaultFileExtensionPropertyValidate)), null);
            FileSelectorUserControl.RootPathProperty = DependencyProperty.Register("RootPath", typeof(string), typeof(FileSelectorUserControl), new PropertyMetadata(null, new PropertyChangedCallback(FileSelectorUserControl.OnRootPathPropertyChanged), new CoerceValueCallback(FileSelectorUserControl.OnRootPathPropertyValidate)), null);
            FileSelectorUserControl.CurrentPathProperty = DependencyProperty.Register("CurrentPath", typeof(string), typeof(FileSelectorUserControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(FileSelectorUserControl.OnCurrentPathPropertyChanged), new CoerceValueCallback(FileSelectorUserControl.OnCurrentPathPropertyValidate))
            {
                BindsTwoWayByDefault = true
            }, null);
            FileSelectorUserControl.CurrentFileProperty = DependencyProperty.Register("CurrentFile", typeof(string), typeof(FileSelectorUserControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(FileSelectorUserControl.OnCurrentFilePropertyChanged), new CoerceValueCallback(FileSelectorUserControl.OnCurrentFilePropertyValidate))
            {
                BindsTwoWayByDefault = true
            }, null);
            FileSelectorUserControl.ShowFileExtensionProperty = DependencyProperty.Register("ShowFileExtension", typeof(bool), typeof(FileSelectorUserControl), new PropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnShowFileExtensionPropertyChanged)));
            FileSelectorUserControl.FileSearchPatternProperty = DependencyProperty.Register("FileSearchPattern", typeof(string), typeof(FileSelectorUserControl), new PropertyMetadata(null, new PropertyChangedCallback(FileSelectorUserControl.OnFileSearchPatternPropertyChanged), new CoerceValueCallback(FileSelectorUserControl.OnFileSearchPatternPropertyValidate)), null);
            FileSelectorUserControl.DataProperty = DependencyProperty.Register("Data", typeof(IFileSelectorData), typeof(FileSelectorUserControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnDataPropertyChanged))
            {
                BindsTwoWayByDefault = true
            });
            FileSelectorUserControl.CommandOKProperty = DependencyProperty.Register("CommandOK", typeof(ICommand), typeof(FileSelectorUserControl));
            FileSelectorUserControl.CommandCancelProperty = DependencyProperty.Register("CommandCancel", typeof(ICommand), typeof(FileSelectorUserControl));
            FileSelectorUserControl.ForceRescanFilesProperty = DependencyProperty.Register("ForceRescanFiles", typeof(bool), typeof(FileSelectorUserControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(FileSelectorUserControl.OnForceRescanFilesPropertyChanged))
            {
                BindsTwoWayByDefault = true
            });
        }

        public FileSelectorUserControl()
        {
            this.InitializeComponent();
            this.FileSelectorType = FileSelectorTypes.Load;
            this.TextFiles = "Files";
            this.TextGoUp = "Go Up";
            this.TextSortByName = "Sort By Name";
            this.TextSortByDate = "Sort By Date";
            this.TextOK = "OK";
            this.TextCancel = "Cancel";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.CancelClicked);
            if (this.FileSelectionCancel != null)
            {
                this.FileSelectionCancel(this, new FileSelectorEventArgs());
            }
            if (this.CommandCancel != null)
            {
                this.CommandCancel.Execute(null);
            }
            if (this.FileSelectionCancel == null && this.CommandCancel == null)
            {
                this.CurrentPath = this.initCurrentPath;
                this.CurrentFile = this.initCurrentFile;
                this.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.CancelClicked);
            }
        }

        private void btnFileInfo_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            IFileSelectorFileInfo fileSelectorFileInfo = null;
            if (radioButton != null)
            {
                fileSelectorFileInfo = this.Data.GetFileSelectorFileInfo(radioButton.Uid);
            }
            if (fileSelectorFileInfo == null)
            {
                this.CurrentFile = "";
                return;
            }
            this.CurrentFile = fileSelectorFileInfo.FileName;
        }

        private void btnFileInfo_Unchecked(object sender, RoutedEventArgs e)
        {
            this.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FileDeselected);
        }

        private void btnFolderInfo_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            if (button == null)
            {
                return;
            }
            IFileSelectorFolderInfo fileSelectorFolderInfo = this.Data.GetFileSelectorFolderInfo(button.Uid);
            if (fileSelectorFolderInfo == null)
            {
                this.CurrentPath = null;
                return;
            }
            this.CurrentPath = fileSelectorFolderInfo.FullFolderName;
            if (this.FileSelectorType == FileSelectorTypes.Load)
            {
                this.CurrentFile = "";
            }
        }

        private void btnGoUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.Data != null && this.Data.ParentPath != null)
            {
                this.CurrentPath = this.Data.ParentPath;
                if (this.FileSelectorType == FileSelectorTypes.Load)
                {
                    this.CurrentFile = "";
                }
            }
            this.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FolderChanged);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentFileView = this.tbxSelectedFile.Text;
            this.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.OKClicked);
            if (this.FileSelectionOK != null)
            {
                this.FileSelectionOK(this, new FileSelectorEventArgs(this.CurrentPath, this.CurrentFile));
            }
            if (this.CommandOK != null)
            {
                this.CommandOK.Execute(new FileSelectorEventArgs(this.CurrentPath, this.CurrentFile));
            }
        }

        private void cboSortOption_SortByDate_Selected(object sender, RoutedEventArgs e)
        {
            if (this.Data != null)
            {
                this.Data.ViewSortByDate();
            }
            this.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.SortChanged);
        }

        private void cboSortOption_SortByName_Selected(object sender, RoutedEventArgs e)
        {
            if (this.Data != null)
            {
                this.Data.ViewSortByName();
            }
            this.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.SortChanged);
        }

        public static UIElement FindUid(DependencyObject parent, string uid)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                UIElement child = VisualTreeHelper.GetChild(parent, i) as UIElement;
                if (child != null)
                {
                    if (child.Uid == uid)
                    {
                        return child;
                    }
                    child = FileSelectorUserControl.FindUid(child, uid);
                    if (child != null)
                    {
                        return child;
                    }
                }
            }
            return null;
        }

        private static void OnCurrentFilePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FileSelectorUserControl fileSelectorUserControl = source as FileSelectorUserControl;
            string newValue = (string)e.NewValue;
            if (fileSelectorUserControl.initCurrentFile == null)
            {
                fileSelectorUserControl.initCurrentFile = newValue;
            }
            if (fileSelectorUserControl.IsLoaded && fileSelectorUserControl.Data != null)
            {
                IFileSelectorFileInfo fileSelectorFileInfo = fileSelectorUserControl.Data.SelectFile(newValue, true);
                if (fileSelectorFileInfo == null || !fileSelectorFileInfo.IsSelected)
                {
                    fileSelectorUserControl.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FileDeselected);
                    return;
                }
                fileSelectorUserControl.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FileSelected);
            }
        }

        private static object OnCurrentFilePropertyValidate(DependencyObject d, object value)
        {
            return (string)value;
        }

        private static void OnCurrentPathPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FileSelectorUserControl fileSearchPattern = source as FileSelectorUserControl;
            string newValue = (string)e.NewValue;
            if (fileSearchPattern.initCurrentPath == null)
            {
                fileSearchPattern.initCurrentPath = newValue;
            }
            if (fileSearchPattern.IsLoaded)
            {
                if (fileSearchPattern.Data != null)
                {
                    fileSearchPattern.Data.FileSearchPattern = fileSearchPattern.FileSearchPattern;
                    fileSearchPattern.Data.DefaultFileExtension = fileSearchPattern.DefaultFileExtension;
                    fileSearchPattern.Data.ViewShowFileExtension = fileSearchPattern.ShowFileExtension;
                    fileSearchPattern.Data.GetFolderFileList(newValue);
                }
                fileSearchPattern.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FolderChanged);
            }
        }

        private static object OnCurrentPathPropertyValidate(DependencyObject d, object value)
        {
            string str = (string)value;
            if (!string.IsNullOrWhiteSpace((string)value))
            {
                str = ((string)value).Trim();
                if (!str.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    string str1 = string.Concat(str, Path.DirectorySeparatorChar);
                    str = str1;
                    str = str1;
                }
            }
            return str;
        }

        private static void OnDataPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FileSelectorUserControl viewListFolderInfo = source as FileSelectorUserControl;
            IFileSelectorData newValue = (IFileSelectorData)e.NewValue;
            if (newValue == null)
            {
                viewListFolderInfo.pnlFolders.ItemsSource = null;
                viewListFolderInfo.pnlFiles.ItemsSource = null;
                viewListFolderInfo.CurrentPath = null;
            }
            else
            {
                viewListFolderInfo.pnlFolders.ItemsSource = newValue.ViewListFolderInfo;
                viewListFolderInfo.pnlFiles.ItemsSource = newValue.ViewListFileInfo;
                newValue.DefaultFileExtension = viewListFolderInfo.DefaultFileExtension;
                newValue.ViewShowFileExtension = viewListFolderInfo.ShowFileExtension;
                newValue.FileSearchPattern = viewListFolderInfo.FileSearchPattern;
                newValue.RootPath = viewListFolderInfo.RootPath;
                if (viewListFolderInfo.IsLoaded)
                {
                    newValue.GetFolderFileList(viewListFolderInfo.CurrentPath);
                    viewListFolderInfo.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FolderChanged);
                    IFileSelectorFileInfo fileSelectorFileInfo = newValue.SelectFile(viewListFolderInfo.CurrentFile, true);
                    if (fileSelectorFileInfo != null && fileSelectorFileInfo.IsSelected)
                    {
                        viewListFolderInfo.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FileSelected);
                        return;
                    }
                    viewListFolderInfo.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FileDeselected);
                    return;
                }
            }
        }

        private static void OnDefaultFileExtensionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FileSelectorUserControl fileSelectorUserControl = source as FileSelectorUserControl;
            string newValue = (string)e.NewValue;
            if (fileSelectorUserControl.Data == null)
            {
                return;
            }
            fileSelectorUserControl.Data.DefaultFileExtension = newValue;
        }

        private static object OnDefaultFileExtensionPropertyValidate(DependencyObject d, object value)
        {
            string str = (string)value;
            if (!string.IsNullOrWhiteSpace((string)value))
            {
                str = ((string)value).Trim();
                if (!str.StartsWith("."))
                {
                    str = string.Concat(".", str);
                }
            }
            return str;
        }

        private static void OnFileSearchPatternPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FileSelectorUserControl fileSelectorUserControl = source as FileSelectorUserControl;
            string newValue = (string)e.NewValue;
            if (fileSelectorUserControl.Data == null)
            {
                return;
            }
            fileSelectorUserControl.Data.FileSearchPattern = newValue;
        }

        private static object OnFileSearchPatternPropertyValidate(DependencyObject d, object value)
        {
            return (string)value;
        }

        private static void OnFileSelectorTypePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FileSelectorUserControl fileSelectorUserControl = source as FileSelectorUserControl;
            FileSelectorTypes newValue = (FileSelectorTypes)e.NewValue;
            fileSelectorUserControl.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.TypeChanged);
            if (fileSelectorUserControl.IsLoaded && fileSelectorUserControl.Data != null)
            {
                if (fileSelectorUserControl.Data.SelectedFile != null)
                {
                    fileSelectorUserControl.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FileSelected);
                    return;
                }
                fileSelectorUserControl.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FileDeselected);
            }
        }

        private static void OnForceRescanFilesPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FileSelectorUserControl fileSelectorUserControl = source as FileSelectorUserControl;
            bool newValue = (bool)e.NewValue;
            if (fileSelectorUserControl.Data == null)
            {
                return;
            }
            if (newValue)
            {
                if (fileSelectorUserControl.Data.UpdateFolderFileList(fileSelectorUserControl.CurrentFile))
                {
                    if (fileSelectorUserControl.Data.SelectedFile == null)
                    {
                        fileSelectorUserControl.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FileDeselected);
                    }
                    else
                    {
                        fileSelectorUserControl.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.FileSelected);
                    }
                }
                fileSelectorUserControl.ForceRescanFiles = false;
            }
        }

        private static void OnRootPathPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FileSelectorUserControl fileSelectorUserControl = source as FileSelectorUserControl;
            string newValue = (string)e.NewValue;
            if (fileSelectorUserControl.Data == null)
            {
                return;
            }
            fileSelectorUserControl.Data.RootPath = newValue;
        }

        private static object OnRootPathPropertyValidate(DependencyObject d, object value)
        {
            string str = (string)value;
            if (!string.IsNullOrWhiteSpace((string)value))
            {
                str = ((string)value).Trim();
                if (!str.EndsWith("\\"))
                {
                    string str1 = string.Concat(str, "\\");
                    str = str1;
                    str = str1;
                }
            }
            return str;
        }

        private static void OnShowFileExtensionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FileSelectorUserControl fileSelectorUserControl = source as FileSelectorUserControl;
            bool newValue = (bool)e.NewValue;
            if (fileSelectorUserControl.Data == null)
            {
                return;
            }
            fileSelectorUserControl.Data.ViewShowFileExtension = newValue;
        }

        private static void OnTextCancelPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
            (source as FileSelectorUserControl).btnCancel.Content = newValue;
        }

        private static void OnTextFilesPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
            (source as FileSelectorUserControl).cbiTypeFiles.Content = newValue;
        }

        private static void OnTextGoUpPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
            (source as FileSelectorUserControl).btnGoUp.Content = newValue;
        }

        private static void OnTextOKPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
            (source as FileSelectorUserControl).btnOK.Content = newValue;
        }

        private static void OnTextSortByDatePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
            (source as FileSelectorUserControl).cbiSortByDate.Content = newValue;
        }

        private static void OnTextSortByNamePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
            (source as FileSelectorUserControl).cbiSortByName.Content = newValue;
        }

        private void tbxSelectedFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.TextChanged);
        }

        private void UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode updateMode)
        {
            FileSelectorTypes fileSelectorType;
            if (!base.IsLoaded)
            {
                return;
            }
            switch (updateMode)
            {
                case FileSelectorUserControl.UIControlsUpdateMode.Loaded:
                    {
                        fileSelectorType = this.FileSelectorType;
                        if (fileSelectorType == FileSelectorTypes.Load)
                        {
                            this.btnOK.IsEnabled = false;
                            this.btnOK.Visibility = System.Windows.Visibility.Hidden;
                            this.tbxSelectedFile.Visibility = System.Windows.Visibility.Hidden;
                            this.tbxSelectedFile.IsEnabled = false;
                            this.tbxSelectedFile.Text = this.CurrentFileView;
                        }
                        else if (fileSelectorType == FileSelectorTypes.Save)
                        {
                            this.btnOK.Visibility = System.Windows.Visibility.Visible;
                            this.tbxSelectedFile.Visibility = System.Windows.Visibility.Visible;
                            this.tbxSelectedFile.IsEnabled = true;
                            this.tbxSelectedFile.Text = this.CurrentFileView;
                        }
                        this.UpdateScrollViewer();
                        return;
                    }
                case FileSelectorUserControl.UIControlsUpdateMode.TypeChanged:
                    {
                        fileSelectorType = this.FileSelectorType;
                        if (fileSelectorType != FileSelectorTypes.Load)
                        {
                            if (fileSelectorType != FileSelectorTypes.Save)
                            {
                                return;
                            }
                            this.btnOK.Visibility = System.Windows.Visibility.Visible;
                            this.tbxSelectedFile.Visibility = System.Windows.Visibility.Visible;
                            this.tbxSelectedFile.IsEnabled = true;
                            this.tbxSelectedFile.Text = this.CurrentFileView;
                            return;
                        }
                        this.btnOK.IsEnabled = false;
                        this.btnOK.Visibility = System.Windows.Visibility.Hidden;
                        this.tbxSelectedFile.Visibility = System.Windows.Visibility.Hidden;
                        this.tbxSelectedFile.IsEnabled = false;
                        this.tbxSelectedFile.Text = this.CurrentFileView;
                        return;
                    }
                case FileSelectorUserControl.UIControlsUpdateMode.FileSelected:
                    {
                        fileSelectorType = this.FileSelectorType;
                        if (fileSelectorType == FileSelectorTypes.Load)
                        {
                            this.btnOK.IsEnabled = true;
                            this.btnOK.Visibility = System.Windows.Visibility.Visible;
                            this.tbxSelectedFile.Visibility = System.Windows.Visibility.Visible;
                            this.tbxSelectedFile.Text = this.CurrentFileView;
                        }
                        else if (fileSelectorType == FileSelectorTypes.Save)
                        {
                            this.tbxSelectedFile.Text = this.CurrentFileView;
                        }
                        this.UpdateScrollViewer();
                        return;
                    }
                case FileSelectorUserControl.UIControlsUpdateMode.FileDeselected:
                    {
                        fileSelectorType = this.FileSelectorType;
                        if (fileSelectorType == FileSelectorTypes.Load)
                        {
                            this.tbxSelectedFile.Visibility = System.Windows.Visibility.Hidden;
                            this.btnOK.IsEnabled = false;
                            this.btnOK.Visibility = System.Windows.Visibility.Hidden;
                            this.tbxSelectedFile.Text = this.CurrentFileView;
                        }
                        else if (fileSelectorType == FileSelectorTypes.Save)
                        {
                            this.tbxSelectedFile.Text = this.CurrentFileView;
                        }
                        this.UpdateScrollViewer();
                        return;
                    }
                case FileSelectorUserControl.UIControlsUpdateMode.FolderChanged:
                    {
                        if (this.Data == null)
                        {
                            this.btnGoUp.IsEnabled = false;
                        }
                        else
                        {
                            this.lblCurrentFolderName.Content = this.Data.ViewCurrentFolderName;
                            if (this.Data.ParentPath != null)
                            {
                                this.btnGoUp.IsEnabled = true;
                            }
                            else
                            {
                                this.btnGoUp.IsEnabled = false;
                            }
                        }
                        this.UpdateScrollViewer();
                        return;
                    }
                case FileSelectorUserControl.UIControlsUpdateMode.SortChanged:
                    {
                        this.UpdateScrollViewer();
                        return;
                    }
                case FileSelectorUserControl.UIControlsUpdateMode.CancelClicked:
                case FileSelectorUserControl.UIControlsUpdateMode.OKClicked:
                    {
                        return;
                    }
                case FileSelectorUserControl.UIControlsUpdateMode.TextChanged:
                    {
                        fileSelectorType = this.FileSelectorType;
                        if (fileSelectorType != FileSelectorTypes.Save)
                        {
                            return;
                        }
                        this.btnOK.IsEnabled = this.Data.IsValidFileName(this.tbxSelectedFile.Text);
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        private void UpdateScrollViewer()
        {
            if (this.Data == null)
            {
                return;
            }
            IFileSelectorFileInfo selectedFile = this.Data.SelectedFile;
            if (selectedFile != null)
            {
                this.pnlFiles.UpdateLayout();
                RadioButton radioButton = FileSelectorUserControl.FindUid(this.pnlFiles, selectedFile.FileInfoUid) as RadioButton;
                if (radioButton != null)
                {
                    Point screen = this.scvFoldersFiles.PointToScreen(new Point(0, 0));
                    double x = screen.X;
                    double actualWidth = x + this.scvFoldersFiles.ActualWidth;
                    screen = radioButton.PointToScreen(new Point(0, 0));
                    double num = screen.X;
                    double actualWidth1 = num + radioButton.ActualWidth;
                    if (num < x || actualWidth1 > actualWidth)
                    {
                        double horizontalOffset = this.scvFoldersFiles.HorizontalOffset + num - x;
                        if (this.scvFoldersFiles.HorizontalOffset != horizontalOffset)
                        {
                            this.scvFoldersFiles.ScrollToHorizontalOffset(horizontalOffset);
                            return;
                        }
                    }
                }
            }
            else if (this.scvFoldersFiles.HorizontalOffset != 0)
            {
                this.scvFoldersFiles.ScrollToHorizontalOffset(0);
            }
        }

        private void usrFileSelector_Loaded(object sender, RoutedEventArgs e)
        {
            FileSelectorUserControl fileSelectorDatum = sender as FileSelectorUserControl;
            if (fileSelectorDatum.Data == null)
            {
                fileSelectorDatum.Data = new FileSelectorData();
            }
            this.UIControlsUpdate(FileSelectorUserControl.UIControlsUpdateMode.Loaded);
            if (this.initCurrentPath != null)
            {
                fileSelectorDatum.CurrentPath = this.initCurrentPath;
            }
            if (this.initCurrentFile != null)
            {
                fileSelectorDatum.CurrentFile = this.initCurrentFile;
            }
        }

        public event FileSelectorUserControl.FileSelection FileSelectionCancel;

        public event FileSelectorUserControl.FileSelection FileSelectionOK;

        public delegate void FileSelection(object sender, FileSelectorEventArgs e);

        private enum UIControlsUpdateMode
        {
            None,
            Loaded,
            TypeChanged,
            FileSelected,
            FileDeselected,
            FolderChanged,
            SortChanged,
            CancelClicked,
            OKClicked,
            TextChanged
        }
    }
}