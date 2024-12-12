using System;
using System.Windows;
using System.Windows.Controls;
using Moq;
using Xunit;
using BPR2_Desktop.Views.Windows;
using BPR2_Desktop.ViewModels.MicroManagement;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace UnitTests
{
    public class MicroManagementTests : IDisposable
    {
        private MicroManagementViewModel _viewModel;
        private Mock<INavigationService> _navigationServiceMock;
        private Mock<INavigationViewPageProvider> _pageProviderMock;

        public MicroManagementTests()
        {
            if (Application.Current == null)
            {
                new Application();
            }

            // Set up mocks and the ViewModel
            _viewModel = new MicroManagementViewModel();
            _navigationServiceMock = new Mock<INavigationService>();
            _pageProviderMock = new Mock<INavigationViewPageProvider>();
        }

        public void Dispose()
        {
            // Close all open windows on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window w in Application.Current.Windows)
                {
                    w.Close();
                }
            });
        }

        [StaFact]
        public void Constructor_SetsDataContextAndViewModel()
        {
            var window = new MicroManagement(_viewModel, _navigationServiceMock.Object);
            Assert.NotNull(window.DataContext);
            Assert.Equal(window, window.DataContext);
            Assert.Same(_viewModel, window.ViewModel);
        }

        [StaFact]
        public void NavigationService_IsSetOnConstruction()
        {
            var window = new MicroManagement(_viewModel, _navigationServiceMock.Object);

            var rootNavigationField = typeof(MicroManagement)
                .GetField("RootNavigation",
                          System.Reflection.BindingFlags.NonPublic |
                          System.Reflection.BindingFlags.Instance);

            var rootNavigation = rootNavigationField?.GetValue(window) as NavigationView;

            _navigationServiceMock.Verify(s => s.SetNavigationControl(rootNavigation), Times.Once);
        }

        [StaFact]
        public void GetNavigation_ReturnsRootNavigation()
        {
            var window = new MicroManagement(_viewModel, _navigationServiceMock.Object);
            var navView = window.GetNavigation();
            Assert.NotNull(navView);
            Assert.IsType<NavigationView>(navView);
        }

        [StaFact]
        public void ViewModelNavigationItems_BindToRootNavigation()
        {
            var window = new MicroManagement(_viewModel, _navigationServiceMock.Object);
            var navView = window.GetNavigation() as NavigationView;

            Assert.NotNull(navView);

            var items = navView.MenuItemsSource;
            Assert.NotNull(items);
            Assert.NotEmpty((System.Collections.IEnumerable)items);
        }
        
        [StaFact]
        public void RootNavigation_IsInitializedOnConstruction()
        {
            // Arrange
            var window = new MicroManagement(_viewModel, _navigationServiceMock.Object);

            // Act
            var navigationView = window.GetNavigation();

            // Assert
            Assert.NotNull(navigationView);
            Assert.IsType<NavigationView>(navigationView);
        }
        
        [StaFact]
        public void SetServiceProvider_ThrowsNotImplementedException()
        {
            // Arrange
            var window = new MicroManagement(_viewModel, _navigationServiceMock.Object);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() =>
                window.SetServiceProvider(new Mock<IServiceProvider>().Object));
        }
        
        [StaFact]
        public void ShowWindow_MakesWindowVisible()
        {
            // Arrange
            var window = new MicroManagement(_viewModel, _navigationServiceMock.Object);

            // Act
            window.ShowWindow();

            // Assert
            Assert.True(window.IsVisible);
        }
        
        [StaFact]
        public void CloseWindow_HidesWindow()
        {
            // Arrange
            var window = new MicroManagement(_viewModel, _navigationServiceMock.Object);

            // Act
            window.ShowWindow(); // Ensure the window is shown first
            window.CloseWindow();

            // Assert
            Assert.False(window.IsVisible);
        }
        [StaFact]
        public void RootNavigation_HasExpectedItems()
        {
            // Arrange
            var window = new MicroManagement(_viewModel, _navigationServiceMock.Object);

            // Act
            var navigationView = window.GetNavigation();
            var items = navigationView.MenuItemsSource;

            // Assert
            Assert.NotNull(items);
            Assert.NotEmpty((System.Collections.IEnumerable)items);
        }




        

    }
    
}
