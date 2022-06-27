using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModel
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
