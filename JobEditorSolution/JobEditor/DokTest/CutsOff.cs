using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.DokTest
{
    public class CutsOff : Attribute
    {
        public bool Value { get; set; }

        public CutsOff(bool value)
        {
            Value = value;
        }
    }
}
