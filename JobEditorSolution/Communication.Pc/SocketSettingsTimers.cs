using Messages;
using System;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	public class SocketSettingsTimers
	{
		private Messages.Address Address;

		public bool ConnectRetryEnable
		{
			get
			{
				if (this.ConnectRetryWaitTime <= 0)
				{
					return false;
				}
				return true;
			}
		}

		public int ConnectRetryWaitTime
		{
			get;
			set;
		}

		public bool KeepAliveEnable
		{
			get
			{
				if (this.KeepAliveSendInterval <= 0)
				{
					return false;
				}
				if (this.KeepAliveTimeOutTime <= 0)
				{
					return false;
				}
				return true;
			}
		}

		public int KeepAliveSendInterval
		{
			get;
			set;
		}

		public int KeepAliveTimeOutTime
		{
			get;
			set;
		}

		public SocketSettingsTimers(Messages.Address parent, int keepAliveSendInterval, int keepAliveTimeOutTime, int connectRetryWaitTime)
		{
			this.Address = new Messages.Address(parent, typeof(SocketSettingsTimers));
			this.ConnectRetryWaitTime = connectRetryWaitTime;
			this.KeepAliveSendInterval = keepAliveSendInterval;
			this.KeepAliveTimeOutTime = keepAliveTimeOutTime;
		}
	}
}