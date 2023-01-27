using Messages;
using Models;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;

namespace Services
{
	public class TemplateService : ServiceBase<TemplateService>, IServiceBaseDerived, IModel
	{
		public TemplateService(UIProtoType uiProtoType)
		{
			this.Init(uiProtoType, typeof(TemplateClient));
			base.RegisterType<PlcTemplateDataRaw>();
			base.RegisterClient("AddIn");
			base.RegisterClient("Hmi");
		}

		public override void OnClientConnect(Reciever client)
		{
		}

		public override void OnClientDisconnect(Reciever client)
		{
		}

		public override void OnNotifiedByClient(int command)
		{
			PlcTemplateDataRaw plcTemplateDataRaw = base.Read<PlcTemplateDataRaw>();
			base.ReadList<PlcTemplateDataRaw>();
			base.ReadClient<string>(base.CurrentClient);
			base.ReadCurrentClient<string>();
			base.WriteClient<string>(base.CurrentClient, 0, "test");
			base.WriteCurrentClient<string>(0, "test");
			base.WriteClients<string>(0, "test");
			base.NotifyClient(base.CurrentClient, 0);
			base.NotifyCurrentClient(0);
			base.NotifyClients(0);
			base.Write<PlcTemplateDataRaw>(plcTemplateDataRaw);
			base.WriteList<PlcTemplateDataRaw>(new List<PlcTemplateDataRaw>());
			base.NotifyPlc(0);
			this.RaiseOnNotify(this, new EventArgs());
			IEventAggregator eventAggregator = this.aggregator;
			Address address = base.CurrentClient.Address;
			PlcTemplateDataRaw plcTemplateDataRaw1 = new PlcTemplateDataRaw();
			eventAggregator.Publish<Telegram>(new Telegram(address, 0, (object)plcTemplateDataRaw1, null), true);
		}

		public override void OnNotifiedByPlc(int command)
		{
			base.Read<PlcTemplateDataRaw>();
			this.RaiseOnNotify(this, new EventArgs());
		}
	}
}