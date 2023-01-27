using Messages;
using Models;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
	public abstract class CommonBase<T> : IService
	where T : IServiceBaseDerived
	{
		protected Task task;

		protected List<ISubscription<Telegram>> _subscriptions = new List<ISubscription<Telegram>>();

		protected IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

		public virtual Address RelayAddress
		{
			get;
			set;
		}

		public virtual Services.Sender Sender
		{
			get;
			set;
		}

		protected CommonBase()
		{
		}

		public void AddAttributedEventType(IModelAttributedEventType modelAttributedEventType)
		{
		}

		public void Dispose()
		{
			foreach (ISubscription<Telegram> _subscription in this._subscriptions)
			{
				this.aggregator.UnSubscribe<Telegram>(_subscription);
			}
			this._subscriptions.Clear();
		}

		public static Telegram Factory(ModuleInfo module)
		{
			return (Telegram)CommonBase<T>.GetProtoType(new Address(new Address(module.GetId(ModuleInfo.Types.Base), "_Unknown_", "", null), typeof(T).Name)).GetMessage(new Address(module.GetId(ModuleInfo.Types.Application), module.GetId(ModuleInfo.Types.ModelAdministrator), "", null), TelegramCommand.Created);
		}

		public static IService Factory(ModelAdministrator administrator, ModuleInfo module)
		{
			Address address = new Address(module.GetId(ModuleInfo.Types.Base), "_Unknown_", "", null);
			IModel model = administrator.FetchModel(CommonBase<T>.GetProtoType(new Address(address, typeof(T).Name)));
			if (!(model is IService))
			{
				return null;
			}
			return model as IService;
		}

		private static UIProtoType GetProtoType(Address address)
		{
			ModelProtoType modelProtoType = new ModelProtoType(address, typeof(T));
			return new UIProtoType(new List<ViewModelProtoType>(), modelProtoType);
		}

		private void HandleAppCommands(Telegram telegram)
		{
			lock (this.task)
			{
				this.task = this.task.ContinueWith((Task ant) => this.HandleCommCommandsTask(telegram), TaskContinuationOptions.None);
			}
		}

		private void HandleCommands(Telegram telegram)
		{
			lock (this.task)
			{
				this.task = this.task.ContinueWith((Task ant) => this.HandleCommandsTask(telegram), TaskContinuationOptions.None);
			}
		}

		public abstract void HandleCommandsTask(Telegram telegram);

		private void HandleCommCommandsTask(Telegram telegram)
		{
			switch (telegram.Command)
			{
				case 11:
				{
					this.OnConnect(telegram);
					return;
				}
				case 12:
				{
					this.OnConnect(telegram);
					return;
				}
				case 13:
				{
					this.OnDisconnect(telegram);
					return;
				}
				default:
				{
					if (telegram.Command == 50)
					{
						break;
					}
					else
					{
						return;
					}
				}
			}
			if (this.TryRegisterRemoteClient(telegram))
			{
				this.OnConnect(telegram);
			}
		}

		public virtual void Init(IAddress address, Type clientType)
		{
			this.Init(GetProtoType(new Address(address)), clientType);
		}

		public virtual void Init(UIProtoType uiProtoType, Type clientType)
		{
			this.Sender = new Services.Sender(uiProtoType, clientType, null);
			this.RegisterAsListener((Telegram telegram) => this.HandleCommands(telegram), this.Sender.Owner);
			this.RegisterAsListener((Telegram telegram) => this.HandleAppCommands(telegram), this.Sender.ApplicationTarget(this.Sender.AppName));
			this.task = Task.Factory.StartNew(() => {
			});
		}

		public abstract void OnConnect(Telegram telegram);

		public abstract void OnDisconnect(Telegram telegram);

		public virtual void RaiseOnNotify(object sender, EventArgs e)
		{
			if (this.OnNotify != null)
			{
				this.OnNotify(sender, e);
			}
		}

		private void RegisterAsListener(Action<Telegram> action, string target)
		{
			this._subscriptions.Add(this.aggregator.Subscribe<Telegram>(action, target));
		}

		public virtual bool TryRegisterRemoteClient(Telegram telegram)
		{
			return false;
		}

		public virtual event EventHandler OnNotify;
	}
}