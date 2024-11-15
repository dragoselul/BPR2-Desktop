using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using YourNamespace;

namespace BPR2_Desktop.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private const int PageSize = 100; // Number of rows to fetch at a time
        private int CurrentOffset = 0;
        public ObservableCollection<Product> Products { get; set; }
        private ObservableCollection<Product> AllProducts { get; set; }
        
        public ObservableCollection<string> Categories { get; set; } 

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                FilterProducts();
            }
        }
        
        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                
                CurrentOffset = 0;
                LoadProducts();
            }
        }

        public ProductViewModel()
        {
            Products = new ObservableCollection<Product>();
            AllProducts = new ObservableCollection<Product>();
            Categories = new ObservableCollection<string>();

            SelectedCategory = "All Categories";
            LoadAllCategories();
            LoadProducts();
        }
        
        private void LoadAllCategories()
        {
            var dbHelper = new DatabaseHelper();
            List<string> allCategories = [];
            var dbThread = new Thread(() => allCategories = dbHelper.GetAllCategories());

            Categories.Add("All Categories"); // Default option
            foreach (var category in allCategories)
            {
                Categories.Add(category);
            }
        }

        public void LoadProducts()
        {
            var dbHelper = new DatabaseHelper();
            DataTable dataTable = new DataTable();
            var dbThread = new Thread(() => dataTable = dbHelper.GetProducts(PageSize, CurrentOffset, SelectedCategory));
            
            if (CurrentOffset == 0)
            {
                Products.Clear();
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var product = new Product
                {
                    StoreName = row["store_name"].ToString(),
                    Department = row["department"].ToString(),
                    Category = row["category"].ToString(),
                    MainEAN = row["main_ean"].ToString(),
                    ProductName = row["product_name"].ToString(),
                    ProductWidth = double.Parse(row["product_width"].ToString()),
                    ProductHeight = double.Parse(row["product_height"].ToString()),
                    ProductDepth = double.Parse(row["product_depth"].ToString())
                };

                AllProducts.Add(product);
                Products.Add(product);
            }

            CurrentOffset += PageSize;
        }
        
        private void FilterProducts()
        {
            var filtered = AllProducts.Where(p =>
                (string.IsNullOrEmpty(SearchQuery) || p.ProductName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) &&
                (SelectedCategory == "All Categories" || string.IsNullOrEmpty(SelectedCategory) || p.Category == SelectedCategory)).ToList();

            Products.Clear();
            foreach (var product in filtered)
            {
                Products.Add(product);
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Product
    {
        public string StoreName { get; set; }
        public string Department { get; set; }
        public string Category { get; set; }
        public string MainEAN { get; set; }
        public string ProductName { get; set; }
        public double ProductWidth { get; set; }
        public double ProductHeight { get; set; }
        public double ProductDepth { get; set; }
    }
}