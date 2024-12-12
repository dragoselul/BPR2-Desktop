using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using BPR2_Desktop.Model.Enums;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ShelfDesignerViewModel : ViewModel
{
    [ObservableProperty] private List<ShelfTypes> _shelfs;
    [ObservableProperty] private List<ModelVisual3D> _sceneObjects;
    [ObservableProperty] private ShelfTypes _selectedItem = ShelfTypes.DoubleSided;
    [ObservableProperty] private int _numberOfShelves = 5;
    [ObservableProperty] private double _distanceBetweenShelves = 20;
    [ObservableProperty] private double _heightOfShelf = 200;
    [ObservableProperty] private double _widthOfShelf = 200;
    [ObservableProperty] private double _shelveThickness = 3;


    public ShelfDesignerViewModel()
    {
        InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        SceneObjects = new List<ModelVisual3D>();
        Shelfs = GetShelfs();
    }

    private List<ShelfTypes> GetShelfs()
    {
        return Enum.GetValues(typeof(ShelfTypes)).Cast<ShelfTypes>().ToList();
    }

    [RelayCommand]
    private void GenerateShelfLines()
    {
        //Can we generate shelf lines?
        if (!CanGenerateShelfLines())
            return;
        // Clear existing lines
        SceneObjects.Clear();
        // Add sunlight to the scene for better visualization
        SceneObjects.Add(new SunLight());

        for (int i = 0; i < NumberOfShelves; i++)
        {
            double yPosition = i * (DistanceBetweenShelves + ShelveThickness);

            // Create a fresh mesh builder for each shelf
            var meshBuilder = new MeshBuilder();

            // AddBox requires the center point and size for the box
            meshBuilder.AddBox(
                center: new Point3D(WidthOfShelf / 2, yPosition, 0), // Center point of the box
                xlength: WidthOfShelf, // Length in the X direction (Width of the shelf)
                ylength: ShelveThickness, // Length in the Y direction (Thickness of the shelf)
                zlength: 0.3 // Length in the Z direction (Depth of the shelf, arbitrarily set to 0.3)
            );

            // Convert the mesh to a GeometryModel3D and add materials
            var boxMesh = meshBuilder.ToMesh();
            var boxMaterial = MaterialHelper.CreateMaterial(Colors.White);

            var shelfBox = new GeometryModel3D
            {
                Geometry = boxMesh,
                Material = boxMaterial,
                BackMaterial = boxMaterial
            };

            // Add the shelf box as a ModelVisual3D to the Lines collection
            SceneObjects.Add(new ModelVisual3D
            {
                Content = shelfBox
            });
        }
    }

    internal bool CanGenerateShelfLines()
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
        return true;
    }
}