using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NergizQuiz.MVVM;
using NergizQuiz.UI.Views;
using System.Windows.Input;
using NergizQuiz.Logic;
namespace NergizQuiz.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor
        public MainWindowViewModel()
        {
            GetNextQuestion();
        }
        #endregion
        #region Public Properties
        private object m_Page = new Welcome();
        public object Page
        {
            get { return m_Page; }
            set
            {
                if (value != m_Page)
                {
                    m_Page = value;
                    RaisePropertyChanged("Page");
                }
            }
        }

        private string m_UserName = "Somebody";
        public string UserName
        {
            get { return m_UserName; }
            set
            {
                if (value != m_UserName)
                {
                    m_UserName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }

        private Question m_Question;
        public Question Question
        {
            get { return m_Question; }
            set
            {
                if (value != m_Question)
                {
                    m_Question = value;
                    RaisePropertyChanged("Question");
                    RaisePropertyChanged("Percentage");
                    RaisePropertyChanged("Progress");
                }
            }
        }

        //TODO: There should some way to minimize these
        private bool m_UserAnswer1;
        public bool UserAnswer1
        {
            get { return m_UserAnswer1; }
            set
            {
                if (value != m_UserAnswer1)
                {
                    m_UserAnswer1 = value;
                    RaisePropertyChanged("UserAnswer1");
                }
            }
        }

        private bool m_UserAnswer2;
        public bool UserAnswer2
        {
            get { return m_UserAnswer2; }
            set
            {
                if (value != m_UserAnswer2)
                {
                    m_UserAnswer2 = value;
                    RaisePropertyChanged("UserAnswer2");
                }
            }
        }

        private bool m_UserAnswer3;
        public bool UserAnswer3
        {
            get { return m_UserAnswer3; }
            set
            {
                if (value != m_UserAnswer3)
                {
                    m_UserAnswer3 = value;
                    RaisePropertyChanged("UserAnswer3");
                }
            }
        }

        private bool m_UserAnswer4;
        public bool UserAnswer4
        {
            get { return m_UserAnswer4; }
            set
            {
                if (value != m_UserAnswer4)
                {
                    m_UserAnswer4 = value;
                    RaisePropertyChanged("UserAnswer4");
                }
            }
        }

        private int m_TotalNumberOfQuestions = 5;
        public int TotalNumberOfQuestions
        {
            get { return m_TotalNumberOfQuestions; }
            set
            {
                if (value != m_TotalNumberOfQuestions)
                {
                    m_TotalNumberOfQuestions = value;
                    RaisePropertyChanged("TotalNumberOfQuestions");
                }
            }
        }

        private int m_TotalNumberOfAnswers = 0;
        public int TotalNumberOfAnswers
        {
            get { return m_TotalNumberOfAnswers; }
            set
            {
                if (value != m_TotalNumberOfAnswers)
                {
                    m_TotalNumberOfAnswers = value;
                    RaisePropertyChanged("TotalNumberOfAnswers");
                }
            }
        }

        private int m_NumberOfCorrectAnswers = 0;
        public int NumberOfCorrectAnswers
        {
            get { return m_NumberOfCorrectAnswers; }
            set
            {
                if (value != m_NumberOfCorrectAnswers)
                {
                    m_NumberOfCorrectAnswers = value;
                    RaisePropertyChanged("NumberOfCorrectAnswers");
                }
            }
        }

        public string Percentage
        {
            get
            {
                if (TotalNumberOfAnswers == 0)
                    return "100 %";

                float percentage = NumberOfCorrectAnswers / (float)TotalNumberOfAnswers;
                return Math.Round(percentage * 100, 1) + " %";
            }
        }
        public string Progress
        {
            get { return TotalNumberOfAnswers + " of " + TotalNumberOfQuestions; }
        }
        #endregion

        #region Commands
        private ICommand m_ShowQuizPageCommand;
        public ICommand ShowQuizPageCommand
        {
            get
            {
                if (m_ShowQuizPageCommand == null)
                    m_ShowQuizPageCommand =
                        new RelayCommand(ShowQuizPageExecute, ShowQuizPageCanExecute);

                return m_ShowQuizPageCommand;
            }

        }
        public void ShowQuizPageExecute()
        {
            Page = new QuizPage();

        }
        public bool ShowQuizPageCanExecute()
        {
            if (UserName != null && UserName != string.Empty)
                return true;
            else
                return false;
        }

        private ICommand m_NextQuestionCommand;
        public ICommand NextQuestionCommand
        {
            get
            {
                if (m_NextQuestionCommand == null)
                    m_NextQuestionCommand =
                        new RelayCommand(NextQuestionExecute, NextQuestionCanExecute);

                return m_NextQuestionCommand;
            }

        }
        public void NextQuestionExecute()
        {
            var ansList = new List<bool>();
            ansList.Add(UserAnswer1);
            ansList.Add(UserAnswer2);
            ansList.Add(UserAnswer3);
            ansList.Add(UserAnswer4);

            if (ansList[Question.CorrectAnswer] == true)
                NumberOfCorrectAnswers++;
            TotalNumberOfAnswers++;

            GetNextQuestion();

        }
        public bool NextQuestionCanExecute()
        {
            if (UserName != null && UserName != string.Empty)
                return true;
            else
                return false;
        }
        #endregion

        #region Private Methods
        private void GetNextQuestion()
        {
            Question = new Question(DataLayer.GetNextQuestion());
            UserAnswer1 = true;
            UserAnswer2 = false;
            UserAnswer3 = false;
            UserAnswer4 = false;
        }
        #endregion

    }
}
