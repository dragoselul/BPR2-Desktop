using BPR2_Desktop.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BPR2_Desktop.Database;

public class ProductContext : DbContext
{
    private readonly string _schema;
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

    public ProductContext(DbContextOptions<ProductContext> options, IOptions<DatabaseConfig> dbConfig) : base(options)
    {
        _schema = dbConfig.Value.Schema ?? "public";
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>()
            .ToTable("products", _schema);
        modelBuilder.Entity<ProductImage>()
            .ToTable("ProductImages", _schema);
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

    public async Task<List<Product>> GetProductsByName(string name)
    {
            var upperName = $"%{name.ToUpper()}%"; // Prepare the search string with UPPER
            return await Products
                .Where(p => EF.Functions.Like(p.Product_Name.ToUpper(), upperName))
                .ToListAsync();
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