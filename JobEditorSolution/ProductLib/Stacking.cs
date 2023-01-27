
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class Stacking : ICloneable
  {
    [XmlAttribute("AltStepLapOffset")]
    public double AltStepLapOffset { get; set; }

    [XmlAttribute("AltStepLapNumOfSame")]
    public double AltStepLapNumOfSame { get; set; }

    [XmlAttribute("AltSheetOffset")]
    public double AltSheetOffset { get; set; }

    [XmlAttribute("AltSheetNumOfSame")]
    public double AltSheetNumOfSame { get; set; }

    public object Clone()
    {
      Stacking stacking = new Stacking()
      {
        AltStepLapOffset = this.AltStepLapOffset,
        AltStepLapNumOfSame = this.AltStepLapNumOfSame,
        AltSheetOffset = this.AltSheetOffset
      };
      stacking.AltStepLapNumOfSame = this.AltStepLapNumOfSame;
      return (object) stacking;
    }
  }
}
