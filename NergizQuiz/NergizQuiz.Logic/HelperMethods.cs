using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NergizQuiz.Logic
{
    static public class HelperMethods
    {
        public static string GetTimeInHumanLanguage(int seconds)
        {
            StringBuilder sb = new StringBuilder();

            int minutes = (seconds / 60);
            int remSeconds = seconds - (minutes * 60);

            if (seconds > 60)
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
        public static string GetTimeInClockFormat(int seconds)
        {
            StringBuilder sb = new StringBuilder();

            int minutes = (seconds / 60);
            int remSeconds = seconds - (minutes * 60);

            sb.Append(minutes.ToString("00:"));
            sb.Append(remSeconds.ToString("00"));

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
        public static string ApplyMeaningfulChars(string str)
        {
            return str.Replace(@"\n", "\n");
        }
        public static int GetNthDigit(int number, int digit)
        {
            double power = Math.Pow(10, digit);
            while (digit-- > 1)
                number /= 10;
            return (int)number % 10;
        }

        /// <summary>
        /// Searches for an XElement by name case-insensitively.
        /// </summary>
        /// <param name="name">The name of the XElement (case-insensitive)</param>
        /// <param name="data">The parent XElement.</param>
        /// <returns></returns>
        public static XElement SelectXElement(string name, XElement data)
        {
            var element = data.Elements().Where(i => i.Name.LocalName.ToLower() == name.ToLower()).First();
            return element;
        }
        /// <summary>
        /// Searches for an XAttribute of an XElement by name case-insensitively.
        /// </summary>
        /// <param name="name">The name of the XAttribute (case-insensitive)</param>
        /// <param name="data">The XElement.</param>
        /// <returns></returns>
        public static XAttribute SelectXAttribute(string name, XElement data)
        {
            var attributes = data.Attributes().Where(i => i.Name.LocalName.ToLower() == name.ToLower());

            if (attributes.Count() > 0)
                return attributes.First();
            else
                return null;
        }
    }
}
