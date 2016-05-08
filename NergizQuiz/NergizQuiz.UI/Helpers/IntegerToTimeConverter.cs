using System;
using System.Globalization;
using System.Windows.Data;
using NergizQuiz.Logic;
namespace NergizQuiz.UI
{
    class IntegerToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int time = (int)value;
            return HelperMethods.GetTimeInHumanLanguage(time);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
