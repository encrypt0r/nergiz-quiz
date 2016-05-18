using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
namespace NergizQuiz.UI.Views
{
    /// <summary>
    /// Interaction logic for QuizPage.xaml
    /// </summary>
    public partial class QuizPage : UserControl, IAnimatedUserControl
    {
        public QuizPage()
        {
            InitializeComponent();
        }

        private void CurrentSession_LittleTimeRemaining(object sender, EventArgs e)
        {
            Storyboard sb = (Storyboard)FindResource("HeartBeat");
            BeginStoryboard(sb);
        }

        public Storyboard StartAnimation()
        {
            Storyboard sb =(Storyboard) FindResource("DramaticEntrance");
            BeginStoryboard(sb);
            return sb;
        }

        private void UserControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ViewModels.MainWindowVM)
                (DataContext as ViewModels.MainWindowVM).CurrentSession.LittleTimeRemaining += CurrentSession_LittleTimeRemaining;
        }
    }
}
