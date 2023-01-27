using ProductLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Xml.Serialization;
using static JobEditor.DokTest.Job;
using Path = System.IO.Path;

namespace JobEditor.DokTest
{
    public static class OptimizedCutSeqGenerator
    {
        private static int ShapeMultiplicationFactor = 100;
        private static Product LoadProduct(String productFilePath)
        {
            Product product;
            using (StreamReader reader = new StreamReader(productFilePath))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    while (!reader.EndOfStream)
                    {
                        var bytes = Encoding.UTF8.GetBytes(reader.ReadLine());
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    stream.Position = 0;
                    XmlSerializer serializer = new XmlSerializer(typeof(Product));
                    product = (Product)serializer.Deserialize(stream);
                }
            }
            return product;
        }
        private static Job ConvertToJob(Product p)
        {
            Job job = new Job() { Layers = new List<Job.Layer>() };
            foreach (var pLayer in p.Layers)
            {
                Job.Layer layer = new Job.Layer() { Id = pLayer.Id, ShapesPerProduct = pLayer.Shapes.Count, Shapes = new List<Job.Shape>() };
                int shapeNo = 1;
                for (int i = 0; i < pLayer.StepLapsDefault.NumberOfSteps; i++)
                {
                    foreach (var pShape in pLayer.Shapes)
                    {
                        Job.Shape shape = new Job.Shape() { No = shapeNo++, Parts = new List<Job.ShapePart>() };
                        foreach (var pShapePart in pShape.ShapeParts)
                        {
                            Job.ShapePart shapePart = new Job.ShapePart()
                            {
                                X = pShapePart.X + pShapePart.StepLap.Steps[i].X,
                                Y = pShapePart.Y + pShapePart.StepLap.Steps[i].Y,
                                OverCut = pShapePart.OverCut,
                                DoubleCut = pShapePart.DoubleCut
                            };
                            switch (pShapePart.Feature)
                            {
                                case ProductLib.EFeature.Cut90:
                                    shapePart.Feature = Job.Feature.CUT_90;
                                    break;
                                case ProductLib.EFeature.Cut45:
                                    shapePart.Feature = Job.Feature.CUT_45;
                                    break;
                                case ProductLib.EFeature.Cut135:
                                    shapePart.Feature = Job.Feature.CUT_135;
                                    break;
                                case ProductLib.EFeature.VTop:
                                    shapePart.Feature = Job.Feature.V_TOP;
                                    break;
                                case ProductLib.EFeature.VBottom:
                                    shapePart.Feature = Job.Feature.V_BOTTOM;
                                    break;
                                case ProductLib.EFeature.CLeft:
                                    shapePart.Feature = Job.Feature.C_LEFT;
                                    break;
                                case ProductLib.EFeature.CRight:
                                    shapePart.Feature = Job.Feature.C_RIGHT;
                                    break;
                                default:
                                    break;
                            }
                            shape.Parts.Add(shapePart);
                        }
                        foreach (var pHole in pShape.Holes)
                        {
                            shape.Parts.Add(new Job.ShapePart()
                            {
                                X = pHole.X,
                                Y = pHole.Y,
                                Feature = pHole.Shape == EHoleShape.Round ? Job.Feature.H_ROUND : Job.Feature.H_OBLONG
                            });
                        }
                        layer.Shapes.Add(shape);
                    }
                }
                job.Layers.Add(layer);
            }
            return job;
        }
        //private static MachineTools LoadMachineTools()
        //{
        //    MachineTools toreturn = new MachineTools()
        //    {
        //        Tools = new List<MachineTools.Tool>() { new MachineTools.Tool() {
        //            Name = "P1",
        //            IsMobile = true,
        //            X = 500,
        //            Y = 0,
        //            XOffset = 300,
        //            YOffset = 0,
        //            Type = MachineTools.ToolType.P_ROUND
        //        },
        //        new MachineTools.Tool() {
        //            Name = "P2",
        //            IsMobile = true,
        //            X = 1000,
        //            Y = 0,
        //            XOffset = 300,
        //            YOffset = 0,
        //            Type = MachineTools.ToolType.P_ROUND
        //        },
        //        new MachineTools.Tool() {
        //            Name = "S1",
        //            IsMobile = false,
        //            X = 750,
        //            Y = 0,
        //            XOffset = 0,
        //            YOffset = 0,
        //            Type = MachineTools.ToolType.CUT_45
        //        },
        //        new MachineTools.Tool() {
        //            Name = "VTop",
        //            IsMobile = false,
        //            X = 300,
        //            Y = 0,
        //            XOffset = 0,
        //            YOffset = 0,
        //            Type = MachineTools.ToolType.V_TOP
        //        },
        //        new MachineTools.Tool() {
        //            Name = "S2",
        //            IsMobile = false,
        //            X = 1750,
        //            Y = 0,
        //            XOffset = 0,
        //            YOffset = 0,
        //            Type = MachineTools.ToolType.CUT_135
        //        }}
        //    };
        //    return toreturn;
        //}
        private static MachineTools LoadMachineTools()
        {
            MachineTools toreturn = new MachineTools()
            {
                Tools = new List<MachineTools.Tool>() { new MachineTools.Tool() {
                    Name = "P1",
                    IsMobile = true,
                    X = 300,
                    Y = 0,
                    XOffset = 300,
                    YOffset = 0,
                    Type = MachineTools.ToolType.P_ROUND
                },
                new MachineTools.Tool() {
                    Name = "P2",
                    IsMobile = true,
                    X = 650,
                    Y = 0,
                    XOffset = 300,
                    YOffset = 0,
                    Type = MachineTools.ToolType.P_ROUND
                },
                new MachineTools.Tool() {
                    Name = "S1",
                    IsMobile = false,
                    X = 2000,
                    Y = 0,
                    XOffset = 0,
                    YOffset = 0,
                    Type = MachineTools.ToolType.CUT_45
                },
                new MachineTools.Tool() {
                    Name = "VTop",
                    IsMobile = false,
                    X = 620,
                    Y = 0,
                    XOffset = 0,
                    YOffset = 0,
                    Type = MachineTools.ToolType.V_TOP
                },
                new MachineTools.Tool() {
                    Name = "S2",
                    IsMobile = false,
                    X = 2700,
                    Y = 0,
                    XOffset = 0,
                    YOffset = 0,
                    Type = MachineTools.ToolType.CUT_135
                }}
            };
            return toreturn;
        }

