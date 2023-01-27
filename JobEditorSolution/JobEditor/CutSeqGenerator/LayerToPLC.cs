using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEditor.CutSeqGenerator
{
    class LayerToPLC
    {
        public int LayerID                  { get; set; }   // Layer ID
        public string LayerName             { get; set; }   // Layer Name
        public int NumberOfLines            { get; set; }   // Number Of Lines
        public int NumberOfStepLaps         { get; set; }   // Number Of Step Laps
        public int NumberOfSame             { get; set; }   // Number Of Same
        public int PiecesToDo               { get; set; }   // Pieces To Do
        public int PiecesOffset             { get; set; }   // Pieces Offset - number of different sheets cuting in time
        public int NumberOfSequenceInCycle  { get; set; }   // Number Of Sequence In Cycle
        public int NumberOfSheetsInSequence { get; set; }   // Number Of Sheets In Sequence
        public double CoilWidth             { get; set; }   // Coil actual width
        public double CoilThick             { get; set; }   // Coil actual thicknees
        public double MaxVYPos              { get; set; }   // Max VY position
        public double MinVYPos              { get; set; }   // Min VY position
        public double FirstVYPos            { get; set; }   // First VY position
        public double P1YPos                { get; set; }   // P1 fix position
        public double P2YPos                { get; set; }   // P2 fix position
        public bool S1_90                   { get; set; }   // true if S1 is in 90 degree, false if S1 is in 45 degree
        public bool UseVB                   { get; set; }   // Use VB tool
        public bool UseVF                   { get; set; }   // Use VF tool
        public bool UseP1                   { get; set; }   // Use P1 tool
        public bool UseP2                   { get; set; }   // Use P2 tool
        public bool UseS1                   { get; set; }   // Use S1 tool
        public bool UseS2                   { get; set; }   // Use S2 tool
        public bool UseS3                   { get; set; }   // Use S3 tool
        public int AddNumber                { get; set; }   // Number to add piece in each new group created in PLC


        // Only for TRL Africa
        public bool TreadingPinActive1                  { get; set; }
        public double PinHoleDistanceFromEndOfPiece1    { get; set; }
        public double PinHoleStepLapOffset1             { get; set; }
        public bool TreadingPinActive2                  { get; set; }
        public double PinHoleDistanceFromEndOfPiece2    { get; set; }
        public double PinHoleStepLapOffset2             { get; set; }
        public double PieceLength1                      { get; set; }
        public double PieceLength2                      { get; set; }



        public List<LayerLine> LayerLines = new List<LayerLine>();   // List of layer lines, max list count 550 
    }
}
