using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Services.TRL
{
	public class DebugClient : ClientBase<DebugClient>, IServiceBaseDerived, IModel
	{
		private List<PlcDebugDataRaw> DataStorage
		{
			get;
			set;
		}

		public Action<DebugService.Commands> OnPlcStateChanged
		{
			get;
			set;
		}

		public DebugClient(UIProtoType uiProtoType)
		{
			this.Init(uiProtoType, typeof(DebugService));
			this.DataStorage = new List<PlcDebugDataRaw>();
		}

		private void AddStrokeDataToBuffer(List<PlcDebugDataRaw> data)
		{
		}

		private void AddStrokeDataToBuffer(double[] newstroke)
		{
			PlcDebugStrokeData plcDebugStrokeDatum = new PlcDebugStrokeData()
			{
				XPos = newstroke[0],
				Ypos = newstroke[1],
				Zpos = newstroke[2],
				ToolId = (PlcDebugStrokeData.ToolName)((int)newstroke[3])
			};
		}

		public void ClearDataBuffer()
		{
			this.DataStorage.Clear();
		}

		public override void OnNotifiedByServer(int command)
		{
			switch (command)
			{
				case 45:
				case 46:
				case 48:
				case 49:
				case 50:
				{
					return;
				}
				case 47:
				{
					this.DataStorage.Clear();
					return;
				}
				case 51:
				{
					this.AddStrokeDataToBuffer(base.ReadServer<List<PlcDebugDataRaw>>());
					return;
				}
				default:
				{
					return;
				}
			}
		}

		public override void OnServerConnect(Reciever server)
		{
		}

		public override void OnServerDisconnect(Reciever server)
		{
		}
	}
}