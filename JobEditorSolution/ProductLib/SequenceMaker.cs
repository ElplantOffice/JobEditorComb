
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductLib
{
  [XmlRoot("SequenceMaker"), Serializable]
  public class SequenceMaker
  {
    [XmlArray(ElementName = "CutSequences"), XmlArrayItem(ElementName = "CutSequence")]
    public List<CutSequence> CutSequences = new List<CutSequence>();
  }
}
