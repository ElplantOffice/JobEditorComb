
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace UserControls.Login
{
  [XmlRoot("LoginData")]
  public class Accounts
  {
    [XmlArray("Accounts")]
    [XmlArrayItem("User")]
    public List<User> Users = new List<User>();
    public const string defaultFilename = "xxx.users.xml";

    public static Accounts Load(string fullFilename = "xxx.users.xml")
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (Accounts));
      StreamReader streamReader = (StreamReader) null;
      try
      {
        streamReader = new StreamReader(fullFilename);
        return (Accounts) xmlSerializer.Deserialize((TextReader) streamReader);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        streamReader?.Close();
      }
    }
  }
}
