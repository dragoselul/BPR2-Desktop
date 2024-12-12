using System;
using System.Windows;
using Moq;
using Xunit;
using BPR2_Desktop.Views.Windows;
using BPR2_Desktop.ViewModels.MacroManagement;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

public class MacroManagementTests
{
    private readonly Mock<INavigationService> _mockNavigationService;
    private readonly Mock<INavigationViewPageProvider> _mockPageProvider;
    private readonly MacroManagement _macroManagement;

    public MacroManagementTests()
    {
        if (Application.Current == null)
        {
            new Application();
        }
        
        var viewModel = new MacroManagementViewModel();
        _mockNavigationService = new Mock<INavigationService>();
        _mockPageProvider = new Mock<INavigationViewPageProvider>();

        _macroManagement = new MacroManagement(viewModel, _mockNavigationService.Object);
    }
    
    [StaFact]
    public void DefaultConstructor_InitializesComponent()
    {
        // Act
        var macroManagement = new MacroManagement();

        // Assert
        Assert.NotNull(macroManagement);
    }
    
    [StaFact]
    public void ShowWindow_DisplaysWindow()
    {
        // Act
        _macroManagement.ShowWindow();

        // Assert
        Assert.True(_macroManagement.IsVisible); // Verifies the window is now visible.
    }
    
    [StaFact]
    public void CloseWindow_ClosesWindow()
    {
        // Act
        _macroManagement.CloseWindow();

        // Assert
        Assert.False(_macroManagement.IsVisible); // Verifies the window is no longer visible.
    }

    [StaFact]
    public void Constructor_SetsDataContextAndViewModel()
    {
        // Assert
        Assert.NotNull(_macroManagement.DataContext);
        Assert.Equal(_macroManagement, _macroManagement.DataContext);
    }

    [StaFact]
    public void GetNavigation_ReturnsRootNavigation()
    {
        // Act
        var navigationView = _macroManagement.GetNavigation();

        // Assert
        Assert.NotNull(navigationView);
    }
    
    
    [StaFact]
    public void SetPageService_SetsPageProviderService()
    {
        // Arrange
        var mockPageProvider = new Mock<INavigationViewPageProvider>();

        // Act
        _macroManagement.SetPageService(mockPageProvider.Object);

        // Assert
        // There is no direct way to validate this without additional instrumentation;
        // ensure it doesn't throw exceptions for now.
    }



    /*[StaFact]
    public void Navigate_CallsNavigationService()
    {
        // Arrange
        var pageType = typeof(MacroManagement);

        // Act
        var result = false;
        if (_macroManagement.Navigate(pageType))
        {
            result = true;
            
        } else {
            result = false;
        }

        // Assert
        Assert.True(result);
    }*/

    [StaFact]
    public void SetPageService_ConfiguresNavigationControl()
    {
        // Act
        _macroManagement.SetPageService(_mockPageProvider.Object);

        // Assert
        _mockNavigationService.Verify(n => n.SetNavigationControl(It.IsAny<INavigationView>()), Times.Once);
    }
    
    [StaFact]
    public void GetNavigation_ThrowsNotImplementedException()
    {
        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            ((INavigationWindow)_macroManagement).GetNavigation());
    }

    [StaFact]
    public void SetServiceProvider_ThrowsNotImplementedException()
    {
        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            _macroManagement.SetServiceProvider(new Mock<IServiceProvider>().Object));
    }


}