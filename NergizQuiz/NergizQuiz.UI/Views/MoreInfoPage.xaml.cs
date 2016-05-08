using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NergizQuiz.UI.Views
{
    /// <summary>
    /// Interaction logic for AnswersPage.xaml
    /// </summary>
    public partial class MoreInfoPage : UserControl, IAnimatedUserControl
    {
        public MoreInfoPage()
        {
            InitializeComponent();
        }

        public Storyboard StartAnimation()
        {
            return null;
        }
    }
}
