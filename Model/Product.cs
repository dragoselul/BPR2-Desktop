namespace BPR2_Desktop.Model;

public class Product
{
    public required string StoreName { get; init; }
    public required string Department { get; init; }
    public required string Category { get; init; }
    public required string MainEAN { get; init; }
    public required string ProductName { get; init; }
    public decimal ProductWidth { get; init; }
    public decimal ProductHeight { get; init; }
    public decimal ProductDepth { get; init; }
}