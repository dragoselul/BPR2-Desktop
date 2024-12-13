using System.Collections.ObjectModel;
using System.Windows;
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
            HeightOfShelf = 50,
            DepthOfShelf = 10,
            ShelveThickness = 2
        };
        
        // Assert
        Assert.True(viewModel.CanGenerateShelves());
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
}
