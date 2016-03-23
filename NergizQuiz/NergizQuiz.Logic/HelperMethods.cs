using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NergizQuiz.Logic
{
    static public class HelperMethods
    {
        public static string GetTimeInHumanLanguage(int seconds)
        {
            StringBuilder sb = new StringBuilder();

            int minutes = (seconds / 60);
            int remSeconds = seconds - (minutes * 60);

            if (minutes > 0)
            {
                
                sb.Append(minutes.ToString("0 Minute" + MakeItPlural(minutes) + " and "));
                sb.Append(remSeconds.ToString("0 Second" + MakeItPlural(remSeconds)));
            }
           else
            {
                sb.Append(remSeconds.ToString("0 Second" + MakeItPlural(remSeconds)));
            }

            return sb.ToString();
        }
        public static int GetLevel(float accuracy)
        {
            if (accuracy >= 0.9)
                return 5;
            else if (accuracy >= 0.8)
                return 4;
            else if (accuracy >= 0.6)
                return 3;
            else if (accuracy >= 0.4)
                return 2;
            else
                return 1;
        }
        public static string GetLevelString(float accuracy)
        {
            int level = GetLevel(accuracy);

            switch (level)
            {
                default:
                    return "Elementary (1)";
                case 2:
                    return "Lower-Intermediate (2)";
                case 3:
                    return "Intermediate (3)";
                case 4:
                    return "Upper-Intermediate (4)";
                case 5:
                    return "Advanced (5)";
            }
        }
        public static string MakeItPlural(int num)
        {
            if (num == 1)
                return "";
            else
                return "s";
        }
    }
}
