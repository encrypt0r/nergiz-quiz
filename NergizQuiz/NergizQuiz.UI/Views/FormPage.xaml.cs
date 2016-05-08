using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NergizQuiz.UI.Views
{
    /// <summary>
    /// Interaction logic for FormPage.xaml
    /// </summary>
    public partial class FormPage : UserControl, IAnimatedUserControl
    {
        public FormPage()
        {
            InitializeComponent();
        }

        public Storyboard StartAnimation()
        {
            throw new NotImplementedException();
        }
    }
}
