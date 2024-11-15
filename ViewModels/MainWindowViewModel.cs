using BPR2_Desktop.Views.Pages;
using BPR2_Desktop.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;
using MessageBox = System.Windows.MessageBox;

namespace BPR2_Desktop.ViewModels;

public partial class MainWindowViewModel
{
    private readonly IServiceProvider _serviceProvider;
    public double maxWidth = SystemParameters.WorkArea.Width;
    public double maxHeight = SystemParameters.WorkArea.Height;

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [RelayCommand]
    private void OnGridMouseLeftButtonDown()
    {
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.DragMove();
    }

    [RelayCommand]
    private void OnMacromanagementClick()
    {
        var currentWindow = Application.Current.MainWindow;
        var macroManagementWindow = _serviceProvider.GetRequiredService<MacroManagement>();
        if (currentWindow == null)
        {
            macroManagementWindow.Show();
            return;
        }
        _resizeNewWindow(macroManagementWindow, currentWindow);
        _displayNewWindowAndCloseCurrent(macroManagementWindow, currentWindow);
    }

    private bool _isFullscreen(Window currentWindow)
    {
        return currentWindow.WindowState == WindowState.Maximized;
    }
    
    private void _resizeNewWindow(Window newWindow, Window currentWindow)
    {
        newWindow.Width = currentWindow.Width;
        newWindow.Height = currentWindow.Height;
        if (_isFullscreen(currentWindow))
        {
            newWindow.Width = maxWidth;
            newWindow.Height = maxHeight;
            newWindow.WindowState = WindowState.Maximized;
        }
    }

    private void _displayNewWindow(INavigationWindow newWindow)
    {
        newWindow.ShowWindow();
        newWindow.Navigate(typeof(Views.Pages.Home));
    }
    
    private void _displayNewWindowAndCloseCurrent(INavigationWindow newWindow, Window currentWindow)
    {
        _displayNewWindow(newWindow);
        currentWindow.Close();
    }

    [RelayCommand]
    private void OnMicromanagementClick()
    {
        var currentWindow = Application.Current.MainWindow;
        var microManagementWindow = _serviceProvider.GetRequiredService<MicroManagement>();
        if (currentWindow == null)
        {
            // microManagementWindow.Show();
            return;
        }
        // _resizeNewWindow(microManagementWindow, currentWindow);
        // _displayNewWindowAndCloseCurrent(microManagementWindow, currentWindow);
    }

    [RelayCommand]
    private void OnHelpClick()
    {
        MessageBox.Show("Why are you gay and press random buttons?", "U Gay", MessageBoxButton.YesNo,
            MessageBoxImage.Stop, MessageBoxResult.Yes);
        MessageBox.Show("We just stole your bank account information. GL&HF");
    }

    [RelayCommand]
    private void OnMaximizeClick()
    {
        Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Normal
            ? WindowState.Maximized
            : WindowState.Normal;
    }

    [RelayCommand]
    private void OnMinimizeClick()
    {
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    [RelayCommand]
    private void OnExitClick()
    {
        Application.Current.Shutdown();
    }
}