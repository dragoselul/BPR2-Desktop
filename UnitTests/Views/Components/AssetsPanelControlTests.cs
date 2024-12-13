// File: AssetsPanelControlTests.cs
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BPR2_Desktop.Views.Components;
using Xunit;


namespace BPR2_Desktop.Tests.Views.Components
{
    [Collection("WPF Tests")]
    public class AssetsPanelControlTests
    {
        [StaFact]
        public void Asset_MouseMove_WithLeftButtonPressed_ShouldExecuteWithoutException()
        {
            // Arrange
            var control = new AssetsPanelControl();

            // Find the Square StackPanel
            var stackPanels = control.Content as StackPanel;
            var squareStackPanel = stackPanels.Children[1] as StackPanel;
            Assert.NotNull(squareStackPanel);

            // Create MouseEventArgs with LeftButton pressed using reflection
            var mouseEventArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0)
            {
                RoutedEvent = UIElement.MouseMoveEvent,
                Source = squareStackPanel
            };

            // Use reflection to set the LeftButton property to Pressed
            typeof(MouseEventArgs)
                .GetProperty("LeftButton", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(mouseEventArgs, MouseButtonState.Pressed, null);

            // Act & Assert
            var exception = Record.Exception(() => control.Asset_MouseMove(squareStackPanel, mouseEventArgs));
            Assert.Null(exception);
        }

        [StaFact]
        public void Asset_MouseMove_WithLeftButtonReleased_ShouldNotExecuteDragDrop()
        {
            // Arrange
            var control = new AssetsPanelControl();

            // Find the Polygon StackPanel
            var stackPanels = control.Content as StackPanel;
            var polygonStackPanel = stackPanels.Children[2] as StackPanel;
            Assert.NotNull(polygonStackPanel);

            // Create MouseEventArgs with LeftButton released using reflection
            var mouseEventArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0)
            {
                RoutedEvent = UIElement.MouseMoveEvent,
                Source = polygonStackPanel
            };

            // Use reflection to set the LeftButton property to Released
            typeof(MouseEventArgs)
                .GetProperty("LeftButton", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(mouseEventArgs, MouseButtonState.Released, null);

            // Act & Assert
            var exception = Record.Exception(() => control.Asset_MouseMove(polygonStackPanel, mouseEventArgs));
            Assert.Null(exception);
        }
        
        /*[StaFact]
        public void Assets_ShouldHaveCorrectImageSources()
        {
            // Arrange
            var control = new AssetsPanelControl();

            // Act & Assert
            var stackPanels = control.Content as StackPanel;
            Assert.NotNull(stackPanels);

            // Verify Square Image Source
            var squareStackPanel = stackPanels.Children[1] as StackPanel;
            Assert.NotNull(squareStackPanel);
            var squareImage = squareStackPanel.Children.OfType<Image>().FirstOrDefault();
            Assert.NotNull(squareImage);
            Assert.Equal("UnitTestsPictures/square.png", squareImage.Source.ToString());

            // Verify Polygon Image Source
            var polygonStackPanel = stackPanels.Children[2] as StackPanel;
            Assert.NotNull(polygonStackPanel);
            var polygonImage = polygonStackPanel.Children.OfType<Image>().FirstOrDefault();
            Assert.NotNull(polygonImage);
            Assert.Equal("UnitTests/Pictures/polygon.png", polygonImage.Source.ToString());
        }*/
        
        /*[StaFact]
        public void AssetsPanelControl_ShouldInitializeWithCorrectElements()
        {
            // Arrange
            var control = new AssetsPanelControl();

            // Act
            // No action needed as we're testing initialization

            // Assert
            var stackPanel = control.Content as StackPanel;
            Assert.NotNull(stackPanel);
            Assert.Equal(4, stackPanel.Children.Count); // "Assets" TextBlock, Square Asset, Polygon Asset

            // Verify "Assets" TextBlock
            var assetsTextBlock = stackPanel.Children[0] as TextBlock;
            Assert.NotNull(assetsTextBlock);
            Assert.Equal("Assets", assetsTextBlock.Text);
            Assert.Equal(24, assetsTextBlock.FontSize);
            Assert.Equal(FontWeights.Bold, assetsTextBlock.FontWeight);

            // Verify Square Asset StackPanel
            var squareStackPanel = stackPanel.Children[1] as StackPanel;
            Assert.NotNull(squareStackPanel);
            Assert.Equal("Square", squareStackPanel.Tag);

            var squareImage = squareStackPanel.Children.OfType<Image>().FirstOrDefault();
            Assert.NotNull(squareImage);
            Assert.Equal("pack://application:,,,/Pictures/square.png", squareImage.Source.ToString());

            var squareTextBlock = squareStackPanel.Children.OfType<TextBlock>().FirstOrDefault();
            Assert.NotNull(squareTextBlock);
            Assert.Equal("Big Shelf (200x100 cm)", squareTextBlock.Text);

            // Verify Polygon Asset StackPanel
            var polygonStackPanel = stackPanel.Children[2] as StackPanel;
            Assert.NotNull(polygonStackPanel);
            Assert.Equal("Polygon", polygonStackPanel.Tag);

            var polygonImage = polygonStackPanel.Children.OfType<Image>().FirstOrDefault();
            Assert.NotNull(polygonImage);
            Assert.Equal("pack://application:,,,/Pictures/polygon.png", polygonImage.Source.ToString());

            var polygonTextBlock = polygonStackPanel.Children.OfType<TextBlock>().FirstOrDefault();
            Assert.NotNull(polygonTextBlock);
            Assert.Equal("Display Stand (100x150 cm)", polygonTextBlock.Text);
        }*/

    }
}
