using NergizQuiz.MVVM;
using NergizQuiz.Logic;
using System;
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
            session = new Session();
            Person = new PersonFacade();
            CurrentQuestionNumber = 1;
            Questions = new ObservableCollection<Question>(DataLayer.GetNewListOfQuestions(DataLayer.NumberOfQuestions));
            
            FetchNextQuestion();

            m_BtnNextText = "Next Question";

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

        private PersonFacade m_Person;
        public PersonFacade Person
        {
            get { return m_Person; }
            set
            {
                if (value != m_Person)
                {
                    m_Person = value;
                    RaisePropertyChanged("Person");
                }
            }
        }

        private string m_BtnNextText;
        public string BtnNextText
        {
            get { return m_BtnNextText; }
            set
            {
                if (value != m_BtnNextText)
                {
                    m_BtnNextText = value;
                    RaisePropertyChanged("BtnNextText");
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
        public int CurrentQuestionNumber
        {
            get { return session.NumberOfAnswersGiven; }
            set
            {
                if (value != session.NumberOfAnswersGiven)
                {
                    session.NumberOfAnswersGiven = value;
                    RaisePropertyChanged("CurrentQuestionNumber");
                    if (Person != null)
                        Person.Accuracy = (float) NumberOfCorrectAnswers / DataLayer.NumberOfQuestions;

                    if (value == DataLayer.NumberOfQuestions)
                        BtnNextText = "Get Results";
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
                }
            }
        }
        private int m_NumberOfParticipants;
        public int NumberOfParticipants
        {
            get { return m_NumberOfParticipants; }
            set
            {
                if (value != m_NumberOfParticipants)
                {
                    m_NumberOfParticipants = value;
                    RaisePropertyChanged("NumberOfParticipants");
                }
            }
        }
        public ObservableCollection<Question> Questions
        {
            get { return session.Questions; }
            set
            {
                if (value != session.Questions)
                {
                    session.Questions = value;
                    RaisePropertyChanged("Questions");
                }
            }
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
            if (CurrentQuestionNumber > DataLayer.NumberOfQuestions)
                return;
            CurrentQuestion = Questions[CurrentQuestionNumber - 1];
            CurrentQuestion.Index = (CurrentQuestionNumber).ToString("00");
        }
        private void dTimer_Tick(object sender, EventArgs e)
        {
            Person.Time += 1;
        }
        #endregion
    }
}
