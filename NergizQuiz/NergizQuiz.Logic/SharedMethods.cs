using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NergizQuiz.Logic
{
    static public class SharedMethods
    {
        public static string GetTimeInHumanLanguage(int deciSeconds)
        {
            StringBuilder sb = new StringBuilder();

            int minutes = (deciSeconds / 60 / 10);

            if (minutes > 0)
            {
                
                int seconds = (deciSeconds - (minutes * 60 * 10)) / 10;
                sb.Append(minutes.ToString("0 Minutes and "));
                sb.Append(seconds.ToString("0 Seconds"));
            }
           else
            {
                float seconds = deciSeconds / 10F;
                sb.Append(seconds.ToString("0.0 Seconds"));
            }

            return sb.ToString();
        }
    }
}
