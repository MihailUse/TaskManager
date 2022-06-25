using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TaskManager.Command;
using TaskManager.Model;
using TaskManager.Model.Database.Repository;

namespace TaskManager.ViewModel
{
    // TODO: make more filters
    internal enum TaskFilter
    {
        AllTasks,
        UserTasks
    }

    internal class TaskListViewModel : BaseViewModel
    {
        public ICommand SetFilterCommand { get; }
        public ICommand ListItemAboutCommand { get; }
        public ICommand ListItemEditCommand { get; }
        public ICommand CreateTaskCommand { get; }

        public List<Task> Tasks
        {
            get { return _tasks; }
            set { _tasks = value; OnPropertyChanged(nameof(Tasks)); }
        }
        public Project CurrentProject
        {
            get { return _currentProject; }
            set { _currentProject = value; loadTasks(); }
        }
        public List<Project> Projects { get; }

        private Project _currentProject;
        private TaskFilter _filter;
        private List<Task> _tasks;
        private readonly User _user;
        private readonly TaskRepository _repository;

        public TaskListViewModel(Project project = null)
        {

            _user = MainViewModel.User;
            _filter = TaskFilter.AllTasks;
            _repository = new TaskRepository(WindowViewModel.DatabaseContext);

            // load default filter 
            Projects = new ProjectRepository(WindowViewModel.DatabaseContext).GetUserProjects(_user);
            CurrentProject = project ?? Projects.FirstOrDefault();

            // init commands
            SetFilterCommand = new RelayCommand(setFilter);
            ListItemAboutCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new TaskViewModel((Task)p));
            ListItemEditCommand = new RelayCommand((p) => throw new NotImplementedException("ListItemEditCommand"));
            CreateTaskCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new CreateTaskViewModel(CurrentProject), (p) => CurrentProject != null);
        }

        // set task filter and reload list
        private void setFilter(object parameter)
        {
            _filter = (TaskFilter)Enum.Parse(typeof(TaskFilter), parameter.ToString());
            loadTasks();
        }

        // load task list by filter
        private void loadTasks()
        {
            if (_currentProject == null) return;

            switch (_filter)
            {
                case TaskFilter.AllTasks:
                    Tasks = _repository.GetTaskItems(_currentProject.Id);
                    break;
                case TaskFilter.UserTasks:
                    Tasks = _repository.GetTaskItems(_currentProject.Id, _user);
                    break;
            }
        }
    }
}
