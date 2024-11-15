using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace BPR2_Desktop.ViewModels;

public partial class MacroManagementViewModel: ViewModel
{
    private bool _isInitialized = false;
    
    [ObservableProperty]
    private double _windowWidth = 0;
    
    [ObservableProperty]
    private double _windowHeight = 0;

    [ObservableProperty]
    private string _applicationTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = [];

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = [];

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "Demo"
    )]
    public MacroManagementViewModel(INavigationService navigationService)
    {
        if (!_isInitialized)
        {
            InitializeViewModel();
        }
    }

    private void InitializeViewModel()
    {
        ApplicationTitle = "Dataverse Wizards - Macro Management Store Layout System";

        NavigationItems =
        [
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.Home)
            },
            new NavigationViewItem()
            {
                Content = "Design Editor",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.DesignEditor)
            },
            new NavigationViewItem()
            {
                Content = "Macro Management Designer",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Cube24 },
                TargetPageType = typeof(Views.Pages.MacroManagementDesigner)
            },
            new NavigationViewItem()
            {
                Content = "Data",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataLine24 },
                TargetPageType = typeof(Views.Pages.MicroManagement)
            },
            
        ];

        NavigationFooter =
        [
            // new NavigationViewItem()
            // {
            //     Content = "Settings",
            //     Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            //     TargetPageType = typeof(Views.Pages.SettingsPage)
            // },
        ];

        TrayMenuItems = [new() { Header = "Home", Tag = "tray_home" }];

        _isInitialized = true;
    }
}