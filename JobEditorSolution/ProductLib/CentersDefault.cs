
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class CentersDefault : ICloneable
  {
    [XmlAttribute("OverCut")]
    public double OverCut { get; set; }

    [XmlAttribute("DoubleCut")]
    public bool DoubleCut { get; set; }

    [XmlAttribute("VOffset")]
    public double VOffset { get; set; }

    [XmlAttribute("CTipOffset")]
    public double CTipOffset { get; set; }

    public CentersDefault()
    {
    }

    public CentersDefault(double overCut, bool doubleCut, double vOffset, double cTipOffset)
    {
      this.OverCut = overCut;
      this.DoubleCut = doubleCut;
      this.VOffset = vOffset;
      this.CTipOffset = cTipOffset;
    }

    public object Clone()
    {
      return (object) new CentersDefault(this.OverCut, this.DoubleCut, this.VOffset, this.CTipOffset);
    }
  }
}
