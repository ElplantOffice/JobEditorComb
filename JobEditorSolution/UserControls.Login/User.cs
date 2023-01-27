
using System.Xml.Serialization;

namespace UserControls.Login
{
  public class User
  {
    [XmlAttribute("PinCode")]
    public string PinCode { get; set; }

    [XmlAttribute("Info")]
    public string Info { get; set; }

    [XmlAttribute("Level")]
    public uint Level { get; set; }
  }
}
