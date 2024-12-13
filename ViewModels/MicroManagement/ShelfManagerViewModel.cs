using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using BPR2_Desktop.Database;
using BPR2_Desktop.Model;
using BPR2_Desktop.ViewModels.UserControls;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ShelfManagerViewModel : ViewModel
{
    [ObservableProperty] private ItemListViewModel _itemListViewModel;
    [ObservableProperty] private ObservableCollection<ModelVisual3D> _sceneObjects;
    [ObservableProperty] private ObservableCollection<string> _shelfNames;
    [ObservableProperty] private string _selectedShelfName;
    private readonly ProductContext _productContext;
    private readonly ShelfContext _shelfContext;

    public ShelfManagerViewModel(ShelfContext shelfContext, ProductContext productContext)
    {
        _shelfContext = shelfContext;
        _productContext = productContext;
        InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        ItemListViewModel = new ItemListViewModel(_productContext, OnItemSelected);
        LoadShelfNames();
        SceneObjects = new ObservableCollection<ModelVisual3D>();
    }

    private void LoadShelfNames()
    {
        Application.Current.Dispatcher.Invoke(async () =>
        {
            var shelfNames = await _shelfContext.GetShelfNames();
            ShelfNames = new ObservableCollection<string>(shelfNames);
        });
    }
    
    [RelayCommand]
    private void OnShelfSelected(string selectedShelfName)
    {
        return;
    }

    private void OnItemSelected(Product selectedShelfName)
    {
        return;
    }
}