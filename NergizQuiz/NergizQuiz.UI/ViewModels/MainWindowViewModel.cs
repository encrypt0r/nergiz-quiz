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
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NergizQuiz.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor
        public MainWindowViewModel()
        {
            dTimer = new DispatcherTimer();
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dTimer.Tick += DTimer_Tick;
            RestartExecute();
        }
        #endregion

        #region Fields
        private DispatcherTimer dTimer;
        #endregion

        #region Public Properties
        private IAnimatedUserControl m_Page;
        public IAnimatedUserControl Page
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

        private ObservableCollection<Question> m_Answers;
        public ObservableCollection<Question> Answers
        {
            get { return m_Answers; }
            set
            {
                if (value != m_Answers)
                {
                    m_Answers = value;
                    RaisePropertyChanged("Answers");
                }
            }
        }

        public string TimeElapsed
        {
            get
            {
                return SharedMethods.GetTimeInHumanLanguage(DeciSecondsElapsed);
            }
        }

        private List<CoolPerson> m_Leaderboard;
        public List<CoolPerson> Leaderboard
        {
            get { return m_Leaderboard; }
            set
            {
                if (value != m_Leaderboard)
                {
                    m_Leaderboard = value;
                    RaisePropertyChanged("Leaderboard");
                }
            }
        }

        private int m_DeciSecondsElapsed;
        public int DeciSecondsElapsed
        {
            get { return m_DeciSecondsElapsed; }
            set
            {
                if (value != m_DeciSecondsElapsed)
                {
                    m_DeciSecondsElapsed = value;
                    RaisePropertyChanged("DeciSecondsElapsed");
                    RaisePropertyChanged("TimeElapsed");
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
            Storyboard sb = Page.StartAnimation();
            sb.Completed += (s, _) => {
                Page = new QuizPage();
                dTimer.Start();
            };
            sb.Begin();

        }


        public bool ShowQuizPageCanExecute()
        {
            if (UserName != null && UserName != string.Empty)
                return true;
            else
                return false;
        }

        private ICommand m_MoreInfoCommand;
        public ICommand MoreInfoCommand
        {
            get
            {
                if (m_MoreInfoCommand == null)
                    m_MoreInfoCommand =
                        new RelayCommand(MoreInfoExecute, MoreInfoCanExecute);

                return m_MoreInfoCommand;
            }

        }

        public void MoreInfoExecute()
        {
            Page = new MoreInfoPage();
        }
        public bool MoreInfoCanExecute()
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
            // Add the current answer to the list of the answers
            // The user have given
            int userAnswer = GetUserAnswer();
            Question.AllAnswers[userAnswer].IsChosenByUser = true;
            Question.Index = (Answers.Count + 1).ToString("00");
            Answers.Add(Question);

            if (userAnswer == Question.CorrectAnswer)
                NumberOfCorrectAnswers++;
            TotalNumberOfAnswers++;

            // Play Animation
            Page.StartAnimation();

            // if all of the questions answered, go to finish page
            if (TotalNumberOfAnswers - 1 == TotalNumberOfQuestions)
            {
                Page = new FinishPage();
                dTimer.Stop();

                CoolPerson thisCp = new CoolPerson();
                thisCp.Name = UserName;
                thisCp.DeciSecondsElapsed = DeciSecondsElapsed;
                thisCp.Accuracy = (float) NumberOfCorrectAnswers / TotalNumberOfQuestions;
                DataLayer.AddToLeaderBoard(thisCp);
                Leaderboard = DataLayer.GetLeaderboard();
            }
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
            Answers = new ObservableCollection<Question>();
            TotalNumberOfAnswers = 1;
            NumberOfCorrectAnswers = 0;
            TotalNumberOfQuestions = 5;
            UserName = "Person";
            DeciSecondsElapsed = 0;

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
            Question.AllAnswers[0].IsChosenByUser = true;
        }
        private int GetUserAnswer()
        {
            foreach (var ans in Question.AllAnswers)
            {
                if (ans.IsChosenByUser)
                    return ans.Index;
            }

            return 0;
        }
        #endregion

        #region Event Handlers
        private void DTimer_Tick(object sender, EventArgs e)
        {
            DeciSecondsElapsed += 1;
        }
        #endregion

    }
}
