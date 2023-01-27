
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class ShapePart : ICloneable
  {
    private TipCut tipCut = new TipCut();
    private StepLap stepLap = new StepLap();

    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("X")]
    public double X { get; set; }

    [XmlAttribute("Y")]
    public double Y { get; set; }

    [XmlAttribute("Feature")]
    public EFeature Feature { get; set; }

    [XmlAttribute("OverCut")]
    public double OverCut { get; set; }

    [XmlAttribute("DoubleCut")]
    public bool DoubleCut { get; set; }

    [XmlElement(ElementName = "TipCut")]
    public TipCut TipCut
    {
      get
      {
        return this.tipCut;
      }
      set
      {
        this.tipCut = value;
      }
    }

    [XmlElement(ElementName = "StepLap")]
    public StepLap StepLap
    {
      get
      {
        return this.stepLap;
      }
      set
      {
        this.stepLap = value;
      }
    }

    public bool Last { get; set; }

    public object Clone()
    {
      return (object) new ShapePart()
      {
        TipCut = (TipCut) this.TipCut.Clone(),
        StepLap = (StepLap) this.StepLap.Clone(),
        Id = this.Id,
        X = this.X,
        Y = this.Y,
        Feature = this.Feature,
        OverCut = this.OverCut,
        DoubleCut = this.DoubleCut
      };
    }
  }
}
