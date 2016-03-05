using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace NergizQuiz.UI
{
    static class UserInfo
    {
        private static string m_UserName;
        public static string UserName
        {
            get { return m_UserName; }
            set
            {
                m_UserName = value;
            }
        }
    }
}
