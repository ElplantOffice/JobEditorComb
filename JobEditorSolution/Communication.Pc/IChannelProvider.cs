using System;
using System.Collections.Generic;

namespace Communication.Pc
{
	public interface IChannelProvider
	{
		ICommChannel AddClientChannel(ICommChannel channel);

		ICommChannel AddServerChannel(ICommChannel listenChannel, OnAcceptEventArgs e);

		void DefineChannel(string channelId, string target);

		ICommChannel GetDefinedChannel(string application);

		List<ICommChannel> GetDefinedChannelsList();

		ICommChannel GetUnDefinedChannel(string schannelId);

		void RemoveClientChannel(ICommChannel channel);
	}
}