using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using BPR2_Desktop.ViewModels.MicroManagement;
using Xunit;

public class ShelfDesignerViewModelTests
{
    [Fact]
    public void InitializeViewModel_ShouldSetInitialValues()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel();

        // Act
        var shelfTypes = viewModel.Shelfs;
        var sceneObjects = viewModel.SceneObjects;

        // Assert
        Assert.NotNull(shelfTypes);
        Assert.NotEmpty(shelfTypes);
        Assert.NotNull(sceneObjects);
        Assert.Empty(sceneObjects);
    }

    [Fact]
    public void GenerateShelfLines_ShouldPopulateSceneObjects()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel
        {
            NumberOfShelves = 3,
            DistanceBetweenShelves = 20,
            ShelveThickness = 3,
            WidthOfShelf = 200,
            HeightOfShelf = 200
        };

        // Act
        viewModel.GenerateShelfLinesCommand.Execute(null);

        // Assert
        Assert.Equal(4, viewModel.SceneObjects.Count); // 3 shelves + 1 SunLight
    }

    [Fact]
    public void GenerateShelfLines_ShouldNotAddObjectsIfInvalidInputs()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel
        {
            NumberOfShelves = -1 // Invalid number of shelves
        };

        // Act
        viewModel.GenerateShelfLinesCommand.Execute(null);

        // Assert
        Assert.Empty(viewModel.SceneObjects);
    }

    [Fact]
    public void CanGenerateShelfLines_ShouldReturnTrueForValidInputs()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel
        {
            DistanceBetweenShelves = 20,
            ShelveThickness = 3,
            WidthOfShelf = 200,
            HeightOfShelf = 200,
            NumberOfShelves = 5
        };

        // Act
        var result = viewModel.CanGenerateShelfLines();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanGenerateShelfLines_ShouldReturnFalseForInvalidInputs()
    {
        // Arrange
        var viewModel = new ShelfDesignerViewModel
        {
            DistanceBetweenShelves = 0 // Invalid distance
        };

        // Act
        var result = viewModel.CanGenerateShelfLines();

        // Assert
        Assert.False(result);
    }
}
