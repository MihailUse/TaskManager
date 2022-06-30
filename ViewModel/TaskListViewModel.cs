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
        public List<Status> Statuses { get; }
        public List<Project> Projects { get; }
        public Project CurrentProject
        {
            get { return _currentProject; }
            set { _currentProject = value; loadTasks(); }
        }
        public Status CurrentStatus
        {
            get { return _currentstatus; }
            set { _currentstatus = value; loadTasks(); }
        }

        private Project _currentProject;
        private Status _currentstatus;
        private TaskFilter _filter;
        private List<Task> _tasks;
        private readonly User _user;
        private readonly TaskRepository _taskRepository;
        private readonly StatusRepository _statusRepository;

        public TaskListViewModel(Project project = null)
        {

            _user = MainViewModel.User;
            _filter = TaskFilter.AllTasks;
            _taskRepository = new TaskRepository(WindowViewModel.DatabaseContext);
            _statusRepository = new StatusRepository(WindowViewModel.DatabaseContext);

            // load default filter 
            Statuses = _statusRepository.ReadAll();
            Statuses.Add(new Status(-1, "all"));
            CurrentStatus = Statuses.Last();
            Projects = new ProjectRepository(WindowViewModel.DatabaseContext).GetUserProjects(_user);
            CurrentProject = project ?? Projects.FirstOrDefault();

            // init commands
            SetFilterCommand = new RelayCommand(setFilter);
            ListItemAboutCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new TaskViewModel((Task)p));
            ListItemEditCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new CreateTaskViewModel((Task)p));
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
            Status status = _currentstatus.Id == -1 ? null : _currentstatus;

            switch (_filter)
            {
                case TaskFilter.AllTasks:
                    Tasks = _taskRepository.GetAllTasks(_currentProject.Id, status);
                    break;
                case TaskFilter.UserTasks:
                    Tasks = _taskRepository.GetAllTasks(_currentProject.Id, status, _user);
                    break;
            }
        }
    }
}
