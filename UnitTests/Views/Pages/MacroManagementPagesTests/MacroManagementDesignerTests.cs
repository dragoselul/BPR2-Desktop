using System.Collections.ObjectModel;
using HelixToolkit.Wpf;
using Moq;
using Xunit;

public class MacroManagementDesignerTests
{
    // Simple mock/stub for ViewModel
    public class MockDesignerViewModel
    {
        public ObservableCollection<object> SceneObjects { get; set; } = new ObservableCollection<object>();
    }

    [WpfFact]
    public void Constructor_ShouldSetViewModel()
    {
        // Arrange
        var mockViewModel = new Mock<BPR2_Desktop.ViewModels.MacroManagement.DesignerViewModel>().Object;

        // Act
        var designer = new BPR2_Desktop.Views.Pages.MacroManagementDesigner(mockViewModel);

        // Assert
        Assert.Equal(mockViewModel, designer.ViewModel);
    }

    [WpfFact]
    public void HelixViewport_ShouldBeInitializedCorrectly()
    {
        // Arrange
        var mockViewModel = new Mock<BPR2_Desktop.ViewModels.MacroManagement.DesignerViewModel>().Object;

        // Act
        var designer = new BPR2_Desktop.Views.Pages.MacroManagementDesigner(mockViewModel);

        // Access the HelixViewport3D using its name
        var helixViewport = designer.FindName("HelixViewport") as HelixToolkit.Wpf.HelixViewport3D;

        // Assert
        Assert.NotNull(helixViewport);
        Assert.True(helixViewport.ShowFrameRate, "HelixViewport3D should have ShowFrameRate enabled.");
    }

    


}