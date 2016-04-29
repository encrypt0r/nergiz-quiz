using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NergizQuiz.MVVM;
using System.Windows.Input;
using System.Reflection;
using NergizQuiz.Logic;
namespace NergizQuiz.UI.ViewModels
{
    class AboutWindowVM : ObservableObject
    {

        public string GitHub
        {
            get { return "https://github.com/encrypt0r/nergiz-quiz"; }
        }
        public string Website
        {
            get { return DataLayer.SITE_URL; }
        }
        public string AppVersion
        {
            get { return "Version " + GetVersion(); }
        }

        private ICommand m_OpenGitHub;
        public ICommand OpenGitHubCommand
        {
            get
            {
                if (m_OpenGitHub == null)
                    m_OpenGitHub =
                        new RelayCommand(OpenGitHubExecute);

                return m_OpenGitHub;
            }

        }
        public void OpenGitHubExecute()
        {
            System.Diagnostics.Process.Start(GitHub);
        }

        private ICommand m_OpenWebsite;
        public ICommand OpenWebsiteCommand
        {
            get
            {
                if (m_OpenWebsite == null)
                    m_OpenWebsite =
                        new RelayCommand(OpenWebsiteExecute);

                return m_OpenWebsite;
            }

        }
        public void OpenWebsiteExecute()
        {
            System.Diagnostics.Process.Start(Website);
        }

        private string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
        }
    }
}
