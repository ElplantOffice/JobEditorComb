using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Services.TRL
{
	public class DebugService : ServiceBase<DebugService>, IServiceBaseDerived, IModel
	{
		private List<PlcDebugDataRaw> DataStorage
		{
			get;
			set;
		}

		public DebugService(UIProtoType uiProtoType)
		{
			this.Init(uiProtoType, typeof(DebugClient));
			base.RegisterType<PlcDebugDataRaw>();
			base.RegisterType<PlcDebugMetaRaw>();
			this.DataStorage = new List<PlcDebugDataRaw>();
		}

		private List<PlcDebugDataRaw> ExtractNewData(ref PlcDebugMetaRaw metaData, List<PlcDebugDataRaw> data)
		{
			this.DataStorage.AddRange(data);
			return this.DataStorage;
		}

		public override void OnClientConnect(Reciever client)
		{
			base.WriteCurrentClient<List<PlcDebugDataRaw>>(51, this.DataStorage);
		}

		public override void OnClientDisconnect(Reciever client)
		{
		}

		public override void OnNotifiedByClient(int command)
		{
			switch (command)
			{
				case 45:
				case 46:
				{
					base.NotifyPlc(command);
					return;
				}
				case 47:
				{
					this.DataStorage.Clear();
					return;
				}
				default:
				{
					return;
				}
			}
		}

		public override void OnNotifiedByPlc(int command)
		{
			DebugService.Commands command1 = (DebugService.Commands)command;
			if (command1 != DebugService.Commands.Init && command1 == DebugService.Commands.Update)
			{
				PlcDebugMetaRaw plcDebugMetaRaw = base.Read<PlcDebugMetaRaw>();
				List<PlcDebugDataRaw> plcDebugDataRaws = base.ReadList<PlcDebugDataRaw>();
				base.WriteClients<List<PlcDebugDataRaw>>(51, this.ExtractNewData(ref plcDebugMetaRaw, plcDebugDataRaws));
				base.Write<PlcDebugMetaRaw>(plcDebugMetaRaw);
				base.NotifyPlc(command);
			}
		}

		public enum Commands
		{
			DebugEnable = 45,
			Step = 46,
			Flush = 47,
			Init = 50,
			Update = 51
		}
	}
}