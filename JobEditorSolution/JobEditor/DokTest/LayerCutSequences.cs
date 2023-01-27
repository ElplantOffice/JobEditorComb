using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.DokTest
{
    public class LayerCutSequences
    {
        public int LayerId { get; set; }
        public int ShapesPerSequence { get; set; }
        public int ShapesPerProduct { get; set; }
        public IList<CutSequence> CutSequences { get; set; }
        public IList<CutSequence> OptimalCutSequences { get; set; }
        public CutSequencesToolsCombination OptimalCutSequencesToolsComb { get; set; }
        public IList<CutSequencesToolsCombination> CutSequencesToolsCombs { get; set; }
    }
}
