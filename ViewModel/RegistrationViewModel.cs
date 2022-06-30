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
    internal class RegistrationViewModel : BaseFormViewModel
    {
        public ICommand NavigateAuthCommand { get; }
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
                if (_user.Password != value)
                    throw new ValidationException("Passwords do not match");
            }
        }

        private User _user;
        private string _repeatPassword;
        private readonly UserRepository _userRepository;

        public RegistrationViewModel(NavigationManager windowNavigationManager)
        {
            _user = new User();
            _userRepository = new UserRepository(WindowViewModel.DatabaseContext);

            // init commands
            NavigateAuthCommand = new NavigateCommand(windowNavigationManager, (p) => new AuthViewModel(windowNavigationManager));
            ConfirmCommand = new NavigateCommand(windowNavigationManager, (p) => new MainViewModel(windowNavigationManager, _user), canExecute, canNavigate);
            PropertyChanged += ConfirmCommand.OnViewModelPropertyChanged;
        }

        // validate uniq login and create new user
        private bool canNavigate()
        {
            if (_userRepository.IsExist(_user.Login))
            {
                ErrorMessage = "Login already exists";
                return false;
            }

            try
            {
                _user.Password = Argon2.Hash(_user.Password, timeCost: 10, parallelism: Environment.ProcessorCount, hashLength: 128);
                _user.Avatar = ImageConvertor.BitmapToBytes(ImageGenerator.GenerateImage());
                _user = _userRepository.Create(_user);
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
                ValidateProperty(_user.Login, nameof(User.Login), _user);
                ValidateProperty(_user.Password, nameof(User.Password), _user);
                if (_user.Password != _repeatPassword)
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
