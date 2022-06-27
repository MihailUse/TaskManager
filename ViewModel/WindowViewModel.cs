using System.Linq;
using System.Windows;
using System.Windows.Input;
using TaskManager.Command;
using TaskManager.Manager;
using TaskManager.Model.Database;

namespace TaskManager.ViewModel
{
    internal class WindowViewModel : BaseViewModel
    {
        public ICommand CloseWindowCommand { get; }
        public ICommand MinimizeWindowCommand { get; }
        public ICommand MaximizeWindowCommand { get; }

        public BaseViewModel CurrentViewModel
        {
            get { return _navigationManager.CurrentViewModel; }
        }

        private readonly NavigationManager _navigationManager;
        public static ITaskManagerContext DatabaseContext { get; private set; }

        public WindowViewModel(Window window)
        {
            WindowViewModel.DatabaseContext = TaskManagerContext.GetInstance();

            // init commands
            CloseWindowCommand = new RelayCommand((p) => window.Close());
            MinimizeWindowCommand = new RelayCommand((p) => window.WindowState = WindowState.Minimized);
            MaximizeWindowCommand = new RelayCommand((p) => window.WindowState = WindowState.Maximized);

            _navigationManager = new NavigationManager();
            //_navigationManager.CurrentViewModel = new AuthViewModel(_navigationManager);
            _navigationManager.CurrentViewModel = new MainViewModel(
                _navigationManager,
                WindowViewModel.DatabaseContext.User.First(x => x.Login == "test")
            );
            _navigationManager.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        // method for subscribe to event
        private void OnCurrentViewModelChanged() => OnPropertyChanged(nameof(CurrentViewModel)); // call event that update the screen
    }
}
