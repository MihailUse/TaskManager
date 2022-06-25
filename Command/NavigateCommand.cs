using System;
using System.ComponentModel;
using TaskManager.Manager;
using TaskManager.ViewModel;

namespace TaskManager.Command
{
    internal class NavigateCommand : BaseCommand
    {
        private readonly NavigationManager _navigationManager;
        private readonly Func<object, BaseViewModel> _createViewModel;
        private readonly Func<bool> _canNavigate;

        public NavigateCommand(
            NavigationManager navigationManager,
            Func<object, BaseViewModel> createViewModel,
            Func<object, bool> canExecute = null,
            Func<bool> canNavigate = null)
        {
            _navigationManager = navigationManager;
            _createViewModel = createViewModel;
            _canExecute = canExecute;
            _canNavigate = canNavigate ?? (() => true);
        }

        // update current view model in NavigationManager
        public override void Execute(object parameter)
        {
            if (_canNavigate())
                _navigationManager.CurrentViewModel = _createViewModel(parameter);
        }

        public void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) => OnCanExecuteChanged();
    }
}
