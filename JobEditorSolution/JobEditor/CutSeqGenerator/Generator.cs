using JobEditor.Properties;
using ProductLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using TwinCAT.Ads;

namespace JobEditor.CutSeqGenerator
{
    class Generator
    {
        Product p = new Product();
        ToolPar tp = new ToolPar();
        int LayersNumber;
        string ProductName = "";

        private TcAdsClient tcClient;

        public void generate(String filePathName)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            XmlSerializer serializer = new XmlSerializer(typeof(Product));
            MemoryStream stream = new MemoryStream(encoding.GetBytes(File.ReadAllText(filePathName)));
            this.p = (Product)serializer.Deserialize(stream);

            stream.Close();

            string[] splited = filePathName.Split('\\');
            ProductName = splited.Last();
            LayersNumber = p.Layers.Count;

            #region Change StapLapOrder
            // Done for JayBee

            // Change order of step laps in case ^ steps and HeightCorrType="PreciseDown"

            //if (p.Layers[0].StepLapsDefault.NumberOfSteps == 6 && (p.Layers[0].LayerDefault.HeightCorrType == EHeightCorrectionType.PreciseDown) && 
            //    (p.Layers[0].StepLapsDefault.NumberOfSame == 1 || p.Layers[0].StepLapsDefault.NumberOfSame == 2))
            //{
            //    foreach (Layer l in p.Layers)
            //    {
            //        if (l.StepLapsDefault.NumberOfSteps == 6 && (l.LayerDefault.HeightCorrType == EHeightCorrectionType.PreciseDown) && (l.StepLapsDefault.NumberOfSame == 1 || l.StepLapsDefault.NumberOfSame == 2))
            //        {
            //            foreach (Shape shape in l.Shapes)
            //            {
            //                foreach (ShapePart sp in shape.ShapeParts)
            //                {
            //                    Step tempStep = new Step();

            //                    if (p.Layers[0].StepLapsDefault.NumberOfSame == 1)
            //                    {
            //                        tempStep = (Step)sp.StepLap.Steps[3].Clone();
            //                        sp.StepLap.Steps[3] = (Step)sp.StepLap.Steps[5].Clone();
            //                        sp.StepLap.Steps[5] = (Step)tempStep.Clone();
            //                        tempStep = null;
            //                    }
            //                    if (p.Layers[0].StepLapsDefault.NumberOfSame == 2)
            //                    {
            //                        tempStep = (Step)sp.StepLap.Steps[6].Clone();
            //                        sp.StepLap.Steps[6] = (Step)sp.StepLap.Steps[10].Clone();
            //                        sp.StepLap.Steps[10] = (Step)tempStep.Clone();
            //                        tempStep = null;

            //                        tempStep = (Step)sp.StepLap.Steps[7].Clone();
            //                        sp.StepLap.Steps[7] = (Step)sp.StepLap.Steps[11].Clone();
            //                        sp.StepLap.Steps[11] = (Step)tempStep.Clone();
            //                        tempStep = null;
            //                    }

            //                }
            //            }
            //        }
            //    }
            //}

            #endregion

            #region Read data from PLC
            if (Settings.Default.PLC_Active)
            {
                try
                {
                    Connect();

                    tp.VB_RefPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[1].lXRefDist"), typeof(Double));
                    tp.VF_RefPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[2].lXRefDist"), typeof(Double));
                    tp.P1_RefPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[3].lXRefDist"), typeof(Double));
                    tp.P2_RefPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[4].lXRefDist"), typeof(Double));
                    tp.S1_RefPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[5].lXRefDist"), typeof(Double));
                    tp.S2_RefPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[6].lXRefDist"), typeof(Double));
                    tp.S3_RefPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[7].lXRefDist"), typeof(Double));

                    tp.VB_RefPosCorrection = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[1].lXRefDistCorrection"), typeof(Double));
                    tp.VF_RefPosCorrection = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[2].lXRefDistCorrection"), typeof(Double));
                    tp.P1_RefPosCorrection = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[3].lXRefDistCorrection"), typeof(Double));
                    tp.P2_RefPosCorrection = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[4].lXRefDistCorrection"), typeof(Double));
                    tp.S1_RefPosCorrection = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[5].lXRefDistCorrection"), typeof(Double));
                    tp.S2_RefPosCorrection = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[6].lXRefDistCorrection"), typeof(Double));
                    tp.S3_RefPosCorrection = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[7].lXRefDistCorrection"), typeof(Double));

                    tp.MinMaterialWidth = (Single)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.tGeneralPar.lWidthMin"), typeof(Single));
                    tp.MaxMaterialWidth = (Single)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.tGeneralPar.lWidthMax"), typeof(Single));

                    tp.VF_YPosOffset = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[2].lrYPosOffset"), typeof(Double));

                    //tp.PYGuidesMaxClereance = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[1].lrGuidesClearanceMax"), typeof(Double));
                    //tp.PYGuidesMinClereance = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arToolPar[1].lrGuidesClearanceMin"), typeof(Double));

                    tp.MaxVYPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arAxisPar[6].lrPosMax"), typeof(Double));
                    tp.MinVYPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arAxisPar[6].lrPosMin"), typeof(Double));
                    //tp.MaxPYPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arAxisPar[6].lrPosMax"), typeof(Double));
                    //tp.MinPYPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arAxisPar[6].lrPosMin"), typeof(Double));
                    //tp.MaxGuidesPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arAxisPar[11].lrPosMax"), typeof(Double));
                    //tp.MinGuidesPos = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.arAxisPar[11].lrPosMin"), typeof(Double));
                    tp.DoubleCutFactor = (Double)tcClient.ReadAny(tcClient.CreateVariableHandle("App_Variables.g_tMachinePar.tGeneralPar.lrDoubleCutFactor"), typeof(Double));

                    Disconnect();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Cannot read PLC variables!\nException: " + ex.ToString());
                    return;
                }

            }
            else
            {
                tp.MaxMaterialWidth = 1000;
                tp.MinMaterialWidth = 50;

                tp.VB_RefPosCorrection = 0;
                tp.VF_RefPosCorrection = 0;
                tp.P1_RefPosCorrection = 0;
                tp.P2_RefPosCorrection = 0;
                tp.S1_RefPosCorrection = 0;
                tp.S2_RefPosCorrection = 0;
                tp.S3_RefPosCorrection = 0;


                tp.VB_RefPos = 300;
                tp.VF_RefPos = 310;
                tp.VF_YPosOffset = 50;
                tp.P1_RefPos = 500;
                tp.P2_RefPos = 510;
                tp.S1_RefPos = 1000;
                tp.S2_RefPos = 1500;
                tp.S3_RefPos = 2000;

                //tp.PYGuidesMaxClereance = 35.9;
                //tp.PYGuidesMinClereance = 225;

                tp.MaxVYPos = 230;
                tp.MinVYPos = 25;
                //tp.MaxPYPos = 245;
                //tp.MinPYPos = 60;
                //tp.MaxGuidesPos = 400;
                //tp.MinGuidesPos = 90;
                tp.DoubleCutFactor = 6;
            }

