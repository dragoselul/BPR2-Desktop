using BPR2_Desktop.ViewModels;

namespace BPR2_Desktop.Views.Windows
{
    public partial class ConfirmationDialog : Window
    {
        private readonly ConfirmationDialogViewModel _viewModel;

        public ConfirmationDialog(ConfirmationDialogViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        internal void YesButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ConfirmAndNavigateToMain();
            this.Close();
        }

        internal void NoButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CloseDialog(this);
        }
    }
}