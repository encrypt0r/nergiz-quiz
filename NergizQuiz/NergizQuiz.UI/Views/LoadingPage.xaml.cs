using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NergizQuiz.UI.Views
{
    /// <summary>
    /// Interaction logic for LoadingPage.xaml
    /// </summary>
    public partial class LoadingPage : UserControl, IAnimatedUserControl
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        public Storyboard StartAnimation()
        {
            throw new NotImplementedException();
        }
    }
}
