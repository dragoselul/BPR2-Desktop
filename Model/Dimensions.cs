namespace BPR2_Desktop.Model;

public class Dimensions
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    
    public Dimensions(double width, double height, double depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }
}