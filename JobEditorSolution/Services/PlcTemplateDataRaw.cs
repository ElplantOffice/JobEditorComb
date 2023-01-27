using Communication.Plc;
using System;

namespace Services
{
	public struct PlcTemplateDataRaw : IMappable
	{
		public short IntegerValue;

		public short SomeOtherInt;

		public short YetAnotherInt;

		public bool BooleanValue;

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
				return "T_TemplateData";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcTemplateDataRaw>(dataBlock, this.TypeName);
		}
	}
}