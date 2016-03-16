using NergizQuiz.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace NergizQuiz.UI.Views
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class WelcomePage : UserControl, IAnimatedUserControl
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        public Storyboard StartAnimation()
        {
            Storyboard sb = (Storyboard)FindResource("DramaticExit");
            return sb;
        }
    }
}
