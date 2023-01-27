using System;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	public class OnConnectEventArgs : EventArgs
	{
		public bool Connect
		{
			get;
			set;
		}

		public OnConnectEventArgs(bool connect)
		{
			this.Connect = connect;
		}
	}
}