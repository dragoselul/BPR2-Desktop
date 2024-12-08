using System.ComponentModel.DataAnnotations;

namespace BPR2_Desktop.Model;

public class ProductImage
{
    [Key]
    public required string Main_EAN { get; init; }
    public required byte[] Image_Blob { get; init; }
}