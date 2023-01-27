
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductLib
{
  public class Shape : ICloneable
  {
    private Stacking stacking = new Stacking();
    private HolesInfo holesInfo = new HolesInfo();
    private SlotsInfo slotsInfo = new SlotsInfo();
    private CentersInfo centersInfo = new CentersInfo();
    [XmlArray(ElementName = "Holes")]
    [XmlArrayItem(ElementName = "Hole")]
    public List<Hole> Holes = new List<Hole>();
    [XmlArray(ElementName = "Slots")]
    [XmlArrayItem(ElementName = "Slot")]
    public List<Slot> Slots = new List<Slot>();
    [XmlArray(ElementName = "ShapeParts")]
    [XmlArrayItem(ElementName = "ShapePart")]
    public List<ShapePart> ShapeParts = new List<ShapePart>();
    private List<ShapePart> shapeParts;

    [XmlIgnore]
    public List<ShapePart> ShapePartList
    {
      get
      {
        return this.ShapeParts;
      }
      set
      {
        this.shapeParts = value;
      }
    }

    [XmlElement(ElementName = "Stacking")]
    public Stacking Stacking
    {
      get
      {
        return this.stacking;
      }
      set
      {
        this.stacking = value;
      }
    }

    [XmlElement(ElementName = "HolesInfo")]
    public HolesInfo HolesInfo
    {
      get
      {
        return this.holesInfo;
      }
      set
      {
        this.holesInfo = value;
      }
    }

    [XmlElement(ElementName = "SlotsInfo")]
    public SlotsInfo SlotsInfo
    {
      get
      {
        return this.slotsInfo;
      }
      set
      {
        this.slotsInfo = value;
      }
    }

    [XmlElement(ElementName = "CentersInfo")]
    public CentersInfo CentersInfo
    {
      get
      {
        return this.centersInfo;
      }
      set
      {
        this.centersInfo = value;
      }
    }

    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("Drawing")]
    public string Drawing { get; set; }

    public object Clone()
    {
      Shape shape = new Shape();
      List<ShapePart> shapePartList = new List<ShapePart>();
      List<Hole> holeList = new List<Hole>();
      List<Slot> slotList = new List<Slot>();
      foreach (ShapePart shapePart in this.ShapePartList)
        shapePartList.Add((ShapePart) shapePart.Clone());
      foreach (Hole hole in this.Holes)
        holeList.Add((Hole) hole.Clone());
      foreach (Slot slot in this.Slots)
        slotList.Add((Slot) slot.Clone());
      shape.ShapeParts = shapePartList;
      shape.Holes = holeList;
      shape.Slots = slotList;
      shape.Stacking = (Stacking) this.Stacking.Clone();
      shape.HolesInfo = (HolesInfo) this.HolesInfo.Clone();
      shape.SlotsInfo = (SlotsInfo) this.SlotsInfo.Clone();
      shape.CentersInfo = (CentersInfo) this.CentersInfo.Clone();
      shape.Id = this.Id;
      shape.Drawing = this.Drawing;
      return (object) shape;
    }
  }
}
