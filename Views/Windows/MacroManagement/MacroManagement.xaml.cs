using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace BPR2_Desktop.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MacroManagement : INavigationWindow
{
    public ViewModels.MacroManagement.MacroManagementViewModel ViewModel { get; }

    public MacroManagement()
    {
        InitializeComponent();
    }
    public MacroManagement(ViewModels.MacroManagement.MacroManagementViewModel viewModel, INavigationService navigationService)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
        
        navigationService.SetNavigationControl(RootNavigation);
    }
    
    public INavigationView GetNavigation() => RootNavigation;

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) =>
        RootNavigation.SetPageProviderService(navigationViewPageProvider);

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        Application.Current.Shutdown();
    }

    INavigationView INavigationWindow.GetNavigation()
    {
        throw new NotImplementedException();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
}