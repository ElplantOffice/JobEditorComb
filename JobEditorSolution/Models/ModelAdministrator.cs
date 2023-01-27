
using Communication.Plc;
using Communication.Plc.Shared;
using Messages;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Models
{
  public class ModelAdministrator
  {
        private IEventAggregator aggregator = (IEventAggregator)SingletonProvider<EventAggregator>.Instance;
    private ModelProvider _modelprovider = new ModelProvider();
    private Dictionary<string, IModel> Models = new Dictionary<string, IModel>();
    private Dictionary<IModel, UIProtoType> modelUIProtoTypeDict = new Dictionary<IModel, UIProtoType>();
    private Task task;
    private ISubscription<Telegram> subclient;

    public Address Address { get; private set; }

    public ModelAdministrator(Address address)
    {
      this.Address = address;
      Action<Telegram> action = new Action<Telegram>(this.HandleModels);
      this.task = Task.Factory.StartNew((Action) (() => {}));
      this.subclient = (ISubscription<Telegram>) this.aggregator.Subscribe<Telegram>(action, this.Address.Owner);
        }

    public virtual void Dispose()
    {
      if (this.subclient != null)
      {
        this.aggregator.UnSubscribe<Telegram>(this.subclient);
        this.subclient = (ISubscription<Telegram>) null;
      }
      lock (this.Models)
      {
        foreach (IModel model in this.Models.Values)
          model.Dispose();
        this.Models.Clear();
        this.modelUIProtoTypeDict.Clear();
      }
    }

    private void HandleModels(Telegram telegram)
    {
      lock (this.task)
        this.task = this.task.ContinueWith((Action<Task>) (ant => this.HandleModelsTask(telegram)), TaskContinuationOptions.None);
    }

    public void HandleModelsTask(Telegram telegram)
    {
            UIProtoType uIProtoType = this.CreatePrototype(telegram);
            if (uIProtoType == null)
            {
                if (!(telegram.Value is ViewModelProtoType))
                {
                    return;
                }
                ViewModelProtoType value = telegram.Value as ViewModelProtoType;
                if (telegram.Command == 201)
                {
                    this.ShowViewModel(value);
                    return;
                }
                if (telegram.Command != 203)
                {
                    return;
                }
                this.HideViewModel(value);
                return;
            }
            switch (telegram.Command)
            {
                case 1:
                    {
                        Application.Current.Dispatcher.Invoke(() => this.CreateViewModel(uIProtoType));
                        return;
                    }
                case 2:
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            this.CreateModel(uIProtoType);
                            this.Acknowledge(telegram);
                            this.AcknowlegdeCreateModelToApp(uIProtoType);
                        });
                        return;
                    }
                case 3:
                    {
                        Application.Current.Dispatcher.Invoke(() => this.DisposeViewModel(telegram.Value as UIProtoType));
                        return;
                    }
                case 4:
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            this.DisposeModel(uIProtoType);
                            this.Acknowledge(telegram);
                            this.AcknowlegdeDisposeModelToApp(uIProtoType);
                        });
                        return;
                    }
                default:
                    {
                        switch (telegram.Command)
                        {
                            case 201:
                                {
                                    Application.Current.Dispatcher.Invoke(() => this.ShowViewModel(uIProtoType.ViewModels[0]));
                                    return;
                                }
                            case 202:
                                {
                                    Application.Current.Dispatcher.Invoke(() => this.Acknowledge(telegram));
                                    return;
                                }
                            case 203:
                                {
                                    Application.Current.Dispatcher.Invoke(() => {
                                        this.HideViewModel(uIProtoType.ViewModels[0]);
                                        this.Acknowledge(telegram);
                                    });
                                    return;
                                }
                            case 204:
                                {
                                    Application.Current.Dispatcher.Invoke(() => this.Acknowledge(telegram));
                                    return;
                                }
                            default:
                                {
                                    return;
                                }
                        }
                        break;
                    }
            }
        }

    private void AcknowlegdeCreateModelToApp(UIProtoType uiProtoType)
    {
      this.aggregator.Publish<Telegram>(new Telegram((IAddress) new Address(this.Address.Owner, new ModuleInfo(this.Address.GetOwnerBasePart()).GetId(ModuleInfo.Types.Application), "", (string) null), 3, (object) uiProtoType, (string) null), true);
        }

    private void AcknowlegdeDisposeModelToApp(UIProtoType uiProtoType)
    {
      this.aggregator.Publish<Telegram>(new Telegram((IAddress) new Address(this.Address.Owner, new ModuleInfo(this.Address.GetOwnerBasePart()).GetId(ModuleInfo.Types.Application), "", (string) null), 4, (object) uiProtoType, (string) null), true);
    }

    private UIProtoType CreatePrototype(Telegram telegram)
    {
      if (telegram.Value is UIProtoType)
        return telegram.Value as UIProtoType;
      if (!(telegram.Value is PlcUiPrototypeRaw))
        return (UIProtoType) null;
      PlcUiPrototypeRaw rawPrototype = (PlcUiPrototypeRaw) telegram.Value;
      PlcAddress address = telegram.Address as PlcAddress;
      string owner = address.Owner;
      string str1 = address.GetTargetAncestor() + "." + (string) rawPrototype.Name;
      string str2 = (string) rawPrototype.Client + "." + (string) rawPrototype.Name;
      string str3 = str1;
      string str4 = str2;
      string channel = address.Channel;
      return new UIProtoType(new PlcAddress(owner, str3, str4, channel), rawPrototype);
    }

    private void Acknowledge(Telegram telegram)
    {
      if (!(telegram.Value is UIProtoType))
        return;
      UIProtoType uiProtoType = (UIProtoType) telegram.Value;
      if (string.IsNullOrEmpty(uiProtoType.Model.Address.Relay))
        return;
      this.aggregator.Publish<Telegram>(new Telegram((IAddress) new Address(telegram.Address.Target, uiProtoType.Model.Address.Relay, telegram.Address.Owner, (string) null), telegram.Command, telegram.Value, (string) null), true);
    }

    private void CreateViewModel(UIProtoType uiProtoType)
    {
      if (uiProtoType.ViewModels.Count <= 0)
        return;
      this.FillInUndefinedViewModelAdministratorAddress(uiProtoType);
      this.aggregator.Publish<Telegram>(new Telegram(this.GetModelAdministratorAddress(uiProtoType), 1, (object) uiProtoType, (string) null), uiProtoType.ViewModels[0].Address.Target, true);
    }

    private void DisposeViewModel(UIProtoType uiProtoType)
    {
      if (uiProtoType.ViewModels.Count <= 0)
        return;
      this.FillInUndefinedViewModelAdministratorAddress(uiProtoType);
      this.aggregator.Publish<Telegram>(new Telegram(this.GetModelAdministratorAddress(uiProtoType), 3, (object) uiProtoType, (string) null), uiProtoType.ViewModels[0].Address.Target, true);
    }

    private void ShowViewModel(ViewModelProtoType viewModelProtoType)
    {
      if (viewModelProtoType == null)
        return;
      this.FillInUndefinedViewModelAdministratorAddress(viewModelProtoType);
      this.aggregator.Publish<Telegram>(new Telegram(this.GetModelAdministratorAddress(viewModelProtoType), 201, (object) viewModelProtoType, (string) null), viewModelProtoType.Address.Target, true);
    }

    private void HideViewModel(ViewModelProtoType viewModelProtoType)
    {
      if (viewModelProtoType == null)
        return;
      this.FillInUndefinedViewModelAdministratorAddress(viewModelProtoType);
      this.aggregator.Publish<Telegram>(new Telegram(this.GetModelAdministratorAddress(viewModelProtoType), 203, (object) viewModelProtoType, (string) null), viewModelProtoType.Address.Target, true);
    }

    private void CreateModel(UIProtoType uiProtoType)
    {
      lock (this.Models)
      {
        string owner = uiProtoType.Model.Address.Owner;
        this.DisposeModel(owner);
        uiProtoType.Model.AddressModelAdministrator = this.GetModelAdministratorAddress(uiProtoType);
        IModel key = this._modelprovider.Create(uiProtoType);
        this.Models.Add(owner, key);
        this.modelUIProtoTypeDict.Add(key, uiProtoType);
      }
    }

    public IModel FetchModel(UIProtoType uiProtoType)
    {
      lock (this.Models)
      {
        string owner = uiProtoType.Model.Address.Owner;
        this.DisposeModel(owner);
        uiProtoType.Model.AddressModelAdministrator = this.GetModelAdministratorAddress(uiProtoType);
        IModel key = this._modelprovider.Create(uiProtoType);
        this.Models.Add(owner, key);
        this.modelUIProtoTypeDict.Add(key, uiProtoType);
        return key;
      }
    }

    private void DisposeModel(string key)
    {
      if (string.IsNullOrWhiteSpace(key))
        return;
      lock (this.Models)
      {
        if (!this.Models.ContainsKey(key))
          return;
        IModel model = this.Models[key];
        if (model != null)
        {
          model.Dispose();
          this.modelUIProtoTypeDict.Remove(model);
        }
        this.Models.Remove(key);
      }
    }

    private void DisposeModel(UIProtoType uiProtoType)
    {
      lock (this.Models)
        this.DisposeModel(uiProtoType.Model.Address.Owner);
    }

    public void DisposeModels(string channelId)
    {
      lock (this.Models)
      {
        foreach (KeyValuePair<string, IModel> keyValuePair in this.Models.Where<KeyValuePair<string, IModel>>((Func<KeyValuePair<string, IModel>, bool>) (x => this.GetClientChannelId(x.Value) == channelId)).ToList<KeyValuePair<string, IModel>>())
          this.DisposeModel(keyValuePair.Key);
      }
    }

    private string GetClientChannelId(IModel model)
    {
      UIProtoType uiProtoType = this.modelUIProtoTypeDict[model];
      string str = (string) null;
      IAddress administratorAddress = this.GetViewModelAdministratorAddress(uiProtoType);
      if (administratorAddress != null)
        str = new Address(administratorAddress, (string) null).GetOwnerBasePart();
      return str;
    }

    private IAddress GetViewModelAdministratorAddress(ViewModelProtoType viewModelProtoType)
    {
      if (viewModelProtoType == null)
        return (IAddress) null;
      if (viewModelProtoType.Address == null)
        return (IAddress) null;
      string str = new ModuleInfo(new Address(viewModelProtoType.Address, (string) null).GetOwnerBasePart()).GetId(ModuleInfo.Types.ViewModelAdministrator);
      if (viewModelProtoType.AddressViewModelAdministrator != null && viewModelProtoType.AddressViewModelAdministrator.Owner != null)
        str = viewModelProtoType.AddressViewModelAdministrator.Owner;
      return (IAddress) new Address(str, this.Address.Owner, "", (string) null);
    }

    private IAddress GetViewModelAdministratorAddress(UIProtoType uiProtoType)
    {
      IAddress iaddress = (IAddress) null;
      if (uiProtoType != null && uiProtoType.ViewModels.Count > 0)
        iaddress = this.GetViewModelAdministratorAddress(uiProtoType.ViewModels[0]);
      return iaddress;
    }

    private IAddress GetModelAdministratorAddress(ViewModelProtoType viewModelProtoType = null)
    {
      IAddress iaddress = (IAddress) null;
      if (viewModelProtoType != null)
        iaddress = this.GetViewModelAdministratorAddress(viewModelProtoType);
      return iaddress != null ? (IAddress) new Address(this.Address.Owner, iaddress.Owner, "", (string) null) : (IAddress) new Address((IAddress) this.Address, (string) null);
    }

    private IAddress GetModelAdministratorAddress(UIProtoType uiProtoType)
    {
      if (uiProtoType == null)
        return (IAddress) null;
      return uiProtoType.ViewModels.Count <= 0 ? this.GetModelAdministratorAddress((ViewModelProtoType) null) : this.GetModelAdministratorAddress(uiProtoType.ViewModels[0]);
    }

    private void FillInUndefinedViewModelAdministratorAddress(ViewModelProtoType viewModelProtoType)
    {
      if (viewModelProtoType == null || viewModelProtoType.AddressViewModelAdministrator != null)
        return;
      string str = (string) null;
      if (viewModelProtoType.Address != null)
      {
        string ownerBasePart = new Address(viewModelProtoType.Address, (string) null).GetOwnerBasePart();
        if (ownerBasePart != null)
          str = new ModuleInfo(ownerBasePart).GetId(ModuleInfo.Types.ViewModelAdministrator);
      }
      else
        str = this.Address.Target;
      viewModelProtoType.AddressViewModelAdministrator = (IAddress) new Address(str, this.Address.Owner, "", (string) null);
    }

    private void FillInUndefinedViewModelAdministratorAddress(UIProtoType uiProtoType)
    {
      if (uiProtoType == null)
        return;
      foreach (ViewModelProtoType viewModel in uiProtoType.ViewModels)
        this.FillInUndefinedViewModelAdministratorAddress(viewModel);
    }
  }
}
