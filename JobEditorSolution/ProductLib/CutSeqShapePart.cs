
using System;
using System.Collections.Generic;

namespace ProductLib
{
  public class CutSeqShapePart : ICloneable
  {
    private IList<PointXY> points;
    private EStepLapType eSLType;
    private EToolCutSequence eShapePart;
    private int sourceIndex;

    public CutSeqShapePart()
    {
      this.points = (IList<PointXY>) new List<PointXY>();
    }

    public IList<PointXY> Points
    {
      get
      {
        return this.points;
      }
      set
      {
        this.points = value;
      }
    }

    public EStepLapType ESLType
    {
      get
      {
        return this.eSLType;
      }
      set
      {
        this.eSLType = value;
      }
    }

    public EToolCutSequence EShapePart
    {
      get
      {
        return this.eShapePart;
      }
      set
      {
        this.eShapePart = value;
      }
    }

    public int SourceIndex
    {
      get
      {
        return this.sourceIndex;
      }
      set
      {
        this.sourceIndex = value;
      }
    }

    public object Clone()
    {
      CutSeqShapePart cutSeqShapePart = new CutSeqShapePart();
      cutSeqShapePart.EShapePart = this.EShapePart;
      cutSeqShapePart.ESLType = this.ESLType;
      cutSeqShapePart.SourceIndex = this.SourceIndex;
      cutSeqShapePart.Points = (IList<PointXY>) new List<PointXY>();
      foreach (PointXY point in (IEnumerable<PointXY>) this.Points)
        cutSeqShapePart.Points.Add((PointXY) point.Clone());
      return (object) cutSeqShapePart;
    }
  }
}
