
using System.Collections.Generic;

namespace ProductLib
{
  public static class ProjectConstants
  {
    public static readonly IDictionary<EToolCutSequence, IList<PointXY>> CutSequenceModel = (IDictionary<EToolCutSequence, IList<PointXY>>) new Dictionary<EToolCutSequence, IList<PointXY>>();

    static ProjectConstants()
    {
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.S90, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(2.0, 0.0, 0),
        new PointXY(2.0, 2.0, 1),
        new PointXY(2.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.S135, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(0.0, 0.0, 0),
        new PointXY(2.0, 2.0, 1),
        new PointXY(4.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.S45, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(4.0, 0.0, 0),
        new PointXY(2.0, 2.0, 1),
        new PointXY(0.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.VB, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(0.0, 0.0, 0),
        new PointXY(2.0, 2.0, 1),
        new PointXY(4.0, 0.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.VF, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(0.0, 4.0, 0),
        new PointXY(2.0, 2.0, 1),
        new PointXY(4.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.VBS, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(1.0, 0.0, 0),
        new PointXY(2.0, 1.0, 1),
        new PointXY(3.0, 0.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.VFS, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(1.0, 4.0, 0),
        new PointXY(2.0, 3.0, 1),
        new PointXY(3.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.Cl, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(4.0, 0.0, 0),
        new PointXY(2.0, 2.0, 1),
        new PointXY(4.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.Cr, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(0.0, 4.0, 0),
        new PointXY(2.0, 2.0, 1),
        new PointXY(0.0, 0.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.CBl, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(2.0, 0.0, 0),
        new PointXY(1.0, 1.0, 1),
        new PointXY(4.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.CBr, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(0.0, 4.0, 0),
        new PointXY(3.0, 1.0, 1),
        new PointXY(2.0, 0.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.CFl, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(4.0, 0.0, 0),
        new PointXY(1.0, 3.0, 1),
        new PointXY(2.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.CFr, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(2.0, 4.0, 0),
        new PointXY(3.0, 3.0, 1),
        new PointXY(0.0, 0.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.S45_TCl, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(4.0, 0.0, 0),
        new PointXY(1.0, 3.0, 1),
        new PointXY(1.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.S45_TCr, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(3.0, 0.0, 0),
        new PointXY(3.0, 1.0, 1),
        new PointXY(0.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.S135_TCl, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(1.0, 0.0, 0),
        new PointXY(1.0, 1.0, 1),
        new PointXY(4.0, 4.0, 2)
      });
      ProjectConstants.CutSequenceModel.Add(EToolCutSequence.S135_TCr, (IList<PointXY>) new List<PointXY>()
      {
        new PointXY(0.0, 0.0, 0),
        new PointXY(3.0, 3.0, 1),
        new PointXY(3.0, 4.0, 2)
      });
    }
  }
}
