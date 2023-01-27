using Messages;
using System;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	public class SocketSettingsBuffers
	{
		private Messages.Address Address;

		public int MaxPacketBufferSize
		{
			get;
			private set;
		}

		public int ReceiveBufferSize
		{
			get;
			private set;
		}

		public int SendBufferSize
		{
			get;
			private set;
		}

		public SocketSettingsBuffers(Messages.Address parent, int sendBufferSize, int receiveBufferSize, int maxPacketBufferSize = 0)
		{
			this.Address = new Messages.Address(parent, typeof(SocketSettingsBuffers));
			this.SendBufferSize = sendBufferSize;
			this.ReceiveBufferSize = receiveBufferSize;
			this.MaxPacketBufferSize = maxPacketBufferSize;
		}
	}
}