using JobEditor.DokTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.DokTest
{
    public class CutSequence : ICloneable
    {
        public class ToolSequence : ICloneable
        {
            public MachineTools.Tool Tool { get; set; }
            public Job.Feature Feature { get; set; }
            public int ShapeNo { get; set; }
            public double XPos { get; set; }
            public double NextXPos { get; set; }
            public double YPos { get; set; }
            public double NextYPos { get; set; }

            public object Clone()
            {
                return new ToolSequence()
                {
                    Tool = Tool,
                    Feature = Feature,
                    ShapeNo = ShapeNo,
                    XPos = XPos,
                    YPos = YPos,
                    NextXPos = NextXPos,
                    NextYPos = NextYPos
                };
            }
        }
        public IList<ToolSequence> ToolSequences { get; set; }
        public double XPosition { get; set; }
        public double XFeed { get; set; }
        public double XFeedFirstPosition { get; set; }

        public object Clone()
        {
            return new CutSequence()
            {
                ToolSequences = ToolSequences.Select(ts => (ToolSequence)ts.Clone()).ToList(),
                XFeed = XFeed,
                XFeedFirstPosition = XFeedFirstPosition,
                XPosition = XPosition   
            };
        }
    }
}
