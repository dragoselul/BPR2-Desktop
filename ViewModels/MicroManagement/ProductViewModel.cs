using BPR2_Desktop.Database;
using BPR2_Desktop.Model;
using BPR2_Desktop.Model.ML;
using BPR2_Desktop.ViewModels.UserControls;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ProductViewModel : ViewModel
{
    [ObservableProperty] private ItemListViewModel _itemListViewModel;

    [ObservableProperty] private ImageDisplayPanelViewModel _imageDisplayViewModel;
    [ObservableProperty] private string productWidth;
    [ObservableProperty] private string productHeight;
    [ObservableProperty] private string productDepth;
    [ObservableProperty] private string storeName;
    [ObservableProperty] private string department;
    [ObservableProperty] private string prediction;

    public ProductViewModel(ProductContext context)
    {
        _itemListViewModel = new ItemListViewModel(context, OnItemSelected);
        _imageDisplayViewModel = new ImageDisplayPanelViewModel(context);
    }

    private void OnItemSelected(Product selectedProduct)
    {
        if (selectedProduct == null)
        {
            ImageDisplayViewModel.ClearImage();
            return;
        }

        // Call a method in ImageDisplayPanelViewModel
        ImageDisplayViewModel.LoadImage(selectedProduct.Main_EAN);
    }
    
    [RelayCommand]
    private async Task PredictButton_Click()
    {
        // Call a method in PredictCategory
        var response = await PredictCategory.PredictButton_Click(ProductWidth, ProductHeight, ProductDepth, StoreName, Department);
        Prediction = response;
    }
}