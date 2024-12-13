using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPR2_Desktop.Model.Entities;

public class ShelfSection
{
    [Key, Column(name: "id", TypeName = "integer")]
    public int Id { get; set; }
    [Column(name:"position")]
    public virtual Position Position { get; set; }
    [Column(name:"product_ids")]
    public string[] Product_Ids { get; set; }
}