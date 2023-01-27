using Messages;
using System;
using System.Collections.Generic;

namespace Communication.Pc
{
	public interface IPacket
	{
		List<IMessage> Messages
		{
			get;
			set;
		}

		int Add(IMessage message);

		int Clear();

		int Remove(IMessage message);

		int Remove(int index);
	}
}