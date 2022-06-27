using System;

namespace TaskManager.Command
{
    internal class RelayCommand : BaseCommand
    {
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
    }
}
