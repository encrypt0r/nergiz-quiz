using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NergizQuiz.UI
{
    class PercentageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is float))
                throw new ArgumentException("Value must be of type float.");

            float accuracy = (float)value;
            if (Math.Round(accuracy * 100) == 100)
                return new SolidColorBrush(Colors.Blue);
            if (Math.Round(accuracy * 100) >= 75)
                return new SolidColorBrush(Colors.Green);
            if (Math.Round(accuracy * 100) >= 50)
                return new SolidColorBrush(Colors.GreenYellow);
            else if (Math.Round(accuracy * 100) == 0)
                return new SolidColorBrush(Colors.Red);
            else
                return new SolidColorBrush(Colors.Orange);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
