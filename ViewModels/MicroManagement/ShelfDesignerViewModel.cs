using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using BPR2_Desktop.Model;
using BPR2_Desktop.Model.Enums;
using BPR2_Desktop.Model.Helpers;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ShelfDesignerViewModel : ViewModel
{
    [ObservableProperty] private List<ShelfTypes> _shelfs;
    [ObservableProperty] private ObservableCollection<ModelVisual3D> _sceneObjects;
    [ObservableProperty] private ShelfTypes _selectedItem = ShelfTypes.DoubleSided;
    [ObservableProperty] private string _shelfName;
    [ObservableProperty] private int _numberOfShelves;
    [ObservableProperty] private double _distanceBetweenShelves;
    [ObservableProperty] private double _heightOfShelf;
    [ObservableProperty] private double _widthOfShelf;
    [ObservableProperty] private double _depthOfShelf;
    [ObservableProperty] private double _shelveThickness;
    [ObservableProperty] private bool _canGenerateShelfLines;
    private Shelf _shelf;


    public ShelfDesignerViewModel()
    {
        InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        SceneObjects = new ObservableCollection<ModelVisual3D>();
        Shelfs = GetShelfs();
        _shelf = new Shelf(ShelfTypes.DoubleSided); // Default shelf type for now
    }

    private List<ShelfTypes> GetShelfs()
    {
        return Enum.GetValues(typeof(ShelfTypes)).Cast<ShelfTypes>().ToList();
    }

    [RelayCommand]
    private void GenerateShelfLines()
    {
        //Can we generate shelf lines?
        HeightOfShelf = NumberOfShelves * (DistanceBetweenShelves + ShelveThickness);
        CanGenerateShelfLines = CanGenerateShelves();
        if (!CanGenerateShelfLines)
            return;

        List<ModelVisual3D> shelfBoxes = ShelfBuilder.CreateShelves(NumberOfShelves, DistanceBetweenShelves, WidthOfShelf,
            DepthOfShelf, ShelveThickness, Colors.White);

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
        SetShelfProperties();
    }

    private void SetShelfProperties()
    {
        var dimensions = new Dimensions(WidthOfShelf, HeightOfShelf, DepthOfShelf);
        var shelfProperties = new ShelfProperties(dimensions, NumberOfShelves, DistanceBetweenShelves, ShelveThickness);
        _shelf.SetProperties(shelfProperties);
    }

    [RelayCommand]
    private void SaveShelf()
    {
        var objects = SceneObjects.Skip(1).ToList();
        _shelf.CreateMergedObject(objects);
        _shelf.SaveAsObj(ShelfName);
    }
    
    public bool CanGenerateShelves()
    {
        if (DistanceBetweenShelves <= 0)
            return false;
        if (ShelveThickness <= 0)
            return false;
        if (WidthOfShelf <= 0)
            return false;
        if (HeightOfShelf <= 0)
            return false;
        if (NumberOfShelves <= 0)
            return false;
        if (DepthOfShelf <= 0)
            return false;
        return true;
    }
}