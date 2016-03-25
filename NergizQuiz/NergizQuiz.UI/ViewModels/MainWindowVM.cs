using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NergizQuiz.Logic;
using NergizQuiz.MVVM;
using System.Windows.Input;
using System.Windows.Media.Animation;
using NergizQuiz.UI.Views;
using System.Collections.ObjectModel;

namespace NergizQuiz.UI.ViewModels
{
    class MainWindowVM : ObservableObject
    {
        #region Constructor
        public MainWindowVM()
        {
            Page = new WelcomePage();
            RestartExecute();
        }
        #endregion

        #region Public Properties
        private SessionFacade m_CurrentSession;
        public SessionFacade CurrentSession
        {
            get { return m_CurrentSession; }
            set
            {
                if (value != m_CurrentSession)
                {
                    m_CurrentSession = value;
                    RaisePropertyChanged("CurrentSession");
                }
            }
        }

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

        private ObservableCollection<Person> m_Leaderboard;
        public ObservableCollection<Person> Leaderboard
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
        #endregion

        #region Commands
        private ICommand m_ShowFormPageCommand;
        private bool welcomePageAnimationIsPlaying = false;
        public ICommand ShowFormPageCommand
        {
            get
            {
                if (m_ShowFormPageCommand == null)
                    m_ShowFormPageCommand =
                        new RelayCommand(ShowFormPageExecute, ShowFormPageCanExecute);

                return m_ShowFormPageCommand;
            }

        }
        public void ShowFormPageExecute()
        {
            Storyboard sb = Page.StartAnimation();
            sb.Completed += SbShowformPage_Animation_Complete;
            sb.Completed += (s, _) =>
            {
                sb.Completed -= SbShowformPage_Animation_Complete;
                welcomePageAnimationIsPlaying = false;
            };
            welcomePageAnimationIsPlaying = true;
            sb.Begin();
        }
        private void SbShowformPage_Animation_Complete(object sender, EventArgs e)
        {
            Page = new FormPage();
            CurrentSession.StartTimer();
        }
        public bool ShowFormPageCanExecute()
        {
            if (CurrentSession.Person.Name != string.Empty && !welcomePageAnimationIsPlaying)
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
            if (CurrentSession.Person.Name != string.Empty)
                return true;
            else
                return false;
        }

        private ICommand m_ShowQuizPageCommand;
        public ICommand ShowQuizPageCommand
        {
            get
            {
                if (m_ShowQuizPageCommand == null)
                    m_ShowQuizPageCommand =
                        new RelayCommand(ShowQuizPageExecute);

                return m_ShowQuizPageCommand;
            }

        }
        public void ShowQuizPageExecute()
        {
            Page = new QuizPage();
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

            // if all of the questions answered, go to finish page
            if (CurrentSession.CurrentQuestionNumber == CurrentSession.NumberOfQuestionsToBeAsked)
            {
                CurrentSession.NextQuestion();
                CurrentSession.StopTimer();

                Page = new LoadingPage();
                var handler = new System.Net.UploadValuesCompletedEventHandler(UploadComplete);
                DataLayer.UploadPersonIntoLeaderboard(CurrentSession.Person.GetPerson(), handler); 
            }
            else
            {
                CurrentSession.NextQuestion();

                // transition between two questions
                Page.StartAnimation();
            }               

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
            CurrentSession = new SessionFacade();
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

        #endregion

        #region Event Handlers
        private void UploadComplete(object sender, System.Net.UploadValuesCompletedEventArgs e)
        {
            string result = Encoding.UTF8.GetString(e.Result);
            string[] parts = result.Split('#');

            int rank = int.Parse(parts[0]);
            int numberOfParticipants = int.Parse(parts[1]);
            string leaderboard = parts[2];

            CurrentSession.Person.Rank = rank;
            CurrentSession.NumberOfParticipants = numberOfParticipants;
            
            Leaderboard = DataLayer.ParseLeaderboard(leaderboard);
            Page = new FinishPage();
        }
        #endregion
    }
}
