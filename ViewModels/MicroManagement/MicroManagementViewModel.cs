﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class MicroManagementViewModel: ViewModel
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
    
    public MicroManagementViewModel()
    {
        if (!_isInitialized)
        {
            InitializeViewModel();
        }
    }

    private void InitializeViewModel()
    {
        ApplicationTitle = "Dataverse Wizards - Micro Management Shelfing Layout System";

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
                Content = "Product Viewer",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Database24 },
                TargetPageType = typeof(Views.Pages.MicroManagement.ProductViewer)
            },
            new NavigationViewItem()
            {
                Content = "Shelf Designer",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Cube24 },
                TargetPageType = typeof(Views.Pages.MicroManagement.ShelfDesigner)
            },
        ];

        NavigationFooter =
        [
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.Settings)
            },
        ];

        TrayMenuItems = [new() { Header = "Home", Tag = "tray_home" }];

        _isInitialized = true;
    }
}