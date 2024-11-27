using System.Windows.Controls;
using BPR2_Desktop.ViewModels.MicroManagement;

namespace BPR2_Desktop.Views.Components.Micromanagement;

public partial class ItemsSidePanel : UserControl
{
    private bool isLoading = false;
    public ItemsSidePanel()
    {
        InitializeComponent();

        DataContext = new ProductViewModel();
    }
    
    private async void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        var scrollViewer = e.OriginalSource as ScrollViewer;
        if (scrollViewer != null && scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight && !isLoading)
        {
            isLoading = true;
            var viewModel = DataContext as ProductViewModel;
            viewModel?.LoadProducts();
            await Task.Delay(200); // Add a slight delay to prevent rapid loading
            isLoading = false;
        }
    }

}