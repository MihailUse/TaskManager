using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaskManager.Command;
using TaskManager.Model;
using TaskManager.Model.Database.Repository;

namespace TaskManager.ViewModel
{
    internal class CreateTaskViewModel : BaseViewModel
    {
        public NavigateCommand ConfirmCommand { get; }

        public bool HasError { get; private set; }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; HasError = true; OnPropertyChanged(nameof(ErrorMessage)); }
        }
        public List<Project> Projects { get; }
        public List<User> Users { get; }
        public Project CurrentProject { get; set; }
        public User CurrentUser { get; set; }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
                ValidateProperty(value, nameof(Task.Title), _task);
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
                ValidateProperty(value, nameof(Task.Description), _task);
            }
        }

        private string _title;
        private string _description;
        private string _errorMessage;
        private Task _task;
        private readonly TaskRepository _taskRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly MembershipRepository _membershipRepository;

        public CreateTaskViewModel(Project project = null)
        {
            _task = new Task();
            _taskRepository = new TaskRepository(WindowViewModel.DatabaseContext);
            _projectRepository = new ProjectRepository(WindowViewModel.DatabaseContext);
            _membershipRepository = new MembershipRepository(WindowViewModel.DatabaseContext);

            CurrentProject = project ?? Projects.First();
            Users = _membershipRepository.GetProjectUsers(CurrentProject);
            Projects = _projectRepository.GetUserProjects(MainViewModel.User);

            //init commands
            ConfirmCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new TaskListViewModel(project), canExecute, canNavigate);
            PropertyChanged += ConfirmCommand.OnViewModelPropertyChanged;
        }

        // validate uniq name and create project
        private bool canNavigate()
        {
            if (_taskRepository.IsExist(_title, CurrentProject.Id))
            {
                ErrorMessage = "Title already exists";
                return false;
            }

            try
            {
                // create project
                _task.Title = _title;
                _task.Description = _description;
                _task = _taskRepository.Create(_task);
            }
            catch (Exception error)
            {
                _task = new Task();
                ErrorMessage = $"Error {error.Message}";
                return false;
            }

            return true;
        }

        // validate fields
        private bool canExecute(object parameter)
        {
            try
            {
                ValidateProperty(_title, nameof(Task.Title), _task);
                ValidateProperty(_description, nameof(Task.Description), _task);
            }
            catch (ValidationException)
            {
                return false;
            }

            return true;
        }
    }
}
