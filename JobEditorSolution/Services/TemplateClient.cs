using Messages;
using Models;
using Patterns.EventAggregator;
using System;

namespace Services
{
	public class TemplateClient : ClientBase<TemplateClient>, IServiceBaseDerived, IModel
	{
		public TemplateClient(UIProtoType uiProtoType)
		{
			this.Init(uiProtoType, typeof(TemplateService));
		}

		public override void OnNotifiedByServer(int command)
		{
			base.ReadServer<string>();
			base.WriteServer<string>(0, "test");
			base.WriteServer<string>(0, "test");
			base.NotifyServer(0);
			this.RaiseOnNotify(this, new EventArgs());
			IEventAggregator eventAggregator = this.aggregator;
			Address address = base.Server.Address;
			PlcTemplateDataRaw plcTemplateDataRaw = new PlcTemplateDataRaw();
			eventAggregator.Publish<Telegram>(new Telegram(address, 0, (object)plcTemplateDataRaw, null), true);
		}

		public override void OnServerConnect(Reciever client)
		{
		}

		public override void OnServerDisconnect(Reciever client)
		{
		}
	}
}