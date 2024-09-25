using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BPR2_Desktop.Views.Components
{
    public partial class DesignCanvasControl : UserControl
    {
        private UIElement draggedElement;
        private Point lastPosition;
        private int squareCounter = 1;
        private int polygonCounter = 1;

        public DesignCanvasControl()
        {
            InitializeComponent();
        }
        
        
        public void LoadDesignFromFile(string filePath)
        {
            
            if (DesignCanvas == null)
            {
                MessageBox.Show("Canvas not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DesignCanvas.Children.Clear();

            string json = File.ReadAllText(filePath);
            var elementPositions = JsonSerializer.Deserialize<List<ElementPosition>>(json);

            if(elementPositions == null)
            {
                MessageBox.Show("Invalid file format or empty file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (var elementPosition in elementPositions)
            {
                Image newElement = new Image
                {
                    Width = 100,
                    Height = 100,
                    Stretch = Stretch.Uniform
                };

                if (elementPosition.ElementName.StartsWith("Square"))
                {
                    newElement.Source = new BitmapImage(new Uri("pack://application:,,,/Pictures/square.png"));
                    newElement.Name = $"Square{squareCounter++}";
                }
                else if (elementPosition.ElementName.StartsWith("Polygon"))
                {
                    newElement.Source = new BitmapImage(new Uri("pack://application:,,,/Pictures/polygon.png"));
                    newElement.Name = $"Polygon{polygonCounter++}";
                }

                Canvas.SetLeft(newElement, elementPosition.X);
                Canvas.SetTop(newElement, elementPosition.Y);

                // Attach events to allow the element to be moved within the canvas
                newElement.MouseLeftButtonDown += Element_MouseLeftButtonDown;
                newElement.MouseMove += Element_MouseMove;
                newElement.MouseLeftButtonUp += Element_MouseLeftButtonUp;

                // Add the element to the canvas
                DesignCanvas.Children.Add(newElement);
            }
        }
        
        public void SaveElementPositionsToFile(string filePath)
        {
            var elementPositions = new List<ElementPosition>();

            foreach (UIElement element in DesignCanvas.Children)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);

                if (double.IsNaN(left)) left = 0;
                if (double.IsNaN(top)) top = 0;

                string elementName = (element as FrameworkElement)?.Name ?? "UnnamedElement";

                elementPositions.Add(new ElementPosition(elementName)
                {
                    ElementName = elementName,
                    X = left,
                    Y = top
                });
            }

            string json = JsonSerializer.Serialize(elementPositions, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(filePath, json);
        }

        
        public class ElementPosition
        {
            public ElementPosition(string elementName)
            {
                ElementName = elementName;
            }

            public string ElementName { get; init; }
            public double X { get; init; }
            public double Y { get; init; }
        }

        // Handles the drop event when a new asset is dragged onto the canvas
        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string assetType = e.Data.GetData(DataFormats.StringFormat) as string;

                // Create an Image element for the corresponding asset
                Image newElement = new Image
                {
                    Width = 100,
                    Height = 100,
                    Stretch = Stretch.Uniform
                };

                if (assetType == "Square")
                {
                    newElement.Source = new BitmapImage(new Uri("pack://application:,,,/Pictures/square.png"));
                    newElement.Height = 40;
                    newElement.Name = $"Square{squareCounter++}";
                }
                else if (assetType == "Polygon")
                {
                    newElement.Source = new BitmapImage(new Uri("pack://application:,,,/Pictures/polygon.png"));
                    newElement.Height = 90;
                    newElement.Name = $"Polygon{polygonCounter++}";
                }

                if (newElement != null)
                {
                    // Position the new element where the drop occurred
                    Point dropPosition = e.GetPosition(DesignCanvas);
                    Canvas.SetLeft(newElement, dropPosition.X - 50);
                    Canvas.SetTop(newElement, dropPosition.Y - 50);

                    // Attach events to allow the element to be moved
                    newElement.MouseLeftButtonDown += Element_MouseLeftButtonDown;
                    newElement.MouseMove += Element_MouseMove;
                    newElement.MouseLeftButtonUp += Element_MouseLeftButtonUp;

                    // Add the element to the canvas
                    DesignCanvas.Children.Add(newElement);
                }
            }
        }

        // Handles dragging over the canvas (changing cursor to indicate drop target)
        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        // Handles element drag start inside the canvas
        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            draggedElement = sender as UIElement;
            if (draggedElement != null)
            {
                lastPosition = e.GetPosition(DesignCanvas);
                draggedElement.CaptureMouse();
            }
        }

        // Handles moving the dragged element within the canvas
        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedElement != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(DesignCanvas);

                // Calculate the delta movement
                double deltaX = currentPosition.X - lastPosition.X;
                double deltaY = currentPosition.Y - lastPosition.Y;

                // Get current position of the dragged element
                double currentLeft = Canvas.GetLeft(draggedElement);
                double currentTop = Canvas.GetTop(draggedElement);

                // Calculate new positions based on delta movement
                double newLeft = currentLeft + deltaX;
                double newTop = currentTop + deltaY;

                // Ensure the new position is within the canvas bounds
                double elementWidth = draggedElement.RenderSize.Width;
                double elementHeight = draggedElement.RenderSize.Height;

                // Restrict the element from moving outside the left and right boundaries
                if (newLeft < 0) newLeft = 0;
                if (newLeft + elementWidth > DesignCanvas.ActualWidth) newLeft = DesignCanvas.ActualWidth - elementWidth;

                // Restrict the element from moving outside the top and bottom boundaries
                if (newTop < 0) newTop = 0;
                if (newTop + elementHeight > DesignCanvas.ActualHeight) newTop = DesignCanvas.ActualHeight - elementHeight;

                // Update the position of the dragged element
                Canvas.SetLeft(draggedElement, newLeft);
                Canvas.SetTop(draggedElement, newTop);

                // Update last position
                lastPosition = currentPosition;
            }
        }


        // Handles when the user stops dragging the element
        private void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedElement != null)
            {
                draggedElement.ReleaseMouseCapture();
                draggedElement = null;
            }
        }
    }
}
