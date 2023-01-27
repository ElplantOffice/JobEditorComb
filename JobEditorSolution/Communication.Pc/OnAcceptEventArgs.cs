using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	public class OnAcceptEventArgs : EventArgs
	{
		public bool Connect
		{
			get;
			set;
		}

		public Socket SocketAfterAccept
		{
			get;
			set;
		}

		public OnAcceptEventArgs(bool connect, Socket socketAfterAccept)
		{
			this.Connect = connect;
			this.SocketAfterAccept = socketAfterAccept;
		}
	}
}