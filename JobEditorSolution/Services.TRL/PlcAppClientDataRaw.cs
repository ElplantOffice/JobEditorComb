using Communication.Plc;
using System;

namespace Services.TRL
{
	public struct PlcAppClientDataRaw : IMappable
	{
		public short LanguageId;

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
				return "T_AppClientData";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcAppClientDataRaw>(dataBlock, this.TypeName);
		}
	}
}