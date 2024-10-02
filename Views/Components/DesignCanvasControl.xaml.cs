using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BPR2_Desktop.Views.Pages;

namespace BPR2_Desktop.Views.Components
{
    public partial class DesignCanvasControl : UserControl
    {
        private UIElement draggedElement;
        private Point lastPosition;
        private int squareCounter = 1;
        private int polygonCounter = 1;
        
        private Point? _lastDragPoint;
        private double _zoom = 1.0;
        private const double ZoomFactor = 0.1;

        public DesignCanvasControl()
        {
            InitializeComponent();
            this.MouseWheel += OnMouseWheelZoom;
        }


        public void LoadDesignFromFile(string filePath, DesignEditor designEditor)
        {
            try
            {
                // Read the JSON file content
                var jsonString = File.ReadAllText(filePath);

                // Deserialize the JSON into a DesignData object
                var designData = JsonSerializer.Deserialize<DesignData>(jsonString);
                
                // Null check for designData
                if (designData == null)
                {
                    throw new Exception("Failed to load design data. The file is empty or formatted incorrectly.");
                }

                // Check for dimensions and apply them to the canvas
                if (designData.dimensions != null)
                {
                    double width = designData.dimensions.width;
                    double length = designData.dimensions.length;
                    double height = designData.dimensions.height;

                    // Update the canvas dimensions or shape
                    UpdateDesignCanvas("Loaded Design", width, length);

                    // Update the dimensions in the DesignEditor
                    designEditor.UpdateDimensions(width, length, height);
                }
                else
                {
                    throw new Exception("Dimensions not found or incorrectly formatted in the file.");
                }

                // Clear existing elements from the canvas
                DesignCanvas.Children.Clear();


                // Recreate the elements on the canvas using images
                foreach (var element in designData.elements)
                {

                    Image newElement = null;

                    if (element.ElementName.Contains("Square"))
                    {
                        // Use the image for the rectangle (e.g., square.png)
                        newElement = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/Pictures/square.png")),
                            Width = 100,
                            Height = 40
                        };
                        // Set the name for the element
                        newElement.Name = element.ElementName;
                    }
                    else if (element.ElementName.Contains("Polygon"))
                    {
                        // Use the image for the polygon (e.g., polygon.png)
                        newElement = new Image
                        {
                            Source = new BitmapImage(new Uri("pack://application:,,,/Pictures/polygon.png")),
                            Width = 100,
                            Height = 90
                        };
                        // Set the name for the element
                        newElement.Name = element.ElementName;
                    }

                    if (newElement != null)
                    {
                        // Set position of the element
                        Canvas.SetLeft(newElement, element.X);
                        Canvas.SetTop(newElement, element.Z);

                        // Attach the drag event handlers
                        newElement.MouseLeftButtonDown += Element_MouseLeftButtonDown;
                        newElement.MouseMove += Element_MouseMove;
                        newElement.MouseLeftButtonUp += Element_MouseLeftButtonUp;

                        // Add the element to the canvas
                        DesignCanvas.Children.Add(newElement);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading design: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        public class DesignData
        {
            public Dimensions dimensions { get; set; }
            [JsonPropertyName("ElementPositions")]
            public List<ElementPosition> elements { get; set; }
        }

        public class Dimensions
        {
            [JsonPropertyName("X")]
            public double width { get; init; }
            [JsonPropertyName("Z")]
            public double length { get; init; }
            [JsonPropertyName("Y")]
            public double height { get; init; }
        }


        public class ElementPosition
        {
            public ElementPosition(string elementName)
            {
                ElementName = elementName;
            }

            [JsonPropertyName("ElementName")] public string ElementName { get; init; }
            [JsonPropertyName("X")] public double X { get; init; }
            [JsonPropertyName("Z")] public double Z { get; init; }
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

        public List<ElementPosition> GetElementPositions()
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
                    Z = top
                });
            }

            return elementPositions;
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
                if (newLeft + elementWidth > DesignCanvas.ActualWidth)
                    newLeft = DesignCanvas.ActualWidth - elementWidth;

                // Restrict the element from moving outside the top and bottom boundaries
                if (newTop < 0) newTop = 0;
                if (newTop + elementHeight > DesignCanvas.ActualHeight)
                    newTop = DesignCanvas.ActualHeight - elementHeight;

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

        private void OnMouseWheelZoom(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _zoom += ZoomFactor;
            }
            else if (_zoom > ZoomFactor)
            {
                _zoom -= ZoomFactor;
            }

            ApplyZoom();
            e.Handled = true;
        }
        
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            _zoom += ZoomFactor;
            ApplyZoom();
        }

        // Zoom out button click event handler
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (_zoom > ZoomFactor)
            {
                _zoom -= ZoomFactor;
            }
            ApplyZoom();
        }

        private void ApplyZoom()
        {
            scaleTransform.ScaleX = _zoom;
            scaleTransform.ScaleY = _zoom;
        }
        
        // Handle dragging start
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == DesignCanvas)
            {
                _lastDragPoint = e.GetPosition(scrollViewer);
                Mouse.Capture(DesignCanvas);
            }
        }
        
        // Handle dragging movement
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_lastDragPoint.HasValue)
            {
                Point currentPos = e.GetPosition(scrollViewer);
                double deltaX = currentPos.X - _lastDragPoint.Value.X;
                double deltaY = currentPos.Y - _lastDragPoint.Value.Y;
                
                Debug.WriteLine($"currentPos: {currentPos}, deltaX: {deltaX}, deltaY: {deltaY}");


                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - deltaX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - deltaY);
                
                Debug.WriteLine($"Before Scroll: Horizontal={scrollViewer.HorizontalOffset}, Vertical={scrollViewer.VerticalOffset}");

                _lastDragPoint = currentPos;
            }
        }

        

        // Handle drag stop
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_lastDragPoint.HasValue)
            {
                Mouse.Capture(null);  // Release the mouse capture
                _lastDragPoint = null;
            }
        }

        public void UpdateDesignCanvas(string shape, double widthInMeters, double lengthInMeters)
        {
            double scaleFactor = 100; // Assume 1 meter = 100 pixels
            double scaledWidth = widthInMeters * scaleFactor;
            double scaledLength = lengthInMeters * scaleFactor;

            DesignCanvas.Width = scaledWidth;
            DesignCanvas.Height = scaledLength;

            // Clear any existing children before adding visualizations
            DesignCanvas.Children.Clear();

            // Optional: Draw a border or indicator for the updated dimensions
            Rectangle border = new Rectangle
            {
                Width = DesignCanvas.Width,
                Height = DesignCanvas.Height,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetLeft(border, 0);
            Canvas.SetTop(border, 0);
            DesignCanvas.Children.Add(border);
        }
    }
}