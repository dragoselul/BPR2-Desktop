using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPR2_Desktop.Model;

public class ProductImage
{
    [Key]
    public required string Main_EAN { get; init; }
    public required byte[] Picture_Blob { get; init; }
}