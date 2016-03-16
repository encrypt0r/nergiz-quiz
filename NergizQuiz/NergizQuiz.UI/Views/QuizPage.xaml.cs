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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
