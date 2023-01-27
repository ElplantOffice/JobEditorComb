using Messages;
using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace Communication.Pc
{
	public class SocketSettings : ICommSettings
	{
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

		public SocketSettingsBuffers Buffers
		{
			get
			{
				return JustDecompileGenerated_get_Buffers();
			}
			set
			{
				JustDecompileGenerated_set_Buffers(value);
			}
		}

		private SocketSettingsBuffers JustDecompileGenerated_Buffers_k__BackingField;

		public SocketSettingsBuffers JustDecompileGenerated_get_Buffers()
		{
			return this.JustDecompileGenerated_Buffers_k__BackingField;
		}

		private void JustDecompileGenerated_set_Buffers(SocketSettingsBuffers value)
		{
			this.JustDecompileGenerated_Buffers_k__BackingField = value;
		}

		public IPEndPoint IpEndPoint
		{
			get;
			private set;
		}

		public SocketSettingsTimers Timers
		{
			get
			{
				return JustDecompileGenerated_get_Timers();
			}
			set
			{
				JustDecompileGenerated_set_Timers(value);
			}
		}

		private SocketSettingsTimers JustDecompileGenerated_Timers_k__BackingField;

		public SocketSettingsTimers JustDecompileGenerated_get_Timers()
		{
			return this.JustDecompileGenerated_Timers_k__BackingField;
		}

		private void JustDecompileGenerated_set_Timers(SocketSettingsTimers value)
		{
			this.JustDecompileGenerated_Timers_k__BackingField = value;
		}

		public SocketSettings(IPEndPoint ipEndPoint, Messages.Address parent, SocketSettingsBuffers buffers = null, SocketSettingsTimers timers = null)
		{
			this.Address = new Messages.Address(parent, typeof(SocketSettings));
			if (buffers == null)
			{
				buffers = new SocketSettingsBuffers(this.Address, 1000000, 1000000, 0);
			}
			if (timers == null)
			{
				timers = new SocketSettingsTimers(this.Address, 0, 0, 500);
			}
			this.Buffers = buffers;
			this.Timers = timers;
			this.IpEndPoint = ipEndPoint;
		}
	}
}