using System.Diagnostics;
using System.IO;

namespace BPR2_Desktop.Model.Enums;

public enum ShelfTypes
{
    Normal,
    Refrigerator,
    Freezer,
    DoubleSided,
    Custom
}

public static class ShelfTypeExtensions
{
    public static string GetPathLocation(this ShelfTypes shelfType)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory + "../../../"; // TODO: make it better XD
        return shelfType switch
        {
            ShelfTypes.Normal => "Normal",
            ShelfTypes.Refrigerator => "Refrigerator",
            ShelfTypes.Freezer => "Freezer",
            ShelfTypes.DoubleSided => Path.Combine(baseDirectory, "3DObjects/shelf.obj"),
            _ => throw new ArgumentOutOfRangeException(nameof(shelfType), shelfType, null),
        };
    }
    
    public static Dimensions GetShelfSizes(this ShelfTypes shelfType)
    {
        return shelfType switch
        {
            ShelfTypes.Normal => new Dimensions(5,2,2),
            ShelfTypes.Refrigerator => new Dimensions(5,2,2),
            ShelfTypes.Freezer => new Dimensions(5,2,2),
            ShelfTypes.DoubleSided =>  new Dimensions(5,2,2),
            _ => throw new ArgumentOutOfRangeException(nameof(shelfType), shelfType, null),
        };
    }
    
    public static bool IsShelfDoubleSided(this ShelfTypes shelfType)
    {
        return shelfType switch
        {
            ShelfTypes.Normal => false,
            ShelfTypes.Refrigerator => false,
            ShelfTypes.Freezer => false,
            ShelfTypes.DoubleSided =>  true,
            _ => throw new ArgumentOutOfRangeException(nameof(shelfType), shelfType, null),
        };
    }

    public static List<ShelfTypes> GetAllShelfTypes()
    {
        return Enum.GetValues(typeof(ShelfTypes)).Cast<ShelfTypes>().ToList();
    }
}