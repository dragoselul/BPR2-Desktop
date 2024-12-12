using BPR2_Desktop.Views.Pages;
using BPR2_Desktop.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;
using MessageBox = System.Windows.MessageBox;
using MicroManagement = BPR2_Desktop.Views.Windows.MicroManagement;

namespace BPR2_Desktop.ViewModels;

public partial class MainWindowViewModel
{
    private readonly IServiceProvider _serviceProvider;
    public readonly double maxWidth = SystemParameters.WorkArea.Width;
    public readonly double maxHeight = SystemParameters.WorkArea.Height;

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [RelayCommand]
    internal void OnGridMouseLeftButtonDown()
    {
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.DragMove();
    }

    [RelayCommand]
    internal void OnMacromanagementClick()
    {
        var currentWindow = Application.Current.MainWindow;
        var macroManagementWindow = _serviceProvider.GetRequiredService<Views.Windows.MacroManagement>();
        if (currentWindow == null)
        {
            macroManagementWindow.Show();
            return;
        }
        _resizeNewWindow(macroManagementWindow, currentWindow);
        _displayNewWindowAndCloseCurrent(macroManagementWindow, currentWindow);
    }

    internal bool _isFullscreen(Window currentWindow)
    {
        return currentWindow.WindowState == WindowState.Maximized;
    }
    
    internal void _resizeNewWindow(Window newWindow, Window currentWindow)
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

    internal void _displayNewWindow(INavigationWindow newWindow)
    {
        newWindow.ShowWindow();
        newWindow.Navigate(typeof(Views.Pages.Home));
    }
    
    internal void _displayNewWindowAndCloseCurrent(INavigationWindow newWindow, Window currentWindow)
    {
        _displayNewWindow(newWindow);
        currentWindow.Close();
    }

    [RelayCommand]
    internal void OnMicromanagementClick()
    {
        var currentWindow = Application.Current.MainWindow;
        var microManagementWindow = _serviceProvider.GetRequiredService<Views.Windows.MicroManagement>();
        if (currentWindow == null)
        {
            microManagementWindow.Show();
        }
        _resizeNewWindow(microManagementWindow, currentWindow);
        _displayNewWindowAndCloseCurrent(microManagementWindow, currentWindow);
    }

    [RelayCommand]
    internal void OnHelpClick()
    {
        MessageBox.Show("Contact the administrator for help", "Help", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    [RelayCommand]
    internal void OnMaximizeClick()
    {
        Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Normal
            ? WindowState.Maximized
            : WindowState.Normal;
    }

    [RelayCommand]
    internal void OnMinimizeClick()
    {
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    [RelayCommand]
    internal void OnExitClick()
    {
        Application.Current.Shutdown();
    }
}