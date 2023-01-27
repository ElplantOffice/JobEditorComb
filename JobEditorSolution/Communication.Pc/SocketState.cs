using Messages;
using System;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	public class SocketState : IChannelState
	{
		private Status _oldcommstate;

		private Status _commstate;

		public Messages.Address Address
		{
			get
			{
				return JustDecompileGenerated_get_Address();
			}
			set
			{
				JustDecompileGenerated_set_Address(value);
			}
		}

		private Messages.Address JustDecompileGenerated_Address_k__BackingField;

		public Messages.Address JustDecompileGenerated_get_Address()
		{
			return this.JustDecompileGenerated_Address_k__BackingField;
		}

		private void JustDecompileGenerated_set_Address(Messages.Address value)
		{
			this.JustDecompileGenerated_Address_k__BackingField = value;
		}

		public string ChannelId
		{
			get
			{
				return JustDecompileGenerated_get_ChannelId();
			}
			set
			{
				JustDecompileGenerated_set_ChannelId(value);
			}
		}

		private string JustDecompileGenerated_ChannelId_k__BackingField;

		public string JustDecompileGenerated_get_ChannelId()
		{
			return this.JustDecompileGenerated_ChannelId_k__BackingField;
		}

		private void JustDecompileGenerated_set_ChannelId(string value)
		{
			this.JustDecompileGenerated_ChannelId_k__BackingField = value;
		}

		public Status CommState
		{
			get
			{
				return this._commstate;
			}
			set
			{
				this._oldcommstate = this._commstate;
				this._commstate = value;
			}
		}

		public string CommType
		{
			get
			{
				return JustDecompileGenerated_get_CommType();
			}
			set
			{
				JustDecompileGenerated_set_CommType(value);
			}
		}

		private string JustDecompileGenerated_CommType_k__BackingField;

		public string JustDecompileGenerated_get_CommType()
		{
			return this.JustDecompileGenerated_CommType_k__BackingField;
		}

		private void JustDecompileGenerated_set_CommType(string value)
		{
			this.JustDecompileGenerated_CommType_k__BackingField = value;
		}

		public Status OldCommState
		{
			get
			{
				return this._oldcommstate;
			}
		}

		public SocketState(Messages.Address parent)
		{
			this.Address = new Messages.Address(parent, typeof(SocketState));
		}

		public IChannelState DeepCopy()
		{
			SocketState address = this.MemberwiseClone() as SocketState;
			address.Address = new Address(this.Address);
			return address;
		}
	}
}