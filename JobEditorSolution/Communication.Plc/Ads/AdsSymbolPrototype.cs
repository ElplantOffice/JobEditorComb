
namespace Communication.Plc.Ads
{
  public class AdsSymbolPrototype
  {
    public string Name { get; set; }

    public string Type { get; set; }

    public AdsSymbolPrototype(string name, string type)
    {
      this.Name = name;
      this.Type = type;
    }
  }
}
