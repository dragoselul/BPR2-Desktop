using System.Windows;
using BPR2_Desktop.ViewModels;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;

namespace BPR2_Desktop.Views.Pages
{
    public partial class InputDialog : FluentWindow
    {
        
        public string ResponseText => ((InputDialogViewModel)DataContext).ResponseText;
        public InputDialog(string prompt)
        {
            InitializeComponent();
            this.DataContext = new InputDialogViewModel { Prompt = prompt };
        }

        public virtual bool? ShowDialogWrapper()
        {
            return ShowDialog();
        }

        internal void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as InputDialogViewModel;

            if (viewModel?.IsInputValid == true)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a valid input.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        internal void InputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            OKButton.IsEnabled = !string.IsNullOrWhiteSpace(InputTextBox.Text);
        }

    }

}
