using NergizQuiz.MVVM;
using NergizQuiz.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace NergizQuiz.UI
{
    /// <summary>
    /// This is the Facade of a Session object.
    /// It wraps all the properties of the objet to
    /// be MVVM-friendly.
    /// </summary>
    class SessionFacade : ObservableObject
    {
        #region Constructor
        public SessionFacade()
        {
            AnswerList = new ObservableCollection<Question>();
            UserName = "Person";
            FetchNextQuestion();
            NumberOfQuestionsToBeAsked = 25;
            CurrentQuestionNumber = 1;
            Time = 0;

            dTimer = new DispatcherTimer();
            dTimer.Interval = new TimeSpan(0, 0, 1);
            dTimer.Tick += dTimer_Tick;
        }
        #endregion

        #region Fields
        private Session session = new Session();
        private DispatcherTimer dTimer;
        #endregion

        #region Public Properties
        public string UserName
        {
            get { return session.UserName; }
            set
            {
                if (value != session.UserName)
                {
                    session.UserName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }
        public Question CurrentQuestion
        {
            get { return session.CurrentQuestion; }
            set
            {
                if (value != session.CurrentQuestion)
                {
                    session.CurrentQuestion = value;
                    RaisePropertyChanged("CurrentQuestion");
                }
            }
        }
        public ObservableCollection<Question> AnswerList
        {
            get { return session.AnswerList; }
            set
            {
                if (value != session.AnswerList)
                {
                    session.AnswerList = value;
                    RaisePropertyChanged("AnswerList");
                }
            }
        }
        public int NumberOfQuestionsToBeAsked
        {
            get { return session.NumberOfQuestionsToBeAsked; }
            set
            {
                if (value != session.NumberOfQuestionsToBeAsked)
                {
                    session.NumberOfQuestionsToBeAsked = value;
                    RaisePropertyChanged("NumberOfQuestionsToBeAsked");
                }
            }
        }
        public int CurrentQuestionNumber
        {
            get { return session.NumberOfAnswersGiven; }
            set
            {
                if (value != session.NumberOfAnswersGiven)
                {
                    session.NumberOfAnswersGiven = value;
                    RaisePropertyChanged("CurrentQuestionNumber");
                    RaisePropertyChanged("Accuracy");
                    RaisePropertyChanged("Level");
                    RaisePropertyChanged("Comment");
                }
            }
        }
        public int NumberOfCorrectAnswers
        {
            get { return session.NumberOfCorrectAnswers; }
            set
            {
                if (value != session.NumberOfCorrectAnswers)
                {
                    session.NumberOfCorrectAnswers = value;
                    RaisePropertyChanged("NumberOfCorrectAnswers");
                    RaisePropertyChanged("Accuracy");
                }
            }
        }
        public int Time
        {
            get { return session.Time; }
            set
            {
                if (value != session.Time)
                {
                    session.Time = value;
                    RaisePropertyChanged("Time");
                }
            }
        }
        public int Rank
        {
            get { return session.Rank; }
            set
            {
                if (value != session.Rank)
                {
                    session.Rank = value;
                    RaisePropertyChanged("Rank");
                }
            }
        }

        public float Accuracy
        {
            get
            {
                float percent = NumberOfCorrectAnswers / (float)(NumberOfQuestionsToBeAsked);
                if (float.IsNaN(percent))
                    return 1f;
                else
                    return percent;
            }
        }
        public string Level
        {
            get { return HelperMethods.GetLevelString(Accuracy); }
        }
        public string Comment
        {
            get { return DataLayer.GetComment(Accuracy);}
        }
        #endregion

        #region Public Methods
        public void NextQuestion()
        {
            // get the user's answer
            int chosenAnswer = 0;
            foreach (var ans in CurrentQuestion.AllAnswers)
                if (ans.IsChosenByUser)
                    chosenAnswer = ans.Index;

            // lets see if the users answer is actually correct
            if (chosenAnswer == CurrentQuestion.CorrectAnswer)
                NumberOfCorrectAnswers++;
            CurrentQuestionNumber++;

            // ask next question
            FetchNextQuestion();
        }
        public void StartTimer()
        {
            dTimer.Start();
        }
        public void StopTimer()
        {
            dTimer.Stop();
        }
        #endregion

        #region Private Methods and Event Handlers
        private void FetchNextQuestion()
        {
            CurrentQuestion = new Question(DataLayer.GetNextQuestion());
            CurrentQuestion.Index = (AnswerList.Count + 1).ToString("00");
            AnswerList.Add(CurrentQuestion);
        }
        private void dTimer_Tick(object sender, EventArgs e)
        {
            Time += 1;
        }
        #endregion
    }
}
