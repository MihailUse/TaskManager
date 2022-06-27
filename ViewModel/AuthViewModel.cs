using Isopoh.Cryptography.Argon2;
using System.ComponentModel.DataAnnotations;
using TaskManager.Command;
using TaskManager.Manager;
using TaskManager.Model;
using TaskManager.Model.Database.Repository;

namespace TaskManager.ViewModel
{
    internal class AuthViewModel : BaseFormViewModel
    {
        public NavigateCommand NavigateRegistrationCommand { get; }
        public NavigateCommand ConfirmCommand { get; }

        public string Login
        {
            get { return _user.Login; }
            set
            {
                _user.Login = value;
                OnPropertyChanged(nameof(Login));
                ValidateProperty(value, nameof(User.Login), _user);
            }
        }
        public string Password
        {
            get { return _user.Password; }
            set
            {
                _user.Password = value;
                OnPropertyChanged(nameof(Password));
                ValidateProperty(value, nameof(User.Password), _user);
            }
        }

        private User _user;
        private readonly UserRepository _userRepository;

        public AuthViewModel(NavigationManager windowNavigationManager)
        {
            _user = new User();
            _userRepository = new UserRepository(WindowViewModel.DatabaseContext);

            //init commands
            NavigateRegistrationCommand = new NavigateCommand(windowNavigationManager, (p) => new RegistrationViewModel(windowNavigationManager));
            ConfirmCommand = new NavigateCommand(windowNavigationManager, (p) => new MainViewModel(windowNavigationManager, _user), canExecute, canNavigate);
            PropertyChanged += ConfirmCommand.OnViewModelPropertyChanged;
        }

        // check login with password
        private bool canNavigate()
        {
            User user = _userRepository.GetByLogin(_user.Login);
            if (user == null || !Argon2.Verify(user.Password, _user.Password))
            {
                ErrorMessage = "Incorrect login or password";
                return false;
            }

            _user = user;
            return true;
        }

        // validate fields
        private bool canExecute(object parameter)
        {
            try
            {
                ValidateProperty(_user.Login, nameof(User.Login), _user);
                ValidateProperty(_user.Password, nameof(User.Password), _user);
            }
            catch (ValidationException)
            {
                return false;
            }

            return true;
        }
    }
}
