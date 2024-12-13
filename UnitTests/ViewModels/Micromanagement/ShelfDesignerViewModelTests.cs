using System.Windows.Media.Media3D;
using BPR2_Desktop.ViewModels.MicroManagement;
using Xunit;

public class ShelfDesignerViewModelTests
{
    [WpfFact]
    public void ViewModel_ShouldInitializeWithDefaultValues()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel();

        // Assert
        Assert.NotNull(viewModel.Shelfs);
        Assert.NotNull(viewModel.SceneObjects);
        Assert.Empty(viewModel.SceneObjects);
    }

    [WpfFact]
    public void CanGenerateShelfLines_ShouldBeTrue_WhenInputsAreValid()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel
        {
            NumberOfShelves = 3,
            DistanceBetweenShelves = 10,
            WidthOfShelf = 100,
            HeightOfShelf = 50
        };

        // Assert
        Assert.True(viewModel.CanGenerateShelfLines);
    }

    [WpfFact]
    public void CanGenerateShelfLines_ShouldBeFalse_WhenInputsAreInvalid()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel
        {
            NumberOfShelves = 0 // Invalid number of shelves
        };

        // Assert
        Assert.False(viewModel.CanGenerateShelfLines);
    }

    [WpfFact]
    public void GenerateShelfLines_ShouldAddSceneObjects_WhenInputsAreValid()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel
        {
            NumberOfShelves = 2,
            DistanceBetweenShelves = 10,
            ShelveThickness = 2,
            WidthOfShelf = 100,
            HeightOfShelf = 50
        };

        // Act
        viewModel.GenerateShelfLinesCommand.Execute(null);

        // Assert
        Assert.NotEmpty(viewModel.SceneObjects);
    }

    [WpfFact]
    public void GenerateShelfLines_ShouldNotAddSceneObjects_WhenInputsAreInvalid()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel
        {
            NumberOfShelves = -1 // Invalid input
        };

        // Act
        viewModel.GenerateShelfLinesCommand.Execute(null);

        // Assert
        Assert.Empty(viewModel.SceneObjects);
    }
}
