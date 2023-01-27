using System;
using System.Runtime.CompilerServices;

namespace Services.TRL
{
	public class PlcDebugStrokeData
	{
		public PlcDebugStrokeData.ToolName ToolId
		{
			get;
			set;
		}

		public double XPos
		{
			get;
			set;
		}

		public double Ypos
		{
			get;
			set;
		}

		public double Zpos
		{
			get;
			set;
		}

		public PlcDebugStrokeData()
		{
		}

		public enum ToolName
		{
			TB1,
			TB2,
			TF1,
			TF2,
			VB,
			VF,
			P1,
			P2,
			P3,
			P4,
			S1,
			S2,
			S1S
		}
	}
}