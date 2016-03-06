using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NergizQuiz.MVVM;
using NergizQuiz.UI.Views;
using System.Windows.Input;
using NergizQuiz.Logic;
using System.Windows.Media;
using System.Windows;

namespace NergizQuiz.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor
        public MainWindowViewModel()
        {
            RestartExecute();
        }
        #endregion
        #region Public Properties
        private object m_Page;
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

        private string m_UserName;
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
                    RaisePropertyChanged("ScoreColor");
                }
            }
        }

        private int m_TotalNumberOfQuestions;
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

        private int m_TotalNumberOfAnswers;
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

        private int m_NumberOfCorrectAnswers;
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
        private float Score
        {
            get
            {
                float percent = NumberOfCorrectAnswers / (float)(TotalNumberOfAnswers - 1);
                if (float.IsNaN(percent))
                    return 1f;
                else
                    return percent;
            }
        }
        public string Percentage
        {
            get
            {
                if (TotalNumberOfAnswers == 0)
                    return "100 %";

                return Math.Round(Score * 100, 1) + " %";
            }
        }
        public string Progress
        {
            get { return TotalNumberOfAnswers + " of " + TotalNumberOfQuestions; }
        }

        public SolidColorBrush ScoreColor
        {
            get
            {
                if (Math.Round(Score * 100) >= 50)
                    return new SolidColorBrush(Colors.Green);
                else if (Math.Round(Score * 100) == 0)
                    return new SolidColorBrush(Colors.Red);
                else
                    return new SolidColorBrush(Colors.Orange);
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
                        new RelayCommand(NextQuestionExecute);

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

            // if all of the questions answered, go to finish page
            if (TotalNumberOfAnswers - 1 == TotalNumberOfQuestions)
                Page = new FinishPage();
            else
                GetNextQuestion();

        }

        private ICommand m_RestartCommand;
        public ICommand RestartCommand
        {
            get
            {
                if (m_RestartCommand == null)
                    m_RestartCommand =
                        new RelayCommand(RestartExecute);

                return m_RestartCommand;
            }

        }
        public void RestartExecute()
        {
            TotalNumberOfAnswers = 1;
            NumberOfCorrectAnswers = 0;
            TotalNumberOfQuestions = 5;
            UserName = "Somebody";

            GetNextQuestion();
            
            Page = new WelcomePage();
        }

        private ICommand m_ShowAboutCommand;
        public ICommand ShowAboutCommand
        {
            get
            {
                if (m_ShowAboutCommand == null)
                    m_ShowAboutCommand =
                        new RelayCommand(ShowAboutExecute);

                return m_ShowAboutCommand;
            }

        }
        public void ShowAboutExecute()
        {
            var aw = new AboutWindow();
            aw.ShowDialog();
        }

        public ICommand ExitCommand
        {
            get
            {
                return new RelayCommand(ExitExecute);
            }
        }
        void ExitExecute()
        {
            Application.Current.Shutdown();
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
