using System.Windows.Media;
using System.Windows.Media.Media3D;
using BPR2_Desktop.Model;
using BPR2_Desktop.Model.Enums;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.ViewModels.MacroManagement;

public partial class DesignerViewModel : ViewModel
{
    [ObservableProperty] private List<ModelVisual3D> _sceneObjects;
    public List<Shelf> Shelves { get; set; }
    
    public ModelVisual3D floor { get; set; }
    
    public DesignerViewModel()
    {
        Shelves = new List<Shelf>();
        _sceneObjects = new List<ModelVisual3D>();
        CreateFloor();
        LoadShelf();
    }

    private void CreateFloor()
    {
        var light = new SunLight();
        var floorBuilder = new MeshBuilder();
        floorBuilder.AddBox(new Point3D(0, 0, 0), 100, 100, 0.01);
        var floorMaterial = MaterialHelper.CreateMaterial(Colors.White);
        var floorModel = new GeometryModel3D
        {
            Geometry = floorBuilder.ToMesh(),
            Material = floorMaterial,
            BackMaterial = floorMaterial
        };
        
        floor = new ModelVisual3D
        {
            Content = floorModel
        };
        SceneObjects.Add(light);
        SceneObjects.Add(floor);
    }
    
    public void LoadShelf()
    {
        var shelf = new Shelf(ShelfType.DoubleSided);
        shelf.SetPosition(new Point3D(0,0,0));
        Shelves.Add(shelf);
        SceneObjects.Add(shelf.LoadObjModel());
    }
    
}