using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace BPR2_Desktop.Views.Windows;

public partial class MicroManagement : INavigationWindow
{
    public ViewModels.MicroManagement.MicroManagementViewModel ViewModel { get; }
    public MicroManagement(ViewModels.MicroManagement.MicroManagementViewModel viewModel, INavigationService navigationService)
    {
        ViewModel = viewModel;
        DataContext = this;
        Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);
        InitializeComponent();
        navigationService.SetNavigationControl(RootNavigation);
    }
    
    public INavigationView GetNavigation() => RootNavigation;
    
    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);
    
    
    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }

    public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) =>
        RootNavigation.SetPageProviderService(navigationViewPageProvider);

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();
}