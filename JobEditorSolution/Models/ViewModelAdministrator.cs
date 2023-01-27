using Messages;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Models
{
    public class ViewModelAdministrator
    {
        private ISubscription<Telegram> subclient;

        private Task task;

        private readonly object lockObjectNamedViews = new object();

        private readonly object lockObjectLocatedLastShownViewNames = new object();

        private SynchronizationContext uiContext;

        private Dictionary<string, IViewModelView> namedViews = new Dictionary<string, IViewModelView>();

        private Dictionary<string, List<string>> locatedLastShownViewNames = new Dictionary<string, List<string>>();

        public Address Address
        {
            get;
            private set;
        }

        public string ViewModelAssembly
        {
            get;
            set;
        }

        public string ViewModelXamlPath
        {
            get;
            set;
        }

        public ViewModelAdministrator(Address address, string viewModelXamlPath = "")
        {
            this.ViewModelXamlPath = viewModelXamlPath;
            this.Address = address;
            this.uiContext = new DispatcherSynchronizationContext(Application.Current.Dispatcher);
            IEventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            Action<Telegram> action = new Action<Telegram>(this.HandleMessage);
            this.task = Task.Factory.StartNew(() => {
            });
            this.subclient = instance.Subscribe<Telegram>(action, this.Address.Owner);
        }

        private void Acknowledge(UIProtoType proto, TelegramCommand command)
        {
            if (proto == null)
            {
                return;
            }
            SingletonProvider<EventAggregator>.Instance.Publish<Telegram>(new Telegram(this.Address, (int)command, proto, null), true);
        }

        private void Acknowledge(ViewModelProtoType proto, TelegramCommand command)
        {
            if (proto == null)
            {
                return;
            }
            SingletonProvider<EventAggregator>.Instance.Publish<Telegram>(new Telegram(this.Address, (int)command, proto, null), true);
        }

        private bool AddLastShownViewName(string location, string viewName)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(viewName))
            {
                return false;
            }
            lock (this.lockObjectLocatedLastShownViewNames)
            {
                if (!this.locatedLastShownViewNames.ContainsKey(location))
                {
                    this.locatedLastShownViewNames.Add(location, new List<string>());
                }
                List<string> item = this.locatedLastShownViewNames[location];
                if (item.Contains(viewName))
                {
                    item.Remove(viewName);
                    if (item.Count == 0)
                    {
                        this.locatedLastShownViewNames.Remove(location);
                    }
                }
                item.Add(viewName);
            }
            return true;
        }

        public void CreateViewModels(UIProtoType uiProtoType)
        {
            if (uiProtoType == null)
            {
                throw new ArgumentNullException("uiProtoType");
            }
            if (uiProtoType.ViewModels == null)
            {
                throw new ArgumentNullException("uiProtoType.ViewModels");
            }
            foreach (ViewModelProtoType viewModel in uiProtoType.ViewModels)
            {
                if (viewModel.Address == null)
                {
                    throw new ArgumentNullException("viewmodelprototype.Address");
                }
                if (viewModel.Address.Owner == null)
                {
                    throw new ArgumentNullException("viewmodelprototype.Address.Owner");
                }
                lock (this.lockObjectNamedViews)
                {
                    if (this.namedViews.ContainsKey(viewModel.Address.Owner))
                    {
                        ViewModelView item = (ViewModelView)this.namedViews[viewModel.Address.Owner];
                        if (item != null)
                        {
                            item.Dispose();
                        }
                        this.namedViews.Remove(viewModel.Address.Owner);
                    }
                }
                this.RemoveLastShownViewName(viewModel.Location, viewModel.Address.Owner);
                ViewModelView viewModelView = new ViewModelView(this, uiProtoType, viewModel);
                if (viewModelView == null)
                {
                    throw new ArgumentNullException("view");
                }
                if (!viewModelView.Create())
                {
                    throw new ArgumentException("view.Create()");
                }
                lock (this.lockObjectNamedViews)
                {
                    this.namedViews.Add(viewModel.Address.Owner, viewModelView);
                }
            }
            EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            Telegram telegram = new Telegram(this.Address, 2, uiProtoType, null);
            ((IEventAggregator)instance).Publish<Telegram>(telegram, true);
        }

        public virtual void Dispose()
        {
            if (this.subclient != null)
            {
                ((IEventAggregator)SingletonProvider<EventAggregator>.Instance).UnSubscribe<Telegram>(this.subclient);
                this.subclient = null;
            }
            lock (this.lockObjectNamedViews)
            {
                foreach (IViewModelView value in this.namedViews.Values)
                {
                    value.Dispose();
                }
                this.namedViews.Clear();
            }
        }

        public void DisposeViewModels(UIProtoType uiProtoType)
        {
            if (uiProtoType == null)
            {
                throw new ArgumentNullException("uiProtoType");
            }
            if (uiProtoType.ViewModels == null)
            {
                throw new ArgumentNullException("uiProtoType.ViewModels");
            }
            foreach (ViewModelProtoType viewModel in uiProtoType.ViewModels)
            {
                if (viewModel.Address == null)
                {
                    throw new ArgumentNullException("viewmodelprototype.Address");
                }
                if (viewModel.Address.Owner == null)
                {
                    throw new ArgumentNullException("viewmodelprototype.Address.Owner");
                }
                lock (this.lockObjectNamedViews)
                {
                    if (this.namedViews.ContainsKey(viewModel.Address.Owner))
                    {
                        ViewModelView item = (ViewModelView)this.namedViews[viewModel.Address.Owner];
                        if (item != null)
                        {
                            item.Dispose();
                        }
                        this.namedViews.Remove(viewModel.Address.Owner);
                    }
                }
                this.RemoveLastShownViewName(viewModel.Location, viewModel.Address.Owner);
                this.ShowLastShownViewModel(viewModel.Location);
            }
            EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            Telegram telegram = new Telegram(this.Address, 4, uiProtoType, null);
            ((IEventAggregator)instance).Publish<Telegram>(telegram, true);
        }

        private string GetLastShownViewName(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return null;
            }
            string item = null;
            lock (this.lockObjectLocatedLastShownViewNames)
            {
                if (this.locatedLastShownViewNames.ContainsKey(location))
                {
                    List<string> strs = this.locatedLastShownViewNames[location];
                    if (strs.Count > 0)
                    {
                        List<string> strs1 = strs;
                        item = strs1[strs1.Count - 1];
                    }
                }
            }
            return item;
        }

        private UIProtoType GetUIProto(ViewModelProtoType viewModelProtoType)
        {
            ViewModelView item = null;
            UIProtoType uIProtoType = null;
            lock (this.lockObjectNamedViews)
            {
                if (this.namedViews.ContainsKey(viewModelProtoType.Address.Owner))
                {
                    item = (ViewModelView)this.namedViews[viewModelProtoType.Address.Owner];
                }
            }
            if (item == null)
            {
                return uIProtoType;
            }
            return item.UiProtoType;
        }

        private void HandleMessage(Telegram telegram)
        {
            lock (this.task)
            {
                this.task = this.task.ContinueWith((Task ant) => this.HandleMessageTask(telegram), TaskContinuationOptions.None);
            }
        }

        public void HandleMessageTask(Telegram message)
        {
            Telegram telegram = message;
            if (telegram == null)
            {
                throw new ArgumentNullException("modelmessage");
            }
            if (telegram.Value == null)
            {
                throw new ArgumentNullException("modelmessage.Value");
            }
            if (message.Value is UIProtoType)
            {
                if (message.Command <= 3)
                {
                    if (message.Command == 1)
                    {
                        Application.Current.Dispatcher.Invoke(() => this.CreateViewModels(message.Value as UIProtoType));
                    }
                    else if (message.Command == 3)
                    {
                        Application.Current.Dispatcher.Invoke(() => this.DisposeViewModels(message.Value as UIProtoType));
                    }
                }
                else if (message.Command == 201)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        UIProtoType value = message.Value as UIProtoType;
                        this.ShowViewModel(value.ViewModels[0]);
                        this.Acknowledge(value, (TelegramCommand)202);
                    });
                }
                else if (message.Command == 203)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        UIProtoType value = message.Value as UIProtoType;
                        this.HideViewModel(value.ViewModels[0]);
                        this.Acknowledge(value, (TelegramCommand)204);
                    });
                }
            }
            if (message.Value is ViewModelProtoType)
            {
                if (message.Command == 201)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        this.Acknowledge(this.ShowViewModel(message.Value as ViewModelProtoType), (TelegramCommand)204);
                        this.Acknowledge(this.GetUIProto(message.Value as ViewModelProtoType), (TelegramCommand)202);
                    });
                    return;
                }
                if (message.Command != 203)
                {
                    return;
                }
                Application.Current.Dispatcher.Invoke(() => {
                    this.HideViewModel(message.Value as ViewModelProtoType);
                    this.Acknowledge(this.GetUIProto(message.Value as ViewModelProtoType), (TelegramCommand)204);
                });
            }
        }

        private UIProtoType HideLastShownViewModel(string location)
        {
            ViewModelView item = null;
            UIProtoType uIProtoType = null;
            string lastShownViewName = this.GetLastShownViewName(location);
            if (lastShownViewName == null)
            {
                return uIProtoType;
            }
            lock (this.lockObjectNamedViews)
            {
                if (this.namedViews.ContainsKey(lastShownViewName))
                {
                    item = (ViewModelView)this.namedViews[lastShownViewName];
                }
            }
            if (item == null)
            {
                return uIProtoType;
            }
            item.Hide();
            return item.UiProtoType;
        }

        public void HideViewModel(ViewModelProtoType viewModelProtoType)
        {
            if (viewModelProtoType == null)
            {
                throw new ArgumentNullException("viewModelProtoType");
            }
            if (viewModelProtoType.Address == null)
            {
                throw new ArgumentNullException("viewModelProtoType.Address");
            }
            if (viewModelProtoType.Address.Owner == null)
            {
                throw new ArgumentNullException("viewModelProtoType.Address.Owner");
            }
            ViewModelView item = null;
            lock (this.lockObjectNamedViews)
            {
                if (this.namedViews.ContainsKey(viewModelProtoType.Address.Owner))
                {
                    item = (ViewModelView)this.namedViews[viewModelProtoType.Address.Owner];
                }
            }
            if (item == null)
            {
                return;
            }
            this.RemoveLastShownViewName(viewModelProtoType.Location, viewModelProtoType.Address.Owner);
            item.Hide();
            this.ShowLastShownViewModel(viewModelProtoType.Location);
        }

        private bool RemoveLastShownViewName(string location, string viewName)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(viewName))
            {
                return false;
            }
            bool flag = false;
            lock (this.lockObjectLocatedLastShownViewNames)
            {
                if (this.locatedLastShownViewNames.ContainsKey(location))
                {
                    List<string> item = this.locatedLastShownViewNames[location];
                    if (item.Contains(viewName))
                    {
                        item.Remove(viewName);
                        if (item.Count == 0)
                        {
                            this.locatedLastShownViewNames.Remove(location);
                        }
                        flag = true;
                    }
                }
            }
            return flag;
        }

        private void ShowLastShownViewModel(string location)
        {
            string lastShownViewName = this.GetLastShownViewName(location);
            if (lastShownViewName == null)
            {
                return;
            }
            ViewModelView item = null;
            lock (this.lockObjectNamedViews)
            {
                if (this.namedViews.ContainsKey(lastShownViewName))
                {
                    item = (ViewModelView)this.namedViews[lastShownViewName];
                }
            }
            if (item == null)
            {
                return;
            }
            item.Show();
        }

        public UIProtoType ShowViewModel(ViewModelProtoType viewModelProtoType)
        {
            if (viewModelProtoType == null)
            {
                throw new ArgumentNullException("viewModelProtoType");
            }
            if (viewModelProtoType.Address == null)
            {
                throw new ArgumentNullException("viewModelProtoType.Address");
            }
            if (viewModelProtoType.Address.Owner == null)
            {
                throw new ArgumentNullException("viewModelProtoType.Address.Owner");
            }
            ViewModelView item = null;
            UIProtoType uIProtoType = null;
            lock (this.lockObjectNamedViews)
            {
                if (this.namedViews.ContainsKey(viewModelProtoType.Address.Owner))
                {
                    item = (ViewModelView)this.namedViews[viewModelProtoType.Address.Owner];
                }
            }
            if (item == null)
            {
                return uIProtoType;
            }
            uIProtoType = this.HideLastShownViewModel(viewModelProtoType.Location);
            item.Show();
            this.AddLastShownViewName(viewModelProtoType.Location, viewModelProtoType.Address.Owner);
            return uIProtoType;
        }
    }
}