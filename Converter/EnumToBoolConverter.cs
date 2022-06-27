using System;
using System.Globalization;
using System.Windows.Data;

namespace TaskManager.Converter
{
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBoolConverter : IValueConverter
    {
        // compare value with parameter and return bool
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return false;

            string enumValue = value.GetType().Name; // get class name
            string targetValue = parameter.ToString();
            bool outputValue = enumValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);

            return outputValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
