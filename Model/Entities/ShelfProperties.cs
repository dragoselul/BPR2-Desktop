using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPR2_Desktop.Model.Entities;

public class ShelfProperties
{
    [Key, Column(name:"id")]
    public int Id { get; set; }
    [Column(name:"dimension")]
    public virtual  Dimensions Dimension { get; set; }
    [Column(name:"number_of_shelves")]
    public int NumberOfShelves { get; set; }
    [Column(name:"distance_between_shelves")]
    public double DistanceBetweenShelves { get; set; }
    [Column(name:"shelve_thickness")]
    public double ShelveThickness { get; set; }
    
}