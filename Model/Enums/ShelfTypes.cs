﻿using System.Diagnostics;
using System.IO;

namespace BPR2_Desktop.Model.Enums;

public enum ShelfTypes
{
    Normal,
    Refrigerator,
    Freezer,
    DoubleSided,
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
    
    public static ShelfTypes GetShelfByName(this string shelfType)
    {
        return shelfType switch
        {
            "Normal" => ShelfTypes.Normal,
            "Refrigerator" => ShelfTypes.Refrigerator,
            "Freezer" => ShelfTypes.Freezer,
            "DoubleSided" => ShelfTypes.DoubleSided,
            _ => throw new ArgumentOutOfRangeException(nameof(shelfType), shelfType, null),
        };
    }
}