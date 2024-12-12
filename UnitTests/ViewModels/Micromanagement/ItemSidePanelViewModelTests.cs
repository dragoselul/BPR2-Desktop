using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BPR2_Desktop.Database;
using BPR2_Desktop.Model;
using BPR2_Desktop.ViewModels.MicroManagement;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class ItemSidePanelViewModelTests
{
    [WpfFact]
    public void ItemSidePanelViewModel_ShouldInitializeWithDefaultValues()
    {
        // Arrange
        var viewModel = new ItemSidePanelViewModel();

        // Act & Assert
        Assert.NotNull(viewModel.Products);
        Assert.NotNull(viewModel.AllProducts);
        Assert.NotNull(viewModel.Categories);
        Assert.Equal("All Categories", viewModel.SelectedCategory);
        Assert.Equal(string.Empty, viewModel.SearchQuery);
    }

    [WpfFact]
    public void FilterProducts_ShouldFilterProductsBasedOnSearchQueryAndCategory()
    {
        // Arrange
        var products = new ObservableCollection<Product>
        {
            new Product
            {
                Store_Name = "Bilka",
                Department = "Beer_water",
                Product_Name = "Product1",
                Category = "Category1",
                Main_EAN = "5005595481",
                Product_Depth = 12,
                Product_Height = 20,
                Product_Width = 22
            },
            new Product
            {
                Store_Name = "Bilka",
                Department = "Beer_water",
                Product_Name = "Product2",
                Category = "Category2",
                Main_EAN = "5005595482",
                Product_Depth = 12,
                Product_Height = 20,
                Product_Width = 22
            },
            new Product
            {
                Store_Name = "Bilka",
                Department = "Beer_water",
                Product_Name = "AnotherProduct",
                Category = "Category3",
                Main_EAN = "5005595483",
                Product_Depth = 12,
                Product_Height = 20,
                Product_Width = 22
            }
        };

        var viewModel = new ItemSidePanelViewModel
        {
            AllProducts = products,
            SearchQuery = "Product",
            SelectedCategory = "Category3"
        };

        // Act
        viewModel.FilterProducts();

        // Assert
        Assert.Single(viewModel.Products);
        Assert.Equal("AnotherProduct", viewModel.Products.First().Product_Name);
    }
    

    /*[WpfFact]
    public async Task LoadProducts_ShouldPopulateProducts()
    {
        // Arrange
        var viewModel = new ItemSidePanelViewModel();

        // Act
        await viewModel.LoadProducts();

        // Assert
        Assert.NotEmpty(viewModel.Products);
    }
    */
    
    /*[Fact]
    public async Task LoadAllCategories_ShouldIterateAndAddCategories()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new ProductContext(options);

        // Seed the database with test categories
        context.Categories.AddRange(
            new Category { Name = "Category1" },
            new Category { Name = "Category2" },
            new Category { Name = "Category3" }
        );
        await context.SaveChangesAsync();

        var viewModel = new ItemSidePanelViewModel(context);

        // Act
        await viewModel.LoadAllCategories();

        // Assert
        Assert.Equal(4, viewModel.Categories.Count); // "All Categories" + 3 test categories
        Assert.Equal("All Categories", viewModel.Categories[0]);
        Assert.Contains("Category1", viewModel.Categories);
        Assert.Contains("Category2", viewModel.Categories);
        Assert.Contains("Category3", viewModel.Categories);
    }
    */




    
    
}
