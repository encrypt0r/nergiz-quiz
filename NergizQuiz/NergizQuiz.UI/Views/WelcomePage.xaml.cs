using System.Windows.Controls;
using System.Windows.Media.Animation;
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
