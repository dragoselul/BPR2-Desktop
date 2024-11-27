namespace BPR2_Desktop.Model;

public class Product
{
    public required string StoreName { get; init; }
    public required string Department { get; init; }
    public required string Category { get; init; }
    public required string MainEAN { get; init; }
    public required string ProductName { get; init; }
    public required decimal ProductWidth { get; init; }
    public required decimal ProductHeight { get; init; }
    public required decimal ProductDepth { get; init; }
}