using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.CutSeqGenerator
{
    public static class Constants
    {
        public static readonly int Multiplier;
        public static readonly double MaxWidth;
        public static readonly double MaxLength;
        public static readonly int MaxSteplapNumber;
        public static readonly int MaxNumberOfSame;
        public static readonly int MaxNumberOfLines;

        //public static readonly bool PLC_Active;

        static Constants()
        {
            Multiplier = 10;
            MaxWidth = 440F;
            MaxLength = 1260F;
            MaxSteplapNumber = 10;
            MaxNumberOfSame = 10;
            MaxNumberOfLines = 550;
            //PLC_Active = false;
        }
    }
}
