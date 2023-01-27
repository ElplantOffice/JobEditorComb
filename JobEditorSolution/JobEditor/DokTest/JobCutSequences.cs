using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.DokTest
{
    public class JobCutSequences
    {
        public String Name { get; set; }
        public bool Valid { get; set; }
        public IList<LayerCutSequences> LayerCutSequences { get; set; }
    }
}
