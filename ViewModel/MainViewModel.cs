using System.Windows.Input;
using TaskManager.Command;
using TaskManager.Manager;
using TaskManager.Model;
using TaskManager.Model.Database;

namespace TaskManager.ViewModel
{
    // this enum must contain names of the view models
    public enum ViewModels
    {
        HomeViewModel,
        TaskListViewModel,
        ProjectListViewModel
    }

    internal class MainViewModel : BaseViewModel
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateTasksCommand { get; }
        public ICommand NavigateProjectsCommand { get; }
        public ICommand LogoutCommand { get; }

        public byte[] Avatar { get; }
        public BaseViewModel CurrentViewModel
        {
            get { return MainViewModel.NavigationManager.CurrentViewModel; }
        }
        public static User User { get; private set; }
        public static NavigationManager NavigationManager { get; private set; }

        public MainViewModel(NavigationManager windowNavigationManager, User user)
        {
            // init static props
            MainViewModel.User = user;
            MainViewModel.NavigationManager = new NavigationManager();
            MainViewModel.NavigationManager.CurrentViewModel = new ProjectListViewModel();
            MainViewModel.NavigationManager.CurrentViewModelChanged += OnCurrentViewModelChanged;

            // init props
            Avatar = user.Avatar;

            // init commands
            NavigateHomeCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new HomeViewModel());
            NavigateTasksCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new TaskListViewModel());
            NavigateProjectsCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new ProjectListViewModel());
            LogoutCommand = new NavigateCommand(windowNavigationManager, (p) => new AuthViewModel(windowNavigationManager));
        }

        // method for subscribe to event
        private void OnCurrentViewModelChanged() => OnPropertyChanged(nameof(CurrentViewModel)); // call event that update the screen
    }
}
