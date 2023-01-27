
using System;
using System.Windows.Input;

namespace Utils
{
  [Serializable]
  public class DelegateCommand : ICommand
  {
    private Action executeMethod;

    public event EventHandler CanExecuteChanged;

    public virtual void RaiseCanExecuteChanged(object sender, EventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.CanExecuteChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.CanExecuteChanged(sender, e);
    }

    public DelegateCommand(Action executeMethod)
    {
      this.executeMethod = executeMethod;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      this.executeMethod();
    }
  }
}
