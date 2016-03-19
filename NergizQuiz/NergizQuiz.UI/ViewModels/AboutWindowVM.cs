using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NergizQuiz.MVVM;
using System.Windows.Input;
using System.Reflection;

namespace NergizQuiz.UI.ViewModels
{
    class AboutWindowVM : ObservableObject
    {


        public string GitHub
        {
            get { return "https://github.com/encrypt0r/nergiz-quiz"; }
        }
        public string AppVersion
        {
            get { return "Version " + GetVersion(); }
        }

        private ICommand m_OpenGitHub;
        public ICommand OpenGitHub
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

        private string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
        }
    }
}
