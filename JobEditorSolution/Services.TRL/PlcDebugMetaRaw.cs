using Communication.Plc;
using System;

namespace Services.TRL
{
	public struct PlcDebugMetaRaw : IMappable
	{
		public short Size;

		public short InPointer;

		public short OutPointer;

		public bool Undefined;

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
				return "T_DebugMetaData";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcDebugMetaRaw>(dataBlock, this.TypeName);
		}
	}
}