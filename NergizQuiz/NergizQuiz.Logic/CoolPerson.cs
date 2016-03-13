using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NergizQuiz.Logic
{
    public class CoolPerson
    {
        public string Name { get; set; }
        public float Accuracy { get; set; }
        public string AccuString
        {
            get { return (Accuracy * 100) + " %"; }
        }
        public int DeciSecondsElapsed { get; set; }
        public int Index { get; set; }
        public string Time
        {
            get
            {
                return SharedMethods.GetTimeInHumanLanguage(DeciSecondsElapsed);
            }
        }
    }
}
