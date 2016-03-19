using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NergizQuiz.Logic
{
    static public class SharedMethods
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
    }
}
