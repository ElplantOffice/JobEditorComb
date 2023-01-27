using Communication.Plc;
using System;

namespace Services.TRL
{
	public struct PlcProductionDataRaw : IMappable
	{
		public bool Selected;

		public bool IsReload;

		public short CurrentState;

		public short Index;

		public short FileIndex;

		public short Layer;

		public short NextOnLayer;

		public short PrevOnLayer;

		public bool DoneLocked;

		public bool TodoLocked;

		public bool ReferenceDone;

		public bool CalculationDone;

		public bool CorrectionDone;

		public double RefMeasurement;

		public double CorrMeasurement;

		public double EndMeasurement;

		public short CorrectedTodoProPile;

		public short MeasureAtSheet;

		public ushort Done;

		public ushort ToDo;

		public double Width;

		public double Thickness;

		public double Height;

		public double HeightAct;

		public double HeightActPrev;

		public string LayerName;

		public string ProductName;

		public string ProductFile;

		public string Info;

		public short ShapeCount;

		public PlcProductionShapeInfoDataRaw[] ShapeInfo;

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
				return "T_ProductionData";
			}
		}

		private object Map(byte[] dataBlock)
		{
			return PlcMapper.Map<PlcProductionDataRaw>(dataBlock, this.TypeName);
		}
	}
}