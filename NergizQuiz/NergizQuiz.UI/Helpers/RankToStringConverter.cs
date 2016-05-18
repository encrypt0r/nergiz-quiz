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
            if (!(value is int))
                throw new ArgumentException("Value must be of type integer.");

            int rank = (int)value;
            string rankStr = rank.ToString();
            int firstDig = HelperMethods.GetNthDigit(rank, 1);
            int secondDig = HelperMethods.GetNthDigit(rank, 2);
            if (rank == 0)
                return "?";

            if (secondDig != 1)
            {
                if (firstDig == 1)
                    return rankStr + "st";
                else if (firstDig == 2)
                    return rankStr + "nd";
                else if (firstDig == 3)
                    return rankStr + "rd";
                else
                    return rankStr + "th";
            }
            else
                return rankStr + "th";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
