using Messages;
using System;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	public interface ICommChannel
	{
		Messages.Address Address
		{
			get;
		}

		string ChannelId
		{
			get;
		}

		IPacket Packet
		{
			get;
			set;
		}

		void Connect();

		int Disconnect();

		Status GetChannelOldState();

		Status GetChannelState();

		ICommSettings GetSettings();

		void Listen();

		void SetChannelState(Status state);

		ICommChannel ShallowCopy(OnAcceptEventArgs e);

		void StartReceiving();

		event OnAcceptEventHandler OnAcceptChannel;

		event OnChangeStateEventHandler OnChangeStateChannel;

		event OnConnectEventHandler OnConnectChannel;

		event OnReceiveEventHandler OnReceivePacket;
	}
}