        private static IList<CutSequence> GenerateCutSequences(Job.Layer layer, MachineTools machineTools)
        {
            List<CutSequence> toReturn = new List<CutSequence>();
            double distance = 0;
            //sort shape parts by X
            ((List<Job.Shape>)layer.Shapes).ForEach(s => ((List<Job.ShapePart>)s.Parts).Sort((x, y) => x.X.CompareTo(y.X)));
            //mulitply shapes to find optimal CutSequences
            MachineTools.ToolType[] previousFeatureTools = null;
            for (int i = 0; i < ShapeMultiplicationFactor; i++)
            {
                foreach (var shape in layer.Shapes)
                {
                    var firstShapePart = shape.Parts.First();
                    //shift shapes to adjust finishing previous one and starting next one 
                    var shift = firstShapePart.X;
                    if (shift != 0)
                        ((List<Job.Shape>)layer.Shapes).Where((s, index) => index >= layer.Shapes.IndexOf(shape)).ToList()
                            .ForEach(s => ((List<Job.ShapePart>)s.Parts).ForEach(sp => sp.X -= shift));

                    foreach (var shapePart in shape.Parts)
                    {
                        var featurePossibleTools = FeatureToolDependencies.Dependencies.First(e => e.Item1 == shapePart.Feature).Item2;
                        var featureAvailableTools = featurePossibleTools.Where(e => e.All(e1 => machineTools.Tools.Select(t => t.Type).Contains(e1)));
                        var featureTools = featureAvailableTools.Where(e => e.Length == featureAvailableTools.Min(e1 => e1.Length))
                            .OrderBy(e => machineTools.Tools.Where(e1 => e1.Type == e[0]).OrderBy(e2 => e2.X).First().X).First();

                        //avoid generating same CutSequences for first shape part regard to last shape part from previous shape except for first shape
                        if (shapePart == firstShapePart && !(i == 0 && shape.No == 1))
                        {
                            featureTools = featureTools.Except(previousFeatureTools).ToArray();
                        }

                        foreach (var featureTool in featureTools)
                        {
                            var tool = machineTools.Tools.Where(e => e.Type == featureTool).OrderBy(e => e.X).First();
                            var shapeNoPerProduct = layer.Shapes.IndexOf(shape) % layer.ShapesPerProduct;
                            var xPosition = FeatureToolDependencies.CalculateShapePartXPosition(shapePart, tool.Type);
                            CutSequence toAdd = new CutSequence()
                            {
                                XPosition = xPosition + tool.X + distance,
                                XFeedFirstPosition = shapeNoPerProduct == 0 ? xPosition + tool.X :
                                    layer.Shapes.Where((s, index) => index >= layer.Shapes.IndexOf(shape) - shapeNoPerProduct && index < layer.Shapes.IndexOf(shape))
                                    .Sum(s => s.Parts.Last().X - s.Parts.First().X) + xPosition + tool.X,
                                ToolSequences = new List<CutSequence.ToolSequence> {
                                        new CutSequence.ToolSequence() { Tool = tool, Feature = shapePart.Feature, XPos = 0, 
                                            YPos = FeatureToolDependencies.CalculateShapePartYPosition(shapePart, tool.Type), ShapeNo = shape.No + i * layer.Shapes.Count} }
                            };
                            toReturn.Add(toAdd);
                            var doubleCut = FeatureToolDependencies.CalculateShapePartDoubleCutXPosition(shapePart, tool.Type);
                            if (doubleCut != 0)
                            {
                                var toAddDoubleCut = (CutSequence)toAdd.Clone();
                                toAddDoubleCut.XPosition += doubleCut;
                                toAddDoubleCut.XFeedFirstPosition += doubleCut;
                                toReturn.Add(toAddDoubleCut);
                            }
                        }
                        previousFeatureTools = featureTools;
                    }
                    distance += shape.Parts.Last().X - shape.Parts.First().X;
                }
            }

            //sort by XPosition and adjust order of CutSequences to prevent that next one is finished before previous one
            toReturn = toReturn.OrderBy(e => e.XPosition).ThenBy(e => e.ToolSequences[0].ShapeNo).ToList();
            //calculate XFeed
            toReturn.ForEach(e => e.XFeed = toReturn.IndexOf(e) == 0 
                ? e.XFeedFirstPosition : e.XPosition - toReturn[toReturn.IndexOf(e) - 1].XPosition);
            return toReturn;
        }
        private static IList<CutSequence> FindOptimalCutSequences(Job.Layer layer, IList<CutSequence> cutSequences)
        {
            int startShapeNo = ShapeMultiplicationFactor * layer.Shapes.Count / 2 + 1;
            int endShapeNo = startShapeNo + layer.Shapes.Count - 1;
            int startIndex = cutSequences.IndexOf(cutSequences.First(e => e.ToolSequences[0].ShapeNo == startShapeNo));
            int endIndex = cutSequences.IndexOf(cutSequences.Last(e => e.ToolSequences[0].ShapeNo == endShapeNo));
            var toReturn = cutSequences.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            var firstCutSequenceForShapeBeforeStartShapeNo = toReturn.FirstOrDefault(e => e.ToolSequences[0].ShapeNo == startShapeNo - 1);
            if (firstCutSequenceForShapeBeforeStartShapeNo != null)
                toReturn = toReturn.Skip(toReturn.IndexOf(firstCutSequenceForShapeBeforeStartShapeNo) + 1).ToList();

            int shapeNoNormalization = ((int)toReturn.Max(cs => cs.ToolSequences.Max(ts => ts.ShapeNo)) / layer.Shapes.Count) * layer.Shapes.Count;
            toReturn.ForEach(cs => ((List<CutSequence.ToolSequence>)cs.ToolSequences).ForEach(ts => ts.ShapeNo -= shapeNoNormalization));
            return toReturn;
        }

