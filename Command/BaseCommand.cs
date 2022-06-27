using System;
using System.Windows.Input;

namespace TaskManager.Command
{
    internal abstract class BaseCommand : ICommand
    {
        protected Action<object> _execute;
        protected Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public virtual void Execute(object parameter) => _execute(parameter);
        public virtual bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}