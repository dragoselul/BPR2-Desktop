using System.Windows;
using System.Windows.Controls;
using Xunit;

public class SettingsTests
{
    [WpfFact]
    public void Settings_ShouldInitializeWithoutErrors()
    {
        // Act
        var settingsPage = new BPR2_Desktop.Views.Pages.Settings();

        // Assert
        Assert.NotNull(settingsPage);
    }

    [WpfFact]
    public void Settings_ShouldInheritFromPage()
    {
        // Act
        var settingsPage = new BPR2_Desktop.Views.Pages.Settings();

        // Assert
        Assert.IsAssignableFrom<Page>(settingsPage);
    }

    [WpfFact]
    public void Settings_GridShouldExist()
    {
        // Arrange
        var settingsPage = new BPR2_Desktop.Views.Pages.Settings();

        // Act
        var grid = settingsPage.Content as Grid;

        // Assert
        Assert.NotNull(grid);
        Assert.IsType<Grid>(grid);
    }

    [WpfFact]
    public void Settings_TitleShouldBeSet()
    {
        // Arrange
        var settingsPage = new BPR2_Desktop.Views.Pages.Settings();

        // Act
        var title = settingsPage.Title;

        // Assert
        Assert.Equal("Settings", title);
    }
}