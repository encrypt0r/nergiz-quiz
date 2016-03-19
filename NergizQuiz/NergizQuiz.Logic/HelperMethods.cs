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
                
                sb.Append(minutes.ToString("0 Minutes and "));
                sb.Append(remSeconds.ToString("0 Seconds"));
            }
           else
            {
                sb.Append(remSeconds.ToString("0 Seconds"));
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
                    return "Level 1: Elementary";
                case 2:
                    return "Level 2: Lower-Intermediate";
                case 3:
                    return "Level 3: Intermediate";
                case 4:
                    return "Level 4: Upper-Intermediate";
                case 5:
                    return "Level 5: Advanced";
            }
        }
    }
}
