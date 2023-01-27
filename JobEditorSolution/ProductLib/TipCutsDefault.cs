
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class TipCutsDefault : ICloneable
  {
    [XmlAttribute("Height")]
    public double Height { get; set; }

    [XmlAttribute("OverCut")]
    public double OverCut { get; set; }

    [XmlAttribute("DoubleCut")]
    public bool DoubleCut { get; set; }

    public object Clone()
    {
      return (object) new TipCutsDefault()
      {
        Height = this.Height,
        OverCut = this.OverCut,
        DoubleCut = this.DoubleCut
      };
    }
  }
}
