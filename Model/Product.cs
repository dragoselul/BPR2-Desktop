using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPR2_Desktop.Model;

public class Product
{
    public required string Store_Name { get; init; }
    public required string Department { get; init; }
    public required string Category { get; init; }
    [Key]
    public required string Main_EAN { get; init; }
    public required string Product_Name { get; init; }
    public required double Product_Width { get; init; }
    public required double Product_Height { get; init; }
    public required double Product_Depth { get; init; }
}