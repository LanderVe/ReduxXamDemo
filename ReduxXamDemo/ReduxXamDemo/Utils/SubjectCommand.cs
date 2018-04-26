using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Windows.Input;

namespace ReduxXamDemo.Utils
{
  public class SubjectCommand : ICommand
  {
    private Predicate<object> canExecute;
    public Subject<object> Subject { get; }

    public SubjectCommand(Predicate<object> canExecute = null)
    {
      this.canExecute = canExecute;
      Subject = new Subject<object>();
    }

    public bool CanExecute(object parameter)
    {
      return canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object parameter)
    {
      Subject.OnNext(parameter);
    }

    public event EventHandler CanExecuteChanged;
    public virtual void OnExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
  }
}
