using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using static JobEditor.DokTest.MachineTools;
using static JobEditor.DokTest.Job;
using System.Runtime.InteropServices.WindowsRuntime;

namespace JobEditor.DokTest
{
    public static class FeatureToolDependencies
    {
        private static readonly int DoubleCutFactor = 3;

        public static readonly Tuple<Feature, ToolType[][]>[] Dependencies =
        {
            new Tuple<Feature, ToolType[][]>(Feature.CUT_45, new[] { new[] { ToolType.CUT_45 }, new[] { ToolType.CUT_45_OR_CUT_90 }, new[] { ToolType.S_SHEAR } }),
            new Tuple<Feature, ToolType[][]>(Feature.CUT_90, new[] { new[] { ToolType.CUT_90 }, new[] { ToolType.CUT_45_OR_CUT_90 }, new[] { ToolType.CUT_135_OR_CUT_90 }, 
                new[] { ToolType.S_SHEAR } }),
            new Tuple<Feature, ToolType[][]>(Feature.CUT_135, new[] { new[] { ToolType.CUT_135 }, new[] { ToolType.S_SHEAR } }),
            new Tuple<Feature, ToolType[][]>(Feature.V_TOP, new[] { new[] { ToolType.V_TOP } }),
            new Tuple<Feature, ToolType[][]>(Feature.V_BOTTOM, new[] { new[] { ToolType.V_BOTTOM } }),
            new Tuple<Feature, ToolType[][]>(Feature.H_OBLONG, new[] { new[] { ToolType.P_OBLONG } }),
            new Tuple<Feature, ToolType[][]>(Feature.H_ROUND, new[] { new[] { ToolType.P_ROUND } }),
            new Tuple<Feature, ToolType[][]>(Feature.CUT_45_TC_LEFT, new[] { new[] { ToolType.CUT_45, ToolType.TC_LEFT } }),
            new Tuple<Feature, ToolType[][]>(Feature.C_LEFT, new[] { new[] { ToolType.CUT_45, ToolType.CUT_135 }, new[] { ToolType.CUT_45_OR_CUT_90, ToolType.CUT_135 }, new[] { ToolType.X_SHEAR } }),
            new Tuple<Feature, ToolType[][]>(Feature.C_RIGHT, new[] { new[] { ToolType.V_TOP, ToolType.CUT_45}, new[] { ToolType.V_TOP, ToolType.CUT_45_OR_CUT_90}, new[] { ToolType.X_SHEAR } })
        };

        public static double CalculateShapePartXPosition(ShapePart shapePart, ToolType toolType)
        {
            if(shapePart.Feature == Feature.C_LEFT && toolType == ToolType.CUT_135)
                return shapePart.X + 2 * shapePart.Y;
            else if(shapePart.Feature == Feature.C_RIGHT && toolType == ToolType.V_TOP)
                return shapePart.X + shapePart.Y + shapePart.OverCut;
            return shapePart.X;
        }
        public static double CalculateShapePartYPosition(ShapePart shapePart, ToolType toolType)
        {
            if (shapePart.Feature == Feature.C_RIGHT && toolType == ToolType.V_TOP)
                return shapePart.Y - shapePart.OverCut;
            return shapePart.Y;
        }
        public static double CalculateShapePartDoubleCutXPosition(ShapePart shapePart, ToolType toolType)
        {
            if(shapePart.DoubleCut && shapePart.Feature == Feature.C_RIGHT && (toolType == ToolType.CUT_45 || toolType == ToolType.CUT_45_OR_CUT_90))
                return DoubleCutFactor * shapePart.OverCut;
            return 0;
        }
    }
}
