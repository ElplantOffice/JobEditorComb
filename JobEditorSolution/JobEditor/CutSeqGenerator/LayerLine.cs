using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.CutSeqGenerator
{
    class LayerLine : ICloneable
    {
        public int NToolName            { get; set; } 
        public int S1_90                { get; set; } //
        public int ActualPiece          { get; set; } 
        public int ActualStep           { get; set; } //
        public bool PieceFinish         { get; set; }
        public bool CLegScrap           { get; set; }
        public double XDistance         { get; set; } //
        public double XPosition         { get; set; } //
        public double XFeed             { get; set; }
        public double XFeedFirstPiece   { get; set; }
        public double YPos              { get; set; }
        public double YNextPos          { get; set; }
        public int GroupNumber          { get; set; } //
        public bool Remove              { get; set; } //
        public int ShapePartNumber      { get; set; } //
        public bool RemoveProtected     { get; set; }
        public bool IsDoubleCut         { get; set; }

        public object Clone()
        {
            LayerLine LayerLineClone = new LayerLine();

            LayerLineClone.NToolName = this.NToolName;
            LayerLineClone.S1_90 = this.S1_90;
            LayerLineClone.ActualPiece = this.ActualPiece;
            LayerLineClone.ActualStep = this.ActualStep;
            LayerLineClone.PieceFinish = this.PieceFinish;
            LayerLineClone.CLegScrap = this.CLegScrap;
            LayerLineClone.XDistance = this.XDistance;
            LayerLineClone.XPosition = this.XPosition;
            LayerLineClone.XFeed = this.XFeed;
            LayerLineClone.XFeedFirstPiece = this.XFeedFirstPiece;
            LayerLineClone.YPos = this.YPos;
            LayerLineClone.YNextPos = this.YNextPos;
            LayerLineClone.GroupNumber = this.GroupNumber;
            LayerLineClone.Remove = this.Remove;
            LayerLineClone.ShapePartNumber = this.ShapePartNumber;
            LayerLineClone.RemoveProtected = this.RemoveProtected;
            LayerLineClone.IsDoubleCut = this.IsDoubleCut;

            return LayerLineClone;
        }
    }
}
