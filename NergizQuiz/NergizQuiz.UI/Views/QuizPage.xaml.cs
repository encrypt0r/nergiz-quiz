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

        public Storyboard StartAnimation()
        {
            Storyboard sb =(Storyboard) FindResource("DramaticEntrance");
            BeginStoryboard(sb);
            return sb;
        }
    }
}
