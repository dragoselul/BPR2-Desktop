using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BPR2_Desktop.Model.Enums;

namespace BPR2_Desktop.Model.Entities;

public class Shelf
{
    [Key, Column(name:"id")] public int Id { get; set; }
    [Column(name: "shelf_type")] public virtual ShelfType Shelf_Type { get; set; }
    [Column(name: "shelf_name")] public string Shelf_Name { get; set; }
    [Column(name: "properties_id")] public int Properties_Id { get; set; }
    [Column(name: "position")] public virtual Position Position { get; set; }
    [Column(name: "shelf_section_id")] public int[] Shelf_Section_Ids { get; set; }
}