using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using BPR2_Desktop.Database;
using BPR2_Desktop.Model;
using BPR2_Desktop.Model.Enums;
using BPR2_Desktop.Model.Helpers;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ShelfDesignerViewModel : ViewModel
{
    [ObservableProperty] private List<ShelfType> _shelfs;
    [ObservableProperty] private ObservableCollection<ModelVisual3D> _sceneObjects;
    [ObservableProperty] private ShelfType _selectedItem = ShelfType.DoubleSided;
    [ObservableProperty] private string _shelfName;
    [ObservableProperty] private int _numberOfShelves;
    [ObservableProperty] private double _distanceBetweenShelves;
    [ObservableProperty] private double _heightOfShelf;
    [ObservableProperty] private double _widthOfShelf;
    [ObservableProperty] private double _depthOfShelf;
    [ObservableProperty] private double _shelveThickness;
    [ObservableProperty] private bool _canGenerateShelfLines;
    private Shelf _shelf;
    private readonly ShelfContext _context;


    public ShelfDesignerViewModel(ShelfContext shelfContext)
    {
        _context = shelfContext;
        InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        SceneObjects = new ObservableCollection<ModelVisual3D>();
        Shelfs = GetShelfs();
        _shelf = new Shelf(ShelfType.DoubleSided); // Default shelf type for now
    }

    private List<ShelfType> GetShelfs()
    {
        return Enum.GetValues(typeof(ShelfType)).Cast<ShelfType>().ToList();
    }

    [RelayCommand]
    private void GenerateShelfLines()
    {
        //Can we generate shelf lines?
        HeightOfShelf = NumberOfShelves * (DistanceBetweenShelves + ShelveThickness);
        CanGenerateShelfLines = CanGenerateShelves();
        if (!CanGenerateShelfLines)
            return;
        List<Point3D> shelfPoints = new List<Point3D>(); // would be good to save the values
        List<ModelVisual3D> shelfBoxes = ShelfBuilder.CreateShelves(NumberOfShelves, DistanceBetweenShelves, WidthOfShelf,
            DepthOfShelf, ShelveThickness, Colors.White, out shelfPoints); // out keyword is used to return multiple values

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
        SetShelfProperties(shelfPoints);
    }

    private void SetShelfProperties(List<Point3D> shelfPoints)
    {
        var dimensions = new Dimensions(WidthOfShelf, HeightOfShelf, DepthOfShelf);
        var shelfProperties = new ShelfProperties(dimensions, NumberOfShelves, DistanceBetweenShelves, ShelveThickness);
        _shelf.SetProperties(shelfProperties);
        var shelfSections = new List<ShelfSection>();
        for (int i = 0; i < shelfPoints.Count; i++)
        {
            shelfSections.Add(new ShelfSection(shelfPoints[i]));
        }
        _shelf.SetShelfSections(shelfSections);
        _shelf.SetShelfName(ShelfName);
        _shelf.SetShelfType(ShelfType.Custom); // For now as custom
    }

    [RelayCommand]
    private void SaveShelf()
    {
        var objects = SceneObjects.Skip(1).ToList();
        _shelf.CreateMergedObject(objects);
        _shelf.SaveAsObj(ShelfName);
        Task.Run(() => SaveShelfInDatabase());
    }
    
    private async Task SaveShelfInDatabase()
    {
        await _context.AddShelf(_shelf);
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