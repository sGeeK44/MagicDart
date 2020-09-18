using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Zg.Core.Windows
{

    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var stringValue = parameter as string;
            var valueToBeVisible = stringValue != null && bool.Parse(stringValue);
            var val = (bool)value;

            return val == valueToBeVisible ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
