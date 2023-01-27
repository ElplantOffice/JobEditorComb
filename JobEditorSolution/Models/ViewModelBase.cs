
using System.Collections.Generic;

namespace Models
{
  public class ViewModelBase : IViewModel
  {
    private readonly List<IViewModelAttributedEventType> viewModelAttributedEventTypes = new List<IViewModelAttributedEventType>();

    public void AddAttributedEventType(IViewModelAttributedEventType viewModelAttributedEventType)
    {
      if (viewModelAttributedEventType == null)
        return;
      this.viewModelAttributedEventTypes.Add(viewModelAttributedEventType);
    }

    public virtual void Dispose()
    {
      foreach (IViewModelAttributedEventType attributedEventType in this.viewModelAttributedEventTypes)
        attributedEventType.Dispose();
      this.viewModelAttributedEventTypes.Clear();
    }
  }
}
