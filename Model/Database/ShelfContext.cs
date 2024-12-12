using System.Text.Json;
using System.Windows.Media.Media3D;
using BPR2_Desktop.Model;
using BPR2_Desktop.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using NpgsqlTypes;
using Shelf = BPR2_Desktop.Model.Entities.Shelf;
using ShelfProperties = BPR2_Desktop.Model.Entities.ShelfProperties;
using ShelfSection = BPR2_Desktop.Model.Entities.ShelfSection;
using Point = NetTopologySuite.Geometries.Point;

namespace BPR2_Desktop.Database;

public class ShelfContext : DbContext
{
    private readonly string _schema;
    public DbSet<Shelf> Shelves { get; set; }
    public DbSet<ShelfSection> ShelfSections { get; set; }
    public DbSet<ShelfProperties> ShelfProperties { get; set; }

    public ShelfContext(DbContextOptions<ShelfContext> options, IOptions<DatabaseConfig> dbConfig) : base(options)
    {
        _schema = dbConfig.Value.Schema ?? "public";
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.Entity<Shelf>(entity =>
        {
            entity.ToTable("shelf", _schema);
            entity.Property(s => s.Position)
                .HasColumnType("JSONB")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Position>(v, (JsonSerializerOptions)null));
        });
        modelBuilder.Entity<ShelfProperties>(entity =>
        {
            entity.ToTable("shelf_properties", _schema);
            entity.Property(s => s.Dimension)
                .HasColumnType("JSONB")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dimensions>(v, (JsonSerializerOptions)null));
        });
        modelBuilder.Entity<ShelfSection>(entity =>
        {
            entity.ToTable("shelf_section", _schema);
            entity.Property(s => s.Position)
                .HasColumnType("JSONB")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Position>(v, (JsonSerializerOptions)null));
        });
        modelBuilder.Entity<Dimensions>(entity => { entity.HasNoKey(); });
        modelBuilder.Entity<Position>(entity => { entity.HasNoKey(); });
        base.OnModelCreating(modelBuilder);
    }

    public Task<List<Shelf>> GetAllShelves()
    {
        return Shelves.ToListAsync();
    }

    public async Task<Shelf?> GetShelfById(int id)
    {
        return await Shelves.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Shelf?> GetShelfByName(string name)
    {
        return await Shelves.FirstOrDefaultAsync(s => s.Shelf_Name == name);
    }

    public async Task<List<ShelfSection>> GetShelfSectionsByShelfId(int shelfId)
    {
        var shelfSections = await Shelves
            .Where(s => s.Id == shelfId)
            .Select(s => s.Shelf_Section_Ids)
            .FirstOrDefaultAsync();

        if (shelfSections == null)
        {
            return new List<ShelfSection>();
        }

        return await ShelfSections
            .Where(ss => shelfSections.Contains(ss.Id))
            .ToListAsync();
    }

    public async Task<ShelfProperties?> GetShelfPropertiesByShelfId(int shelfId)
    {
        return await ShelfProperties
            .FirstOrDefaultAsync(sp => sp.Id == shelfId);
    }

    public async Task AddShelf(BPR2_Desktop.Model.Shelf shelf)
    {
        var shelfProperties = GetShelfProperties(shelf);
        var sPEntry = await ShelfProperties.AddAsync(shelfProperties);
        await SaveChangesAsync();
        var shelfSections = GetShelfSections(shelf);
        var ssEntry = new List<EntityEntry<ShelfSection>>();
        foreach (var section in shelfSections)
        {
            ssEntry.Add(await ShelfSections.AddAsync(section));
        }
        await SaveChangesAsync();

        var shelfEntity = new Shelf
        {
            Shelf_Type = shelf.GetShelfType(),
            Shelf_Name = shelf.GetShelfName(),
            Position = new Position(shelf.GetPosition().X, shelf.GetPosition().Y, shelf.GetPosition().Z),
            Properties_Id = sPEntry.Entity.Id,
            Shelf_Section_Ids = ssEntry.Select(section => section.Entity.Id).ToArray()
        };
        await Shelves.AddAsync(shelfEntity);
        await SaveChangesAsync();
    }

    private ShelfProperties GetShelfProperties(BPR2_Desktop.Model.Shelf shelf)
    {
        return new ShelfProperties
        {
            Dimension = shelf.GetProperties().Dimension,
            DistanceBetweenShelves = shelf.GetProperties().DistanceBetweenShelves,
            NumberOfShelves = shelf.GetProperties().NumberOfShelves,
            ShelveThickness = shelf.GetProperties().ShelveThickness
        };
    }

    private List<ShelfSection> GetShelfSections(BPR2_Desktop.Model.Shelf shelf)
    {
        var shelfSections = new List<ShelfSection>();
        foreach (var section in shelf.GetShelfSections())
        {
            shelfSections.Add(new ShelfSection
            {
                Position = new Position(section.GetPosition().X, section.GetPosition().Y, section.GetPosition().Z),
                Product_Ids = section.GetProducts().Select(product => product.Main_EAN).ToArray()
            });
        }

        return shelfSections;
    }
}