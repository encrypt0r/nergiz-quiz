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
            _model = new Session();
            Person = new PersonFacade();
            SecondsToNextQuestion = SECONDS_TO_NEXT_QUESTION;
            CurrentQuestionNumber = 1;
            Questions = new ObservableCollection<Question>(DataLayer.GetNewListOfQuestions(DataLayer.NumberOfQuestions));
            
            FetchNextQuestion();

            m_BtnNextText = "Next Question";

            sessionTimer = new DispatcherTimer();
            sessionTimer.Interval = new TimeSpan(0, 0, 1);
            sessionTimer.Tick += SessionTimer_Tick;

            questionTimer = new DispatcherTimer();
            questionTimer.Interval = new TimeSpan(0, 0, 1);
            questionTimer.Tick += QuestionTimer_Tick;

        }




        #endregion

        #region Events
        public event EventHandler TimeUp;
        private void OnTimeUp()
        {
            TimeUp?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Fields
        private const int SECONDS_TO_NEXT_QUESTION = 3;
        private Session _model;
        private DispatcherTimer sessionTimer;
        private DispatcherTimer questionTimer;
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
            get { return _model.CurrentQuestion; }
            set
            {
                if (value != _model.CurrentQuestion)
                {
                    _model.CurrentQuestion = value;
                    RaisePropertyChanged("CurrentQuestion");
                }
            }
        }
        public int CurrentQuestionNumber
        {
            get { return _model.NumberOfAnswersGiven; }
            set
            {
                if (value != _model.NumberOfAnswersGiven)
                {
                    _model.NumberOfAnswersGiven = value;
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
            get { return _model.NumberOfCorrectAnswers; }
            set
            {
                if (value != _model.NumberOfCorrectAnswers)
                {
                    _model.NumberOfCorrectAnswers = value;
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

        public int SecondsToNextQuestion
        {
            get { return _model.SecondsToNextQuestion; }
            set
            {
                if (value != _model.SecondsToNextQuestion)
                {
                    _model.SecondsToNextQuestion = value;
                    RaisePropertyChanged("SecondsToNextQuestion");
                }
            }
        }
        public ObservableCollection<Question> Questions
        {
            get { return _model.Questions; }
            set
            {
                if (value != _model.Questions)
                {
                    _model.Questions = value;
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

            SecondsToNextQuestion = SECONDS_TO_NEXT_QUESTION;
        }
        public void StartTimer()
        {
            sessionTimer.Start();
            questionTimer.Start();
        }
        public void StopTimer()
        {
            sessionTimer.Stop();
            questionTimer.Stop();
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
        private void SessionTimer_Tick(object sender, EventArgs e)
        {
            Person.Time += 1;
        }
        private void QuestionTimer_Tick(object sender, EventArgs e)
        {
            if (SecondsToNextQuestion > 0)
                SecondsToNextQuestion--;
            else
                OnTimeUp();
        }
        #endregion
    }
}