            #endregion

            foreach (Layer basicLayer in p.Layers)
            {
                LayerToPLC l = new LayerToPLC();

                l.LayerID = basicLayer.Id + 1;

                l.NumberOfStepLaps = p.Layers[basicLayer.Id].LayerDefault.NumberOfSteps;
                l.NumberOfSame = p.Layers[basicLayer.Id].LayerDefault.NumberOfSame;
                l.CoilWidth = p.Layers[basicLayer.Id].LayerDefault.Width;
                l.CoilThick = p.Layers[basicLayer.Id].LayerDefault.MaterialThickness;
                string[] sheets = p.Layers[basicLayer.Id].Name.Split('-');
                l.NumberOfSheetsInSequence = sheets.Count();
                l.NumberOfSequenceInCycle = l.NumberOfStepLaps * l.NumberOfSame;
                l.AddNumber = l.NumberOfSequenceInCycle * l.NumberOfSheetsInSequence;
                l.LayerName = p.Layers[basicLayer.Id].Name;

                if (p.HeightRefType == EHeightRefType.Number)
                    l.PiecesToDo = Int32.Parse(p.Layers[basicLayer.Id].LayerDefault.Height.ToString());
                else
                    l.PiecesToDo = (Int32)(p.Layers[basicLayer.Id].LayerDefault.Height / l.CoilThick);

                LayerLine emptyLine = new LayerLine();  // Instance of empty line
                LayerLine line = new LayerLine();       // Instance of line
                List<LayerLine> Lines = new List<LayerLine>(); // List of Layer lines
                List<LayerLine> SimpleLines = new List<LayerLine>(); // First creation of lines
                List<LayerLine> StepLapLines = new List<LayerLine>(); // First creation of step laps
                List<LayerLine> JoiningLines = new List<LayerLine>(); // Joining lengths line
                List<LayerLine> PositioningLines = new List<LayerLine>(); // Positioning Lines
                List<LayerLine> PreLines = new List<LayerLine>(); // Lines before norming Actual Piece numbers
                List<LayerLine> VLines = new List<LayerLine>(); // Lines consist of anly V tool (tool number 2)

                double accumulatedLengthShapes = 0; // in case there are more pieces in sequence
                double lastShapePartLength = 0;
                double lastShapeLength = 0;

                #region PinData 
                // Only for TRL Africa
                try
                {
                    if (basicLayer.Shapes.Count > 0 && basicLayer.Shapes[0].ShapeParts.Last() != null)
                    {
                        l.PieceLength1 = basicLayer.Shapes[0].ShapeParts.Last().X;
                        l.PinHoleStepLapOffset1 = Math.Abs(basicLayer.Shapes[0].ShapeParts.Last().StepLap.Steps.Last().X) * 2;
                        if (basicLayer.Shapes[0].Holes.Count > 0 && basicLayer.Shapes[0].Holes.Last() != null)
                        {
                            l.PinHoleDistanceFromEndOfPiece1 = l.PieceLength1 - basicLayer.Shapes[0].Holes.Last().X;
                            l.TreadingPinActive1 = true;
                        }

                    }

                    if (basicLayer.Shapes.Count > 1 && basicLayer.Shapes[1].ShapeParts.Last() != null)
                    {
                        l.PieceLength2 = basicLayer.Shapes[1].ShapeParts.Last().X;
                        l.PinHoleStepLapOffset2 = Math.Abs(basicLayer.Shapes[1].ShapeParts.Last().StepLap.Steps.Last().X) * 2;
                        if (basicLayer.Shapes[1].Holes.Count > 0 && basicLayer.Shapes[1].Holes.Last() != null)
                        {
                            l.PinHoleDistanceFromEndOfPiece2 = l.PieceLength2 - basicLayer.Shapes[1].Holes.Last().X;
                            l.TreadingPinActive2 = true;
                        }

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Pin data cannot be send" + e);
                }
               


                #endregion


                #region Create SimpleLines
                // Simple lines is definition of all sequence without step lap
                for (int i = 0; i < p.Layers[basicLayer.Id].Shapes.Count; i++)
                {
                    if (i > 0)
                        lastShapeLength = SimpleLines.Last().XDistance;

                    for (int j = 0; j < p.Layers[basicLayer.Id].Shapes[i].ShapeParts.Count; j++)
                    {
                        switch (p.Layers[basicLayer.Id].Name)
                        {
                            case "001":
                            case "003":
                            case "002-002":
                            case "004-004":
                            case "2004-004":
                            case "2004-2004":
                            case "2004.2-004":
                            case "2004.3-004":

                                line = (LayerLine)emptyLine.Clone();

                                line.NToolName = GetToolName(p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature).ToolName;
                                line.S1_90 = GetToolName(p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature).S1_90;
                                line.XDistance = accumulatedLengthShapes + p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].X;

                                if (p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature == EFeature.VTop) 
                                    line.YPos = p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Y;

                                if (p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature == EFeature.VBottom)
                                    line.YPos = p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Y + tp.VF_YPosOffset;  // Added offset for VBottom or VFront

                                line.ActualPiece = i + 1;

                                if (p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature == EFeature.Cut90 || p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature == EFeature.Cut45 || p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature == EFeature.Cut135)
                                {
                                    line.PieceFinish = true;
                                    if (j == 0)
                                        line.Remove = true; // First S1 tool for removing
                                }

                                line.ShapePartNumber = j + 1;

                                SimpleLines.Add(line);

                                break;

                            case "006":
                            case "007":

                                if (p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature == EFeature.CLeft)
                                {
                                    // Add S1 - to remove
                                    line = (LayerLine)emptyLine.Clone();
                                    line.NToolName = 5;
                                    line.XDistance = p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].X;
                                    line.PieceFinish = true;
                                    line.ActualPiece = (i + 1);
                                    line.ShapePartNumber = j + 1;
                                    line.Remove = true;
                                    SimpleLines.Add(line);


                                    // Add S2
                                    line = (LayerLine)emptyLine.Clone();
                                    line.NToolName = 6;
                                    line.XDistance = p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].X + 2 * p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Y;
                                    line.CLegScrap = true;
                                    line.ActualPiece = (i + 1);
                                    line.ShapePartNumber = j + 1;
                                    SimpleLines.Add(line);
                                }

                                if (p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature == EFeature.CRight)
                                {
                                    // Add VBack(PLC) or VTop(Editor)
                                    line = (LayerLine)emptyLine.Clone();
                                    line.NToolName = 1;
                                    line.XDistance = p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].X + p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Y
                                        + basicLayer.CentersDefault.OverCut;
                                    //line.YPos = l.CoilWidth / 2 + p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Y - p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].OverCut;
                                    line.YPos = p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Y - p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].OverCut;
                                    line.ActualPiece = (i + 1);
                                    line.ShapePartNumber = j + 1;
                                    SimpleLines.Add(line);


                                    // Add S1
                                    line = (LayerLine)emptyLine.Clone();
                                    line.NToolName = 5;
                                    line.XDistance = p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].X;
                                    //if (!(p.CentersDefault.DoubleCut))
                                    //line.PieceFinish = true;
                                    line.PieceFinish = true;

