using System.Windows.Media.Media3D;

namespace BPR2_Desktop.Model;

public class ShelfSection
{
    private Point3D _position { get; set; }
    private List<Product> _products { get; set; }

    public ShelfSection()
    {
        _position = new Point3D(0, 0, 0);
        _products = new List<Product>();
    }
    
    public ShelfSection(Point3D position)
    {
        _position = position;
        _products = new List<Product>();
    }
    
    public Point3D GetPosition() => _position;
    public List<Product> GetProducts() => _products;

    public void AddProduct(Product product) => _products.Add(product);

    public void RemoveProduct(Product product) => _products.Remove(product);

    public void SetPosition(Point3D point) => _position = point;

    public List<string> GetProductsMainEAN() => _products.Select(product => product.Main_EAN).ToList();
}