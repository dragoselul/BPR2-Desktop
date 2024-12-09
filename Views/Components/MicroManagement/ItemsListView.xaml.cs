using System.Diagnostics;
using System.Windows.Controls;
using BPR2_Desktop.ViewModels;
using BPR2_Desktop.ViewModels.UserControls;

namespace BPR2_Desktop.Views.Components.MicroManagement;

public partial class ItemsListView : UserControl
{
    public ItemListViewModel ViewModel { get; set; }
    private bool isLoading = false;

    public ItemsListView()
    {
        // // or Option 2: Handle Loaded event: nice to know that it only runs once
        Loaded += ItemsListView_Loaded; 
        InitializeComponent();
    }

    

    private void ItemsListView_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ItemListViewModel vm) return;
        ViewModel = vm;
        DataContext = this;
        Task.Run(async () => await LoadComponent());
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
        if (ViewModel == null)
            return;
        var scrollViewer = e.OriginalSource as ScrollViewer;
        if (scrollViewer != null && scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight && !isLoading)
        {
            isLoading = true;
            await Task.Delay(200); // Add a slight delay to prevent rapid loading
            await ViewModel.LoadProducts();
            isLoading = false;
        }
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.OnSelectedProduct(ViewModel.SelectedProduct);
    }
}

// TODO: remove this comment later, since this is nice to know
// Option 1: Handle DataContextChanged event: runs multiple times depending on the DataContext changes
// DataContextChanged += ItemsListView_DataContextChanged;

// private void ItemsListView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
// {
//     if (DataContext is not ItemListViewModel vm) return;
//     ViewModel = vm;
//     Task.Run(async () => await LoadComponent());
// }