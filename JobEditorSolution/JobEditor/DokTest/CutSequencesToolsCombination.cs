using System;
using System.Collections.Generic;
using System.Linq;

namespace JobEditor.DokTest
{
    public class CutSequencesToolsCombination : ICloneable
    {
        public List<List<MachineTools.Tool>> ToolsComb { get; set; }
        public IList<CutSequence> CutSequences { get; set; }

        public object Clone()
        {
            return new CutSequencesToolsCombination()
            {
                ToolsComb = ToolsComb.Select(e => e.Select(e1 => e1).ToList()).ToList(),
                CutSequences = CutSequences.Select(cs => (CutSequence)cs.Clone()).ToList()
            };
        }
    }
}