        private static IList<CutSequencesToolsCombination> GenerateCutSequencesToolsCombinations1(CutSequencesToolsCombination cutSequencesToolsComb,
            IEnumerable<IEnumerable<MachineTools.Tool>> combs, int shapesPerSequence, IEnumerable<MachineTools.Tool> unusedMobileTools)
        {
            List<CutSequencesToolsCombination> toReturn = new List<CutSequencesToolsCombination>();

            bool isApplied = false;
            foreach (var comb in combs)
            {
                var cutSequencesToolsCombClone = (CutSequencesToolsCombination)cutSequencesToolsComb.Clone();
                cutSequencesToolsCombClone = ApplyToolsCombination1(cutSequencesToolsCombClone, comb, shapesPerSequence, unusedMobileTools);

                if (cutSequencesToolsCombClone.CutSequences.Count < cutSequencesToolsComb.CutSequences.Count)
                {
                    isApplied = true;
                    cutSequencesToolsCombClone.ToolsComb.Add(comb.ToList());
                    toReturn.AddRange(GenerateCutSequencesToolsCombinations1(cutSequencesToolsCombClone, 
                        combs.Where(c => !c.SequenceEqual(comb)), shapesPerSequence, unusedMobileTools));
                }
            }
            if (!isApplied)
                toReturn.Add(cutSequencesToolsComb);
            return toReturn;
        }
        private static int ApplySingleToolsCombinationStartingFromSequence(CutSequencesToolsCombination cutSequencesToolsComb,
            IEnumerable<MachineTools.Tool> comb, int startSeqIndex, int targetSeqIndex, int shapesPerSequence, IEnumerable<MachineTools.Tool> unusedMobileTools)
        {
            int count = targetSeqIndex >= 0 && targetSeqIndex < cutSequencesToolsComb.CutSequences.Count ? cutSequencesToolsComb.CutSequences.Count : startSeqIndex + 1;
            for (int i = startSeqIndex; i < count; i++)
            {
                var srcToolSeqs = cutSequencesToolsComb.CutSequences[i].ToolSequences;
                if (srcToolSeqs.All(ts => ts.Tool.IsMobile) && srcToolSeqs.Count() < comb.Count())
                {
                    var srcUnusedToolSeqs = srcToolSeqs.Where(ts => unusedMobileTools.Contains(ts.Tool));
                    if (comb.Intersect(srcUnusedToolSeqs.Select(ts => ts.Tool)).Count() == srcUnusedToolSeqs.Count())
                    {
                        var srcUsedToolSeqs = srcToolSeqs.Except(srcUnusedToolSeqs);
                        var combUnmatchedTools = comb.Except(srcUnusedToolSeqs.Select(uts => uts.Tool));
                        if (srcUsedToolSeqs.All(ts => combUnmatchedTools.Any(t => t.Type == ts.Tool.Type)))
                        {
                            var maxPossMovement = srcUnusedToolSeqs.Count() > 0 ? srcUnusedToolSeqs.Min(ts => ts.Tool.XOffset - ts.XPos)
                                : srcUsedToolSeqs.Max(ts => combUnmatchedTools.Where(t => t.Type == ts.Tool.Type).Max(t => t.X - ts.Tool.X + t.XOffset - ts.XPos));
                            int j = i + 1;
                            var trgCutSequence = cutSequencesToolsComb.CutSequences[j % cutSequencesToolsComb.CutSequences.Count];
                            double distance = trgCutSequence.XFeed;

                            while (distance <= maxPossMovement)
                            {
                                if ((targetSeqIndex == -1 || (i == targetSeqIndex || j % cutSequencesToolsComb.CutSequences.Count == targetSeqIndex)) 
                                    && trgCutSequence.ToolSequences.Count == comb.Count() - srcToolSeqs.Count()
                                    && comb.Intersect(trgCutSequence.ToolSequences.Select(ts => ts.Tool)).Count() == trgCutSequence.ToolSequences.Count())
                                {
                                    var unmatchedTools = comb.Except(srcToolSeqs.Union(trgCutSequence.ToolSequences).Select(ts => ts.Tool));
                                    bool replaceTool = unmatchedTools.Count() == 1 && srcToolSeqs.Count() == 1
                                        && srcToolSeqs.First().Tool.Type == unmatchedTools.First().Type
                                        && unmatchedTools.First().XOffset >= srcToolSeqs.First().XPos
                                        && (distance - (unmatchedTools.First().X - srcToolSeqs.First().Tool.X)) >= 0
                                        && (distance - (unmatchedTools.First().X - srcToolSeqs.First().Tool.X)) <= unmatchedTools.First().XOffset;
                                    if ((unmatchedTools.Count() == 0 && distance <= srcToolSeqs.Min(ts => ts.Tool.XOffset - ts.XPos)) || replaceTool)
                                    {
                                        if (replaceTool)
                                        {
                                            //replace source tool
                                            distance -= unmatchedTools.First().X - srcToolSeqs.First().Tool.X;
                                            srcToolSeqs.First().Tool = unmatchedTools.First();
                                        }
                                        //adjust xfeed for next cut sequence relative to source cut sequence
                                        cutSequencesToolsComb.CutSequences[(i + 1) % cutSequencesToolsComb.CutSequences.Count].XFeed += cutSequencesToolsComb.CutSequences[i].XFeed;
                                        //adjust source tool sequence xpos
                                        ((List<CutSequence.ToolSequence>)cutSequencesToolsComb.CutSequences[i].ToolSequences).ForEach(ts => ts.XPos += distance);
                                        //adjust source tool sequences shapeNo
                                        ((List<CutSequence.ToolSequence>)cutSequencesToolsComb.CutSequences[i].ToolSequences)
                                            .ForEach(ts => ts.ShapeNo -= ((int)(j / cutSequencesToolsComb.CutSequences.Count)) * shapesPerSequence);
                                        //move source tool sequences on target cut sequences
                                        ((List<CutSequence.ToolSequence>)trgCutSequence.ToolSequences).InsertRange(0, cutSequencesToolsComb.CutSequences[i].ToolSequences);
                                        //remove source cut sequence
                                        cutSequencesToolsComb.CutSequences.Remove(cutSequencesToolsComb.CutSequences[i]);
                                        return cutSequencesToolsComb.CutSequences.IndexOf(trgCutSequence);
                                    }
                                }
                                j++;
                                trgCutSequence = cutSequencesToolsComb.CutSequences[j % cutSequencesToolsComb.CutSequences.Count];
                                distance += trgCutSequence.XFeed;
                            };
                        }
                    }
                }
            }
            return -1;
        }
        private static CutSequencesToolsCombination ApplyToolsCombination1(CutSequencesToolsCombination cutSequencesToolsComb, IEnumerable<MachineTools.Tool> comb,
            int shapesPerSequence, IEnumerable<MachineTools.Tool> unusedMobileTools)
        {
            int i = 0;
            do
            {
                int targetSeqIndex = -1;
                var cutSequencesToolsCombClone = (CutSequencesToolsCombination)cutSequencesToolsComb.Clone();
                int j = 2;
                do
                {
                    targetSeqIndex = ApplySingleToolsCombinationStartingFromSequence(cutSequencesToolsCombClone, comb.Take(j), j == 2 ? i : 0,
                        targetSeqIndex, shapesPerSequence, unusedMobileTools);
                } while (++j <= comb.Count() && targetSeqIndex != -1);

                if (targetSeqIndex != -1)
                    cutSequencesToolsComb = cutSequencesToolsCombClone;
                else
                    i++;
            } while (i < cutSequencesToolsComb.CutSequences.Count);
            return cutSequencesToolsComb;
        }

