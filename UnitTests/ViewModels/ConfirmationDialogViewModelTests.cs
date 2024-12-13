// using System;
// using System.Windows;
// using BPR2_Desktop.ViewModels;
// using BPR2_Desktop.Views.Windows;
// using Moq;
// using NUnit.Framework;
// using Assert = NUnit.Framework.Assert;
//
// [TestFixture]
// public class ConfirmationDialogViewModelTests
// {
//     private Mock<IServiceProvider> _mockServiceProvider;
//     private ConfirmationDialogViewModel _viewModel;
//
//     [SetUp]
//     public void SetUp()
//     {
//         _mockServiceProvider = new Mock<IServiceProvider>();
//
//         var mockMainWindow = new Mock<Window>();
//         _mockServiceProvider
//             .Setup(provider => provider.GetService(typeof(MainWindow)))
//             .Returns(mockMainWindow.Object);
//
//         _viewModel = new ConfirmationDialogViewModel(_mockServiceProvider.Object);
//     }
//
//     [Test, Apartment(ApartmentState.STA)]
//     public void ConfirmAndNavigateToMain_SetsMainWindowAndShowsIt()
//     {
//         
//         if (Application.Current == null)
//     {
//         new Application(); // Create a new application instance
//     }
//         // Arrange
//         var serviceProvider = new Mock<IServiceProvider>();
//         var mainWindow = new Window();
//
//         serviceProvider
//             .Setup(sp => sp.GetService(typeof(MainWindow)))
//             .Returns(mainWindow);
//
//         var viewModel = new ConfirmationDialogViewModel(serviceProvider.Object);
//
//         // Act
//         viewModel.ConfirmAndNavigateToMain();
//
//         // Assert
//         Assert.That(Application.Current.MainWindow, Is.EqualTo(mainWindow), "MainWindow should be set as the application's main window.");
//         Assert.That(mainWindow.IsVisible, Is.True, "MainWindow should be visible.");
//     }
//
//
//
//
//
//     [Test, Apartment(ApartmentState.STA)]
//     public void CloseDialog_ClosesProvidedWindow()
//     {
//         // Arrange
//         var testWindow = new Window();
//         var viewModel = new ConfirmationDialogViewModel(new Mock<IServiceProvider>().Object);
//
//         // Act
//         viewModel.CloseDialog(testWindow);
//
//         // Assert
//         Assert.That(testWindow.IsLoaded, Is.False, "The window should be closed after CloseDialog is called.");
//     }
//
//
//     }
//
//
//
//
//     
//
