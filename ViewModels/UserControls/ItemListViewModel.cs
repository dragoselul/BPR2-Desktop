using System.Collections.ObjectModel;
using System.Windows.Threading;
using BPR2_Desktop.Database;
using BPR2_Desktop.Model;

namespace BPR2_Desktop.ViewModels.UserControls;

public partial class ItemListViewModel : ViewModel
{
    private const int PageSize = 100; // Number of rows to fetch at a time
    private int _currentOffset = 0;
    private readonly ProductContext _context;
    [ObservableProperty] private ObservableCollection<Product> _products;
    [ObservableProperty] private ObservableCollection<Product> _allProducts;
    [ObservableProperty] private ObservableCollection<string> _categories;
    [ObservableProperty] private string _searchQuery;
    [ObservableProperty] private string _selectedCategory;
    [ObservableProperty] private Product _selectedProduct;
    [ObservableProperty] private Action<Product> _onSelectedProduct;

    public ItemListViewModel(ProductContext context, Action<Product> onSelectedProduct)
    {
        _context = context;
        _onSelectedProduct = onSelectedProduct;
        InitializeViewModel();
    }

    public ItemListViewModel()
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
        if (string.IsNullOrEmpty(value))
        {
            UpdateProducts(AllProducts.ToList());
            return;
        }
        
        Application.Current.Dispatcher.Invoke( async () =>
        {
            List<Product> products = await FilterProducts(value);
            UpdateProducts(products);
        });
    }
    
    public void UpdateProducts(List<Product> products)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        });
    }

    partial void OnSelectedCategoryChanged(string value)
    {
        _currentOffset = 0;
        if (_categories.Count == 0)
        {
            return;
        }

        Task.Run(async () => await LoadProducts());
    }

    public async Task LoadAllCategories()
    {
        try
        {
            // Fetch categories asynchronously - this will already run on a background thread
            var allCategories = await _context.GetAllCategories();

            // Use Dispatcher to update UI-bound collection on the main thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                Categories.Clear();
                Categories.Add("All Categories"); // Default option
                SelectedCategory = Categories[0]; // Set the default selection

                // Now, add the fetched categories
                foreach (var category in allCategories)
                {
                    Categories.Add(category);
                }
            });
        }
        catch (Exception ex)
        {
            // Handle any exceptions here, such as logging
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    public async Task LoadProducts()
    {
        try
        {
            // Fetch products asynchronously - runs on a background thread
            List<Product> products = await _context.GetProducts(PageSize, _currentOffset, SelectedCategory);

            // Use the Dispatcher to update the UI-bound collections on the main thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_currentOffset == 0)
                {
                    Products.Clear(); // Clear Products collection if we're starting fresh
                }

                foreach (var product in products)
                {
                    AllProducts.Add(product);
                    Products.Add(product);
                }

                _currentOffset += PageSize; // Update the offset for pagination
            });
        }
        catch (Exception ex)
        {
            // Handle any exceptions, such as logging
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    internal async Task<List<Product>> FilterProducts(string name)
    {
        List<Product> products = await _context.GetProductsByName(name);
        return products;
    }
}