        private static void ApplyToolsCombination(CutSequencesToolsCombination cutSequencesToolsComb, IEnumerable<MachineTools.Tool> comb, 
            int shapesPerSequence, IEnumerable<MachineTools.Tool> unusedMobileTools)
        {
            for (int i = 0; i < cutSequencesToolsComb.CutSequences.Count; i++)
            {
                var srcToolSeqs = cutSequencesToolsComb.CutSequences[i].ToolSequences;
                if (srcToolSeqs.All(ts => ts.Tool.IsMobile) && srcToolSeqs.Count() < comb.Count())
                {
                    var srcUnusedToolSeqs = srcToolSeqs.Where(ts => unusedMobileTools.Contains(ts.Tool));
                    if (comb.Intersect(srcUnusedToolSeqs.Select(ts => ts.Tool)).Count() == srcUnusedToolSeqs.Count())
                    {
                        var srcUsedToolSeqs = srcToolSeqs.Except(srcUnusedToolSeqs);
                        var combUnmatchedTools = comb.Except(srcUnusedToolSeqs.Select(uts => uts.Tool));
                        if (srcUsedToolSeqs.All(ts => combUnmatchedTools.Any(t => t.Type == ts.Tool.Type)))
                        {
                            var maxPossMovement = srcUnusedToolSeqs.Count() > 0 ? srcUnusedToolSeqs.Min(ts => ts.Tool.XOffset - ts.XPos)
                                : srcUsedToolSeqs.Max(ts => combUnmatchedTools.Where(t => t.Type == ts.Tool.Type).Max(t => t.X - ts.Tool.X + t.XOffset - ts.XPos));
                            int j = i + 1;
                            var trgCutSequence = cutSequencesToolsComb.CutSequences[j % cutSequencesToolsComb.CutSequences.Count];
                            double distance = trgCutSequence.XFeed;

                            while (distance <= maxPossMovement)
                            {
                                if (trgCutSequence.ToolSequences.Count == comb.Count() - srcToolSeqs.Count()
                                    && comb.Intersect(trgCutSequence.ToolSequences.Select(ts => ts.Tool)).Count() == trgCutSequence.ToolSequences.Count())
                                {
                                    var unmatchedTools = comb.Except(srcToolSeqs.Union(trgCutSequence.ToolSequences).Select(ts => ts.Tool));
                                    bool replaceTool = unmatchedTools.Count() == 1 && srcToolSeqs.Count() == 1
                                        && srcToolSeqs.First().Tool.Type == unmatchedTools.First().Type
                                        && unmatchedTools.First().XOffset >= srcToolSeqs.First().XPos
                                        && (distance - (unmatchedTools.First().X - srcToolSeqs.First().Tool.X)) >= 0
                                        && (distance - (unmatchedTools.First().X - srcToolSeqs.First().Tool.X)) <= unmatchedTools.First().XOffset;
                                    if ((unmatchedTools.Count() == 0 && distance <= srcToolSeqs.Min(ts => ts.Tool.XOffset - ts.XPos)) || replaceTool)
                                    {
                                        if (replaceTool)
                                        {
                                            //replace source tool
                                            distance -= unmatchedTools.First().X - srcToolSeqs.First().Tool.X;
                                            srcToolSeqs.First().Tool = unmatchedTools.First();
                                        }
                                        //adjust xfeed for next cut sequence relative to source cut sequence
                                        cutSequencesToolsComb.CutSequences[(i + 1) % cutSequencesToolsComb.CutSequences.Count].XFeed += cutSequencesToolsComb.CutSequences[i].XFeed;
                                        //adjust source tool sequence xpos
                                        ((List<CutSequence.ToolSequence>)cutSequencesToolsComb.CutSequences[i].ToolSequences).ForEach(ts => ts.XPos += distance);
                                        //adjust source tool sequences shapeNo
                                        ((List<CutSequence.ToolSequence>)cutSequencesToolsComb.CutSequences[i].ToolSequences)
                                            .ForEach(ts => ts.ShapeNo -= ((int)(j / cutSequencesToolsComb.CutSequences.Count)) * shapesPerSequence);
                                        //move source tool sequences on target cut sequences
                                        ((List<CutSequence.ToolSequence>)trgCutSequence.ToolSequences).InsertRange(0, cutSequencesToolsComb.CutSequences[i].ToolSequences);
                                        //remove source cut sequence
                                        cutSequencesToolsComb.CutSequences.Remove(cutSequencesToolsComb.CutSequences[i]);
                                        i--;
                                        break;
                                    }
                                }
                                j++;
                                trgCutSequence = cutSequencesToolsComb.CutSequences[j % cutSequencesToolsComb.CutSequences.Count];
                                distance += trgCutSequence.XFeed;
                            };
                        }
                    }
                }
            }
        }
        private static IList<CutSequencesToolsCombination> GenerateCutSequencesToolsCombinations(CutSequencesToolsCombination cutSequencesToolsComb, 
            IEnumerable<IEnumerable<MachineTools.Tool>> combs, int shapesPerSequence, IEnumerable<MachineTools.Tool> unusedMobileTools)
        {
            List<CutSequencesToolsCombination> toReturn = new List<CutSequencesToolsCombination>();

            bool isApplied = false;
            foreach (var comb in combs)
            {
                var cutSequencesToolsCombClone = (CutSequencesToolsCombination)cutSequencesToolsComb.Clone();
                ApplyToolsCombination(cutSequencesToolsCombClone, comb, shapesPerSequence, unusedMobileTools);
                if (cutSequencesToolsCombClone.CutSequences.Count < cutSequencesToolsComb.CutSequences.Count)
                {
                    isApplied = true;
                    cutSequencesToolsCombClone.ToolsComb.Add(comb.ToList());
                    toReturn.AddRange(GenerateCutSequencesToolsCombinations(cutSequencesToolsCombClone, combs.Where(c => !c.SequenceEqual(comb)), shapesPerSequence, unusedMobileTools));
                }
            }
            if (!isApplied)
                toReturn.Add(cutSequencesToolsComb);
            return toReturn;
        }
        private static IList<CutSequencesToolsCombination> FindOptimalCutSequencesToolsCombination(IList<CutSequencesToolsCombination> cutSequencesToolsCombs)
        {
            //filter by min size
            var filteredCutSecCombs = cutSequencesToolsCombs.Where(e => e.CutSequences.Count() == cutSequencesToolsCombs.Min(e1 => e1.CutSequences.Count));
            //filter by min distance between shapes
            filteredCutSecCombs = filteredCutSecCombs.OrderBy(e => e.CutSequences.Max(cs => cs.ToolSequences.Max(ts => ts.ShapeNo) - cs.ToolSequences.Min(ts => ts.ShapeNo)));
            //filter by min number of tool movements along x axis
            filteredCutSecCombs = filteredCutSecCombs.GroupBy(e => e.CutSequences.Sum(cs => cs.ToolSequences.Count(ts => ts.XPos != 0))).OrderBy(g => g.Key).First().ToList();
            return filteredCutSecCombs.ToList();
        }
        private static Job ConvertToJob(JobCutSequences jobCutSequences)
        {
            Job toReturn = new Job() { Name = jobCutSequences.Name, Layers = new List<Job.Layer>() };

            foreach (var layerCutSequences in jobCutSequences.LayerCutSequences)
            {
                Job.Layer toAdd = new Job.Layer()
                {
                    Id = layerCutSequences.LayerId,
                    ShapesPerProduct = layerCutSequences.ShapesPerProduct,
                    Shapes = new List<Job.Shape>()
                };

                double distance = 0;
                bool firstStrokeDone = false;
                int iteration = 0;
                while (layerCutSequences.OptimalCutSequencesToolsComb.CutSequences.Min(cs => cs.ToolSequences.Min(ts => ts.ShapeNo)) 
                    + iteration * layerCutSequences.ShapesPerSequence <= layerCutSequences.ShapesPerSequence)
                {
                    foreach (var cutSequence in layerCutSequences.OptimalCutSequencesToolsComb.CutSequences)
                    {
                        if (firstStrokeDone)
                            distance += cutSequence.XFeed;
                        foreach (var toolSequence in cutSequence.ToolSequences)
                        {
                            int numberOfCutShapeParts = ((CutsOff)toolSequence.Tool.Type.GetType().GetField(toolSequence.Tool.Type.ToString())
                                    .GetCustomAttribute(typeof(CutsOff))).Value ? 2 : 1;
                            for (int i = 0; i < numberOfCutShapeParts; i++)
                            {
                                if (toolSequence.ShapeNo + iteration * layerCutSequences.ShapesPerSequence + i >= 1
                                    && toolSequence.ShapeNo + iteration * layerCutSequences.ShapesPerSequence + i <= layerCutSequences.ShapesPerSequence)
                                {
                                    if (toAdd.Shapes.Count == 0)
                                    {
                                        distance += cutSequence.XFeedFirstPosition;
                                        firstStrokeDone = true;
                                    }

                                    var shape = toAdd.Shapes.FirstOrDefault(s => s.No == toolSequence.ShapeNo + iteration * layerCutSequences.ShapesPerSequence + i);

                                    if (shape == null)
                                    {
                                        shape = new Job.Shape() { No = toolSequence.ShapeNo + iteration * layerCutSequences.ShapesPerSequence + i, Parts = new List<Job.ShapePart>() };
                                        toAdd.Shapes.Add(shape);
                                    }

                                    shape.Parts.Add(new Job.ShapePart()
                                    {
                                        Feature = toolSequence.Feature,
                                        X = distance - (toolSequence.Tool.X + toolSequence.XPos),
                                        Y = toolSequence.YPos
                                    });
                                }
                            }
                        }
                    }
                    iteration++;
                }
                //sort shape by No
                ((List<Job.Shape>)toAdd.Shapes).Sort((x, y) => x.No.CompareTo(y.No));
                //sort shape parts by X
                ((List<Job.Shape>)toAdd.Shapes).ForEach(s => ((List<Job.ShapePart>)s.Parts).Sort((x, y) => x.X.CompareTo(y.X)));
                //shift shapes by X in order to start from X = 0 
                foreach (var shape in toAdd.Shapes)
                {
                    if (shape.Parts.Count > 0)
                    {
                        var shift = shape.Parts.First().X;
                        ((List<Job.Shape>)toAdd.Shapes).Where((s, index) => index >= toAdd.Shapes.IndexOf(shape)).ToList()
                            .ForEach(s => ((List<Job.ShapePart>)s.Parts).ForEach(sp => sp.X -= shift));
                    }
                }
                toReturn.Layers.Add(toAdd);
            }
            
            return toReturn;
        }

