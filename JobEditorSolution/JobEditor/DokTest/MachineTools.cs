using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.DokTest
{
    public class MachineTools
    {
        public enum ToolType
        {
            [CutsOff(true)]
            CUT_45,
            [CutsOff(false)]
            TC_LEFT,
            [CutsOff(true)]
            CUT_90,
            [CutsOff(true)]
            CUT_45_OR_CUT_90,
            [CutsOff(true)]
            CUT_135_OR_CUT_90,
            [CutsOff(true)]
            CUT_135,
            [CutsOff(true)]
            S_SHEAR,
            [CutsOff(false)]
            V_TOP,
            [CutsOff(false)]
            V_BOTTOM,
            [CutsOff(true)]
            X_SHEAR,
            [CutsOff(false)]
            P_OBLONG,
            [CutsOff(false)]
            P_ROUND
        }
        public class Tool
        {
            public ToolType Type { get; set; }
            public String Name { get; set; }
            public bool IsMobile { get; set; }
            public double X { get; set; }
            public double XOffset { get; set; }
            public double Y { get; set; }
            public double YOffset { get; set; }
        }
        public IList<Tool> Tools { get; set; }
    }
}
