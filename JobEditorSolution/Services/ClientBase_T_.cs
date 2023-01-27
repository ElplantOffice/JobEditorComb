using Messages;
using Models;
using Patterns.EventAggregator;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Services
{
	public abstract class ClientBase<T> : CommonBase<T>
	where T : IServiceBaseDerived
	{
		public Telegram CurrentTelegram
		{
			get;
			set;
		}

		public override Services.Sender Sender
		{
			get;
			set;
		}

		public Reciever Server
		{
			get;
			private set;
		}

		protected ClientBase()
		{
		}

		public override void HandleCommandsTask(Telegram telegram)
		{
			this.Server.TryConnect(this.Sender, telegram.Address);
			this.CurrentTelegram = telegram;
			this.OnNotifiedByServer(telegram.Command);
		}

		public override void Init(UIProtoType uiProtoType, Type serviceType)
		{
			base.Init(uiProtoType, serviceType);
			this.Server = new Reciever("Server");
		}

		public void NotifyServer(int command)
		{
			this.WriteServer<object>(command, null);
		}

		public override void OnConnect(Telegram telegram)
		{
			if (!this.Server.IsConnected)
			{
				this.Server.TryConnect(this.Sender, telegram.Address);
			}
			if (this.Server.IsConnected)
			{
				this.RegisterClientOnServer();
				this.OnServerConnect(this.Server);
			}
		}

		public override void OnDisconnect(Telegram telegram)
		{
			if (this.Server == null)
			{
				return;
			}
			string str = telegram.Address.Owner.Substring(0, telegram.Address.Owner.IndexOf('.'));
			if (!this.Server.AppName.Equals(str))
			{
				return;
			}
			this.Server.Disconnect();
			this.OnServerDisconnect(this.Server);
		}

		public virtual void OnNotifiedByServer(int Command)
		{
		}

		public virtual void OnServerConnect(Reciever server)
		{
		}

		public virtual void OnServerDisconnect(Reciever server)
		{
		}

		public U ReadServer<U>()
		{
			if (this.Server == null || !(this.Server.Value is U))
			{
				return default(U);
			}
			return (U)this.Server.Value;
		}

		private void RegisterClientOnServer()
		{
			this.aggregator.Publish<Telegram>(new Telegram(new Address(this.Sender.Owner, this.Sender.ApplicationTarget(this.Server.AppName), "", null), 50, typeof(T).Name, null), true);
		}

		public void WriteServer<U>(int command, U data)
		{
			if (this.Server == null)
			{
				return;
			}
			Address address = new Address(this.Sender.Owner, this.Server.Target, "", null);
			this.aggregator.Publish<Telegram>(new Telegram(address, command, (object)data, null), true);
		}
	}
}