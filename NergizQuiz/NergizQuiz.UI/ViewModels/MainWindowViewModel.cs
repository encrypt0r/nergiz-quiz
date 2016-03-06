using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NergizQuiz.MVVM;
using NergizQuiz.UI.Views;
using System.Windows.Input;

namespace NergizQuiz.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Public Properties
        private object  m_Page = new Welcome();
        public object  Page
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
                return true ;
            else
                return false;
        }
        #endregion

    }
}
