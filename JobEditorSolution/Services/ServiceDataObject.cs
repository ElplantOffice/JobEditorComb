using Communication.Plc.Shared;
using System;
using System.Runtime.CompilerServices;

namespace Services
{
	public class ServiceDataObject
	{
		public PlcTelegramRaw telegram;

		public PlcEventSourceRaw Source;

		public PlcBaseRefPntrRaw DataPntr;

		public object Data
		{
			get;
			set;
		}

		public ulong DataPntrHandle
		{
			get;
			set;
		}

		public ServiceDataObject()
		{
		}
	}
}