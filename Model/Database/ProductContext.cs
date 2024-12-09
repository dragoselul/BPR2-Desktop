using System.Diagnostics;
using System.Transactions;
using System.Windows.Documents;
using BPR2_Desktop.Model;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

namespace BPR2_Desktop.Database;

public class ProductContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }

    public Task<List<string>> GetAllCategories()
    {
        return Products
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    public Task<List<Product>> GetProductsByCategory(string category)
    {
        return Products
            .Where(p => p.Category == category)
            .ToListAsync();
    }

    public Task<Product?> GetProductByEAN(string ean)
    {
        return Products
            .FirstOrDefaultAsync(p => p.Main_EAN == ean);
    }

    public Task<List<Product>> GetProductsByDepartment(string department)
    {
        return Products
            .Where(p => p.Department == department)
            .ToListAsync();
    }

    public Task<List<Product>> GetAllProducts(string department)
    {
        return Products.ToListAsync();
    }

    public Task<List<Product>> GetProducts(int pageSize = 100, int offset = 0, string category = null)
    {
        IQueryable<Product> query = Products;

        // Apply filtering if category is provided
        if (!string.IsNullOrEmpty(category) && category != "All Categories")
        {
            query = query.Where(p => p.Category == category);
        }
        

        // Apply pagination
        query = query.OrderBy(p => p.Main_EAN)
            .Skip(offset * pageSize)
            .Take(pageSize);
        Task<List<Product>> list = query.ToListAsync();
        return list;
    }
    
    public Task<List<ProductImage>> GetImagesForProducts(List<string> eans)
    {
        return ProductImages
            .Where(pi => eans.Contains(pi.Main_EAN))
            .ToListAsync();
    }
    
    public Task<ProductImage?> GetImageByEAN(string ean)
    {
        return ProductImages.FirstOrDefaultAsync(pi => pi.Main_EAN == ean);
    }
}