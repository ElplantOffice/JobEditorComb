
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductLib
{
  public class StepLap : ICloneable
  {
    [XmlArray(ElementName = "Steps")]
    [XmlArrayItem(ElementName = "Step")]
    public List<Step> Steps = new List<Step>();

    [XmlAttribute("Type")]
    public EStepLapType Type { get; set; }

    [XmlAttribute("NumberOfSteps")]
    public int NumberOfSteps { get; set; }

    [XmlAttribute("NumberOfSame")]
    public int NumberOfSame { get; set; }

    [XmlAttribute("Value")]
    public double Value { get; set; }

    public object Clone()
    {
      StepLap stepLap = new StepLap();
      stepLap.Type = this.Type;
      stepLap.NumberOfSteps = this.NumberOfSteps;
      stepLap.NumberOfSame = this.NumberOfSame;
      stepLap.Value = this.Value;
      foreach (Step step in this.Steps)
        stepLap.Steps.Add((Step) step.Clone());
      return (object) stepLap;
    }
  }
}
