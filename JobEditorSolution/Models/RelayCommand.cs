
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Models
{
  public class RelayCommand : ICommand
  {
    private readonly Func<bool> _canExecute;
    private readonly Action _execute;

    public RelayCommand(Action execute)
      : this(execute, (Func<bool>) null)
    {
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
      if (execute == null)
        throw new ArgumentNullException(nameof (execute));
      this._execute = execute;
      this._canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        if (this._canExecute == null)
          return;
        CommandManager.RequerySuggested += value;
      }
      remove
      {
        if (this._canExecute == null)
          return;
        CommandManager.RequerySuggested -= value;
      }
    }

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      if (this._canExecute != null)
        return this._canExecute();
      return true;
    }

    public void Execute(object parameter)
    {
      this._execute();
    }
  }
}
