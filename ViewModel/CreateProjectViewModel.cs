using ImageLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using TaskManager.Command;
using TaskManager.Model;
using TaskManager.Model.Database;
using TaskManager.Model.Database.Repository;

namespace TaskManager.ViewModel
{
    internal class CreateProjectViewModel : BaseFormViewModel
    {
        public NavigateCommand ConfirmCommand { get; }
        public ICommand GenerateImageCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand SetTesterCommand { get; }
        public ICommand SetDeveloperCommand { get; }
        public ICommand SetAdministratorCommand { get; }

        public byte[] Logo
        {
            get { return _project.Logo; }
            set
            {
                _project.Logo = value;
                OnPropertyChanged(nameof(Logo));
            }
        }
        public string Name
        {
            get { return _project.Name; }
            set
            {
                _project.Name = value;
                OnPropertyChanged(nameof(Name));
                ValidateProperty(value, nameof(Project.Name), _project);
            }
        }
        public string Description
        {
            get { return _project.Description; }
            set
            {
                _project.Description = value;
                OnPropertyChanged(nameof(Description));
                ValidateProperty(value, nameof(Project.Description), _project);
            }
        }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                if (_searchText != null && _searchText.Length > 1)
                    setSearchList(value);
                else
                    SearchableUsers.Clear();
            }
        }
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (value != null && _searchText.Length > 1)
                {
                    _selectedUser = value;

                    Memberships.Add(new Membership()
                    {
                        UserId = value.Id,
                        User = value,
                        RoleId = (int)Roles.Developer
                    });
                    SearchText = String.Empty;
                }
            }
        }
        public ObservableCollection<User> SearchableUsers { get; set; }
        public ObservableCollection<Membership> Memberships { get; set; }

        private string _searchText;
        private User _selectedUser;
        private Project _project;
        private readonly User _user;
        private readonly UserRepository _userRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly MembershipRepository _membershipRepository;

        public CreateProjectViewModel(Project project = null)
        {
            _user = MainViewModel.User;
            _userRepository = new UserRepository(WindowViewModel.DatabaseContext);
            _projectRepository = new ProjectRepository(WindowViewModel.DatabaseContext);
            _membershipRepository = new MembershipRepository(WindowViewModel.DatabaseContext);
            _project = project == null
                ? new Project() { Logo = _user.Avatar }
                : _projectRepository.Find(project.Id);

            // init props
            SearchableUsers = new ObservableCollection<User>();
            Memberships = project == null
                ? new ObservableCollection<Membership>() { new Membership() { UserId = _user.Id, User = _user, RoleId = (int)Roles.Owner } }
                : new ObservableCollection<Membership>(_membershipRepository.GetProjectMemberships(_project.Id));

            //init commands
            ConfirmCommand = new NavigateCommand(MainViewModel.NavigationManager, (p) => new ProjectListViewModel(), canExecute, canNavigate);
            PropertyChanged += ConfirmCommand.OnViewModelPropertyChanged;
            GenerateImageCommand = new RelayCommand(generateImage);
            DeleteUserCommand = new RelayCommand(deleteMembership, isNotOwner);
            SetTesterCommand = new RelayCommand(setRole(Roles.Tester), isNotOwner);
            SetDeveloperCommand = new RelayCommand(setRole(Roles.Developer), isNotOwner);
            SetAdministratorCommand = new RelayCommand(setRole(Roles.Administrator), isNotOwner);
        }

        #region membership list methods
        private bool isNotOwner(object parameter)
        {
            Membership membership = parameter as Membership;
            return membership.UserId != _user.Id;
        }
        private void deleteMembership(object parameter)
        {
            Membership membership = parameter as Membership;
            Memberships.Remove(membership);

            // if dont exists
            if (membership.Id != 0)
                _membershipRepository.Delete(membership);
        }
        private Action<object> setRole(Roles role)
        {
            return (parameter) =>
            {
                Membership membership = parameter as Membership;
                Membership selectedMembership = Memberships.First(x => x.UserId == membership.UserId);
                selectedMembership.RoleId = (int)role;

                // update membership in collection
                int index = Memberships.IndexOf(selectedMembership);
                Memberships.RemoveAt(index);
                Memberships.Insert(index, selectedMembership);
            };
        }
        private void setSearchList(string login)
        {
            List<long> userIds = Memberships
                .Select(m => m.UserId)
                .ToList();

            List<User> users = _userRepository
                .GetAllByLogin(login)
                .Where(x => !userIds.Contains(x.Id) && x.Id != _user.Id)
                .ToList();

            SearchableUsers = new ObservableCollection<User>(users);
            OnPropertyChanged(nameof(SearchableUsers));
        }
        #endregion

        private void generateImage(object parameter)
        {
            Logo = ImageGenerator.GenerateImage();
        }

        // validate uniq name and create project
        private bool canNavigate()
        {
            if (_projectRepository.IsExist(_project))
            {
                ErrorMessage = "Name already exists";
                return false;
            }

            try
            {
                // create project
                _project = _projectRepository.CreateOrUpdate(_project);

                // create memberships
                List<Membership> memberships = Memberships.ToList();
                memberships.ForEach(x => x.ProjectId = _project.Id); // update project id

                _membershipRepository.CreateOrUpdate(memberships);
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
                ValidateProperty(_project.Name, nameof(Project.Name), _project);
                ValidateProperty(_project.Description, nameof(Project.Description), _project);
            }
            catch (ValidationException)
            {
                return false;
            }

            return true;
        }
    }
}
