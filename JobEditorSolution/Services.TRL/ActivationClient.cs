using Messages;
using Models;
using Patterns.EventAggregator;
using Services;
using System;
using System.Runtime.CompilerServices;

namespace Services.TRL
{
	public class ActivationClient : ClientBase<ActivationClient>, IServiceBaseDerived, IModel
	{
		private ActivationClient.Commands plcState;

		public Action<ActivationClient.Commands> OnPlcStateChanged
		{
			get;
			set;
		}

		public ActivationClient.Commands PlcState
		{
			get
			{
				this.RequestState();
				return this.plcState;
			}
			private set
			{
				this.plcState = value;
			}
		}

		public ActivationClient(UIProtoType uiProtoType)
		{
            this.Init(uiProtoType, typeof(ProductService));
			this.OnPlcStateChanged = null;
			this.PlcState = ActivationClient.Commands.PlcStateStopped;
		}

		public ActivationClient(IAddress ownerAddress)
		{
			this.Init(ownerAddress, typeof(ProductService));
			this.OnPlcStateChanged = null;
			this.PlcState = ActivationClient.Commands.PlcStateStopped;
		}

		public override void OnNotifiedByServer(int command)
		{
			if (base.CurrentTelegram == null)
			{
				return;
			}
			ActivationClient.Commands command1 = (ActivationClient.Commands)command;
			if (command1 == ActivationClient.Commands.PlcStateStopped || command1 == ActivationClient.Commands.PlcStateRunning)
			{
				this.PlcState = (ActivationClient.Commands)command;
				if (this.OnPlcStateChanged != null)
				{
					this.OnPlcStateChanged(this.plcState);
				}
			}
			this.RaiseOnNotify(this, new EventArgs());
		}

		private void RequestState()
		{
			this.aggregator.Publish<Telegram>(new Telegram(base.Server.Address, 21, null, null), true);
		}

		public void Send(string fullFilenameProduct)
		{
			this.aggregator.Publish<Telegram>(new Telegram(base.Server.Address, 20, new ServiceProductData(fullFilenameProduct), null), true);
		}

		public enum Commands
		{
			PlcStateNone = 0,
			ProcessJobs = 20,
			RequestState = 21,
			PlcStateStopped = 30,
			PlcStateRunning = 31
		}
	}
}