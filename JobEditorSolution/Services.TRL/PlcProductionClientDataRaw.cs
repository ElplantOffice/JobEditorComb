using Communication.Plc;
using System;

namespace Services.TRL
{
	public struct PlcProductionClientDataRaw : IMappable
	{
		public short Index;

		public string FileName;

		public string ProcessPath;

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
				return "T_ProductionClientData";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcProductionClientDataRaw>(dataBlock, this.TypeName);
		}
	}
}