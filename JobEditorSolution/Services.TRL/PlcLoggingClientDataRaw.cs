using Communication.Plc;
using System;

namespace Services.TRL
{
	public struct PlcLoggingClientDataRaw : IMappable
	{
		public string Path;

		public bool LogToXml;

		public Func<byte[], object> Mapper
		{
			get
			{
				return new Func<byte[], object>(this.Map);
			}
		}

		public string TypeName
		{
			get
			{
				return "T_LoggingClientData";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcLoggingClientDataRaw>(dataBlock, this.TypeName);
		}
	}
}