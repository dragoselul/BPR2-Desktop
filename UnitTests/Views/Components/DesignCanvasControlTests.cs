/*
// File: DesignCanvasControlTests.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BPR2_Desktop.Views;
using BPR2_Desktop.Views.Components;
using BPR2_Desktop.Views.Pages;
using Xunit;
using Path = System.IO.Path;


namespace BPR2_Desktop.Tests.Views.Components
{
    public class DesignCanvasControlTests : WpfTestBase
    {
        // Stub implementation of IDesignEditor
        public class StubDesignEditor : IDesignEditorService
        {
            public bool UpdateDimensionsCalled { get; private set; }
            public double UpdatedWidth { get; private set; }
            public double UpdatedLength { get; private set; }
            public double UpdatedHeight { get; private set; }

            public void UpdateDimensions(double width, double length, double height)
            {
                UpdateDimensionsCalled = true;
                UpdatedWidth = width;
                UpdatedLength = length;
                UpdatedHeight = height;
            }
        }

        /*[StaFact]
        public void LoadDesignFromFile_ShouldLoadElementsOntoCanvas()
        {
            // Arrange
            var stubEditor = new StubDesignEditor();

            // Create test JSON data
            var testDesign = new DesignCanvasControl.DesignData
            {
                dimensions = new DesignCanvasControl.Dimensions { width = 300, length = 400, height = 200 },
                elements = new List<DesignCanvasControl.ElementPosition>
                {
                    new DesignCanvasControl.ElementPosition("Square1") { X = 50, Z = 60, Rotation = 0 },
                    new DesignCanvasControl.ElementPosition("Polygon1") { X = 150, Z = 160, Rotation = 45 }
                }
            };

            // Serialize test data to JSON and write to a temporary file
            string testFilePath = Path.Combine(Path.GetTempPath(), "testDesign.json");
            File.WriteAllText(testFilePath, JsonSerializer.Serialize(testDesign));

            try
            {
                // Act
                CanvasControl.LoadDesignFromFile(testFilePath, stubEditor);

                // Assert
                // Verify that two images are loaded onto the canvas
                Assert.Equal(2, CanvasControl.DesignCanvas.Children.OfType<Image>().Count());

                // Verify the first element
                var firstElement = CanvasControl.DesignCanvas.Children.OfType<Image>().FirstOrDefault();
                Assert.NotNull(firstElement);
                Assert.Equal("Square1", firstElement.Name);
                Assert.Equal(50, Canvas.GetLeft(firstElement));
                Assert.Equal(60, Canvas.GetTop(firstElement));

                // Verify the second element
                var secondElement = CanvasControl.DesignCanvas.Children.OfType<Image>().LastOrDefault();
                Assert.NotNull(secondElement);
                Assert.Equal("Polygon1", secondElement.Name);
                Assert.Equal(150, Canvas.GetLeft(secondElement));
                Assert.Equal(160, Canvas.GetTop(secondElement));

                // Verify rotation
                var rotateTransform = secondElement.RenderTransform as RotateTransform;
                Assert.NotNull(rotateTransform);
                Assert.Equal(45, rotateTransform.Angle);

                // Verify that UpdateDimensions was called correctly
                Assert.True(stubEditor.UpdateDimensionsCalled, "UpdateDimensions was not called.");
                Assert.Equal(300, stubEditor.UpdatedWidth);
                Assert.Equal(400, stubEditor.UpdatedLength);
                Assert.Equal(200, stubEditor.UpdatedHeight);
            }
            finally
            {
                // Cleanup
                if (File.Exists(testFilePath))
                    File.Delete(testFilePath);
            }
        }#1#

        [StaFact]
        public void GetElementPositions_ShouldReturnCorrectPositions()
        {
            // Arrange
            var squareImage = new Image
            {
                Source = new BitmapImage(new Uri("UnitTests/Pictures/square.png", UriKind.Relative)),
                Name = "Square1"
            };

            // Set position on the canvas
            Canvas.SetLeft(squareImage, 100);
            Canvas.SetTop(squareImage, 200);

            CanvasControl.DesignCanvas.Children.Add(squareImage);

            // Act
            var positions = CanvasControl.GetElementPositions();

            // Assert
            Assert.Single(positions);
            var position = positions.First();
            Assert.Equal("Square1", position.ElementName);
            Assert.Equal(100, position.X);
            Assert.Equal(200, position.Z);
            Assert.Equal(0, position.Rotation); // Default rotation
        }

        [StaFact]
        public void ZoomIn_Click_ShouldIncreaseZoom()
        {
            // Arrange
            double initialZoom = CanvasControl.GetZoomLevel(CanvasControl);

            // Act
            CanvasControl.ZoomIn_Click(new object(), new RoutedEventArgs());

            // Assert
            double newZoom = CanvasControl.GetZoomLevel(CanvasControl);
            Assert.True(newZoom > initialZoom, "Zoom level did not increase.");
        }

        [StaFact]
        public void ZoomOut_ShouldDecreaseZoomBySpecifiedAmount()
        {
            // Arrange
            CanvasControl.SetZoomLevel(CanvasControl, 0.9);
            double zoomFactor = CanvasControl.ZoomFactor;

            // Act
            CanvasControl.ZoomOut_Click(new object(), new RoutedEventArgs());

            // Assert
            double actualZoom = CanvasControl.GetZoomLevel(CanvasControl);
            double expectedZoom = 0.9 - zoomFactor;

            // Use a tolerance to handle floating-point inaccuracies
            Assert.InRange(actualZoom, expectedZoom - 0.00001, expectedZoom + 0.00001);
        }

        [StaFact]
        public void Element_MouseMove_ShouldUpdateElementPosition()
        {
            // Arrange
            var element = new Image
            {
                Source = new BitmapImage(new Uri("UnitTests/Pictures/square.png", UriKind.Relative)),
                Name = "DraggableElement",
                Width = 100,
                Height = 100
            };

            // Set initial position
            Canvas.SetLeft(element, 50);
            Canvas.SetTop(element, 50);
            CanvasControl.DesignCanvas.Children.Add(element);

            // Simulate dragging
            var initialPosition = new Point(50, 50);
            CanvasControl.Element_MouseLeftButtonDown(element, new MouseButtonEventArgs(
                Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseLeftButtonDownEvent,
                Source = element
            });

            var simulatedMouseMove = new Point(100, 150); // Simulated new mouse position
            Canvas.SetLeft(element, simulatedMouseMove.X);
            Canvas.SetTop(element, simulatedMouseMove.Y);

            // Act
            CanvasControl.Element_MouseMove(element, new MouseEventArgs(
                Mouse.PrimaryDevice, 0)
            {
                RoutedEvent = UIElement.MouseMoveEvent,
                Source = element
            });

            // Assert
            double updatedLeft = Canvas.GetLeft(element);
            double updatedTop = Canvas.GetTop(element);

            Assert.NotEqual(initialPosition.X, updatedLeft);
            Assert.NotEqual(initialPosition.Y, updatedTop);
            Assert.Equal(simulatedMouseMove.X, updatedLeft);
            Assert.Equal(simulatedMouseMove.Y, updatedTop);
        }

        [StaFact]
        public void ApplyRotation_ShouldSetCorrectRotation()
        {
            // Arrange
            var element = new Image
            {
                Name = "RotatableElement",
                Width = 100,
                Height = 100
            };

            // Act
            CanvasControl.ApplyRotation(element, 45);

            // Assert
            var rotateTransform = element.RenderTransform as RotateTransform;
            Assert.NotNull(rotateTransform);
            Assert.Equal(45, rotateTransform.Angle);
        }

        /*[StaFact]
        public void DrawBoundingBox_ShouldAddRedRectangleAroundElement_WithInMemoryImage()
        {
            // Arrange
            // Create a simple in-memory bitmap
            var bitmap = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
            var drawingVisual = new System.Windows.Media.DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                // Draw a blue rectangle as the image content
                drawingContext.DrawRectangle(Brushes.Blue, null, new Rect(0, 0, 100, 100));
            }
            bitmap.Render(drawingVisual);

            var element = new Image
            {
                Name = "TestElement",
                Width = 100,
                Height = 100,
                Source = bitmap
            };
            Canvas.SetLeft(element, 50);
            Canvas.SetTop(element, 50);
            CanvasControl.DesignCanvas.Children.Add(element);

            // Act
            CanvasControl.DrawBoundingBox(element);

            // Assert
            var boundingBox = CanvasControl.DesignCanvas.Children
                .OfType<Rectangle>()
                .FirstOrDefault(r => r.Tag as string == "BoundingBox");

            Assert.NotNull(boundingBox);
            Assert.Equal(100, boundingBox.Width);
            Assert.Equal(100, boundingBox.Height);
            Assert.Equal(50, Canvas.GetLeft(boundingBox));
            Assert.Equal(50, Canvas.GetTop(boundingBox));
            Assert.Equal(Brushes.Red, boundingBox.Stroke);
            Assert.Equal(2, boundingBox.StrokeThickness);
        }#1#


        [StaFact]
        public void UpdateDesignCanvas_ShouldSetDimensionsAndAddBorder()
        {
            // Arrange
            double newWidth = 500;
            double newHeight = 600;

            // Act
            CanvasControl.UpdateDesignCanvas("TestShape", newWidth, newHeight);

            // Assert
            Assert.Equal(newWidth, CanvasControl.DesignCanvas.Width);
            Assert.Equal(newHeight, CanvasControl.DesignCanvas.Height);

            // Verify that a black border rectangle is added
            var border = CanvasControl.DesignCanvas.Children
                .OfType<Rectangle>()
                .FirstOrDefault(r => r.Stroke == Brushes.Black && r.StrokeThickness == 2);

            Assert.NotNull(border);
            Assert.Equal(newWidth, border.Width);
            Assert.Equal(newHeight, border.Height);
            Assert.Equal(0, Canvas.GetLeft(border));
            Assert.Equal(0, Canvas.GetTop(border));
        }

        [StaFact]
        public void OnMouseWheelZoom_ShouldAdjustZoomLevel()
        {
            // Arrange
            double initialZoom = CanvasControl.GetZoomLevel(CanvasControl);
            Assert.Equal(1.0, initialZoom, 5); // Allowing precision

            // Simulate mouse wheel up (zoom in)
            CanvasControl.OnMouseWheelZoom(CanvasControl, new MouseWheelEventArgs(
                    Mouse.PrimaryDevice, 0, 120) // Delta = 120 indicates mouse wheel up
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = CanvasControl
                });

            double zoomAfterIn = CanvasControl.GetZoomLevel(CanvasControl);
            Assert.Equal(1.1, zoomAfterIn, 5); // ZoomFactor = 0.1

            // Simulate mouse wheel down (zoom out)
            CanvasControl.OnMouseWheelZoom(CanvasControl, new MouseWheelEventArgs(
                    Mouse.PrimaryDevice, 0, -120) // Delta = -120 indicates mouse wheel down
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = CanvasControl
                });

            double zoomAfterOut = CanvasControl.GetZoomLevel(CanvasControl);
            Assert.Equal(1.0, zoomAfterOut, 5); // Should return to initial zoom
        }

        [StaFact]
        public void Canvas_Drop_ShouldAddElementToCanvas()
        {
            // Arrange
            var data = new DataObject();
            var element = new Image
            {
                Name = "DroppedElement",
                Width = 100,
                Height = 100,
                Source = new BitmapImage(new Uri("UnitTests/Pictures/square.png", UriKind.Absolute))
            };
            data.SetData(typeof(Image), element);

            // Create DragEventArgs
            var dragEventArgs = new DragEventArgs(DragDrop.PreviewDropEvent, Mouse.PrimaryDevice, 0, 0, DragDropEffects.Copy, data)
            {
                RoutedEvent = DragDrop.DropEvent,
                Source = CanvasControl.DesignCanvas
            };

            // Act
            CanvasControl.Canvas_Drop(CanvasControl.DesignCanvas, dragEventArgs);

            // Assert
            var addedElement = CanvasControl.DesignCanvas.Children.OfType<Image>().FirstOrDefault(e => e.Name == "DroppedElement");
            Assert.NotNull(addedElement);
            Assert.Equal(100, addedElement.Width);
            Assert.Equal(100, addedElement.Height);
        }
        
        [StaFact]
        public void Canvas_DragOver_ShouldAllowCopyEffect()
        {
            // Arrange
            var data = new DataObject();
            var dragEventArgs = new DragEventArgs(DragDrop.PreviewDragOverEvent, Mouse.PrimaryDevice, 0, 0, DragDropEffects.Copy, data)
            {
                RoutedEvent = DragDrop.DragOverEvent,
                Source = CanvasControl.DesignCanvas
            };

            // Act
            CanvasControl.Canvas_DragOver(CanvasControl.DesignCanvas, dragEventArgs);

            // Assert
            Assert.Equal(DragDropEffects.Copy, dragEventArgs.Effects);
            Assert.True(dragEventArgs.Handled);
        }
        
        [StaFact]
        public void Canvas_MouseLeftButtonDown_ShouldDeselectAllElements()
        {
            // Arrange
            var element1 = new Image
            {
                Name = "Element1",
                Width = 100,
                Height = 100,
                Source = new BitmapImage(new Uri("UnitTests/Pictures/square.png", UriKind.Absolute)),
                Tag = "Selected" // Simulate that this element is selected
            };
            Canvas.SetLeft(element1, 100);
            Canvas.SetTop(element1, 200);
            CanvasControl.DesignCanvas.Children.Add(element1);

            // Create MouseButtonEventArgs
            var mouseButtonEventArgs = new MouseButtonEventArgs(
                Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseLeftButtonDownEvent,
                Source = CanvasControl.DesignCanvas
            };

            // Act
            CanvasControl.Element_MouseLeftButtonDown(CanvasControl.DesignCanvas, mouseButtonEventArgs);

            // Assert
            // Assuming that deselecting removes the "Selected" tag
            var updatedElement = CanvasControl.DesignCanvas.Children.OfType<Image>().FirstOrDefault(e => e.Name == "Element1");
            Assert.NotNull(updatedElement);
            Assert.Null(updatedElement.Tag); // Tag should be cleared
        }
        
        [StaFact]
        public void Canvas_MouseUp_ShouldStopPanning()
        {
            // Arrange
            // Start panning by simulating MouseDown
            var mouseDownArgs = new MouseButtonEventArgs(
                Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseDownEvent,
                Source = CanvasControl.DesignCanvas
            };
            CanvasControl.Canvas_MouseDown(CanvasControl.DesignCanvas, mouseDownArgs);

            // Create MouseButtonEventArgs for MouseUp
            var mouseUpEventArgs = new MouseButtonEventArgs(
                Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseUpEvent,
                Source = CanvasControl.DesignCanvas
            };

            // Act
            CanvasControl.Canvas_MouseUp(CanvasControl.DesignCanvas, mouseUpEventArgs);

            // Assert
            // Assuming panning is controlled via a flag
            // Replace 'IsPanning' with your actual implementation
            // Example:
            // Assert.False(CanvasControl.IsPanning);
        }
        
        [StaFact]
        public void Canvas_MouseDown_ShouldStartPanning()
        {
            // Arrange
            // Create MouseButtonEventArgs for left button down
            var mouseButtonEventArgs = new MouseButtonEventArgs(
                Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseDownEvent,
                Source = CanvasControl.DesignCanvas
            };

            // Act
            CanvasControl.Canvas_MouseDown(CanvasControl.DesignCanvas, mouseButtonEventArgs);

            // Assert
            // Assuming panning sets a flag or modifies canvas state
            // Replace 'IsPanning' with your actual implementation
            // Example:
            // Assert.True(CanvasControl.IsPanning);
        }
        
        [StaFact]
        public void Element_MouseLeftButtonDown_ShouldSelectElement()
        {
            // Arrange
            var element = new Image
            {
                Name = "SelectableElement",
                Width = 100,
                Height = 100,
                Source = new BitmapImage(new Uri("pack://application:,,,/UnitTests;component/Pictures/square.png", UriKind.Absolute)),
                Tag = null // Initially not selected
            };
            Canvas.SetLeft(element, 250);
            Canvas.SetTop(element, 350);
            CanvasControl.DesignCanvas.Children.Add(element);

            // Create MouseButtonEventArgs
            var mouseButtonEventArgs = new MouseButtonEventArgs(
                Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseLeftButtonDownEvent,
                Source = element
            };

            // Act
            CanvasControl.Element_MouseLeftButtonDown(element, mouseButtonEventArgs);

            // Assert
            // Assuming selection is marked by setting the "Selected" tag
            Assert.Equal("Selected", element.Tag as string);
        }
        
        [StaFact]
        public void DrawPivotPoint_ShouldAddPivotMarkerToElement()
        {
            // Arrange
            var element = new Image
            {
                Name = "ElementWithPivot",
                Width = 100,
                Height = 100,
                Source = new BitmapImage(new Uri("pack://application:,,,/UnitTests;component/Pictures/square.png", UriKind.Absolute))
            };
            Canvas.SetLeft(element, 200);
            Canvas.SetTop(element, 300);
            CanvasControl.DesignCanvas.Children.Add(element);

            // Act
            CanvasControl.DrawPivotPoint(element);

            // Assert
            var pivotPoint = CanvasControl.DesignCanvas.Children
                .OfType<Ellipse>()
                .FirstOrDefault(e => e.Tag as string == "PivotPoint");

            Assert.NotNull(pivotPoint);
            Assert.Equal(10, pivotPoint.Width);
            Assert.Equal(10, pivotPoint.Height);
            Assert.Equal(200 + (element.Width / 2) - (pivotPoint.Width / 2), Canvas.GetLeft(pivotPoint), precision: 0.001);
            Assert.Equal(300 + (element.Height / 2) - (pivotPoint.Height / 2), Canvas.GetTop(pivotPoint), precision: 0.001);
            Assert.Equal(Brushes.Green, pivotPoint.Fill);
        }




        
    }



}
*/
