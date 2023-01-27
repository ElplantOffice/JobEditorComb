using Communication.Plc;
using System;

namespace Services.TRL
{
	public struct PlcLayerDataRaw : IMappable
	{
		public short Index;

		public short ProductLink;

		public short MeasuredProductId;

		public bool IsLast;

		public bool ReferenceEna;

		public bool CorrectionEna;

		public bool CorrectionUp;

		public bool CorrectionBySheet;

		public bool Absolute;

		public bool ReferenceDone;

		public bool CalculationDone;

		public bool CorrectionDone;

		public double Height;

		public double HeightAct;

		public double HeightActPrev;

		public double HeightTrgPrev;

		public double RefMeasurement;

		public double CorrMeasurement;

		public double EndMeasurement;

		public short CorrectedTodoProPile;

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
				return "T_LayerData";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcLayerDataRaw>(dataBlock, this.TypeName);
		}
	}
}