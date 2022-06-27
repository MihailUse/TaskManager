using System;
using TaskManager.ViewModel;


namespace TaskManager.Manager
{
    internal class NavigationManager
    {
        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; OnCurrentViewModelChanged(); }
        }
        public event Action CurrentViewModelChanged;

        private BaseViewModel _currentViewModel;
        private void OnCurrentViewModelChanged() => CurrentViewModelChanged?.Invoke();
    }
}
