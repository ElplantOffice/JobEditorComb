using Communication.Plc;
using System;

namespace Services.TRL
{
	public struct PlcProcessClientDataRaw : IMappable
	{
		public string Path;

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
				return "T_ProcessClientData";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcProcessClientDataRaw>(dataBlock, this.TypeName);
		}
	}
}