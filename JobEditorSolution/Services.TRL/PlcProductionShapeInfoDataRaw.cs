using Communication.Plc;
using System;

namespace Services.TRL
{
	public struct PlcProductionShapeInfoDataRaw : IMappable
	{
		public bool Highlight;

		public short StackNr;

		public short ProductsDone;

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
				return "T_ShapeInfoData";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcProductionShapeInfoDataRaw>(dataBlock, this.TypeName);
		}
	}
}