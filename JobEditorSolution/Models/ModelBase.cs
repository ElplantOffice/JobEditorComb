
using Messages;
using Patterns.EventAggregator;
using System.Collections.Generic;

namespace Models
{
  public class ModelBase : IModel
  {
    private IEventAggregator aggregator = (IEventAggregator) SingletonProvider<EventAggregator>.Instance;
    private readonly List<IModelAttributedEventType> modelAttributedEventTypes = new List<IModelAttributedEventType>();

    public void AddAttributedEventType(IModelAttributedEventType modelAttributedEventType)
    {
      if (modelAttributedEventType == null)
        return;
      this.modelAttributedEventTypes.Add(modelAttributedEventType);
    }

    public virtual void Dispose()
    {
      foreach (IModelAttributedEventType attributedEventType in this.modelAttributedEventTypes)
        attributedEventType.Dispose();
      this.modelAttributedEventTypes.Clear();
    }

    public bool Show(UIProtoType uiProtoType, TelegramCommand showCommand = TelegramCommand.Show)
    {
      if (uiProtoType == null)
        return false;
      bool flag = true;
      foreach (ViewModelProtoType viewModel in uiProtoType.ViewModels)
      {
        if (!this.Show(viewModel, showCommand))
          flag = false;
      }
      return flag;
    }

    public bool Show(UIProtoType uiProtoType, bool show)
    {
      TelegramCommand showCommand = (TelegramCommand) 203;
      if (show)
        showCommand = (TelegramCommand) 201;
      return this.Show(uiProtoType, showCommand);
    }

    public bool Show(ViewModelProtoType viewModelProtoType, TelegramCommand showCommand = TelegramCommand.Show)
    {
            if (viewModelProtoType == null)
            {
                return false;
            }
            if (viewModelProtoType.AddressViewModelAdministrator == null)
            {
                return false;
            }
            Address address = new Address(viewModelProtoType.Address.Target, viewModelProtoType.AddressViewModelAdministrator.Owner, "", null);
            IEventMessage message = viewModelProtoType.GetMessage(address, showCommand);
            string target = viewModelProtoType.Address.Target;
            this.aggregator.Publish<Telegram>((Telegram)message, target, true);
            return true;
        }

    public bool Show(ViewModelProtoType viewModelProtoType, bool show)
    {
      TelegramCommand showCommand = (TelegramCommand) 203;
      if (show)
        showCommand = (TelegramCommand) 201;
      return this.Show(viewModelProtoType, showCommand);
    }
  }
}
