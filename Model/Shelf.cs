using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using BPR2_Desktop.Model.Enums;
using BPR2_Desktop.Model.Helpers;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.Model;

public class Shelf
{
    private ShelfType _shelfType { get; set; }
    private string _shelfName { get; set; }
    private Point3D _position { get; set; }
    private float _rotation { get; set; }
    private ShelfProperties _properties { get; set; }
    private ModelVisual3D _shelfModel { get; set; }
    private List<ShelfSection> _shelfSections { get; set; }

    public Shelf(ShelfType shelfType)
    {
        _position = new Point3D(0, 0, 0);
        _shelfType = shelfType;
        _shelfModel = LoadObjModel();
        _properties = new ShelfProperties();
        _shelfSections = new List<ShelfSection>();
        _rotation = 0;
    }

    public ModelVisual3D LoadObjModel()
    {
        // Create a new importer
        var importer = new ObjReader();
        // Load the 3D model from the file
        Model3DGroup model = importer.Read(_shelfType.GetPathLocation());

        // Create a ModelVisual3D to hold the 3D model
        ModelVisual3D modelVisual = new ModelVisual3D
        {
            Content = model
        };
        return modelVisual;
    }
    
    public ShelfType GetShelfType() => _shelfType;

    public Point3D GetPosition() => _position;
    public void SetPosition(Point3D point) => _position = point;

    public void MoveShelf(Point3D point)
    {
        _position = point;
        _shelfModel.Transform = new TranslateTransform3D(_position.X, _position.Y, _position.Z);
    }

    public void SetProperties(ShelfProperties properties) => _properties = properties;
    
    public void SetShelfName(string name) => _shelfName = name;
    
    public void SetShelfSections(List<ShelfSection> sections) => _shelfSections = sections;
    
    public void SetShelfType(ShelfType type) => _shelfType = type;

    public void CreateMergedObject(List<ModelVisual3D> models)
    {
        var sidesAndBack = GetShelfSidesAndBack();
        sidesAndBack.AddRange(models);
        ModelVisual3D mergedModel = ObjExporter3D.MergeModels(sidesAndBack);
        _shelfModel = mergedModel;
    }

    public void SaveAsObj(string name, string? path = null)
    {
        if (path == null)
        {
            path = AppContext.BaseDirectory + $"../../../3DObjects/";
        }

        if (_shelfModel == null)
        {
            throw new InvalidOperationException("The shelf model is not initialized.");
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("The provided path is invalid.", nameof(path));
        }

        try
        {
            Model3D model = _shelfModel.Content;
            ObjExporter3D.Export(model, path, name);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to export model: {ex.Message}");
            throw; // Re-throwing to let higher layers handle it if needed
        }
    }

    public List<ModelVisual3D> GetShelfSidesAndBack()
    {
        // Define shelf dimensions
        List<ModelVisual3D> shelfParts = ShelfBuilder.GetShelfSidesAndBack(_properties.Dimension,
            _properties.ShelveThickness, _properties.ShelveThickness);
        return shelfParts;
    }

    public string GetShelfName() => _shelfName;
    
    public ShelfProperties GetProperties() => _properties;
    
    public List<ShelfSection> GetShelfSections() => _shelfSections;
}