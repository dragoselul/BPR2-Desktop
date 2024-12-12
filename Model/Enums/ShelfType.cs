using System.Diagnostics;
using System.IO;

namespace BPR2_Desktop.Model.Enums;

public enum ShelfType
{
    Normal,
    Refrigerator,
    Freezer,
    DoubleSided,
    Custom
}

public static class ShelfTypeExtensions
{
    public static string GetPathLocation(this ShelfType shelfType)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory + "../../../"; // TODO: make it better XD
        return shelfType switch
        {
            ShelfType.Normal => "Normal",
            ShelfType.Refrigerator => "Refrigerator",
            ShelfType.Freezer => "Freezer",
            ShelfType.DoubleSided => Path.Combine(baseDirectory, "3DObjects/shelf.obj"),
            _ => throw new ArgumentOutOfRangeException(nameof(shelfType), shelfType, null),
        };
    }
    
    public static Dimensions GetShelfSizes(this ShelfType shelfType)
    {
        return shelfType switch
        {
            ShelfType.Normal => new Dimensions(5,2,2),
            ShelfType.Refrigerator => new Dimensions(5,2,2),
            ShelfType.Freezer => new Dimensions(5,2,2),
            ShelfType.DoubleSided =>  new Dimensions(5,2,2),
            _ => throw new ArgumentOutOfRangeException(nameof(shelfType), shelfType, null),
        };
    }
    
    public static bool IsShelfDoubleSided(this ShelfType shelfType)
    {
        return shelfType switch
        {
            ShelfType.Normal => false,
            ShelfType.Refrigerator => false,
            ShelfType.Freezer => false,
            ShelfType.DoubleSided =>  true,
            _ => throw new ArgumentOutOfRangeException(nameof(shelfType), shelfType, null),
        };
    }

    public static List<ShelfType> GetAllShelfTypes()
    {
        return Enum.GetValues(typeof(ShelfType)).Cast<ShelfType>().ToList();
    }
}