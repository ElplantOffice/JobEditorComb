
namespace Models
{
  public interface ITreeCollectionCommand<TTreeElementData, TTreeElementControl>
  {
    bool Execute(TreeElement<TTreeElementData> element, TTreeElementControl controlitem, int index);

    bool Update(TTreeElementData dataitem, TTreeElementControl controlitem, int index);

    void UpdateInfo();

    void UpdateInfo(string info);
  }
}
