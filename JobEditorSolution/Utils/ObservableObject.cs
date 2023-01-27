
using System.ComponentModel;

namespace Utils
{
  public class ObservableObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string PropertyName)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.PropertyChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(PropertyName));
    }
  }
}
