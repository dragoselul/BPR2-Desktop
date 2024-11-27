using System.Windows.Media.Media3D;
using BPR2_Desktop.Model.Enums;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.Model;

public class Shelf
{
    private Point3D _position { get; set; }
    private ShelfTypes _shelfType { get; set; }
    private ModelVisual3D _shelfGeometry { get; set; }
    
    private Dimensions _dimension { get; set; }
    
    public Shelf(Point3D position, ShelfTypes shelfType)
    {
        _position = position;
        _shelfType = shelfType;
        _shelfGeometry = LoadObjModel();
        _dimension = shelfType.GetShelfSizes();
    }
    
    private ModelVisual3D LoadObjModel()
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
    
    public Point3D GetPosition() => _position;
    
    public void MoveShelf(Point3D point)
    {
        _position = point;
        _shelfGeometry.Transform = new TranslateTransform3D(_position.X, _position.Y, _position.Z);
        
    }
}