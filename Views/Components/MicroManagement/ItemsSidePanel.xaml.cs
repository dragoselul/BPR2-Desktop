using System.Diagnostics;
using System.Windows.Controls;
using BPR2_Desktop.Helpers;
using BPR2_Desktop.ViewModels;
using BPR2_Desktop.ViewModels.MicroManagement;
using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Components.MicroManagement;

public partial class ItemsSidePanel: UserControl
{
    public ItemSidePanelViewModel ViewModel { get; }
    private bool isLoading = false;

    public ItemsSidePanel()
    {
        InitializeComponent();
    }
    
    public ItemsSidePanel(ItemSidePanelViewModel vm)
    {
        DataContext = this;
        ViewModel = vm;
        Task.Run(async () => await LoadComponent());
        InitializeComponent();
    }
    
    private async Task LoadComponent()
    {
        isLoading = true;
        await ViewModel.LoadAllCategories();
        await ViewModel.LoadProducts();
        isLoading = false;
    }

    private async void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        var scrollViewer = e.OriginalSource as ScrollViewer;
        if (scrollViewer != null && scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight && !isLoading)
        {
            isLoading = true;
            await Task.Delay(200); // Add a slight delay to prevent rapid loading
            await ViewModel.LoadProducts();
            isLoading = false;
        }
    }

}