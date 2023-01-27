
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Utils
{
  public class ViewableCollection<T> : ObservableCollection<T>
  {
    private ListCollectionView view;

    public ListCollectionView View
    {
      get
      {
        if (this.view == null)
          this.view = new ListCollectionView((IList) this);
        return this.view;
      }
    }
  }
}
