using BPR2_Desktop.Database;
using BPR2_Desktop.Model;
using BPR2_Desktop.ViewModels.UserControls;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ProductViewModel : ViewModel
{
    [ObservableProperty]
    private ItemListViewModel _itemListViewModel;

    [ObservableProperty]
    private ImageDisplayPanelViewModel _imageDisplayViewModel;
    
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
}