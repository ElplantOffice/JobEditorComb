using Messages;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Communication.Pc
{
	public class CommAdministrator
	{
		private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

		public IChannelProvider Channels;

		public Messages.Address Address
		{
			get;
			private set;
		}

		public Role CommunicationRole
		{
			get;
			private set;
		}

		public Messages.Address OwnerAddress
		{
			get;
			private set;
		}

		public CommAdministrator(Messages.Address owner, Role communicationRole, ICommChannel channel = null)
		{
			this.OwnerAddress = owner;
			this.Address = owner.GetBasePart(typeof(CommAdministrator));
			this.Channels = new SocketChannelProvider(this.Address, channel);
			this.CommunicationRole = communicationRole;
			if (communicationRole == Role.Server)
			{
				channel.OnChangeStateChannel -= new OnChangeStateEventHandler(this.OnChangeState);
				channel.OnChangeStateChannel += new OnChangeStateEventHandler(this.OnChangeState);
				channel.OnAcceptChannel -= new OnAcceptEventHandler(this.OnAccept);
				channel.OnAcceptChannel += new OnAcceptEventHandler(this.OnAccept);
				channel.Listen();
			}
		}

		public void AddChannel(ICommChannel channel)
		{
			if (channel == null)
			{
				throw new ArgumentNullException("channel");
			}
			channel.OnChangeStateChannel -= new OnChangeStateEventHandler(this.OnChangeState);
			channel.OnChangeStateChannel += new OnChangeStateEventHandler(this.OnChangeState);
			if (this.CommunicationRole == Role.Client)
			{
				this.Channels.AddClientChannel(channel);
				channel.OnConnectChannel -= new OnConnectEventHandler(this.OnConnect);
				channel.OnConnectChannel += new OnConnectEventHandler(this.OnConnect);
				channel.Connect();
			}
		}

		public ICommChannel GetChannel(string channelId)
		{
			return this.Channels.GetDefinedChannel(channelId);
		}

		public List<ICommChannel> GetChannels()
		{
			return this.Channels.GetDefinedChannelsList();
		}

		public void InitChannel(ICommChannel channel, Telegram message)
		{
			if (channel == null)
			{
				throw new ArgumentNullException("channel");
			}
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			string value = message.Value as string;
			this.Channels.DefineChannel(channel.ChannelId, value);
			this.Channels.GetDefinedChannel(value);
			EventAggregator instance = SingletonProvider<EventAggregator>.Instance;
			Action<Telegram> action = new Action<Telegram>(this.SendMessage);
			((IEventAggregator)instance).UnSubscribe<Telegram>(value);
			((IEventAggregator)instance).Subscribe<Telegram>(action, value);
		}

		public void OnAccept(object sender, OnAcceptEventArgs e)
		{
			ICommChannel commChannel = this.Channels.AddServerChannel((ICommChannel)sender, e);
			commChannel.OnReceivePacket -= new OnReceiveEventHandler(this.OnReceivePacket);
			commChannel.OnReceivePacket += new OnReceiveEventHandler(this.OnReceivePacket);
			if (!e.Connect)
			{
				this.PushState(commChannel, Status.Disconnected);
			}
			else
			{
				this.PushState(commChannel, Status.Connected);
			}
			commChannel.StartReceiving();
		}

		public void OnChangeState(object sender, OnChangeStateEventArgs e)
		{
			ICommChannel commChannel = (ICommChannel)sender;
			string channelId = e.ChannelId;
			IChannelState state = e.State;
			Telegram telegram = new Telegram(new Messages.Address(this.Address.Owner, this.OwnerAddress.Owner, "", null), 0, state, channelId);
			this.aggregator.Publish<Telegram>(telegram, "CommAdministrator.OnChangeState", false);
		}

		public void OnConnect(object sender, OnConnectEventArgs e)
		{
			ICommChannel commChannel = (ICommChannel)sender;
			if (e.Connect)
			{
				this.PushState(commChannel, Status.Connected);
				commChannel.OnReceivePacket += new OnReceiveEventHandler(this.OnReceivePacket);
				MessagePacket messagePacket = new MessagePacket();
				Telegram telegram = new Telegram(null, 11, this.Address.GetOwnerBasePart(), null);
				messagePacket.Add(telegram);
				commChannel.Packet = messagePacket;
				return;
			}
			if (commChannel.GetChannelState() == Status.Connecting)
			{
				ICommSettings settings = commChannel.GetSettings();
				if (settings.Timers.ConnectRetryEnable)
				{
					if (settings.Timers.ConnectRetryWaitTime > 0)
					{
						Thread.Sleep(settings.Timers.ConnectRetryWaitTime);
					}
					commChannel.Connect();
				}
			}
		}

		public void OnReceivePacket(object sender, OnReceiveEventArgs e)
		{
			ICommChannel commChannel = (ICommChannel)sender;
			foreach (IMessage message in e.Packet.Messages)
			{
				if (message.Address != null)
				{
					((IEventAggregator)SingletonProvider<EventAggregator>.Instance).Publish<Telegram>(message as Telegram, false);
				}
				else
				{
					int command = message.Command;
					if (command == 1)
					{
						continue;
					}
					switch (command)
					{
						case 11:
						{
							if (this.CommunicationRole != Role.Server)
							{
								continue;
							}
							this.InitChannel(commChannel, message as Telegram);
							Telegram telegram = new Telegram(null, 12, this.Address.GetOwnerBasePart(), null);
							MessagePacket messagePacket = new MessagePacket();
							messagePacket.Add(telegram);
							commChannel.Packet = messagePacket;
							continue;
						}
						case 12:
						{
							if (this.CommunicationRole != Role.Client)
							{
								continue;
							}
							this.InitChannel(commChannel, message as Telegram);
							Telegram telegram1 = new Telegram(null, 13, this.Address.GetOwnerBasePart(), null);
							MessagePacket messagePacket1 = new MessagePacket();
							messagePacket1.Add(telegram1);
							commChannel.Packet = messagePacket1;
							this.PushState(commChannel, Status.Linked);
							continue;
						}
						case 13:
						{
							if (this.CommunicationRole != Role.Server)
							{
								continue;
							}
							this.PushState(commChannel, Status.Linked);
							continue;
						}
						default:
						{
							continue;
						}
					}
				}
			}
		}

		private void PushState(ICommChannel channel, Status channelstate)
		{
			if (channel == null)
			{
				throw new ArgumentNullException("channel");
			}
			channel.SetChannelState(channelstate);
		}

		public void RemoveChannel(ICommChannel channel)
		{
			if (channel == null)
			{
				throw new ArgumentNullException("channel");
			}
			this.aggregator.UnSubscribe<Telegram>(channel.ChannelId);
			channel.Disconnect();
			channel.OnChangeStateChannel -= new OnChangeStateEventHandler(this.OnChangeState);
			if (this.CommunicationRole == Role.Client)
			{
				channel.OnConnectChannel -= new OnConnectEventHandler(this.OnConnect);
			}
			this.Channels.RemoveClientChannel(channel);
		}

		public void RemoveChannel(string channelId)
		{
			ICommChannel definedChannel = this.Channels.GetDefinedChannel(channelId);
			if (definedChannel != null)
			{
				this.RemoveChannel(definedChannel);
			}
		}

		public void RemoveChannels()
		{
			foreach (ICommChannel definedChannelsList in this.Channels.GetDefinedChannelsList())
			{
				this.RemoveChannel(definedChannelsList);
			}
		}

		public void SendMessage(Telegram telegram)
		{
			Messages.Address address = telegram.Address as Messages.Address;
			ICommChannel definedChannel = this.Channels.GetDefinedChannel(address.GetTargetBasePart());
			if (definedChannel == null)
			{
				throw new ArgumentNullException("channel");
			}
			Status channelState = definedChannel.GetChannelState();
			definedChannel.GetSettings();
			if (this.CommunicationRole == Role.Client && (channelState == Status.Connected || channelState == Status.Linked) || this.CommunicationRole != Role.Client && channelState == Status.Linked)
			{
				IPacket messagePacket = new MessagePacket();
				messagePacket.Add(telegram);
				if (definedChannel != null)
				{
					definedChannel.Packet = messagePacket;
				}
			}
		}

		public enum Command
		{
			None = 0,
			KeepAlive = 1,
			InitServer = 11,
			InitClient = 12,
			InitDone = 13
		}
	}
}