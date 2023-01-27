using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Models
{
    public class ViewModelView : IViewModelView
    {
        private ViewModelAdministrator viewModelAdministrator;

        private IViewModel viewModel;

        private Thread thread;

        private Window window;

        private Grid xamlGrid;

        private bool disposeCalled;

        public UIProtoType UiProtoType
        {
            get;
            private set;
        }

        public ViewModelProtoType ViewModelProtoType
        {
            get;
            private set;
        }

        public ViewModelView(ViewModelAdministrator viewModelAdministrator, UIProtoType uiProtoType, ViewModelProtoType viewModelProtoType)
        {
            this.viewModelAdministrator = viewModelAdministrator;
            this.UiProtoType = uiProtoType;
            this.ViewModelProtoType = viewModelProtoType;
        }

        private void CloseViewChild()
        {
            if (this.xamlGrid == null)
            {
                return;
            }
            Application current = Application.Current;
            Grid grid = null;
            current.Dispatcher.Invoke(new Action(() => grid = (Grid)current.MainWindow.FindName(this.ViewModelProtoType.Location)), DispatcherPriority.Background, null);
            if (grid == null)
            {
                throw new ArgumentException("viewModelProtoType.Location");
            }
            current.Dispatcher.Invoke(new Action(() => this.xamlGrid.DataContext = null), DispatcherPriority.Background, null);
            current.Dispatcher.Invoke(new Action(() => grid.Children.Remove(this.xamlGrid)), DispatcherPriority.Background, null);
            int count = 0;
            current.Dispatcher.Invoke(new Action(() => count = grid.Children.Count), DispatcherPriority.Background, null);
            if (count < 1)
            {
                current.Dispatcher.Invoke(new Action(() => ViewModelView.PushChildToBottomZOrder(grid)), DispatcherPriority.Background, null);
            }
            this.xamlGrid = null;
        }

        private void CloseViewWindow()
        {
            if (this.window == null)
            {
                return;
            }
            Window window = this.window;
            window.Dispatcher.Invoke(new Action(() => window.Close()), DispatcherPriority.Background, null);
        }

        public bool Create()
        {
            this.disposeCalled = false;
            if (!this.CreateViewModel())
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(this.ViewModelProtoType.Location))
            {
                return this.CreateViewChild();
            }
            return this.CreateViewWindow();
        }

        private bool CreateViewChild()
        {
            try
            {
                if (Application.Current == null)
                    throw new ArgumentNullException("Application.Current");
                if (this.viewModelAdministrator == null)
                    throw new ArgumentNullException("viewModelAdministrator");
                if (this.viewModelAdministrator.ViewModelXamlPath == null)
                    throw new ArgumentNullException("viewModelAdministrator.ViewModelXamlPath");
                if (this.ViewModelProtoType == null)
                    throw new ArgumentNullException("viewModelProtoType");
                if (this.ViewModelProtoType.Xaml == null)
                    throw new ArgumentNullException("viewModelProtoType.Xaml");
                if (this.ViewModelProtoType.Location == null)
                    throw new ArgumentNullException("viewModelProtoType.Location");
                Application curapp = Application.Current;
                Grid child = null;
                curapp.Dispatcher.Invoke(new Action(() => child = (Grid)curapp.MainWindow.FindName(this.ViewModelProtoType.Location)), DispatcherPriority.Background, null);
                //TODO DOK
                if (child == null)
                    return true;
                    //throw new ArgumentException("viewModelProtoType.Location");
                string path = this.viewModelAdministrator.ViewModelXamlPath + this.ViewModelProtoType.Xaml;
                Grid xamlgrid = null;
                foreach (string manifestResourceName in this.ViewModelProtoType.Assembly.GetManifestResourceNames())
                {
                    if (manifestResourceName.Contains(this.ViewModelProtoType.Xaml))
                    {
                        StreamReader streamReader = new StreamReader(this.ViewModelProtoType.Assembly.GetManifestResourceStream(manifestResourceName));
                        try
                        {
                            curapp.Dispatcher.Invoke(new Action(() => xamlgrid = (Grid)XamlReader.Load(streamReader.BaseStream)), DispatcherPriority.Background, null);
                            curapp.Dispatcher.Invoke(new Action(() => xamlgrid.DataContext = this.viewModel), DispatcherPriority.Background, null);
                            this.xamlGrid = xamlgrid;
                        }
                        finally
                        {
                            if (streamReader != null)
                                streamReader.Dispose();
                        }
                        return true;
                    }
                }
                FileStream filestream = new FileStream(path, FileMode.Open);
                if (filestream == null)
                    throw new ArgumentException("filename");
                curapp.Dispatcher.Invoke(new Action(() => xamlgrid = (Grid)XamlReader.Load((Stream)filestream)), DispatcherPriority.Background, null);
                filestream.Close();
                if (xamlgrid == null)
                    throw new ArgumentException("filestream");
                curapp.Dispatcher.Invoke(new Action(() => xamlgrid.DataContext = this.viewModel), DispatcherPriority.Background, null);
                this.xamlGrid = xamlgrid;
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Exception: " + ex.ToString());
                return false;
            }
            return true;
        }

        private bool CreateViewModel()
        {
            bool flag;
            try
            {
                if (this.UiProtoType == null)
                {
                    throw new ArgumentNullException("uiProtoType");
                }
                if (this.ViewModelProtoType == null)
                {
                    throw new ArgumentNullException("viewModelProtoType");
                }
                if (this.ViewModelProtoType.Assembly == null)
                {
                    throw new ArgumentNullException("viewModelProtoType.Assembly");
                }
                if (this.ViewModelProtoType.Type == null)
                {
                    throw new ArgumentNullException("viewModelProtoType.Type");
                }
                if (this.ViewModelProtoType.Address == null)
                {
                    throw new ArgumentNullException("viewModelProtoType.Address");
                }
                object[] viewModelProtoType = new object[1];
                if (viewModelProtoType == null)
                {
                    throw new ArgumentNullException("arg");
                }
                viewModelProtoType[0] = this.ViewModelProtoType;
                string name = this.ViewModelProtoType.Assembly.GetName().Name;
                if (name == null)
                {
                    throw new ArgumentNullException("assemblyname");
                }
                string str = this.ViewModelProtoType.Type.Name;
                if (str == null)
                {
                    throw new ArgumentNullException("typename");
                }
                if ((new StringBuilder(name)).Append('.').Append(str) == null)
                {
                    throw new ArgumentNullException("assemblytype");
                }
                object obj = Activator.CreateInstance(this.ViewModelProtoType.Type, viewModelProtoType);
                if (obj == null)
                {
                    throw new ArgumentNullException("viewmodelobject");
                }
                this.viewModel = (IViewModel)obj;
                return true;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("Exception: ", exception.ToString()));
                flag = false;
            }
            return flag;
        }

        private bool CreateViewWindow()
        {
            if (this.window != null)
            {
                return false;
            }
            Thread thread = new Thread(() => {
                try
                {
                    if (this.viewModelAdministrator == null)
                    {
                        throw new ArgumentNullException("viewModelAdministrator");
                    }
                    if (this.viewModelAdministrator.ViewModelXamlPath == null)
                    {
                        throw new ArgumentNullException("viewModelAdministrator.ViewModelXamlPath");
                    }
                    if (this.ViewModelProtoType == null)
                    {
                        throw new ArgumentNullException("viewModelProtoType");
                    }
                    if (this.ViewModelProtoType.Xaml == null)
                    {
                        throw new ArgumentNullException("viewModelProtoType.Xaml");
                    }
                    FileStream fileStream = new FileStream(string.Concat(this.viewModelAdministrator.ViewModelXamlPath, this.ViewModelProtoType.Xaml), FileMode.Open);
                    if (fileStream == null)
                    {
                        throw new ArgumentException("filename");
                    }
                    Window window = (Window)XamlReader.Load(fileStream);
                    fileStream.Close();
                    if (window == null)
                    {
                        throw new ArgumentException("filestream");
                    }
                    window.DataContext = this.viewModel;
                    window.Closed += new EventHandler((object s, EventArgs e) => window.Dispatcher.InvokeShutdown());
                    this.window = window;
                    Dispatcher.Run();
                    window.Closed -= new EventHandler((object s, EventArgs e) => window.Dispatcher.InvokeShutdown());
                    window.DataContext = null;
                    this.window = null;
                    this.thread = null;
                    if (this.disposeCalled)
                    {
                        this.window = null;
                    }
                    else
                    {
                        this.viewModelAdministrator.DisposeViewModels(this.UiProtoType);
                        return;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(string.Concat("Exception: ", exception.ToString()));
                }
            });
            if (thread == null)
            {
                return false;
            }
            thread.SetApartmentState(ApartmentState.STA);
            this.thread = thread;
            thread.Start();
            return true;
        }

        public void Dispose()
        {
            this.disposeCalled = true;
            if (string.IsNullOrWhiteSpace(this.ViewModelProtoType.Location))
            {
                this.CloseViewWindow();
            }
            else
            {
                this.CloseViewChild();
            }
            this.DisposeViewModel(this.viewModel);
        }

        private void DisposeViewModel(IViewModel viewmodelobject)
        {
            if (viewmodelobject != null)
            {
                viewmodelobject.Dispose();
            }
        }

        public bool Hide()
        {
            if (!string.IsNullOrWhiteSpace(this.ViewModelProtoType.Location))
            {
                return this.HideViewChild();
            }
            return this.HideViewWindow();
        }

        private bool HideViewChild()
        {
            if (this.xamlGrid == null)
            {
                return false;
            }
            Application current = Application.Current;
            Grid grid = null;
            current.Dispatcher.Invoke(new Action(() => grid = (Grid)current.MainWindow.FindName(this.ViewModelProtoType.Location)), DispatcherPriority.Background, null);
            if (grid == null)
            {
                throw new ArgumentException("viewModelProtoType.Location");
            }
            bool flag = false;
            current.Dispatcher.Invoke(new Action(() => flag = grid.Children.Contains(this.xamlGrid)), DispatcherPriority.Background, null);
            if (flag)
            {
                current.Dispatcher.Invoke(new Action(() => grid.Children.Remove(this.xamlGrid)), DispatcherPriority.Background, null);
            }
            int count = 0;
            current.Dispatcher.Invoke(new Action(() => count = grid.Children.Count), DispatcherPriority.Background, null);
            if (count < 1)
            {
                current.Dispatcher.Invoke(new Action(() => ViewModelView.PushChildToBottomZOrder(grid)), DispatcherPriority.Background, null);
            }
            return true;
        }

        private bool HideViewWindow()
        {
            if (this.window == null)
            {
                return false;
            }
            Window window = this.window;
            window.Dispatcher.Invoke(new Action(() => window.Hide()), DispatcherPriority.Background, null);
            return true;
        }

        private static void PushChildToBottomZOrder(FrameworkElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            Panel parent = element.Parent as Panel;
            if (parent != null)
            {
                int? nullable = null;
                foreach (UIElement child in parent.Children)
                {
                    if (child == element)
                    {
                        continue;
                    }
                    int zIndex = Panel.GetZIndex(child);
                    if (nullable.HasValue)
                    {
                        int? nullable1 = nullable;
                        if ((zIndex < nullable1.GetValueOrDefault() ? !nullable1.HasValue : true))
                        {
                            continue;
                        }
                    }
                    nullable = new int?(zIndex);
                }
                if (nullable.HasValue)
                {
                    Panel.SetZIndex(element, nullable.Value - 1);
                }
            }
        }

        private static void PushChildToTopZOrder(FrameworkElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            Panel parent = element.Parent as Panel;
            if (parent != null)
            {
                int? nullable = null;
                foreach (UIElement child in parent.Children)
                {
                    if (child == element)
                    {
                        continue;
                    }
                    int zIndex = Panel.GetZIndex(child);
                    if (nullable.HasValue)
                    {
                        int? nullable1 = nullable;
                        if ((zIndex > nullable1.GetValueOrDefault() ? !nullable1.HasValue : true))
                        {
                            continue;
                        }
                    }
                    nullable = new int?(zIndex);
                }
                if (nullable.HasValue)
                {
                    Panel.SetZIndex(element, nullable.Value + 1);
                }
            }
        }

        public bool Show()
        {
            if (!string.IsNullOrWhiteSpace(this.ViewModelProtoType.Location))
            {
                return this.ShowViewChild();
            }
            return this.ShowViewWindow();
        }

        private bool ShowViewChild()
        {
            if (this.xamlGrid == null)
            {
                return false;
            }
            Application current = Application.Current;
            Grid grid = null;
            current.Dispatcher.Invoke(new Action(() => grid = (Grid)current.MainWindow.FindName(this.ViewModelProtoType.Location)), DispatcherPriority.Background, null);
            if (grid == null)
            {
                throw new ArgumentException("viewModelProtoType.Location");
            }
            bool flag = false;
            current.Dispatcher.Invoke(new Action(() => flag = grid.Children.Contains(this.xamlGrid)), DispatcherPriority.Background, null);
            if (!flag)
            {
                current.Dispatcher.Invoke(new Action(() => grid.Children.Add(this.xamlGrid)), DispatcherPriority.Background, null);
            }
            current.Dispatcher.Invoke(new Action(() => ViewModelView.PushChildToTopZOrder(grid)), DispatcherPriority.Background, null);
            return true;
        }

        private bool ShowViewWindow()
        {
            if (this.window == null)
            {
                return false;
            }
            Window window = this.window;
            window.Dispatcher.Invoke(new Action(() => window.Show()), DispatcherPriority.Background, null);
            return true;
        }
    }
}