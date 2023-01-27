using Messages;
using System;

namespace Communication.Pc
{
	public interface IChannelState
	{
		Messages.Address Address
		{
			get;
		}

		string ChannelId
		{
			get;
		}

		Status CommState
		{
			get;
			set;
		}

		string CommType
		{
			get;
		}

		Status OldCommState
		{
			get;
		}

		IChannelState DeepCopy();
	}
}