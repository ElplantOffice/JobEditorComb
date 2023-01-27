using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.CutSeqGenerator
{
    public class ToolPar
    {
        public double P1_RefPos                 { get; set; }
        public double P2_RefPos                 { get; set; }
        public double VB_RefPos                 { get; set; }
        public double VF_RefPos                 { get; set; }
        public double VF_YPosOffset             { get; set; }
        public double S1_RefPos                 { get; set; }
        public double S2_RefPos                 { get; set; }
        public double S3_RefPos                 { get; set; }

        public double P1_RefPosCorrection       { get; set; }
        public double P2_RefPosCorrection       { get; set; }
        public double VB_RefPosCorrection       { get; set; }
        public double VF_RefPosCorrection       { get; set; }
        public double S1_RefPosCorrection       { get; set; }
        public double S2_RefPosCorrection       { get; set; }
        public double S3_RefPosCorrection       { get; set; }

        public float MinMaterialWidth          { get; set; }
        public float MaxMaterialWidth          { get; set; }

        public double PYGuidesMinClereance      { get; set; }
        public double PYGuidesMaxClereance      { get; set; }

        public double MaxVYPos                  { get; set; }
        public double MinVYPos                  { get; set; }
        //public double MaxP1YPos                  { get; set; }
        //public double MinP1YPos                  { get; set; }
        public double MaxGuidesPos              { get; set; }
        public double MinGuidesPos              { get; set; }

        public double DoubleCutFactor           { get; set; }


    }
}
