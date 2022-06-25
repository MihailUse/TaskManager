using System;
using System.Globalization;
using System.Windows.Data;
using TaskManager.Model.Database;

namespace TaskManager.Converter
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class RoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Roles.Developer.ToString();

            Roles role = (Roles)value;
            return role.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
