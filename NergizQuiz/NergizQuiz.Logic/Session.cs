using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NergizQuiz.Logic
{
    /// <summary>
    /// This Class represents a session of the quiz.
    /// Each participant plays one session
    /// </summary>
    public class Session
    {
        public Question CurrentQuestion { get; set; }
        public ObservableCollection <Question> Questions { get; set; }
        public int NumberOfAnswersGiven { get; set; }
        public int NumberOfCorrectAnswers { get; set; }
        public int SecondsToNextQuestion { get; set; }
    }
}
