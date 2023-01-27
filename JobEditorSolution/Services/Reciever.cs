using Messages;
using System;
using System.Runtime.CompilerServices;

namespace Services
{
	public class Reciever
	{
		public Messages.Address Address
		{
			get;
			private set;
		}

		public string AppName
		{
			get;
			private set;
		}

		public bool IsConnected
		{
			get;
			internal set;
		}

		public string Target
		{
			get
			{
				if (this.Address == null)
				{
					return "";
				}
				return this.Address.Target;
			}
		}

		internal object Value
		{
			get;
			set;
		}

		public Reciever(string appName)
		{
			this.AppName = appName;
		}

		public void Disconnect()
		{
			this.Address = null;
			this.IsConnected = false;
		}

		public void TryConnect(Sender sender, IAddress receivedAddress)
		{
			if (this.IsConnected)
			{
				return;
			}
			this.TryConnect(sender, Sender.ExtractAppName(receivedAddress.Owner));
		}

		public void TryConnect(Sender sender, string recieverApp)
		{
			if (this.IsConnected)
			{
				return;
			}
			if (sender == null)
			{
				return;
			}
			if (!this.AppName.Equals(recieverApp))
			{
				return;
			}
			this.Address = new Messages.Address(sender.Owner, string.Concat(this.AppName, sender.PartialTarget), "", null);
			this.IsConnected = true;
		}
	}
}