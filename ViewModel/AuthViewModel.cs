using Isopoh.Cryptography.Argon2;
using System.ComponentModel.DataAnnotations;
using TaskManager.Command;
using TaskManager.Manager;
using TaskManager.Model;
using TaskManager.Model.Database.Repository;

namespace TaskManager.ViewModel
{
    internal class AuthViewModel : BaseViewModel
    {
        public NavigateCommand NavigateRegistrationCommand { get; }
        public NavigateCommand ConfirmCommand { get; }

        public bool HasError { get; private set; }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; HasError = true; OnPropertyChanged(nameof(ErrorMessage)); }
        }
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
                ValidateProperty(value, nameof(User.Login), _user);
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ValidateProperty(value, nameof(User.Password), _user);
            }
        }

        private User _user;
        private string _login;
        private string _password;
        private string _errorMessage;
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
            _user = _userRepository.Read(x => (x.Login == _login) && !x.DetetedAt.HasValue);
            if (_user == null || !Argon2.Verify(_user.Password, _password))
            {
                _user = new User();
                ErrorMessage = "Incorrect login or password";
                return false;
            }

            return true;
        }

        // validate fields
        private bool canExecute(object parameter)
        {
            try
            {
                ValidateProperty(_login, nameof(User.Login), _user);
                ValidateProperty(_password, nameof(User.Password), _user);
            }
            catch (ValidationException)
            {
                return false;
            }

            return true;
        }
    }
}
