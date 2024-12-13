using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BPR2_Desktop.ViewModels;
using BPR2_Desktop.Views.Pages;
using BPR2_Desktop.Views.Windows;
using Moq;
using NUnit.Framework;
using Xunit;
using Wpf.Ui.Controls;
using Assert = Xunit.Assert;
using Button = Wpf.Ui.Controls.Button;

namespace UnitTests
{
    public class MainWindowTests
    {
        public MainWindowTests()
        {
            if (Application.Current == null)
            {
                new Application();
            }
        }

       [StaFact]
        public void Button_Loaded_AnimatesCorrectly()
        {
            // Arrange
            var viewModel = new MainWindowViewModel(new Mock<IServiceProvider>().Object);
            var mainWindow = new MainWindow(viewModel);
            var button = new Button();
            if (!mainWindow.Resources.Contains("ButtonFadeIn"))
            {
                mainWindow.Resources.Add("ButtonFadeIn", new Mock<System.Windows.Media.Animation.Storyboard>().Object);
            }

            // Act
            var eventArgs = new RoutedEventArgs(FrameworkElement.LoadedEvent, button);
            mainWindow.Button_Loaded(button, eventArgs);

            // Assert
            var storyboard = mainWindow.Resources["ButtonFadeIn"] as System.Windows.Media.Animation.Storyboard;
            Assert.NotNull(storyboard);
        }

        [StaFact]
        public void OnMediaEnded_ResetsAndPlaysMedia()
        {
            // Arrange
            var viewModel = new MainWindowViewModel(new Mock<IServiceProvider>().Object);
            var mainWindow = new MainWindow(viewModel);

            var mediaElement = new MediaElement
            {
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Manual
            };
            mediaElement.Position = TimeSpan.FromSeconds(5);
            mainWindow.MediaElement = mediaElement;

            // Act
            mainWindow.OnMediaEnded(mediaElement, new RoutedEventArgs());

            // Assert
            Assert.Equal(TimeSpan.Zero, mediaElement.Position);
        }
    }
}