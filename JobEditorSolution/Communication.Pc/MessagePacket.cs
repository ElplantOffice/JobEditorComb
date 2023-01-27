using Messages;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	[Serializable]
	public class MessagePacket : IPacket
	{
		public List<IMessage> Messages
		{
			get;
			set;
		}

		public MessagePacket()
		{
			this.Messages = new List<IMessage>();
			this.Messages.Clear();
		}

		public int Add(IMessage message)
		{
			this.Messages.Add(message);
			return this.Messages.Count - 1;
		}

		public int Clear()
		{
			this.Messages.Clear();
			return 0;
		}

		public int Remove(IMessage message)
		{
			this.Messages.Remove(message);
			return 0;
		}

		public int Remove(int iindex)
		{
			this.Messages.RemoveAt(iindex);
			return 0;
		}
	}
}