using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace NergizQuiz.UI
{
    class PercentageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float accuracy = (float)value;

            if (Math.Round(accuracy * 100) >= 50)
                return new SolidColorBrush(Colors.Green);
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
