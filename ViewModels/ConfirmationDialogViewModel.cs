using System;
using System.Windows;
using BPR2_Desktop.Views.Windows;

namespace BPR2_Desktop.ViewModels
{
    public class ConfirmationDialogViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        public ConfirmationDialogViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ConfirmAndNavigateToMain()
        {
            
            var mainWindow = _serviceProvider.GetService(typeof(MainWindow)) as Window;
            if (mainWindow == null)
                throw new InvalidOperationException("MainWindow service not found.");

            Application.Current.MainWindow = mainWindow;
            Console.WriteLine(_serviceProvider.GetService(typeof(MainWindow))); 
            mainWindow.Show();
        }

        public void CloseDialog(Window dialog)
        {
            dialog?.Close();
        }
    }
}