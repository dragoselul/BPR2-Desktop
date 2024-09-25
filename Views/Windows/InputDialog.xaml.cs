using System.Windows;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;

namespace BPR2_Desktop.Views.Pages
{
    public partial class InputDialog : FluentWindow
    {
        public string ResponseText { get; private set; }

        public InputDialog(string prompt)
        {
            InitializeComponent();
            this.Title = prompt;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                ResponseText = InputTextBox.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a design name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            OKButton.IsEnabled = !string.IsNullOrWhiteSpace(InputTextBox.Text);
        }
    }
}
