using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NergizQuiz.UI.Views
{
    /// <summary>
    /// Interaction logic for FinishPage.xaml
    /// </summary>
    public partial class FinishPage : UserControl, IAnimatedUserControl
    {
        public FinishPage()
        {
            InitializeComponent();
        }

        public Storyboard StartAnimation()
        {
            return null;
        }
    }
}
