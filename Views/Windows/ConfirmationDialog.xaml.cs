using System.Windows;

namespace BPR2_Desktop.Views.Windows
{
    public partial class ConfirmationDialog : Window
    {
        public ConfirmationDialog()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            // var openingScreen = new MainWindow();
            // Application.Current.MainWindow = openingScreen;
            // openingScreen.Show();
            //
            // this.Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}