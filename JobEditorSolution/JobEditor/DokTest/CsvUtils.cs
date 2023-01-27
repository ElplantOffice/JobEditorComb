using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static JobEditor.DokTest.CutSequence;

namespace JobEditor.DokTest
{
    internal class CsvUtils
    {
        private static String GetCsvLines(IList<CutSequence> cutSequences)
        {
            PropertyInfo[] toolSeqProperties = typeof(ToolSequence).GetProperties();
            PropertyInfo[] cutSeqProperties = typeof(CutSequence).GetProperties();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(",", cutSeqProperties.Select(p => p.Name)));

            foreach (var cutSeq in cutSequences)
            {
                var cutSeqValues = cutSeqProperties.Select(csp => csp.PropertyType == typeof(IList<ToolSequence>)
                    ? string.Join(" ", toolSeqProperties.Select(tsp => tsp.Name + ": " + string.Join(";", cutSeq.ToolSequences
                    .Select(ts => tsp.PropertyType == typeof(MachineTools.Tool) ? ts.Tool.Name : tsp.GetValue(ts))))) : csp.GetValue(cutSeq));

                sb.AppendLine(string.Join(",", cutSeqValues));
            }
            return sb.ToString();
        }
        public static void SaveJobCutSequencesToCsv(JobCutSequences jobCutSequences, string dirPath)
        {
            foreach (var layerCutSequences in jobCutSequences.LayerCutSequences)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Valid: " + jobCutSequences.Valid).AppendLine();
                sb.Append("Optimal CutSequences Tools Combination: ")
                    .AppendLine(string.Join("; ", layerCutSequences.OptimalCutSequencesToolsComb.ToolsComb.Select(e => string.Join("", e.Select(e1 => e1.Name)))));
                

                sb.AppendLine(GetCsvLines(layerCutSequences.OptimalCutSequencesToolsComb.CutSequences));

                sb.AppendLine("Optimal CutSequences");
                sb.AppendLine(GetCsvLines(layerCutSequences.OptimalCutSequences));

                for (int i = 0; i < layerCutSequences.CutSequencesToolsCombs.Count; i++)
                {
                    sb.Append("Optimal CutSequences Tools Combination ").Append(i + 1).Append(": ")
                        .AppendLine(string.Join("; ", layerCutSequences.CutSequencesToolsCombs[i].ToolsComb.Select(e => string.Join("", e.Select(e1 => e1.Name)))));
                    sb.AppendLine(GetCsvLines(layerCutSequences.CutSequencesToolsCombs[i].CutSequences));
                }

                sb.AppendLine("CutSequences");
                sb.AppendLine(GetCsvLines(layerCutSequences.CutSequences));

                File.WriteAllText(string.Format("{0}/{1}.Layer.{2}.csv", dirPath, jobCutSequences.Name, layerCutSequences.LayerId), sb.ToString());
            }
        }
    }
}
