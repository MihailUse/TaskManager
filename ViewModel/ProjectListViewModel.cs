using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TaskManager.Command;
using TaskManager.Model;
using TaskManager.Model.Database;
using TaskManager.Model.Database.Repository;
using TaskManager.Model.SubModels;

namespace TaskManager.ViewModel
{
    // TODO: make more filters
    internal enum ProjectFilter
    {
        AllProjects,
        ProjectsUserOwner
    }

    internal class ProjectListViewModel : BaseViewModel
    {
        public ICommand SetFilterCommand { get; }
        public ICommand ListItemAboutCommand { get; }
        public ICommand ListItemEditCommand { get; }
        public ICommand ListItemTasksCommand { get; }
        public ICommand ListItemDeleteCommand { get; }
        public ICommand CreateProjectCommand { get; }

        public List<ProjectItem> Projects
        {
            get { return _projects; }
            private set { _projects = value; OnPropertyChanged(nameof(Projects)); }
        }

        private ProjectFilter _filter;
        private List<ProjectItem> _projects;
        private readonly User _user;
        private readonly ProjectRepository _repository;

        public ProjectListViewModel()
        {
            _user = MainViewModel.User;
            _filter = ProjectFilter.AllProjects;
            _repository = new ProjectRepository(WindowViewModel.DatabaseContext);

            // load with default filter 
            loadTasks();

            // init commands
            SetFilterCommand = new RelayCommand(setFilter);
            ListItemAboutCommand = new RelayCommand((p) => { }, (p) => false);
            ListItemEditCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new CreateProjectViewModel((ProjectItem)p), canEditProject);
            ListItemTasksCommand = new NavigateCommand(MainViewModel.NavigationManager, navigateTasks);
            ListItemDeleteCommand = new RelayCommand(deleteProject, canDeleteProject);
            CreateProjectCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new CreateProjectViewModel());
        }

        private bool canEditProject(object parameter)
        {
            ProjectItem projectItem = parameter as ProjectItem;
            return projectItem.Role <= Roles.Administrator;
        }
        private bool canDeleteProject(object parameter)
        {
            ProjectItem projectItem = parameter as ProjectItem;
            return projectItem.Role == Roles.Owner;
        }
        private void deleteProject(object parameter)
        {
            ProjectItem projectItem = parameter as ProjectItem;
            _repository.Delete(projectItem.Id);
            loadTasks();
        }
        private BaseViewModel navigateTasks(object parameter)
        {
            ProjectItem projectItem = parameter as ProjectItem;
            // if just pass ProjectItem or new Project then default value for the combo box doesn't work 
            return new TaskListViewModel(_repository.GetUserProjects(_user).Find(x => x.Id == projectItem.Id));
        }

        // set task filter and reload list
        private void setFilter(object parameter)
        {
            _filter = (ProjectFilter)Enum.Parse(typeof(ProjectFilter), parameter.ToString());
            loadTasks();
        }

        // load task list by filter
        private void loadTasks()
        {
            switch (_filter)
            {
                case ProjectFilter.AllProjects:
                    Projects = _repository.GetProjectItems(_user.Id);

                    break;
                case ProjectFilter.ProjectsUserOwner:
                    Projects = _repository.GetProjectItems(_user.Id, true);
                    break;
            }
        }
    }
}
