// using System.Windows;
// using BPR2_Desktop.ViewModels;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using Wpf.Ui;
// using Xunit;
//
// public class MainWindowViewModelTests
// {
//     /*[WpfFact]
//     public void OnGridMouseLeftButtonDown_ShouldCallDragMove()
//     {
//         // Arrange
//         if (Application.Current == null)
//         {
//             new Application(); // Ensure the Application instance exists
//         }
//
//         var testWindow = new Window();
//         Application.Current.MainWindow = testWindow;
//         var viewModel = new MainWindowViewModel(null);
//
//         // Act
//         var exception = Record.Exception(() => viewModel.OnGridMouseLeftButtonDown());
//
//         // Assert
//         Assert.Null(exception); // Ensure no exception is thrown
//     }*/
//
//
//     [WpfFact]
//     public void OnMacromanagementClick_ShouldOpenMacroManagementWindow()
//     {
//         // Arrange
//         if (Application.Current == null)
//         {
//             new Application(); // Ensure the Application instance exists
//         }
//
//         var mockServiceProvider = new MinimalServiceProvider();
//         var viewModel = new MainWindowViewModel(mockServiceProvider);
//
//         // Act
//         Application.Current.Dispatcher.Invoke(() => viewModel.OnMacromanagementClick());
//         var openedWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
//
//         // Assert
//         Assert.NotNull(openedWindow);
//         Assert.IsType<BPR2_Desktop.Views.Windows.MacroManagement>(openedWindow);
//         Assert.True(openedWindow.IsVisible);
//     }
//
//
//     /*[WpfFact]
//     public void OnMicromanagementClick_ShouldOpenMicroManagementWindow()
//     {
//         // Arrange
//         if (Application.Current == null)
//         {
//             new Application(); // Ensure the Application instance exists
//         }
//
//         var mockServiceProvider = new MinimalServiceProvider();
//         var viewModel = new MainWindowViewModel(mockServiceProvider);
//
//         // Act
//         viewModel.OnMicromanagementClick();
//         var openedWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
//
//         // Assert
//         Assert.NotNull(openedWindow);
//         Assert.IsType<BPR2_Desktop.Views.Windows.MicroManagement>(openedWindow);
//         Assert.True(openedWindow.IsVisible);
//     }*/
//
//     private class MinimalServiceProvider : IServiceProvider
//         {
//             public object GetService(Type serviceType)
//             {
//                 if (serviceType == typeof(BPR2_Desktop.Views.Windows.MacroManagement))
//                 {
//                     return new BPR2_Desktop.Views.Windows.MacroManagement(
//                         new Mock<BPR2_Desktop.ViewModels.MacroManagement.MacroManagementViewModel>().Object,
//                         new Mock<INavigationService>().Object
//                     );
//                 }
//                 
//                 if (serviceType == typeof(BPR2_Desktop.Views.Windows.MicroManagement))
//                 {
//                     return new BPR2_Desktop.Views.Windows.MicroManagement(
//                         new Mock<BPR2_Desktop.ViewModels.MicroManagement.MicroManagementViewModel>().Object,
//                         new Mock<INavigationService>().Object
//                     );
//                 }
//     
//                 return null;
//             }
//         }
//
//     [WpfFact]
//     public void OnHelpClick_ShouldShowHelpMessageBox()
//     {
//         // Arrange
//         var viewModel = new MainWindowViewModel(Mock.Of<IServiceProvider>());
//
//         // Act
//         var exception = Record.Exception(() => viewModel.OnHelpClick());
//
//         // Assert
//         Assert.Null(exception); // Ensure no exceptions are thrown
//     }
//
//     
//     /*[WpfFact]
//     public void OnMaximizeClick_ShouldToggleWindowState()
//     {
//         // Arrange
//         if (Application.Current == null)
//         {
//             new Application(); // Ensure the Application instance exists
//         }
//
//         var testMainWindow = new Window { WindowState = WindowState.Normal };
//         Application.Current.MainWindow = testMainWindow;
//
//         var viewModel = new MainWindowViewModel(Mock.Of<IServiceProvider>());
//
//         // Act
//         viewModel.OnMaximizeClick();
//
//         // Assert
//         Assert.Equal(WindowState.Maximized, testMainWindow.WindowState);
//
//         // Act again
//         viewModel.OnMaximizeClick();
//
//         // Assert
//         Assert.Equal(WindowState.Normal, testMainWindow.WindowState);
//     }*/
//
//
//     /*[WpfFact]
//     public void OnMinimizeClick_ShouldSetWindowStateToMinimized()
//     {
//         // Arrange
//         if (Application.Current == null)
//         {
//             new Application(); // Ensure the Application instance exists
//         }
//
//         var testWindow = new Window
//         {
//             WindowState = WindowState.Normal
//         };
//
//         Application.Current.MainWindow = testWindow;
//
//         var viewModel = new MainWindowViewModel(Mock.Of<IServiceProvider>());
//
//         // Act
//         viewModel.OnMinimizeClick();
//
//         // Assert
//         Assert.Equal(WindowState.Minimized, testWindow.WindowState);
//     }*/
//
//
//
//     /*[WpfFact]
//     public void OnExitClick_ShouldNotThrowException()
//     {
//         // Arrange
//         var dispatcherApp = new Application();
//         var viewModel = new MainWindowViewModel(Mock.Of<IServiceProvider>());
//
//         // Act
//         var exception = Record.Exception(() => viewModel.OnExitClick());
//
//         // Cleanup
//         dispatcherApp.Shutdown();
//
//         // Assert
//         Assert.Null(exception);
//     }*/
//     
//     [WpfFact]
//     public void _displayNewWindow_ShouldDisplayNewWindow()
//     {
//         // Arrange
//         var mockWindow = new Mock<INavigationWindow>();
//         var viewModel = new MainWindowViewModel(Mock.Of<IServiceProvider>());
//
//         // Act
//         viewModel._displayNewWindow(mockWindow.Object);
//
//         // Assert
//         mockWindow.Verify(w => w.ShowWindow(), Times.Once);
//     }
//     
//     /*
//     [WpfFact]
//     public void _resizeNewWindow_ShouldResizeNewWindow()
//     {
//         // Arrange
//         var mockCurrentWindow = new Mock<Window>();
//         var mockNewWindow = new Mock<Window>();
//         Application.Current.MainWindow = mockCurrentWindow.Object;
//         mockCurrentWindow.Setup(w => w.Width).Returns(800.0);
//         mockCurrentWindow.Setup(w => w.Height).Returns(600.0);
//
//         var viewModel = new MainWindowViewModel(Mock.Of<IServiceProvider>());
//
//         // Act
//         viewModel._resizeNewWindow(mockNewWindow.Object, mockCurrentWindow.Object);
//
//         // Assert
//         mockNewWindow.VerifySet(w => w.Width = 800.0, Times.Once);
//         mockNewWindow.VerifySet(w => w.Height = 600.0, Times.Once);
//     }
//     
//     [WpfFact]
//     public void _isFullscreen_ShouldReturnTrueIfWindowIsMaximized()
//     {
//         // Arrange
//         var mockWindow = new Mock<Window>();
//         mockWindow.Setup(w => w.WindowState).Returns(WindowState.Maximized);
//         var viewModel = new MainWindowViewModel(Mock.Of<IServiceProvider>());
//
//         // Act
//         var result = viewModel._isFullscreen(mockWindow.Object);
//
//         // Assert
//         Assert.True(result);
//     }
//
//     [WpfFact]
//     public void _isFullscreen_ShouldReturnFalseIfWindowIsNotMaximized()
//     {
//         // Arrange
//         var mockWindow = new Mock<Window>();
//         mockWindow.Setup(w => w.WindowState).Returns(WindowState.Normal);
//         var viewModel = new MainWindowViewModel(Mock.Of<IServiceProvider>());
//
//         // Act
//         var result = viewModel._isFullscreen(mockWindow.Object);
//
//         // Assert
//         Assert.False(result);
//     }
//     
//     [WpfFact]
//     public void _displayNewWindowAndCloseCurrent_ShouldDisplayNewWindowAndCloseCurrent()
//     {
//         // Arrange
//         var currentWindow = new Mock<INavigationWindow>(); // Mock the INavigationWindow for the current window
//         var newWindow = new Mock<INavigationWindow>(); // Mock the INavigationWindow for the new window
//
//         Application.Current.MainWindow = currentWindow.Object;
//
//         var viewModel = new MainWindowViewModel(Mock.Of<IServiceProvider>());
//
//         // Act
//         viewModel._displayNewWindowAndCloseCurrent(newWindow.Object, currentWindow.Object);
//
//         // Assert
//         Assert.True(newWindow.Object.IsVisible);  // Check if the new window is visible (ensure that IsVisible exists for INavigationWindow)
//     }
//     */
//     
// }
