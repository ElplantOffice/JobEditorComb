
using Messages;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Models
{
  public class ViewModelAttributedEventType<TValue> : INotifyPropertyChanged, IViewModelAttributedEventType
  {
    private UiElementData<TValue> uiElementData = new UiElementData<TValue>();
    private CountingLatch tvalueLatch = new CountingLatch();
    private EventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;
    private IViewModel viewModel;
    private ViewModelProtoType viewModelProtoType;
    private Address controlAddress;
    private string controlGUID;
    private ISubscription<Telegram> subscription;
    private Task task;
    public Action<Telegram> OnPublisher;
    public Action<Telegram> OnListener;

    public ViewModelAttributedEventType(IViewModel viewModel, ViewModelProtoType viewModelProtoType, System.Linq.Expressions.Expression<Func<ViewModelAttributedEventType<TValue>>> expression)
    {
      if (viewModelProtoType == null)
        throw new ArgumentNullException(nameof (viewModelProtoType));
      this.viewModelProtoType = viewModelProtoType;
      if (expression.NodeType != ExpressionType.Lambda)
        throw new ArgumentException("Value must be a lamda expression", nameof (expression));
      if (this.controlGUID == null)
        this.controlGUID = VariableInfo.GetName((LambdaExpression) expression);
      this.viewModel = viewModel;
      this.controlAddress = new Address(viewModelProtoType.Address, this.controlGUID);
      this.uiElementData.Type = typeof (TValue);
      this.uiElementData.Name = this.controlGUID;
      viewModel?.AddAttributedEventType((IViewModelAttributedEventType) this);
      this.task = Task.Factory.StartNew((Action) (() => {}));
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

    public void Register(Action<Telegram> customListenerAction = null, Action<Telegram> customPublisherAction = null)
    {
      this.RegisterAsListener(customListenerAction);
      this.RegisterAsPublisher(customPublisherAction);
    }

    public void RegisterAsListener(Action<Telegram> customListenerAction = null)
    {
            this.OnListener = customListenerAction;
            if (this.OnListener == null)
            {
                this.OnListener = new Action<Telegram>(this.DefaultListenerAction);
            }
            this.subscription = this.aggregator.Subscribe<Telegram>(this.OnListener, this.controlAddress.Owner);
        }

    public void RegisterAsPublisher(Action<Telegram> customPublisherAction = null)
    {
      this.OnPublisher = customPublisherAction;
      if (this.OnPublisher != null)
        return;
      this.OnPublisher = new Action<Telegram>(this.DefaultPublisherAction);
    }

    public void DefaultListenerAction(Telegram telegram)
    {
      lock (this.task)
        this.task = this.task.ContinueWith((Action<Task>) (ant => this.DefaultListenerActionTask(telegram)), TaskContinuationOptions.None);
    }

    public void DefaultListenerActionTask(Telegram telegram)
    {
      if (this.uiElementData == null)
        return;
      UiElementData<TValue> uiElementData = (UiElementData<TValue>) telegram.Value;
      if (uiElementData == null)
      {
        uiElementData = new UiElementData<TValue>();
        uiElementData.IsEnabled = false;
        uiElementData.Visibility = UiElementDataVisibility.Collapsed;
      }
      if (!EqualityComparer<int>.Default.Equals(this.uiElementData.ValueUpdateCounter, uiElementData.ValueUpdateCounter))
      {
        this.uiElementData.ValueUpdateCounter = uiElementData.ValueUpdateCounter;
        this.uiElementData.Value = uiElementData.Value;
        this.RaisePropertyChanged("Value");
      }
      if (!EqualityComparer<string>.Default.Equals(this.uiElementData.TextId, uiElementData.TextId))
      {
        this.uiElementData.TextId = uiElementData.TextId;
        this.RaisePropertyChanged("TextId");
      }
      if (!EqualityComparer<string>.Default.Equals(this.uiElementData.ContentText, uiElementData.ContentText))
      {
        int num = uiElementData.TextId == "Active" ? 1 : 0;
        this.uiElementData.ContentText = uiElementData.ContentText;
        this.RaisePropertyChanged("ContentText");
      }
      if (!EqualityComparer<string>.Default.Equals(this.uiElementData.ContentImage, uiElementData.ContentImage))
      {
        if (uiElementData.ContentImage != "")
          new FileResources().Data.Get<ImageSource>("abort.png");
        this.uiElementData.ContentImage = uiElementData.ContentImage;
        this.RaisePropertyChanged("ContentImage");
        this.RaisePropertyChanged("ContentImageAsImageSource");
      }
      if (!EqualityComparer<int>.Default.Equals(this.uiElementData.ContentData, uiElementData.ContentData))
      {
        this.uiElementData.ContentData = uiElementData.ContentData;
        this.RaisePropertyChanged("ContentData");
      }
      if (!EqualityComparer<UiElementDataVisibility>.Default.Equals(this.uiElementData.Visibility, uiElementData.Visibility))
      {
        this.uiElementData.Visibility = uiElementData.Visibility;
        this.RaisePropertyChanged("Visibility");
      }
      if (!EqualityComparer<bool>.Default.Equals(this.uiElementData.IsEnabled, uiElementData.IsEnabled))
      {
        this.uiElementData.IsEnabled = uiElementData.IsEnabled;
        this.RaisePropertyChanged("IsEnabled");
        this.RaisePropertyChanged("Command");
      }
      if (!EqualityComparer<uint>.Default.Equals(this.uiElementData.BackGround, uiElementData.BackGround))
      {
        this.uiElementData.BackGround = uiElementData.BackGround;
        this.RaisePropertyChanged("Background");
      }
      if (!EqualityComparer<bool>.Default.Equals(this.uiElementData.Blink, uiElementData.Blink))
      {
        this.uiElementData.Blink = uiElementData.Blink;
        this.RaisePropertyChanged("IsBlinkingEnabled");
      }
      if (!EqualityComparer<int>.Default.Equals(this.uiElementData.Localisation, uiElementData.Localisation))
      {
        this.uiElementData.Localisation = uiElementData.Localisation;
        this.RaisePropertyChanged("Localisation");
      }
      if (EqualityComparer<UiElementDataFormat>.Default.Equals(this.uiElementData.Format, uiElementData.Format))
        return;
      this.uiElementData.Format = uiElementData.Format;
      this.RaisePropertyChanged("Format");
    }

    public void DefaultPublisherAction(Telegram telegram)
    {
            this.aggregator.Publish<Telegram>(telegram, true);
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
        if (this.uiElementData == null || EqualityComparer<TValue>.Default.Equals(this.uiElementData.Value, value))
          return;
        this.uiElementData.Value = value;
        this.tvalueLatch.RunIfNotLatched((Action) (() =>
        {
          if (this.OnPublisher != null)
            this.OnPublisher(new Telegram((IAddress) this.controlAddress, 0, (object) this.uiElementData, (string) null));
          this.RaisePropertyChanged(nameof (Value));
        }));
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
        if (this.uiElementData == null)
          return;
        this.uiElementData.TextId = value;
        this.RaisePropertyChanged(nameof (TextId));
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
        if (this.uiElementData == null)
          return;
        this.uiElementData.ContentText = value;
        this.RaisePropertyChanged(nameof (ContentText));
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
        if (this.uiElementData == null)
          return;
        this.uiElementData.ContentImage = value;
        this.RaisePropertyChanged(nameof (ContentImage));
        this.RaisePropertyChanged("ContentImageAsImageSource");
      }
    }

    public ImageSource ContentImageAsImageSource
    {
      get
      {
        ImageSource imageSource = (ImageSource) null;
        if (this.uiElementData != null && this.uiElementData.ContentImage != null)
          imageSource = (ImageSource) new BitmapImage(new Uri(this.uiElementData.ContentImage, UriKind.RelativeOrAbsolute));
        return imageSource;
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
        if (this.uiElementData == null)
          return;
        this.uiElementData.ContentData = value;
        this.RaisePropertyChanged(nameof (ContentData));
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
        if (this.uiElementData == null)
          return;
        this.uiElementData.IsEnabled = value;
        this.RaisePropertyChanged(nameof (IsEnabled));
        this.RaisePropertyChanged("Command");
      }
    }

    public System.Windows.Visibility? Visibility
    {
      get
      {
        if (this.uiElementData == null)
          return new System.Windows.Visibility?();
        switch (this.uiElementData.Visibility)
        {
          case UiElementDataVisibility.Collapsed:
            return new System.Windows.Visibility?(System.Windows.Visibility.Collapsed);
          case UiElementDataVisibility.Hidden:
            return new System.Windows.Visibility?(System.Windows.Visibility.Hidden);
          case UiElementDataVisibility.Visible:
            return new System.Windows.Visibility?(System.Windows.Visibility.Visible);
          default:
            return new System.Windows.Visibility?();
        }
      }
      set
      {
        if (this.uiElementData == null)
          return;
        if (value.HasValue)
        {
          System.Windows.Visibility? nullable = value;
          if (nullable.HasValue)
          {
            switch (nullable.GetValueOrDefault())
            {
              case System.Windows.Visibility.Visible:
                this.uiElementData.Visibility = UiElementDataVisibility.Visible;
                break;
              case System.Windows.Visibility.Hidden:
                this.uiElementData.Visibility = UiElementDataVisibility.Hidden;
                break;
              case System.Windows.Visibility.Collapsed:
                this.uiElementData.Visibility = UiElementDataVisibility.Collapsed;
                break;
            }
          }
        }
        else
          this.uiElementData.Visibility = UiElementDataVisibility.Collapsed;
        this.RaisePropertyChanged(nameof (Visibility));
      }
    }

    public bool IsBlinkingEnabled
    {
      get
      {
        if (this.uiElementData == null)
          return false;
        return this.uiElementData.Blink;
      }
      set
      {
        if (this.uiElementData == null)
          return;
        this.uiElementData.Blink = value;
        this.RaisePropertyChanged(nameof (IsBlinkingEnabled));
      }
    }

    public Color Background
    {
      get
      {
        if (this.uiElementData == null)
          return Colors.Transparent;
        int num1 = (int) (byte) (this.uiElementData.BackGround >> 24);
        byte num2 = (byte) (this.uiElementData.BackGround >> 16);
        byte num3 = (byte) (this.uiElementData.BackGround >> 8);
        byte backGround = (byte) this.uiElementData.BackGround;
        int num4 = (int) num2;
        int num5 = (int) num3;
        int num6 = (int) backGround;
        return Color.FromArgb((byte) num1, (byte) num4, (byte) num5, (byte) num6);
      }
      set
      {
        if (this.uiElementData == null)
          return;
        this.uiElementData.BackGround = (uint) ((int) value.A << 24 | (int) value.R << 16 | (int) value.G << 8) | (uint) value.B;
        this.RaisePropertyChanged(nameof (Background));
      }
    }

    private bool CanExecCommand()
    {
      return this.OnPublisher != null && this.uiElementData != null && this.uiElementData.IsEnabled;
    }

    private void ExecCommand()
    {
      if (this.OnPublisher == null || this.uiElementData == null)
        return;
      this.uiElementData.EventState = true;
      this.OnPublisher(new Telegram((IAddress) this.controlAddress, 0, (object) this.uiElementData, (string) null));
      this.uiElementData.EventState = false;
    }

    public ICommand Command
    {
      get
      {
        return (ICommand) new RelayCommand(new Action(this.ExecCommand), new Func<bool>(this.CanExecCommand));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string propertyName)
    {
      // ISSUE: reference to a compiler-generated field
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
