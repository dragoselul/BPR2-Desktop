using System.Collections.ObjectModel;
using System.Data;
using BPR2_Desktop.Database;
using BPR2_Desktop.Model;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ProductViewModel : ViewModel
{
    private const int PageSize = 100; // Number of rows to fetch at a time
    private int _currentOffset = 0;

    [ObservableProperty] private ObservableCollection<Product> _products;
    [ObservableProperty] private ObservableCollection<Product> _allProducts;
    [ObservableProperty] private ObservableCollection<string> _categories;
    [ObservableProperty] private string _searchQuery;
    [ObservableProperty] private string _selectedCategory;
    
    public ProductViewModel()
    {
        Products = new ObservableCollection<Product>();
        AllProducts = new ObservableCollection<Product>();
        Categories = new ObservableCollection<string>();
        SearchQuery = string.Empty;
        SelectedCategory = "All Categories";
        LoadAllCategories();
        LoadProducts();
    }
    
    partial void OnSearchQueryChanged(string value)
    {
        FilterProducts();
    }
    
    partial void OnSelectedCategoryChanged(string value)
    {
        _currentOffset = 0;
        LoadProducts();
    }

    private async void LoadAllCategories()
    {
        var dbHelper = new DatabaseHelper();
        Task<List<string>> allCategoriesTask = Task.Run(() => dbHelper.GetAllCategories());
        var allCategories = await allCategoriesTask;

        Categories.Add("All Categories"); // Default option
        foreach (var category in allCategories)
        {
            Categories.Add(category);
        }
    }

    public async void LoadProducts()
    {
        var dbHelper = new DatabaseHelper();
        Task<DataTable> dataTask = Task.Run(() => dbHelper.GetProducts(PageSize, _currentOffset, SelectedCategory));
        var dataTable = await dataTask;
        if (_currentOffset == 0)
        {
            Products.Clear();
        }

        foreach (DataRow row in dataTable.Rows)
        {
            var product = new Product
            {
                StoreName = row.Field<string>("store_name") ?? string.Empty,
                Department = row.Field<string>("department") ?? string.Empty,
                Category = row.Field<string>("category") ?? string.Empty,
                MainEAN = row.Field<string>("main_ean") ?? string.Empty,
                ProductName = row.Field<string>("product_name") ?? string.Empty,
                ProductWidth = row.Field<decimal>("product_width"),
                ProductHeight = row.Field<decimal>("product_height"),
                ProductDepth = row.Field<decimal>("product_depth")
            };

            AllProducts.Add(product);
            Products.Add(product);
        }

        _currentOffset += PageSize;
    }

    private void FilterProducts()
    {
        var filtered = AllProducts.Where(p =>
            (string.IsNullOrEmpty(SearchQuery) ||
             p.ProductName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) &&
            (SelectedCategory == "All Categories" || string.IsNullOrEmpty(SelectedCategory) ||
             p.Category == SelectedCategory)).ToList();

        Products.Clear();
        foreach (var product in filtered)
        {
            Products.Add(product);
        }
    }
}