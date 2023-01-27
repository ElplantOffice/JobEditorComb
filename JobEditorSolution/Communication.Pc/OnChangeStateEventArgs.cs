using System;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	public class OnChangeStateEventArgs : EventArgs
	{
		public string ChannelId
		{
			get;
			set;
		}

		public IChannelState State
		{
			get;
			set;
		}

		public OnChangeStateEventArgs(IChannelState state, string channelId)
		{
			this.ChannelId = channelId;
			this.State = state.DeepCopy();
		}
	}
}