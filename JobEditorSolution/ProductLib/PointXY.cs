
using System;

namespace ProductLib
{
  public class PointXY : ICloneable
  {
    private double x;
    private double y;
    private EStepLapType eSLType;
    private int pointPartIndex;
    private int partIndex;

    public PointXY(double x, double y, int pointPartIndex)
    {
      this.x = x;
      this.y = y;
      this.pointPartIndex = pointPartIndex;
    }

    public double X
    {
      get
      {
        return this.x;
      }
      set
      {
        this.x = value;
      }
    }

    public double Y
    {
      get
      {
        return this.y;
      }
      set
      {
        this.y = value;
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

    public int PointPartIndex
    {
      get
      {
        return this.pointPartIndex;
      }
      set
      {
        this.pointPartIndex = value;
      }
    }

    public int PartIndex
    {
      get
      {
        return this.partIndex;
      }
      set
      {
        this.partIndex = value;
      }
    }

    public object Clone()
    {
      return (object) new PointXY(this.x, this.y, this.pointPartIndex);
    }
  }
}
