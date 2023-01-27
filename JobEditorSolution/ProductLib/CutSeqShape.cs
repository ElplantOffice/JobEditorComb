
using System;
using System.Collections.Generic;

namespace ProductLib
{
  public class CutSeqShape : ICloneable
  {
    private IList<CutSeqShapePart> parts;
    private CutSeqShapeHole shapeHole;
    private CutSeqShapeSlot shapeSlot;
    private CutSeqShapeCenter shapeCenter;
    private bool tipCutStart;
    private bool tipCutEnd;

    public CutSeqShape()
    {
      this.parts = (IList<CutSeqShapePart>) new List<CutSeqShapePart>();
      this.tipCutStart = false;
      this.tipCutEnd = false;
    }

    public CutSeqShapeHole ShapeHole
    {
      get
      {
        return this.shapeHole;
      }
      set
      {
        this.shapeHole = value;
      }
    }

    public CutSeqShapeSlot ShapeSlot
    {
      get
      {
        return this.shapeSlot;
      }
      set
      {
        this.shapeSlot = value;
      }
    }

    public CutSeqShapeCenter ShapeCenter
    {
      get
      {
        return this.shapeCenter;
      }
      set
      {
        this.shapeCenter = value;
      }
    }

    public bool TipCatStart
    {
      get
      {
        return this.tipCutStart;
      }
      set
      {
        this.tipCutStart = value;
      }
    }

    public bool TipCatEnd
    {
      get
      {
        return this.tipCutEnd;
      }
      set
      {
        this.tipCutEnd = value;
      }
    }

    public IList<CutSeqShapePart> Parts
    {
      get
      {
        return this.parts;
      }
      set
      {
        this.parts = value;
      }
    }

    public object Clone()
    {
      CutSeqShape cutSeqShape = new CutSeqShape();
      cutSeqShape.ShapeCenter = this.ShapeCenter;
      cutSeqShape.ShapeHole = this.ShapeHole;
      cutSeqShape.ShapeSlot = this.ShapeSlot;
      cutSeqShape.TipCatStart = this.TipCatStart;
      cutSeqShape.TipCatEnd = this.TipCatEnd;
      cutSeqShape.Parts = (IList<CutSeqShapePart>) new List<CutSeqShapePart>();
      foreach (CutSeqShapePart part in (IEnumerable<CutSeqShapePart>) this.Parts)
        cutSeqShape.Parts.Add((CutSeqShapePart) part.Clone());
      return (object) cutSeqShape;
    }
  }
}
