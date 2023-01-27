using Communication.Plc;
using Communication.Plc.Channel;
using Messages;
using Models;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Services
{
	public abstract class ServiceBase<T> : PlcClientBase<T>, IService
	where T : IServiceBaseDerived
	{
		private List<Reciever> clientList;

		public Reciever CurrentClient
		{
			get;
			private set;
		}

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

		protected ServiceBase()
		{
		}

		public override void HandleCommandsTask(Telegram telegram)
		{
			this.CurrentTelegram = telegram;
			if (telegram.Address is PlcAddress)
			{
				this.PlcServices = (Communication.Plc.Channel.PlcServices)telegram.ServiceLink;
				this.Handler = base.GetHandler(telegram.Value);
				this.OnNotifiedByPlc(this.Handler.Command);
				return;
			}
			if (telegram.ServiceLink != null)
			{
				this.PlcServices = (Communication.Plc.Channel.PlcServices)telegram.ServiceLink;
				this.Handler = base.GetHandler(telegram.Value);
				this.OnNotifiedByPlc(this.Handler.Command);
				return;
			}
			this.CurrentClient = this.clientList.Find((Reciever x) => x.AppName == Services.Sender.ExtractAppName(telegram.Address.Owner));
			if (this.CurrentClient != null)
			{
				if (!this.CurrentClient.IsConnected)
				{
					this.CurrentClient.TryConnect(this.Sender, telegram.Address);
					if (this.CurrentClient.IsConnected)
					{
						this.CurrentClient.Value = telegram.Value;
						this.OnClientConnect(this.CurrentClient);
						this.CurrentClient.Value = telegram.Value;
					}
				}
				this.OnNotifiedByClient(telegram.Command);
			}
		}

		public override void Init(UIProtoType uiProtoType, Type clientType)
		{
			base.Init(uiProtoType, clientType);
			this.clientList = new List<Reciever>();
		}

		public void NotifyClient(Reciever client, int command)
		{
			this.WriteClient<object>(client, command, null);
		}

		public void NotifyClients(int command)
		{
			foreach (Reciever reciever in this.clientList)
			{
				this.WriteClient<object>(reciever, command, null);
			}
		}

		public void NotifyCurrentClient(int command)
		{
			if (this.CurrentClient != null)
			{
				this.WriteClient<object>(this.CurrentClient, command, null);
			}
		}

		public virtual void OnClientConnect(Reciever client)
		{
		}

		public virtual void OnClientDisconnect(Reciever client)
		{
		}

		public override void OnConnect(Telegram telegram)
		{
			Reciever reciever = this.clientList.Find((Reciever x) => x.AppName == Services.Sender.ExtractAppName(telegram.Address.Owner));
			if (reciever != null)
			{
				reciever.TryConnect(this.Sender, telegram.Address);
				this.OnClientConnect(reciever);
			}
		}

		public override void OnDisconnect(Telegram telegram)
		{
			Reciever reciever = this.clientList.Find((Reciever x) => x.AppName == Services.Sender.ExtractAppName(telegram.Address.Owner));
			if (reciever != null)
			{
				if (this.CurrentClient == reciever)
				{
					this.CurrentClient = null;
				}
				reciever.Disconnect();
				this.OnClientDisconnect(reciever);
			}
		}

		public virtual void OnNotifiedByClient(int Command)
		{
		}

		public virtual void OnNotifiedByPlc(int Command)
		{
		}

		public U ReadClient<U>(Reciever client)
		{
			Reciever reciever = this.clientList.Find((Reciever x) => x.AppName == client.AppName);
			if (reciever != null && reciever.Value is U)
			{
				return (U)reciever.Value;
			}
			return default(U);
		}

		public U ReadCurrentClient<U>()
		{
			if (this.CurrentClient == null || !(this.CurrentClient.Value is U))
			{
				return default(U);
			}
			return (U)this.CurrentClient.Value;
		}

		public void RegisterClient(string appName)
		{
			this.clientList.Add(new Reciever(appName));
		}

		public override bool TryRegisterRemoteClient(Telegram telegram)
		{
			if (!(telegram.Value is string))
			{
				return false;
			}
			if (telegram.Value as string != this.Sender.ReceiverType)
			{
				return false;
			}
			this.RegisterClient(Services.Sender.ExtractAppName(telegram.Address.Owner));
			return true;
		}

		public void WriteClient<U>(Reciever client, int command, U data)
		{
			Address address = new Address(this.Sender.Owner, client.Target, "", null);
			this.aggregator.Publish<Telegram>(new Telegram(address, command, (object)data, null), true);
		}

		public void WriteClients<U>(int command, U data)
		{
			foreach (Reciever reciever in this.clientList)
			{
				this.WriteClient<U>(reciever, command, data);
			}
		}

		public void WriteCurrentClient<U>(int command, U data)
		{
			Address address = new Address(this.Sender.Owner, this.CurrentClient.Target, "", null);
			this.aggregator.Publish<Telegram>(new Telegram(address, command, (object)data, null), true);
		}
	}
}