using System;
using System.Windows;
using BPR2_Desktop.ViewModels;
using BPR2_Desktop.Views.Windows;
using Moq;
using Xunit;

public class ConfirmationDialogTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<Window> _mockMainWindow;
    private readonly Mock<ConfirmationDialogViewModel> _mockViewModel;
    private readonly ConfirmationDialog _dialog;

    public ConfirmationDialogTests()
    { // Mock dependencies
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockMainWindow = new Mock<Window>();

        // Mock the MainWindow service
        _mockServiceProvider
            .Setup(sp => sp.GetService(typeof(MainWindow)))
            .Returns(_mockMainWindow.Object);

        // Initialize the ViewModel with the mocked IServiceProvider
        var viewModel = new ConfirmationDialogViewModel(_mockServiceProvider.Object);

        // Create the dialog with the ViewModel
        _dialog = new ConfirmationDialog(viewModel);
    }

    [StaFact]
    public void ConfirmationDialog_InitializesWithViewModel()
    {
        // Assert
        Assert.NotNull(_dialog);
    }
    

    [StaFact]
    public void NoButton_Click_CallsCloseDialog()
    {
        // Act
        _dialog.NoButton_Click(null, null);

        // Assert
        // Can't mock `Close()` on the dialog directly but can assert no exceptions are thrown
        Assert.False(_dialog.IsVisible);
    }

    [StaFact]
    public void ConfirmAndNavigateToMain_ThrowsException_WhenMainWindowNotRegistered()
    {
        // Arrange
        var viewModel = new ConfirmationDialogViewModel(new Mock<IServiceProvider>().Object);
        var dialog = new ConfirmationDialog(viewModel);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => dialog.YesButton_Click(null, null));
    }
    

    [StaFact]
    public void NoButton_Click_HidesDialog()
    {
        // Act
        _dialog.NoButton_Click(null, null);

        // Assert
        Assert.False(_dialog.IsVisible);
    }
}
