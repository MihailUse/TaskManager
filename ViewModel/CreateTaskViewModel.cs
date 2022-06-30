using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaskManager.Command;
using TaskManager.Model;
using TaskManager.Model.Database.Repository;

namespace TaskManager.ViewModel
{
    internal class CreateTaskViewModel : BaseFormViewModel
    {
        public NavigateCommand ConfirmCommand { get; }

        public List<User> Users { get; }
        public List<Status> Statuses { get; }
        public List<Project> Projects { get; }
        public User CurrentUser { get; set; }
        public Status CurrentStatus { get; set; }
        public Project CurrentProject { get; set; }
        public string Title
        {
            get { return _task.Title; }
            set
            {
                _task.Title = value;
                OnPropertyChanged(nameof(Title));
                ValidateProperty(value, nameof(Task.Title), _task);
            }
        }
        public string Description
        {
            get { return _task.Description; }
            set
            {
                _task.Description = value;
                OnPropertyChanged(nameof(Description));
                ValidateProperty(value, nameof(Task.Description), _task);
            }
        }

        private Task _task;
        private readonly TaskRepository _taskRepository;
        private readonly StatusRepository _statusRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly MembershipRepository _membershipRepository;

        public CreateTaskViewModel(Project project = null)
        {
            _task = new Task();
            _taskRepository = new TaskRepository(WindowViewModel.DatabaseContext);
            _statusRepository = new StatusRepository(WindowViewModel.DatabaseContext);
            _projectRepository = new ProjectRepository(WindowViewModel.DatabaseContext);
            _membershipRepository = new MembershipRepository(WindowViewModel.DatabaseContext);


            // init values
            Projects = _projectRepository.GetUserProjects(MainViewModel.User);
            CurrentProject = project ?? Projects.First();
            Users = _membershipRepository.GetProjectUsers(CurrentProject);
            CurrentUser = Users.Single(x => x.Id == MainViewModel.User.Id);
            Statuses = _statusRepository.ReadAllExclude(Model.Database.Statuses.Done);
            CurrentStatus = Statuses.Single(x => x.Id == (int)Model.Database.Statuses.TODO);

            //init commands
            ConfirmCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new TaskListViewModel(project), canExecute, canNavigate);
            PropertyChanged += ConfirmCommand.OnViewModelPropertyChanged;
        }

        public CreateTaskViewModel(Task task)
        {
            _task = task;
            _taskRepository = new TaskRepository(WindowViewModel.DatabaseContext);
            _statusRepository = new StatusRepository(WindowViewModel.DatabaseContext);
            _projectRepository = new ProjectRepository(WindowViewModel.DatabaseContext);
            _membershipRepository = new MembershipRepository(WindowViewModel.DatabaseContext);

            // init values
            Projects = _projectRepository.GetUserProjects(MainViewModel.User);
            CurrentProject = Projects.Single(x => x.Id == _task.ProjectId);
            Users = _membershipRepository.GetProjectUsers(CurrentProject);
            CurrentUser = Users.Single(x => x.Id == _task.UserId);
            Statuses = _statusRepository.ReadAllExclude(Model.Database.Statuses.Done);
            CurrentStatus = Statuses.Single(x => x.Id == _task.StatusId);

            //init commands
            ConfirmCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new TaskListViewModel(CurrentProject), canExecute, canNavigate);
            PropertyChanged += ConfirmCommand.OnViewModelPropertyChanged;
        }

        // validate uniq name and create project
        private bool canNavigate()
        {
            if (_taskRepository.IsExist(_task, CurrentProject))
            {
                ErrorMessage = "Title already exists";
                return false;
            }

            try
            {
                // create task
                _task.UserId = CurrentUser.Id;
                _task.StatusId = CurrentStatus.Id;
                _task.ProjectId = CurrentProject.Id;
                _taskRepository.CreateOrUpdate(_task);
            }
            catch (Exception error)
            {
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
                ValidateProperty(_task.Title, nameof(Task.Title), _task);
                ValidateProperty(_task.Description, nameof(Task.Description), _task);
            }
            catch (ValidationException)
            {
                return false;
            }

            return true;
        }
    }
}