                                    line.ActualPiece = (i + 1);
                                    line.ShapePartNumber = j + 1;
                                    SimpleLines.Add(line);


                                    // Add S1 if double cut is active
                                    if (basicLayer.CentersDefault.DoubleCut)
                                    {
                                        line = (LayerLine)emptyLine.Clone();
                                        line.NToolName = 5;
                                        line.IsDoubleCut = true;
                                        line.XDistance = accumulatedLengthShapes + p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].X + tp.DoubleCutFactor * p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].OverCut;
                                        //line.PieceFinish = true;
                                        line.ActualPiece = (i + 1);
                                        line.ShapePartNumber = j + 1;
                                        SimpleLines.Add(line);
                                    }
                                }

                                if (p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].Feature == EFeature.Cut45)
                                {
                                    // Add S1
                                    line = (LayerLine)emptyLine.Clone();
                                    line.NToolName = 5;
                                    line.XDistance = p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].X;
                                    line.PieceFinish = true;
                                    line.ActualPiece = (i + 1);
                                    line.ShapePartNumber = j + 1;
                                    SimpleLines.Add(line);
                                }


                                break;


                            default:
                                break;
                        }

                        lastShapePartLength = p.Layers[basicLayer.Id].Shapes[i].ShapeParts[j].X;
                    }

                    foreach (Hole h in p.Layers[basicLayer.Id].Shapes[i].Holes)
                    {
                        line = (LayerLine)emptyLine.Clone();

                        if (h.Shape == EHoleShape.Round)
                            line.NToolName = 3;  // P1
                        else
                            line.NToolName = 4;  // P2


                        line.XDistance = accumulatedLengthShapes * i + h.X;
                        line.YPos = h.Y;
                        line.ActualPiece = i + 1;

                        SimpleLines.Add(line);
                    }

                    accumulatedLengthShapes += lastShapePartLength;
                }

                SimpleLines = SimpleLines.OrderBy(o => o.XDistance).ToList();

                #endregion

                #region StepLapLines
                switch (p.Layers[basicLayer.Id].Name)
                {
                    case "001":
                    case "003":
                    case "002-002":
                    case "004-004":
                    case "2004-004":
                    case "2004-2004":
                    case "2004.2-004":
                    case "2004.3-004":

                        if (p.Layers[basicLayer.Id].Name == "001" || p.Layers[basicLayer.Id].Name == "002-002")
                            l.S1_90 = true;

                        for (int i = 0; i < l.NumberOfStepLaps * l.NumberOfSame; i++)
                        {
                            foreach (LayerLine ll in SimpleLines)
                            {
                                line = (LayerLine)emptyLine.Clone();
                                line = (LayerLine)ll.Clone();
                                line.ActualStep = i + 1;

                                if (ll.NToolName != 3 && ll.NToolName != 4) //Not Punches
                                {
                                    line.XDistance += p.Layers[basicLayer.Id].Shapes[line.ActualPiece - 1].ShapeParts[line.ShapePartNumber - 1].StepLap.Steps[line.ActualStep - 1].X;
                                    line.YPos += p.Layers[basicLayer.Id].Shapes[line.ActualPiece - 1].ShapeParts[line.ShapePartNumber - 1].StepLap.Steps[line.ActualStep - 1].Y;
                                }

                                StepLapLines.Add(line);
                            }
                        }
                        break;


                    case "006":
                    case "007":

                        for (int i = 0; i < l.NumberOfStepLaps * l.NumberOfSame; i++)
                        {
                            foreach (LayerLine ll in SimpleLines)
                            {
                                line = (LayerLine)emptyLine.Clone();
                                line = (LayerLine)ll.Clone();
                                line.ActualStep = i + 1;

                                if (ll.NToolName != 3 && ll.NToolName != 4)
                                {
                                    if (ll.NToolName == 1) // VBack
                                    {
                                        line.XDistance += p.Layers[basicLayer.Id].Shapes[line.ActualPiece - 1].ShapeParts[line.ShapePartNumber - 1].StepLap.Steps[line.ActualStep - 1].X;
                                        line.YPos += -p.Layers[basicLayer.Id].Shapes[line.ActualPiece - 1].ShapeParts[line.ShapePartNumber - 1].StepLap.Steps[line.ActualStep - 1].Y;
                                    }

                                    if (ll.NToolName == 5) // S1
                                        line.XDistance += p.Layers[basicLayer.Id].Shapes[line.ActualPiece - 1].ShapeParts[line.ShapePartNumber - 1].StepLap.Steps[line.ActualStep - 1].X + p.Layers[basicLayer.Id].Shapes[line.ActualPiece - 1].ShapeParts[line.ShapePartNumber - 1].StepLap.Steps[line.ActualStep - 1].Y;

                                    if (ll.NToolName == 6) // S2
                                        line.XDistance += p.Layers[basicLayer.Id].Shapes[line.ActualPiece - 1].ShapeParts[line.ShapePartNumber - 1].StepLap.Steps[line.ActualStep - 1].X - p.Layers[basicLayer.Id].Shapes[line.ActualPiece - 1].ShapeParts[line.ShapePartNumber - 1].StepLap.Steps[line.ActualStep - 1].Y;

                                }

                                StepLapLines.Add(line);
                            }
                        }

                        break;

                    default:
                        break;
                }

                // Calculate First feed position
                double Difference = 0;
                foreach (LayerLine ll in StepLapLines)
                {
                    if ((ll.Remove && ll.NToolName == 5) || (ll.Remove && ll.NToolName == 7))
                        Difference = ll.XDistance;
                    if (ll.NToolName != 5 || ll.NToolName != 7)
                        ll.XFeedFirstPiece = (ll.XDistance - Difference) + GetToolRefPos(ll.NToolName, l.S1_90, l.CoilWidth);
                    else
                        ll.XFeedFirstPiece = (-Difference) + GetToolRefPos(ll.NToolName, l.S1_90, l.CoilWidth);

                }

                for (int i = 0; i < StepLapLines.Count; i++)
                {
                    if ((StepLapLines[i].NToolName == 5 || StepLapLines[i].NToolName == 7) && StepLapLines[i].PieceFinish && StepLapLines[i].Remove)
                    {
                        StepLapLines[i].RemoveProtected = true;
                        break;
                    }
                }

                #endregion

                #region JoiningLines

                double lastSheetPosition = 0;
                double lastDistance = 0;
                double join = 0;
                int actStep = 1;

                switch (p.Layers[basicLayer.Id].Name)
                {
                    case "001":
                    case "003":
                    case "002-002":
                    case "004-004":
                    case "2004-004":
                    case "2004-2004":
                    case "2004.2-004":
                    case "2004.3-004":

                        while (actStep <= l.NumberOfStepLaps * l.NumberOfSame)
                        {
                            foreach (LayerLine ll in StepLapLines)
                            {
                                if (ll.ActualStep == actStep)
                                {
                                    JoiningLines.Add((LayerLine)ll.Clone());
                                    JoiningLines.Last().XDistance += lastDistance;
                                    if (JoiningLines.Last().Remove)
                                    {
                                        join = JoiningLines.Last().XDistance - lastSheetPosition;
                                    }
                                    else
                                    {
                                        JoiningLines.Last().XDistance -= join;
                                        if (JoiningLines.Last().PieceFinish)
                                            lastSheetPosition = JoiningLines.Last().XDistance;
                                    }
                                }
                            }
                            actStep += 1;
                            lastDistance = JoiningLines.Last().XDistance;
                        }

                        break;


                    case "006":
                    case "007":
                        while (actStep <= l.NumberOfStepLaps * l.NumberOfSame)
                        {
                            //pronadji layer line remove xdistance
                            LayerLine llRemove = StepLapLines.FirstOrDefault(sll => sll.ActualStep == actStep && sll.Remove);
                            double llRemoveXDistance = llRemove.XDistance;  
                            //anuliranje po layer line remove distance 
                            foreach (LayerLine ll in StepLapLines)
                            {
                                if (ll.ActualStep == actStep)
                                {
                                    if (actStep == 1 || !ll.Remove)
                                    {
                                        JoiningLines.Add((LayerLine)ll.Clone());
                                        JoiningLines.Last().XDistance -= llRemoveXDistance;
                                        JoiningLines.Last().XDistance += lastSheetPosition;
                                    }
                                }
                            }
                            List<LayerLine> s3Lines = StepLapLines.FindAll(sll => sll.ActualStep == actStep && (sll.NToolName == 5 || sll.NToolName == 7));
                            lastSheetPosition += s3Lines[s3Lines.Count - 1].XDistance - s3Lines[0].XDistance;
                            
                            actStep++;
                            //foreach (LayerLine ll in StepLapLines)
                            //{
                            //    if (ll.ActualStep == actStep)
                            //    {
                            //        JoiningLines.Add((LayerLine)ll.Clone());
                            //        JoiningLines.Last().XDistance += lastDistance;
                            //        if (JoiningLines.Last().Remove)
                            //        {
                            //            join = JoiningLines.Last().XDistance - lastSheetPosition;
                            //        }
                            //        else
                            //        {
                            //            JoiningLines.Last().XDistance -= join;

                            //            //if (JoiningLines.Last().PieceFinish) //It Possible it will work for all cases -should be tested!!!
                            //            lastSheetPosition = JoiningLines.Last().XDistance;

                            //        }
                            //    }
                            //}
                            //actStep += 1;
                            //lastDistance = JoiningLines.Last().XDistance;
                        }

                        break;

                    default:
                        break;
                }




                for (int i = 0; i < JoiningLines.Count; i++)
                {
                    if (JoiningLines[i].RemoveProtected)
                    {
                        JoiningLines[i].XDistance = 0;
                        JoiningLines[i].ActualPiece = 0;
                    }
                    else
                    {
                        if (JoiningLines[i].Remove && !JoiningLines[i].RemoveProtected)
                        {
                            JoiningLines.Remove(JoiningLines[i]);
                        }
                    }
                }

                foreach (LayerLine ll in JoiningLines)
                {
                    ll.XPosition = ll.XDistance + GetToolRefPos(ll.NToolName, l.S1_90, l.CoilWidth);
                    ll.ActualPiece += (ll.ActualStep - 1) * l.NumberOfSheetsInSequence;
                }

                l.NumberOfLines = JoiningLines.Count - 1;

                if (l.NumberOfLines > Constants.MaxNumberOfLines)
                {
                    MessageBox.Show("This product is not valid (too much data). Layer Number: " + l.LayerID.ToString());
                    return;
                }
                #endregion

                #region PositioningLines

                double maxS1Pos = 0;
                double minS1Pos = 0;

                for (int i = 0; i < JoiningLines.Count; i++)
                {
                    if (JoiningLines[i].NToolName == 5 || JoiningLines[i].NToolName == 7)
                    {
                        minS1Pos = JoiningLines[i].XPosition;
                        break;
                    }
                }
                for (int i = 0; i < JoiningLines.Count; i++)
                {
                    if (JoiningLines[i].NToolName == 5 || JoiningLines[i].NToolName == 7)
                    {
                        maxS1Pos = JoiningLines[i].XPosition;
                    }
                }

                LayerLine llRemove1 = JoiningLines.FirstOrDefault(jl => jl.Remove);

                PositioningLines.Add((LayerLine)llRemove1.Clone()); // Add first line only one time

                for (int i = 0; i < Constants.Multiplier; i++) // copy 10 times all rest lines and change actual piece and and XPosition
                {
                    foreach (LayerLine ll in JoiningLines.FindAll(ll => !ll.Remove))
                    {
                        PositioningLines.Add((LayerLine)ll.Clone());
                        PositioningLines.Last().ActualPiece += JoiningLines.Last().ActualPiece * i;

                        PositioningLines.Last().XPosition += (maxS1Pos - minS1Pos) * i;
                    }
                    //for (int j = 1; j < JoiningLines.Count; j++)
                    //{
                    //    PositioningLines.Add((LayerLine)JoiningLines[j].Clone());
                    //    PositioningLines.Last().ActualPiece += JoiningLines.Last().ActualPiece * i;


                    //    PositioningLines.Last().XPosition += (maxS1Pos - minS1Pos) * i;
                    //    //PositioningLines.Last().XPosition += (JoiningLines.Last().XPosition - JoiningLines.First().XPosition) * i;

                    //}
                }

                // Calculate YNextPos
                double tempYPos = 0;
                for (int i = PositioningLines.Count - 1; i >= 0; --i)
                {
                    if (PositioningLines[i].NToolName == 1 || PositioningLines[i].NToolName == 2)
                    {
                        PositioningLines[i].YNextPos = tempYPos;
                        tempYPos = PositioningLines[i].YPos;
                    }
                }

                // Feed calculation
                PositioningLines = PositioningLines.OrderBy(o => o.XPosition).ToList();

                PositioningLines.First().XFeed = PositioningLines.First().XPosition;
                for (int i = 1; i < PositioningLines.Count; i++)
                {
                    PositioningLines[i].XFeed = PositioningLines[i].XPosition - PositioningLines[i - 1].XPosition;
                }

                #endregion

                #region PreLines

                // Start position calculation.
                int startPos = 0;
                for (int i = 0; i < PositioningLines.Count; i++)
                {
                    if ((PositioningLines[i].NToolName == 5 || PositioningLines[i].NToolName == 7) && PositioningLines[i].XDistance == 0)
                    {
                        startPos = i;
                        break;
                    }

                }

                for (int i = startPos + 1; i < JoiningLines.Count + startPos; i++)
                {
                    PreLines.Add(PositioningLines[i]);
                }

                // Calculate Decrease Actual Piece Value
                int MaxActualPieceNumber = PreLines.Max(x => x.ActualPiece);

                int DecreasingValue = (MaxActualPieceNumber / l.AddNumber + 1) * l.AddNumber;


                #endregion

                #region Lines

                foreach (LayerLine ll in PreLines)
                {
                    Lines.Add((LayerLine)ll.Clone());
                    Lines.Last().ActualPiece -= DecreasingValue;
                }

                #endregion

                #region Rest calculations

                bool bFirstVYPosSet = false;
                for (int i = 0; i < Lines.Count; i++)
                {
                    if (l.FirstVYPos == 0 && !bFirstVYPosSet && (Lines[i].NToolName == 1 || Lines[i].NToolName == 2))
                    {
                        l.FirstVYPos = Lines[i].YPos;
                        bFirstVYPosSet = true;
                    }
                    
                    if (Lines[i].NToolName == 1 || Lines[i].NToolName == 2)
                        VLines.Add((LayerLine)Lines[i].Clone());

                    if (Lines[i].NToolName == 1)
                    {
                        l.UseVB = true;
                    }
                    if (Lines[i].NToolName == 2)
                    {
                        l.UseVF = true;
                    }
                    if (Lines[i].NToolName == 3)
                    {
                        l.P1YPos = Lines[i].YPos;
                        l.UseP1 = true;
                    }
                    if (Lines[i].NToolName == 4)
                    {
                        l.P2YPos = Lines[i].YPos;
                        l.UseP2 = true;
                    }
                    if (Lines[i].NToolName == 5)
                    {
                        l.UseS1 = true;
                    }
                    if (Lines[i].NToolName == 6)
                    {
                        l.UseS2 = true;
                    }
                    if (Lines[i].NToolName == 7)
                    {
                        l.UseS3 = true;
                    }
                }
                if (VLines.Count > 0)
                {
                    l.MaxVYPos = VLines.Max(x => x.YPos);
                    l.MinVYPos = VLines.Min(x => x.YPos);
                }



                #endregion

                if (!(Product_Valid(p)))
                {
                    return;
                }

                if (Directory.Exists(Settings.Default.LastActivatedProductPath))
                {
                    #region Delete all files in last Activate dir
                    System.IO.DirectoryInfo di = new DirectoryInfo(Settings.Default.LastActivatedProductPath);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    //foreach (DirectoryInfo dir in di.GetDirectories())
                    //{
                    //    dir.Delete(true);
                    //}
                    #endregion

                    #region Create and fill .csv files for each layer
                    try
                    {

                        string path = Settings.Default.LastActivatedProductPath + "\\" + this.ProductName + "_Layer_" + l.LayerID + ".csv";
                        File.Create(path).Dispose();
                        using (TextWriter tw = new StreamWriter(path, true))
                        {

                            tw.WriteLine("ELPLANT DOO,Kragujevac Serbia, www.elplant.com");
                            tw.WriteLine("Last activate Layer");
                            tw.WriteLine("{0,5} {1,17:g}", "Date:", DateTime.Now);
                            tw.WriteLine("");
                            tw.WriteLine("Product Name: " + this.ProductName);
                            tw.WriteLine("Layer Name: " + l.LayerName);
                            tw.WriteLine("");
                            tw.WriteLine("Layer ID: " + l.LayerID.ToString());
                            tw.WriteLine("Number Of Lines: " + l.NumberOfLines.ToString());
                            tw.WriteLine("Number Of Step Laps: " + l.NumberOfStepLaps.ToString());
                            tw.WriteLine("Number Of Same: " + l.NumberOfSame.ToString());
                            tw.WriteLine("NumberOfSequenceInCycle: " + l.NumberOfSequenceInCycle.ToString());
                            tw.WriteLine("NumberOfSheetsInSequence: " + l.NumberOfSheetsInSequence.ToString());
                            tw.WriteLine("AddNumber: " + l.AddNumber.ToString());
                            tw.WriteLine("Coil Width: " + l.CoilWidth.ToString());
                            tw.WriteLine("Coil Thick: " + l.CoilThick.ToString());
                            tw.WriteLine("Max VY Pos: " + l.MaxVYPos.ToString());
                            tw.WriteLine("Min VY Pos: " + l.MinVYPos.ToString());
                            tw.WriteLine("First VY Pos: " + l.FirstVYPos.ToString());
                            tw.WriteLine("P1Y Pos: " + l.P1YPos.ToString());
                            tw.WriteLine("P2Y Pos: " + l.P2YPos.ToString());
                            tw.WriteLine("Use VB: " + l.UseVB.ToString());
                            tw.WriteLine("Use VF: " + l.UseVF.ToString());
                            tw.WriteLine("Use P1: " + l.UseP1.ToString());
                            tw.WriteLine("Use P2: " + l.UseP2.ToString());
                            tw.WriteLine("Use S1: " + l.UseS1.ToString());
                            tw.WriteLine("Use S2: " + l.UseS2.ToString());
                            tw.WriteLine("Use S3: " + l.UseS3.ToString());
                            tw.WriteLine("");
                            tw.WriteLine("VB Ref Pos: " + tp.VB_RefPos.ToString());
                            tw.WriteLine("VF Ref Pos: " + tp.VF_RefPos.ToString());
                            tw.WriteLine("P1 Ref Pos: " + tp.P1_RefPos.ToString());
                            tw.WriteLine("P2 Ref Pos: " + tp.P2_RefPos.ToString());
                            tw.WriteLine("S1 Ref Pos: " + tp.S1_RefPos.ToString());
                            tw.WriteLine("S2 Ref Pos: " + tp.S2_RefPos.ToString());
                            tw.WriteLine("S3 Ref Pos: " + tp.S3_RefPos.ToString());
                            tw.WriteLine("Max Guides Pos: " + tp.MaxGuidesPos.ToString());
                            tw.WriteLine("Min Guides Pos: " + tp.MinGuidesPos.ToString());
                            //tw.WriteLine("Max PY Pos: " + tp.MaxPYPos.ToString());
                            //tw.WriteLine("Min PY Pos: " + tp.MinPYPos.ToString());
                            tw.WriteLine("Max VY Pos: " + tp.MaxVYPos.ToString());
                            tw.WriteLine("Min VY Pos: " + tp.MinVYPos.ToString());
                            //tw.WriteLine("PYGuides Max CLereance: " + tp.PYGuidesMaxClereance.ToString());
                            //tw.WriteLine("PYGuides Min CLereance: " + tp.PYGuidesMinClereance.ToString());
                            tw.WriteLine("");
                            tw.WriteLine("Min Material Width: " + tp.MinMaterialWidth.ToString());
                            tw.WriteLine("Max Material Width: " + tp.MaxMaterialWidth.ToString());
                            tw.WriteLine("S1 Calculated Ref Pos: " + GetToolRefPos(5, false, l.CoilWidth).ToString());
                            tw.WriteLine("S2 Calculated Ref Pos: " + GetToolRefPos(6, false, l.CoilWidth).ToString());


                            tw.WriteLine("");
                            tw.WriteLine("Lines");
                            tw.WriteLine(",nToolName, iActualPiece, iStepLap, bPieceFinish, bScrap, bRemove, ShapePartNumber, lrDistance, lrPosition, lrXFeed, lrXFeedFirstPos,  lrYPos, lrYNextPos");
                            foreach (LayerLine ll in Lines)
                            {
                                tw.WriteLine(", " + ll.NToolName.ToString() + ", " + ll.ActualPiece.ToString() + ", " + ll.ActualStep.ToString() + ", " + ll.PieceFinish.ToString() + ", " + ll.CLegScrap.ToString() + ", " + ll.Remove.ToString() + ", " + ll.ShapePartNumber.ToString() + ", " + ll.XDistance.ToString() + ", " + ll.XPosition.ToString() + ", " + ll.XFeed.ToString() + ", " + ll.XFeedFirstPiece.ToString() + ", " + ll.YPos.ToString() + ", " + ll.YNextPos);
                            }

                            tw.WriteLine("");
                            tw.WriteLine("SimpleLines");
                            tw.WriteLine(",nToolName, iActualPiece, iStepLap, bPieceFinish, bScrap, bRemove, ShapePartNumber, lrDistance, lrPosition, lrXFeed, lrXFeedFirstPos,  lrYPos, lrYNextPos");
                            foreach (LayerLine ll in SimpleLines)
                            {
                                tw.WriteLine(", " + ll.NToolName.ToString() + ", " + ll.ActualPiece.ToString() + ", " + ll.ActualStep.ToString() + ", " + ll.PieceFinish.ToString() + ", " + ll.CLegScrap.ToString() + ", " + ll.Remove.ToString() + ", " + ll.ShapePartNumber.ToString() + ", " + ll.XDistance.ToString() + ", " + ll.XPosition.ToString() + ", " + ll.XFeed.ToString() + ", " + ll.XFeedFirstPiece.ToString() + ", " + ll.YPos.ToString() + ", " + ll.YNextPos);
                            }

                            tw.WriteLine("");
                            tw.WriteLine("StepLapLines");
                            tw.WriteLine(",nToolName, iActualPiece, iStepLap, bPieceFinish, bScrap, bRemove, ShapePartNumber, lrDistance, lrPosition, lrXFeed, lrXFeedFirstPos,  lrYPos, lrYNextPos");
                            foreach (LayerLine ll in StepLapLines)
                            {
                                tw.WriteLine(", " + ll.NToolName.ToString() + ", " + ll.ActualPiece.ToString() + ", " + ll.ActualStep.ToString() + ", " + ll.PieceFinish.ToString() + ", " + ll.CLegScrap.ToString() + ", " + ll.Remove.ToString() + ", " + ll.ShapePartNumber.ToString() + ", " + ll.XDistance.ToString() + ", " + ll.XPosition.ToString() + ", " + ll.XFeed.ToString() + ", " + ll.XFeedFirstPiece.ToString() + ", " + ll.YPos.ToString() + ", " + ll.YNextPos);
                            }

                            tw.WriteLine("");
                            tw.WriteLine("JoiningLines");
                            tw.WriteLine(",nToolName, iActualPiece, iStepLap, bPieceFinish, bScrap, bRemove, ShapePartNumber, lrDistance, lrPosition, lrXFeed, lrXFeedFirstPos,  lrYPos, lrYNextPos");
                            foreach (LayerLine ll in JoiningLines)
                            {
                                tw.WriteLine(", " + ll.NToolName.ToString() + ", " + ll.ActualPiece.ToString() + ", " + ll.ActualStep.ToString() + ", " + ll.PieceFinish.ToString() + ", " + ll.CLegScrap.ToString() + ", " + ll.Remove.ToString() + ", " + ll.ShapePartNumber.ToString() + ", " + ll.XDistance.ToString() + ", " + ll.XPosition.ToString() + ", " + ll.XFeed.ToString() + ", " + ll.XFeedFirstPiece.ToString() + ", " + ll.YPos.ToString() + ", " + ll.YNextPos);
                            }

                            tw.WriteLine("");
                            tw.WriteLine("PositioningLines");
                            tw.WriteLine(",nToolName, iActualPiece, iStepLap, bPieceFinish, bScrap, bRemove, ShapePartNumber, lrDistance, lrPosition, lrXFeed, lrXFeedFirstPos,  lrYPos, lrYNextPos");
                            foreach (LayerLine ll in PositioningLines)
                            {
                                tw.WriteLine(", " + ll.NToolName.ToString() + ", " + ll.ActualPiece.ToString() + ", " + ll.ActualStep.ToString() + ", " + ll.PieceFinish.ToString() + ", " + ll.CLegScrap.ToString() + ", " + ll.Remove.ToString() + ", " + ll.ShapePartNumber.ToString() + ", " + ll.XDistance.ToString() + ", " + ll.XPosition.ToString() + ", " + ll.XFeed.ToString() + ", " + ll.XFeedFirstPiece.ToString() + ", " + ll.YPos.ToString() + ", " + ll.YNextPos);
                            }


                            tw.WriteLine("");
                            tw.WriteLine("PreLines");
                            tw.WriteLine(",nToolName, iActualPiece, iStepLap, bPieceFinish, bScrap, bRemove, ShapePartNumber, lrDistance, lrPosition, lrXFeed, lrXFeedFirstPos,  lrYPos, lrYNextPos");
                            foreach (LayerLine ll in PreLines)
                            {
                                tw.WriteLine(", " + ll.NToolName.ToString() + ", " + ll.ActualPiece.ToString() + ", " + ll.ActualStep.ToString() + ", " + ll.PieceFinish.ToString() + ", " + ll.CLegScrap.ToString() + ", " + ll.Remove.ToString() + ", " + ll.ShapePartNumber.ToString() + ", " + ll.XDistance.ToString() + ", " + ll.XPosition.ToString() + ", " + ll.XFeed.ToString() + ", " + ll.XFeedFirstPiece.ToString() + ", " + ll.YPos.ToString() + ", " + ll.YNextPos);
                            }


                            tw.Close();
                        }
                    }
                    catch (Exception e1)
                    {

                        System.Windows.MessageBox.Show("Error during saving .txt file: " + e1.Message);
                    }
                }

                #endregion


                #region Send Layers to PLC

                if (Settings.Default.PLC_Active)
                {
                    try
                    {
                        Connect();
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].iLayerID"), Convert.ToInt16(l.LayerID));
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].sLayerName"), l.LayerName, new int[] { 80 });
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].iNumberOfLines"), Convert.ToInt16(l.NumberOfLines));
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].iNumberOfStepLaps"), Convert.ToInt16(l.NumberOfStepLaps));
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].iNumberOfSame"), Convert.ToInt16(l.NumberOfSame));
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].iPiecesToDo"), Convert.ToInt16(l.PiecesToDo));
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].iPiecesOffset"), Convert.ToInt16(l.PiecesOffset));
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].iNumberOfSequenceInCycle"), Convert.ToInt16(l.NumberOfSequenceInCycle));
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].iNumberOfSheetsInSequence"), Convert.ToInt16(l.NumberOfSheetsInSequence));
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrCoilWidth"), l.CoilWidth);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrCoilThick"), l.CoilThick);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrMaxVYPos"), l.MaxVYPos);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrMinVYPos"), l.MinVYPos);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrFirstVYPos"), l.FirstVYPos);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrP1YPos"), l.P1YPos);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrP2YPos"), l.P2YPos);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].bS1_90"), l.S1_90);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].abToolUsed[1]"), l.UseVB);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].abToolUsed[2]"), l.UseVF);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].abToolUsed[3]"), l.UseP1);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].abToolUsed[4]"), l.UseP2);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].abToolUsed[5]"), l.UseS1);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].abToolUsed[6]"), l.UseS2);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].abToolUsed[7]"), l.UseS3);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].iAddNumber"), Convert.ToInt16(l.AddNumber));

                        // Only for TRL Africa
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].bTreadingPinActive1"), l.TreadingPinActive1);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrPinHoleDistanceFromEndOfPiece1"), l.PinHoleDistanceFromEndOfPiece1);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrPinHoleStepLapOffset1"), l.PinHoleStepLapOffset1);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrPieceLength1"), l.PieceLength1);

                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].bTreadingPinActive2"), l.TreadingPinActive2);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrPinHoleDistanceFromEndOfPiece2"), l.PinHoleDistanceFromEndOfPiece2);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrPinHoleStepLapOffset2"), l.PinHoleStepLapOffset2);
                        tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].lrPieceLength2"), l.PieceLength2);
                        //

                        for (int i = 0; i < Lines.Count; i++)
                        {
                            tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].arLayerLines[" + i.ToString() + "].nToolName"), Convert.ToInt16(Lines[i].NToolName));
                            tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].arLayerLines[" + i.ToString() + "].iActualPiece"), Convert.ToInt16(Lines[i].ActualPiece));
                            tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].arLayerLines[" + i.ToString() + "].bPieceFinish"), Lines[i].PieceFinish);
                            tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].arLayerLines[" + i.ToString() + "].bClegScrap"), Lines[i].CLegScrap);
                            tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].arLayerLines[" + i.ToString() + "].lrXFeed"), Lines[i].XFeed);
                            tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].arLayerLines[" + i.ToString() + "].lrXFeedFirstPiece"), Lines[i].XFeedFirstPiece);
                            tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].arLayerLines[" + i.ToString() + "].lrYPos"), Lines[i].YPos);
                            tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.atLayers[" + l.LayerID.ToString() + "].arLayerLines[" + i.ToString() + "].lrYNextPos"), Lines[i].YNextPos);
                        }

                        Disconnect();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("It is not possible to send Layer: " + basicLayer.Id.ToString() + " to PLC!\nException: " + ex.ToString());
                        return;
                    }
                }

                #endregion

            }

            #region Send to PLC

            if (Settings.Default.PLC_Active)
            {
                try
                {
                    Connect();

                    tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.sProductName"), this.ProductName, new int[] { 80 });
                    tcClient.WriteAny(tcClient.CreateVariableHandle("App_Variables.g_tProduct.iLayersNumber"), Convert.ToInt16(this.LayersNumber));

                    Disconnect();
                }
                catch (Exception ex)
                {

                    System.Windows.MessageBox.Show("It is not possible to send Product data to PLC!\nException: " + ex.ToString());
                    return;
                }
            }

            #endregion
        }


        ToolData GetToolName(EFeature f)
        {
            ToolData td = new ToolData();
            switch (f)
            {
                case EFeature.Cut135:
                    td.ToolName = 6;
                    //td.ToolRefPos = tp.S2_RefPos;
                    td.S1_90 = 0;
                    break;
                case EFeature.Cut45:
                    td.ToolName = 5;
                    //td.ToolRefPos = tp.S1_RefPos;
                    td.S1_90 = 0;
                    break;
                case EFeature.Cut90:
                    td.ToolName = 7;
                    //td.ToolRefPos = tp.S3_RefPos;
                    td.S1_90 = 1;
                    break;
                case EFeature.VTop:
                    td.ToolName = 1;
                    //td.ToolRefPos = tp.VB_RefPos;
                    td.S1_90 = 0;
                    break;
                case EFeature.VBottom:
                    td.ToolName = 2;
                    //td.ToolRefPos = tp.VF_RefPos;
                    td.S1_90 = 0;
                    break;
                default:
                    break;
            }

            return td;
        }

        // NU , This is not universal function, do not use all possible tools - Update later for other machines
        double GetToolRefPos(int ToolNumber, bool S1_90, double MaterialWidth)
        {
            double toReturn = 0;

            switch (ToolNumber)
            {
                case 1:
                    toReturn = tp.VB_RefPos;
                    break;

                case 2:
                    toReturn = tp.VF_RefPos;
                    break;

                case 3:
                    toReturn = tp.P1_RefPos;
                    break;

                case 4:
                    toReturn = tp.P2_RefPos;
                    break;

                case 5:
                    toReturn = tp.S1_RefPos + tp.S1_RefPosCorrection * (MaterialWidth - tp.MinMaterialWidth) /(tp.MaxMaterialWidth - tp.MinMaterialWidth) - MaterialWidth / 2;
                    //toReturn = tp.S1_RefPos + tp.S1_RefPosCorrection * (530 - MaterialWidth) / (530 - 60) - MaterialWidth / 2;
                    break;

                case 6:
                    toReturn = tp.S2_RefPos + tp.S2_RefPosCorrection * (MaterialWidth - tp.MinMaterialWidth) / (tp.MaxMaterialWidth - tp.MinMaterialWidth) + MaterialWidth / 2;
                    //toReturn = tp.S2_RefPos + tp.S2_RefPosCorrection * (530 - MaterialWidth) / (530 - 60) + MaterialWidth / 2;
                    break;

                case 7:
                    toReturn = tp.S3_RefPos;
                    break;

                default:
                    break;
            }

            return toReturn;
        }

        private bool Product_Valid(Product p)
        {
            bool toReturn = true;

            double MaxHolePos = 0;
            double HoleYPos = 0;

            // Check holes position


            foreach (Layer l in p.Layers)
            {

                if (l.LayerDefault.NumberOfSteps > 10)
                {
                    toReturn = false;
                    MessageBox.Show("Number of steps cannot be bigger then: " + (Constants.MaxSteplapNumber).ToString() + ". Layer Number: " + (l.Id + 1).ToString());
                }

                if (l.LayerDefault.NumberOfSame > 10)
                {
                    toReturn = false;
                    MessageBox.Show("Number of same cannot be bigger then: " + (Constants.MaxNumberOfSame).ToString() + ". Layer Number: " + (l.Id + 1).ToString());
                }

                foreach (Shape s in l.Shapes)
                {
                    foreach (ShapePart sp in s.ShapeParts)
                    {
                        if (Math.Abs(sp.Y) > l.LayerDefault.Width)
                        {
                            toReturn = false;
                            MessageBox.Show("Product is not valid. Shape Number: " + (s.Id + 1).ToString() + " in Layer Number: " + (l.Id + 1).ToString() + " has wrong values");
                        }
                    }

                    for (int i = 0; i < s.Holes.Count; i++)
                    {
                        if (s.Holes[i].X > MaxHolePos)
                        {
                            MaxHolePos = s.Holes[i].X;
                            HoleYPos = s.Holes[i].Y;
                        }

                        if (s.Holes[i].X < 0 || s.Holes[i].X > s.ShapeParts.Last().X)
                        {
                            toReturn = false;
                            MessageBox.Show("Product is not valid. Hole number: " + (i + 1).ToString() + " is out of Shape Number: " + (s.Id + 1).ToString() + " in Layer Number: " + (l.Id + 1).ToString() + " in X direction");
                        }

                        if (s.Holes[i].Y < 0 || s.Holes[i].Y > l.LayerDefault.Width)
                        {
                            toReturn = false;
                            MessageBox.Show("Product is not valid. Hole number: " + (i + 1).ToString() + " is out of Shape Number: " + (s.Id + 1).ToString() + " in Layer Number: " + (l.Id + 1).ToString() + " in Y direction");
                        }
                    }

                }

            }



            return toReturn;
        }

        private void Connect()
        {
            // Create instance of class TcAdsClient
            tcClient = new TcAdsClient();

            try
            {
                // Connect to local PLC - Runtime 1 - TwinCAT 3 Port=851
                tcClient.Connect(Settings.Default.PLC_Port);
            }
            catch (Exception err)
            {
                MessageBox.Show("Connection cannot be esablish" + err.Message);
            }
        }

        private void Disconnect()
        {
            tcClient.Dispose();
        }
    }
}
