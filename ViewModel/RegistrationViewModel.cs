using ImageLibrary;
using Isopoh.Cryptography.Argon2;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using TaskManager.Command;
using TaskManager.Manager;
using TaskManager.Model;
using TaskManager.Model.Database.Repository;

namespace TaskManager.ViewModel
{
    internal class RegistrationViewModel : BaseViewModel
    {
        public ICommand NavigateAuthCommand { get; }
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
                OnPropertyChanged(nameof(RepeatPassword));
                ValidateProperty(value, nameof(User.Password), _user);
            }
        }
        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                _repeatPassword = value;
                OnPropertyChanged(nameof(RepeatPassword));
                if (_password != value)
                    throw new ValidationException("Passwords do not match");
            }
        }

        private User _user;
        private string _login;
        private string _password;
        private string _repeatPassword;
        private string _errorMessage;
        private readonly UserRepository _userRepository;

        public RegistrationViewModel(NavigationManager windowNavigationManager)
        {
            _user = new User();
            _userRepository = new UserRepository(WindowViewModel.DatabaseContext);

            NavigateAuthCommand = new NavigateCommand(windowNavigationManager, (p) => new AuthViewModel(windowNavigationManager));
            ConfirmCommand = new NavigateCommand(windowNavigationManager, (p) => new MainViewModel(windowNavigationManager, _user), canExecute, canNavigate);
            PropertyChanged += ConfirmCommand.OnViewModelPropertyChanged;
        }

        // validate uniq login and create new user
        private bool canNavigate()
        {
            if (_userRepository.IsExist(_login))
            {
                ErrorMessage = "Login already exists";
                return false;
            }

            try
            {
                _user.Login = _login;
                _user.Password = Argon2.Hash(_password, timeCost: 10, parallelism: Environment.ProcessorCount, hashLength: 128);
                _user.Avatar = ImageConvertor.BitmapToBytes(ImageGenerator.GenerateImage());
                _user = _userRepository.Create(_user);
            }
            catch (Exception error)
            {
                _user = new User();
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
                ValidateProperty(_login, nameof(User.Login), _user);
                ValidateProperty(_password, nameof(User.Password), _user);
                if (_password != _repeatPassword)
                    throw new ValidationException("Passwords do not match");
            }
            catch (ValidationException)
            {
                return false;
            }

            return true;
        }
    }
}
