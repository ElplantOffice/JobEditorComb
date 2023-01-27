using System;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	public class OnReceiveEventArgs : EventArgs
	{
		public IPacket Packet
		{
			get;
			set;
		}

		public OnReceiveEventArgs(IPacket packet)
		{
			this.Packet = packet;
		}
	}
}