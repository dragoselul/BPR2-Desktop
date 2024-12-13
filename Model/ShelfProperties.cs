namespace BPR2_Desktop.Model;

public class ShelfProperties
{
    public Dimensions Dimension { get; set; }
    public int NumberOfShelves { get; set; }
    public double DistanceBetweenShelves { get; set; }
    public double ShelveThickness { get; set; }
    
    public ShelfProperties()
    {
        Dimension = new Dimensions();
        NumberOfShelves = 0;
        DistanceBetweenShelves = 0;
        ShelveThickness = 0;
    }
    
    public ShelfProperties(Dimensions dimension, int numberOfShelves, double distanceBetweenShelves, double shelveThickness)
    {
        Dimension = dimension;
        NumberOfShelves = numberOfShelves;
        DistanceBetweenShelves = distanceBetweenShelves;
        ShelveThickness = shelveThickness;
    }
}