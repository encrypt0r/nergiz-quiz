using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using NergizQuiz.Logic;
namespace NergizQuiz.UI
{
    class RankToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string rank = (value).ToString();
            int lastDigit = int.Parse(rank[rank.Length - 1].ToString());
            if (lastDigit == 1)
                return rank + "st";
            else if (lastDigit == 2)
                return rank + "nd";
            else if (lastDigit == 3)
                return rank + "rd";
            else
                return rank + "th";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
