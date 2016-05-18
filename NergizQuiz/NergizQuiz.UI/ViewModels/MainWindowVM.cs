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
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace NergizQuiz.UI.ViewModels
{
    class MainWindowVM : ObservableObject, IDataErrorInfo
    {
        #region Constructor
        public MainWindowVM()
        {
            Page = new WelcomePage();
            RestartExecute();
        }
        #endregion

        #region Public Properties
        public int NumberOfQuestions
        {
            get { return DataLayer.NumberOfQuestions; }
            set
            {
                if (value != DataLayer.NumberOfQuestions)
                {
                    DataLayer.NumberOfQuestions = value;
                    RaisePropertyChanged("NumberOfQuestions");
                }
            }
        }
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
        private void ShowFormPageExecute()
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
        }
        private bool ShowFormPageCanExecute()
        {
            if (CurrentSession.Person["Name"] == string.Empty && !welcomePageAnimationIsPlaying)
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
                        new RelayCommand(MoreInfoExecute);

                return m_MoreInfoCommand;
            }

        }
        private void MoreInfoExecute()
        {
            Page = new MoreInfoPage();
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
        private void ShowQuizPageExecute()
        {
            Page = new QuizPage();
            CurrentSession.StartTimer();
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
        private void NextQuestionExecute()
        {

            // if all of the questions answered, go to finish page
            if (CurrentSession.CurrentQuestionNumber == DataLayer.NumberOfQuestions)
            {
                CurrentSession.NextQuestion();
                CurrentSession.StopTimer();

                Page = new LoadingPage();
                var handler = new System.Net.UploadValuesCompletedEventHandler(UploadComplete);
                DataLayer.SendPersonToDatabase(CurrentSession.Person.GetPerson(), handler);
            }
            else
            {
                CurrentSession.NextQuestion();

                // transition between two questions
                Page.StartAnimation();

            }

            // a little bit of time is wasted from the user
            // so, lets make it up for him
            CurrentSession.Person.Time -= 1;
        }
        private bool NextQuestionCanExecute()
        {
            return (CurrentSession.CurrentQuestion.UserAnswer != -1);
        }

        private ICommand m_RestartCommand;
        public ICommand RestartCommand
        {
            get
            {
                if (m_RestartCommand == null)
                    m_RestartCommand =
                        new RelayCommand(RestartExecute, RestartCanExecute);

                return m_RestartCommand;
            }

        }
        private void RestartExecute()
        {
            CurrentSession?.Dispose();
            CurrentSession = new SessionFacade();
            Page = new WelcomePage();
            CurrentSession.TimeUp += CurrentSession_TimeUp;
        }

        private void CurrentSession_TimeUp(object sender, EventArgs e)
        {
            NextQuestionExecute();
        }

        private bool RestartCanExecute()
        {
            return (Page is WelcomePage) ? false : true;
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
        private void ShowAboutExecute()
        {
            var aw = new AboutWindow();
            aw.ShowDialog();
        }

        private ICommand m_GoToWebsiteCommand;
        public ICommand GoToWebsiteCommand
        {
            get
            {
                if (m_GoToWebsiteCommand == null)
                    m_GoToWebsiteCommand =
                        new RelayCommand(GoToWebsiteExecute);

                return m_GoToWebsiteCommand;
            }

        }

        private ICommand m_ShowSettingsCommand;
        public ICommand ShowSettingsCommand
        {
            get
            {
                if (m_ShowSettingsCommand == null)
                    m_ShowSettingsCommand =
                        new RelayCommand(ShowSettingsCommandExecute, ShowSettingsCanExecute);

                return m_ShowSettingsCommand;
            }

        }
        private void ShowSettingsCommandExecute()
        {
            var sw = new SettingsWindow();
            sw.DataContext = this;
            sw.ShowDialog();
        }
        private bool ShowSettingsCanExecute()
        {
            return (Page is WelcomePage) ? true : false;
        }

        private ICommand m_AcceptSettingsCommand;
        public ICommand AcceptSettingsCommand
        {
            get
            {
                if (m_AcceptSettingsCommand == null)
                    m_AcceptSettingsCommand = new RelayCommand(RestartExecute);

                return m_AcceptSettingsCommand;
            }

        }
        #endregion

        #region Private Methods
        private void GoToWebsiteExecute()
        {
            System.Diagnostics.Process.Start(DataLayer.SITE_URL);
        }
        private void UploadComplete(object sender, System.Net.UploadValuesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string result = Encoding.UTF8.GetString(e.Result);
                string[] parts = result.Split('#');

                int rank = int.Parse(parts[0]);
                int numberOfParticipants = int.Parse(parts[1]);
                string leaderboard = parts[2];

                CurrentSession.Person.Rank = rank;
                CurrentSession.NumberOfParticipants = numberOfParticipants;

                Leaderboard = DataLayer.ParseLeaderboard(leaderboard);
            }
            else
            {
                string message = Strings.ErrorOccuredServer + Environment.NewLine +
                                 e.Error.Message + Environment.NewLine + Strings.TryAgain;

                System.Windows.MessageBox.Show(message, "Nergiz Quiz");
            }
            Page = new FinishPage();
        }
        #endregion

        #region IData Error Info
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (columnName == "Leaderboard")
                    error = ValidateLeaderboard();
                return error;
            }
        }
        private string ValidateLeaderboard()
        {
            if (Leaderboard == null)
                return Strings.CouldNotFetchLeaderboard;
            else
                return string.Empty;
        }
        #endregion
    }
}
