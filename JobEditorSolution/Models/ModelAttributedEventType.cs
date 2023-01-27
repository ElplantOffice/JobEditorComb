
using Messages;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Models
{
  public class ModelAttributedEventType<TValue> : IModelAttributedEventType
  {
    private CountingLatch tvalueLatch = new CountingLatch();
    private UiElementData<TValue> uiElementData = new UiElementData<TValue>();
    private EventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;
    private bool skipequaltest = true;
    private IModel model;
    private List<Address> addresses;
    private List<Address> relayAddresses;
    private string valueGUID;
    private ISubscription<Telegram> subscription;
    public Action<Telegram> OnValuePublisher;
    public Action<Telegram> OnValueRelayer;
    public Func<Telegram, bool> PrePublishRelayer;

    public ModelAttributedEventType(IModel model, IAddress address, Expression<Func<ModelAttributedEventType<TValue>>> expression)
    {
      if (expression.NodeType != ExpressionType.Lambda)
        throw new ArgumentException("Value must be a lamda expression", nameof (expression));
      this.valueGUID = VariableInfo.GetName((LambdaExpression) expression);
      this.addresses = new List<Address>();
      this.relayAddresses = new List<Address>();
      this.AddAddress(address);
      this.model = model;
      this.uiElementData.Type = typeof (TValue);
      this.uiElementData.Name = this.valueGUID;
      model?.AddAttributedEventType((IModelAttributedEventType) this);
    }

    public void Dispose()
    {
      if (this.subscription == null)
        return;
      if (this.aggregator == null)
        throw new ArgumentException("Event aggregator not initialised");
      this.aggregator.UnSubscribe<Telegram>(this.subscription);
      this.subscription = (ISubscription<Telegram>) null;
    }

    public UiElementData<TValue> Data
    {
      get
      {
        return this.uiElementData;
      }
      set
      {
        this.uiElementData = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public TValue Value
    {
      get
      {
        if (this.uiElementData == null)
          return default (TValue);
        return this.uiElementData.Value;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<TValue>.Default.Equals(this.uiElementData.Value, value))
          return;
        this.uiElementData.Value = value;
        ++this.uiElementData.ValueUpdateCounter;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public string TextId
    {
      get
      {
        if (this.uiElementData == null)
          return (string) null;
        return this.uiElementData.TextId;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<string>.Default.Equals(this.uiElementData.TextId, value))
          return;
        this.uiElementData.TextId = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public string ContentText
    {
      get
      {
        if (this.uiElementData == null)
          return (string) null;
        return this.uiElementData.ContentText;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<string>.Default.Equals(this.uiElementData.ContentText, value))
          return;
        this.uiElementData.ContentText = value;
        int num = this.uiElementData.TextId == "Active" ? 1 : 0;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public string ContentImage
    {
      get
      {
        if (this.uiElementData == null)
          return (string) null;
        return this.uiElementData.ContentImage;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<string>.Default.Equals(this.uiElementData.ContentImage, value))
          return;
        this.uiElementData.ContentImage = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public int ContentData
    {
      get
      {
        if (this.uiElementData == null)
          return 0;
        return this.uiElementData.ContentData;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<int>.Default.Equals(this.uiElementData.ContentData, value))
          return;
        this.uiElementData.ContentData = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public uint BackGround
    {
      get
      {
        if (this.uiElementData == null)
          return 0;
        return this.uiElementData.BackGround;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<uint>.Default.Equals(this.uiElementData.BackGround, value))
          return;
        this.uiElementData.BackGround = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public bool Blink
    {
      get
      {
        if (this.uiElementData == null)
          return false;
        return this.uiElementData.Blink;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<bool>.Default.Equals(this.uiElementData.Blink, value))
          return;
        this.uiElementData.Blink = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public bool IsEnabled
    {
      get
      {
        if (this.uiElementData == null)
          return false;
        return this.uiElementData.IsEnabled;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<bool>.Default.Equals(this.uiElementData.IsEnabled, value))
          return;
        this.uiElementData.IsEnabled = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public UiElementDataVisibility Visibility
    {
      get
      {
        if (this.uiElementData == null)
          return UiElementDataVisibility.Collapsed;
        return this.uiElementData.Visibility;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<UiElementDataVisibility>.Default.Equals(this.uiElementData.Visibility, value))
          return;
        this.uiElementData.Visibility = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public UiElementDataEventType EventType
    {
      get
      {
        if (this.uiElementData == null)
          return UiElementDataEventType.None;
        return this.uiElementData.EventType;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<UiElementDataEventType>.Default.Equals(this.uiElementData.EventType, value))
          return;
        this.uiElementData.EventType = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public bool EventState
    {
      get
      {
        if (this.uiElementData == null)
          return false;
        return this.uiElementData.EventState;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<bool>.Default.Equals(this.uiElementData.EventState, value))
          return;
        this.uiElementData.EventState = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public int Localisation
    {
      get
      {
        if (this.uiElementData == null)
          return 0;
        return this.uiElementData.Localisation;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<int>.Default.Equals(this.uiElementData.Localisation, value))
          return;
        this.uiElementData.Localisation = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public UiElementDataFormat Format
    {
      get
      {
        if (this.uiElementData == null)
          return UiElementDataFormat.None;
        return this.uiElementData.Format;
      }
      set
      {
        if (this.uiElementData == null || !this.skipequaltest && EqualityComparer<UiElementDataFormat>.Default.Equals(this.uiElementData.Format, value))
          return;
        this.uiElementData.Format = value;
        this.Publish();
        this.skipequaltest = false;
      }
    }

    public void Publish()
    {
      this.tvalueLatch.RunIfNotLatched((Action) (() =>
      {
        if (this.OnValuePublisher == null)
          return;
        using (List<Address>.Enumerator enumerator = this.addresses.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Telegram telegram = new Telegram((IAddress) enumerator.Current, 0, (object) this.uiElementData, (string) null);
            if (this.PrePublishRelayer != null && this.PrePublishRelayer(telegram))
              this.Relay(401);
            this.OnValuePublisher(telegram);
          }
        }
      }));
    }

    private void Relay(int cmd = 0)
    {
      if (this.OnValueRelayer == null)
        return;
      using (List<Address>.Enumerator enumerator = this.relayAddresses.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.OnValueRelayer(new Telegram((IAddress) enumerator.Current, cmd, (object) this.uiElementData, (string) null));
      }
    }

    public void AddAddress(IAddress address)
    {
      Address address1 = new Address(address, this.valueGUID);
      this.addresses.Add(address1);
      if (string.IsNullOrEmpty(address.Relay))
        return;
      this.relayAddresses.Add(new Address(address1.Owner, address1.Relay, address1.Target, (string) null));
    }

    public void AddRelayAddress(IAddress address)
    {
      if (string.IsNullOrEmpty(address.Relay))
        return;
      Address address1 = new Address(address, this.valueGUID);
      this.relayAddresses.Add(new Address(address1.Owner, address1.Relay, address1.Target, (string) null));
    }

    public void RegisterAsPublisher(Func<Telegram, bool> prePublishAction)
    {
      this.OnValuePublisher = (Action<Telegram>) (telegram => this.aggregator.Publish<Telegram>(telegram, true));
      this.PrePublishRelayer = prePublishAction;
    }

    public void RegisterAsPublisher()
    {
      this.RegisterAsPublisher((Func<Telegram, bool>) null);
    }

    public void RegisterAsListener(Action<Telegram> action)
    {
      if (this.addresses.Count <= 0)
        return;
      this.subscription = (ISubscription<Telegram>) this.aggregator.Subscribe<Telegram>(action, ((IEnumerable<Address>) this.addresses).First<Address>().Owner);
    }

    public void Register(Action<Telegram> listenerAction)
    {
      this.Register(listenerAction, (Func<Telegram, bool>) null);
    }

    public void Register(Action<Telegram> listenerAction, Func<Telegram, bool> prePublishAction)
    {
      this.RegisterAsListener(listenerAction);
      this.RegisterAsPublisher(prePublishAction);
    }

    public void RegisterRelay()
    {
      this.RegisterRelay((Func<Telegram, bool>) null);
    }

    public void RegisterRelay(Func<Telegram, bool> prePublishAction)
    {
      this.OnValueRelayer = (Action<Telegram>) (telegram => this.aggregator.Publish<Telegram>(telegram, true));
      this.RegisterAsListener(new Action<Telegram>(this.RelayAction));
      this.RegisterAsPublisher(prePublishAction);
    }

    public void RelayAction(Telegram telegram)
    {
      if (telegram.Value is UiElementData<TValue>)
      {
        this.uiElementData = telegram.Value as UiElementData<TValue>;
        this.Relay(0);
      }
      else
      {
        using (List<Address>.Enumerator enumerator = this.addresses.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            if (string.Compare(enumerator.Current.Target, telegram.Address.Owner) == 0)
              return;
          }
        }
        if (!this.uiElementData.Map(telegram.Value))
          return;
        this.Publish();
      }
    }
  }
}
