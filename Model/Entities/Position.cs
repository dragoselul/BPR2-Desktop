namespace BPR2_Desktop.Model.Entities;

public class Position
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Position()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public Position(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}