using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NergizQuiz.UI.ViewModels;
namespace NergizQuiz.UI
{
    public static class GlobalInfo
    {
        static public MainWindowViewModel TheViewModel;
        public static void Initialize()
        {
            TheViewModel = new MainWindowViewModel();
        }
    }
}
