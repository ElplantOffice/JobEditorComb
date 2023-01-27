using Communication.Plc;
using System;

namespace Services.TRL
{
	public struct PlcMachineConfigurationDataRaw : IMappable
	{
		public bool HeightCorrectionEnable;

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
				return "T_ProductionMachineConfig";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcMachineConfigurationDataRaw>(dataBlock, this.TypeName);
		}
	}
}