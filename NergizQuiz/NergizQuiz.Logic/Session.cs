using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NergizQuiz.Logic
{
    /// <summary>
    /// This Class represents a session of the quiz.
    /// Each participant plays one session
    /// </summary>
    public class Session
    {
        public string UserName { get; set; }
        public Question CurrentQuestion { get; set; }
        public ObservableCollection<Question> AnswerList { get; set; }
        public int NumberOfQuestionsToBeAsked { get; set; }
        public int NumberOfAnswersGiven { get; set; }
        public int NumberOfCorrectAnswers { get; set; }
        public int Rank { get; set; }
        public int Time { get; set; }
    }
}
