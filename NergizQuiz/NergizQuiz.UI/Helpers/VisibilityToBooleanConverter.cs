using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
namespace NergizQuiz.UI
{
    class VisibilityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = (bool)value;

            if (isVisible)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = (Visibility)value;
            if (isVisible == Visibility.Visible)
                return true;
            else
                return false;
        }

    }
}
