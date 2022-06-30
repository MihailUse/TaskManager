using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModel
{
    internal abstract class BaseFormViewModel : BaseViewModel
    {
        public bool HasError
        {
            get { return _hasError; }
            set { _hasError = value; OnPropertyChanged(nameof(HasError)); }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; HasError = true; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        private bool _hasError;
        private string _errorMessage;

        protected void ValidateProperty<T>(T value, string name, object instanse)
        {
            ValidationContext validationContext = new ValidationContext(instanse)
            {
                MemberName = name
            };

            Validator.ValidateProperty(value, validationContext);
        }
    }
}