        private static void ApplyToolsNextPositions(CutSequencesToolsCombination cutSequencesToolsComb)
        {
            for (int i = 0; i < cutSequencesToolsComb.CutSequences.Count; i++)
            {
                foreach (var toolSequence in cutSequencesToolsComb.CutSequences.ElementAt(i).ToolSequences)
                {
                    if (toolSequence.Tool.IsMobile)
                    {
                        var nextCutSequence = cutSequencesToolsComb.CutSequences.Where((cs, index) => index > i)
                            .FirstOrDefault(cs => cs.ToolSequences.Any(ts => ts.Tool == toolSequence.Tool));
                        if (nextCutSequence == null && i > 0)
                            nextCutSequence = cutSequencesToolsComb.CutSequences.Where((cs, index) => index >= 0 && index < i)
                            .FirstOrDefault(cs => cs.ToolSequences.Any(ts => ts.Tool == toolSequence.Tool));
                        if (nextCutSequence != null)
                        {
                            toolSequence.NextXPos = nextCutSequence.ToolSequences.First(ts => ts.Tool == toolSequence.Tool).XPos;
                            toolSequence.NextYPos = nextCutSequence.ToolSequences.First(ts => ts.Tool == toolSequence.Tool).YPos;
                        }
                    }
                }
            }
        }
        public static JobCutSequences GenerateCutSequences(String productFilePath)
        {
            Product product = LoadProduct(productFilePath);
            Job job = ConvertToJob(product);
            job.Name = Path.GetFileName(productFilePath);
            JobCutSequences toReturn = new JobCutSequences() { Name = job.Name, LayerCutSequences = new List<LayerCutSequences>() };
            MachineTools machineTools = LoadMachineTools();

            //generate tools combinations that include at most 1 non mobile tool in combination and sort by size asc then sort by mobile desc
            var toolsCombs = CombinationGenerator.Combinations(machineTools.Tools, 2, machineTools.Tools.Count(e => e.IsMobile) + 1)
                .Where(c => c.Count(t => !t.IsMobile) <= 1).OrderBy(c => c.Count()).ThenByDescending(c => c.Count(t => t.IsMobile));

            foreach (var layer in job.Layers)
            {
                //generate cut sequences 
                var cutSequences = GenerateCutSequences(layer, machineTools);

                //find optimal cut sequences 
                var optimalCutSequences = FindOptimalCutSequences(layer, cutSequences);

                //find unusedTools
                var unusedMobileTools = machineTools.Tools.Where(t => t.IsMobile && !optimalCutSequences.Any(cs => cs.ToolSequences.Any(ts => ts.Tool == t)));

                //generate cut sequences tools combinations
                var cutSequencesToolsCombs = GenerateCutSequencesToolsCombinations1(
                    new CutSequencesToolsCombination() { CutSequences = optimalCutSequences, ToolsComb = new List<List<MachineTools.Tool>>() },
                    toolsCombs.OrderByDescending(c => c.Count()), layer.Shapes.Count, unusedMobileTools);

                ////generate cut sequences tools combinations
                //var cutSequencesToolsCombs = GenerateCutSequencesToolsCombinations(
                //    new CutSequencesToolsCombination() { CutSequences = optimalCutSequences, ToolsComb = new List<List<MachineTools.Tool>>() },
                //    toolsCombs, layer.Shapes.Count, unusedMobileTools);

                //find optimal cut sequences tool combinations
                var optimalCutSequencesToolsCombs = FindOptimalCutSequencesToolsCombination(cutSequencesToolsCombs);

                var optimalCutSequencesToolsComb = optimalCutSequencesToolsCombs.First();

                //apply tools next positions
                ApplyToolsNextPositions(optimalCutSequencesToolsComb);

                toReturn.LayerCutSequences.Add(new LayerCutSequences()
                {
                    LayerId = layer.Id,
                    ShapesPerSequence = layer.Shapes.Count,
                    ShapesPerProduct = layer.ShapesPerProduct,
                    CutSequences = cutSequences,
                    OptimalCutSequences = optimalCutSequences,
                    CutSequencesToolsCombs = cutSequencesToolsCombs,
                    OptimalCutSequencesToolsComb = optimalCutSequencesToolsComb
                }); 
            }
            toReturn.Valid = job.CompareTo(ConvertToJob(toReturn)) == 0 ? true : false;
            CsvUtils.SaveJobCutSequencesToCsv(toReturn, Path.GetDirectoryName(productFilePath));
            
            return toReturn;
        }
        
    }
}
