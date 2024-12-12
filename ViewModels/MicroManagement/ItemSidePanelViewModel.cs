using System.Collections.ObjectModel;
using BPR2_Desktop.Database;
using BPR2_Desktop.Model;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ItemSidePanelViewModel: ViewModel
{
    private const int PageSize = 100; // Number of rows to fetch at a time
    private int _currentOffset = 0;
    private readonly ProductContext _context;
    [ObservableProperty] private ObservableCollection<Product> _products;
    [ObservableProperty] private ObservableCollection<Product> _allProducts;
    [ObservableProperty] private ObservableCollection<string> _categories;
    [ObservableProperty] private string _searchQuery;
    [ObservableProperty] private string _selectedCategory;
    
    public ItemSidePanelViewModel(ProductContext context)
    {
        _context = context;
        InitializeViewModel();
    }

    public ItemSidePanelViewModel()
    {
        InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        Products = new ObservableCollection<Product>();
        AllProducts = new ObservableCollection<Product>();
        Categories = new ObservableCollection<string>();
        SearchQuery = string.Empty;
        SelectedCategory = "All Categories";
    }
    
    partial void OnSearchQueryChanged(string value)
    {
        FilterProducts();
    }
    
    partial void OnSelectedCategoryChanged(string value)
    {
        _currentOffset = 0;
        if(Categories.Count == 0)
        {
            return;
        }

        Task.Run(async () => await LoadProducts());
    }

    public async Task LoadAllCategories()
    {
        List<string> allCategories = await _context.GetAllCategories();

        Categories.Add("All Categories"); // Default option
        SelectedCategory = Categories[0];
        foreach (var category in allCategories)
        {
            Categories.Add(category);
        }
    }
    public async Task LoadProducts()
    {
        List<Product> products = await _context.GetProducts(PageSize, _currentOffset, SelectedCategory);
        if (_currentOffset == 0)
        {
            Products.Clear();
        }
        foreach (var product in products)
        {
            AllProducts.Add(product);
            Products.Add(product);
        }
        _currentOffset += PageSize;
    }

    internal void FilterProducts()
    {
        var filtered = AllProducts.Where(p =>
            (string.IsNullOrEmpty(SearchQuery) ||
             p.Product_Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) &&
            (SelectedCategory == "All Categories" || string.IsNullOrEmpty(SelectedCategory) ||
             p.Category == SelectedCategory)).ToList();

        Products.Clear();
        foreach (var product in filtered)
        {
            Products.Add(product);
        }
    }
}