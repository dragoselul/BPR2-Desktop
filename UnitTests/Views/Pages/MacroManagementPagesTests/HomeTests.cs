using System.Windows.Controls;
using Xunit;

public class HomeTests
{
    [WpfFact]
    public void Home_ShouldInitializeWithoutErrors()
    {
        // Act
        var homePage = new BPR2_Desktop.Views.Pages.Home(null);

        // Assert
        Assert.NotNull(homePage);
    }

    [WpfFact]
    public void Home_ShouldContainTextBoxWithExpectedContent()
    {
        // Arrange
        var homePage = new BPR2_Desktop.Views.Pages.Home(null);

        // Act
        var grid = homePage.Content as Grid;
        var textBox = grid?.Children[0] as TextBox;

        // Assert
        Assert.NotNull(textBox);
        Assert.Equal("Hi there", textBox.Text);
    }
}