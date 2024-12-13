using System.Windows.Media.Media3D;

namespace BPR2_Desktop.Model;

public class ShelfSection
{
    private int _id { get; init; }
    private Point3D _position { get; set; }
    private List<Product> _products { get; set; }

    public ShelfSection(int id)
    {
        _id = id;
        _position = new Point3D(0, 0, 0);
        _products = new List<Product>();
    }

    public void AddProduct(Product product) => _products.Add(product);

    public void RemoveProduct(Product product) => _products.Remove(product);

    public void SetPosition(Point3D point) => _position = point;

    public List<string> GetProductsMainEAN() => _products.Select(product => product.Main_EAN).ToList();
}