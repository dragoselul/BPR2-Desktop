using System.Windows;
using Wpf.Ui.Controls;

namespace BPR2_Desktop.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    private bool _isUserClosedPane;
    private bool _isPaneOpenedOrClosedFromCode;
    public MainWindow()
    {
        InitializeComponent();

        Loaded += (sender, args) =>
        {
            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(
                this, // Window class
                Wpf.Ui.Controls.WindowBackdropType.Auto, // Background type
                true // Whether to change accents automatically
            );
        };
    }
    
    private void OnNavigationSelectionChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not Wpf.Ui.Controls.NavigationView navigationView)
        {
            return;
        }

        // NavigationView.SetCurrentValue(
        //     NavigationView.HeaderVisibilityProperty,
        //     navigationView.SelectedItem?.TargetPageType != typeof(DashboardPage)
        //         ? Visibility.Visible
        //         : Visibility.Collapsed
        // );
    }
    
    private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_isUserClosedPane)
        {
            return;
        }

        _isPaneOpenedOrClosedFromCode = true;
        NavigationView.SetCurrentValue(NavigationView.IsPaneOpenProperty, e.NewSize.Width > 1200);
        _isPaneOpenedOrClosedFromCode = false;
    }

    private void NavigationView_OnPaneOpened(NavigationView sender, RoutedEventArgs args)
    {
        if (_isPaneOpenedOrClosedFromCode)
        {
            return;
        }

        _isUserClosedPane = false;
    }

    private void NavigationView_OnPaneClosed(NavigationView sender, RoutedEventArgs args)
    {
        if (_isPaneOpenedOrClosedFromCode)
        {
            return;
        }

        _isUserClosedPane = true;
    }
}