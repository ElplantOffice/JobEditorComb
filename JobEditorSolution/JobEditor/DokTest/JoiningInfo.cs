using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.DokTest
{
    public class JoiningInfo
    {
        public enum Joining
        {
            MOVING_CLOSER,
            MOVING_AWAY
        }
        public double MinDistance { get; set; }
        public double MaxDistance { get; set; }
        public Joining JoiningType { get; set; }
    }
}
