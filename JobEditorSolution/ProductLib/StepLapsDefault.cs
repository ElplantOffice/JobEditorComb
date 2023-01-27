
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class StepLapsDefault : ICloneable
  {
    [XmlAttribute("NumberOfSteps")]
    public int NumberOfSteps { get; set; }

    [XmlAttribute("NumberOfSame")]
    public int NumberOfSame { get; set; }

    [XmlAttribute("Value")]
    public double Value { get; set; }

    public StepLapsDefault()
    {
    }

    public StepLapsDefault(int nmbrSteps, int nmbrSame, double stepValue)
    {
      this.NumberOfSteps = nmbrSteps;
      this.NumberOfSame = nmbrSame;
      this.Value = stepValue;
    }

    public object Clone()
    {
      return (object) new StepLapsDefault(this.NumberOfSteps, this.NumberOfSame, this.Value);
    }
  }
}
