using System.Windows.Controls;
using Moq;
using Xunit;

public class ShelfDesignerTests
{
    [WpfFact]
    public void ShelfDesigner_ShouldInitializeWithoutErrors()
    {
        // Act
        var shelfDesigner = new BPR2_Desktop.Views.Pages.MicroManagement.ShelfDesigner(null);

        // Assert
        Assert.NotNull(shelfDesigner);
    }

    [WpfFact]
    public void ShelfDesigner_ShouldContainComboBoxForShelfType()
    {
        // Arrange
        var shelfDesigner = new BPR2_Desktop.Views.Pages.MicroManagement.ShelfDesigner(null);

        // Act
        var grid = shelfDesigner.Content as Grid;
        var comboBox = grid?.FindName("TypeOfShelf") as ComboBox;

        // Assert
        Assert.NotNull(comboBox);
        Assert.Equal("TypeOfShelf", comboBox.Name);
    }

    [WpfFact]
    public void ShelfDesigner_ShouldContainTextBoxForNumberOfShelves()
    {
        // Arrange
        var shelfDesigner = new BPR2_Desktop.Views.Pages.MicroManagement.ShelfDesigner(null);

        // Act
        var grid = shelfDesigner.Content as Grid;
        var textBox = grid?.FindName("NumberOfShelves") as TextBox;

        // Assert
        Assert.NotNull(textBox);
        Assert.Equal("NumberOfShelves", textBox.Name);
    }

    /*[WpfFact]
    public void ShelfDesigner_OnTextChanged_ShouldInvokeGenerateShelfLinesCommand()
    {
        // Arrange
        var mockViewModel = new Mock<BPR2_Desktop.ViewModels.MicroManagement.ShelfDesignerViewModel>();
        var shelfDesigner = new BPR2_Desktop.Views.Pages.MicroManagement.ShelfDesigner(mockViewModel.Object);
        var textBox = shelfDesigner.FindName("NumberOfShelves") as TextBox;

        // Act
        textBox.Text = "5"; // Simulate user input
        var textChangedArgs = new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None);
        textBox.RaiseEvent(textChangedArgs);

        // Assert
        mockViewModel.Verify(vm => vm.GenerateShelfLinesCommand.Execute(null), Times.Once);
    }*/
}