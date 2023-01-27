
namespace Models
{
  public interface ITreeElementCommand<TTreeElementData>
  {
    bool Execute(TTreeElementData dataitem);
  }
}
