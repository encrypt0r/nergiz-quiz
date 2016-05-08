namespace NergizQuiz.UI.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MahApps.Metro.Controls.MetroWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}
