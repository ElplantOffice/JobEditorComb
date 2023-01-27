
using Patterns.EventAggregator;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ProductLib
{
  public class SequenceMakerDefinition
  {
    public SequenceMaker SequenceMakerData { get; set; }

    public SequenceMakerDefinition(string filePath)
    {
      this.SequenceMakerData = new SequenceMaker();
      if (string.IsNullOrEmpty(filePath))
        return;
      this.Load(filePath);
    }

    public void Load(string filePath)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (SequenceMaker));
      StreamReader streamReader = (StreamReader) null;
      try
      {
        streamReader = new StreamReader(filePath);
        this.SequenceMakerData = (SequenceMaker) xmlSerializer.Deserialize((TextReader) streamReader);
      }
      catch (IOException ex)
      {
        Stream stream = (Stream) new FileResources().Data.Get<Stream>(((IEnumerable<string>) filePath.Split('\\')).Last<string>());
        this.SequenceMakerData = (SequenceMaker) xmlSerializer.Deserialize(stream);
      }
      finally
      {
        streamReader?.Close();
      }
    }
  }
}
