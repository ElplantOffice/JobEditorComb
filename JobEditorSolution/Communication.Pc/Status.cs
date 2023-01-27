using System;

namespace Communication.Pc
{
	public enum Status
	{
		None,
		Disconnected,
		Disconnecting,
		Connected,
		Connecting,
		Linked,
		Error,
		KeepAliveTimeOut
	}
}