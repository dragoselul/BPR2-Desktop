using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using BPR2_Desktop.Database;
using BPR2_Desktop.Model;
using BPR2_Desktop.Model.Helpers;
using BPR2_Desktop.ViewModels.UserControls;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ShelfManagerViewModel : ViewModel
{
    [ObservableProperty] private ItemListViewModel _itemListViewModel;
    [ObservableProperty] private ObservableCollection<ModelVisual3D> _sceneObjects;
    [ObservableProperty] private ObservableCollection<string> _shelfNames;
    [ObservableProperty] private string _selectedShelfName;
    private readonly ProductContext _productContext;
    private readonly ShelfContext _shelfContext;
    private List<Point3D> shelfPoints;

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
    private async void OnShelfSelected(string selectedShelfName)
    {
        var shelf = await _shelfContext.GetShelfPropertiesByShelfName(selectedShelfName);
        if (shelf == null)
        {
            return;
        }

        shelfPoints = new List<Point3D>(); // would be good to save the values
        var numberOfShelves = shelf.NumberOfShelves;
        var distanceBetweenShelves = shelf.DistanceBetweenShelves;
        var widthOfShelf = shelf.Dimension.Width;
        var depthOfShelf = shelf.Dimension.Depth;
        var shelveThickness = shelf.ShelveThickness;
        List<ModelVisual3D> shelfBoxes = ShelfBuilder.CreateShelves(numberOfShelves, distanceBetweenShelves,
            widthOfShelf, depthOfShelf, shelveThickness, Colors.White,
            out shelfPoints); // out keyword is used to return multiple values

        Application.Current.Dispatcher.Invoke(() =>
            {
                // Clear existing lines
                SceneObjects.Clear();
                // Add sunlight to the scene for better visualization
                SceneObjects.Add(new SunLight());
                for (int i = 0; i < shelfBoxes.Count; i++)
                {
                    SceneObjects.Add(shelfBoxes[i]);
                }
            }
        );
    }

    private void OnItemSelected(Product selectedShelfName)
    {
        return;
    }
}