
using System;
using System.Xml.Serialization;

namespace ProductLib
{
  public class HolesInfo : ICloneable
  {
    public HolesInfo()
    {
      this.NumberOfHoles = 0;
      this.MeasuringType = EMeasuringType.Absolute;
      this.Shape1 = EHoleShape.Round;
      this.Shape2 = EHoleShape.Round;
      this.Shape3 = EHoleShape.Round;
      this.Shape4 = EHoleShape.Round;
      this.Shape5 = EHoleShape.Round;
      this.Shape6 = EHoleShape.Round;
      this.Shape7 = EHoleShape.Round;
      this.Shape8 = EHoleShape.Round;
      this.Shape9 = EHoleShape.Round;
      this.Shape10 = EHoleShape.Round;
      this.Length1 = 0.0;
      this.Length2 = 0.0;
      this.Length3 = 0.0;
      this.Length4 = 0.0;
      this.Length5 = 0.0;
      this.Length6 = 0.0;
      this.Length7 = 0.0;
      this.Length8 = 0.0;
      this.Length9 = 0.0;
      this.Length10 = 0.0;
    }

    [XmlAttribute("NumberOfHoles")]
    public int NumberOfHoles { get; set; }

    [XmlAttribute("MeasuringType")]
    public EMeasuringType MeasuringType { get; set; }

    [XmlAttribute("Shape1")]
    public EHoleShape Shape1 { get; set; }

    [XmlAttribute("Shape2")]
    public EHoleShape Shape2 { get; set; }

    [XmlAttribute("Shape3")]
    public EHoleShape Shape3 { get; set; }

    [XmlAttribute("Shape4")]
    public EHoleShape Shape4 { get; set; }

    [XmlAttribute("Shape5")]
    public EHoleShape Shape5 { get; set; }

    [XmlAttribute("Shape6")]
    public EHoleShape Shape6 { get; set; }

    [XmlAttribute("Shape7")]
    public EHoleShape Shape7 { get; set; }

    [XmlAttribute("Shape8")]
    public EHoleShape Shape8 { get; set; }

    [XmlAttribute("Shape9")]
    public EHoleShape Shape9 { get; set; }

    [XmlAttribute("Shape10")]
    public EHoleShape Shape10 { get; set; }

    [XmlAttribute("Length1")]
    public double Length1 { get; set; }

    [XmlAttribute("Length2")]
    public double Length2 { get; set; }

    [XmlAttribute("Length3")]
    public double Length3 { get; set; }

    [XmlAttribute("Length4")]
    public double Length4 { get; set; }

    [XmlAttribute("Length5")]
    public double Length5 { get; set; }

    [XmlAttribute("Length6")]
    public double Length6 { get; set; }

    [XmlAttribute("Length7")]
    public double Length7 { get; set; }

    [XmlAttribute("Length8")]
    public double Length8 { get; set; }

    [XmlAttribute("Length9")]
    public double Length9 { get; set; }

    [XmlAttribute("Length10")]
    public double Length10 { get; set; }

    public object Clone()
    {
      return (object) new HolesInfo()
      {
        NumberOfHoles = this.NumberOfHoles,
        MeasuringType = this.MeasuringType,
        Shape1 = this.Shape1,
        Shape2 = this.Shape2,
        Shape3 = this.Shape3,
        Shape4 = this.Shape4,
        Shape5 = this.Shape5,
        Shape6 = this.Shape6,
        Shape7 = this.Shape7,
        Shape8 = this.Shape8,
        Shape9 = this.Shape9,
        Shape10 = this.Shape10,
        Length1 = this.Length1,
        Length2 = this.Length2,
        Length3 = this.Length3,
        Length4 = this.Length4,
        Length5 = this.Length5,
        Length6 = this.Length6,
        Length7 = this.Length7,
        Length8 = this.Length8,
        Length9 = this.Length9,
        Length10 = this.Length10
      };
    }
  }
}
