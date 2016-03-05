using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NergizQuiz.MVVM;
using NergizQuiz.UI.Views;

namespace NergizQuiz.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

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


    }
}
