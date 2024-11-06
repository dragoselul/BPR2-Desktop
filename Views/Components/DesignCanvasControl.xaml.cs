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
        public event Action<UIElement> ElementClicked;
        public event Action CanvasClicked;

        private readonly Dictionary<UIElement, (double width, double height)> _originalDimensions = new();

        public DesignCanvasControl()
        {
            InitializeComponent();
            this.MouseWheel += OnMouseWheelZoom;
        }

        private (double newWidth, double newHeight) GetRotatedBoundingBox(double width, double height, double angle)
        {
            // Normalize the angle to fall within [0, 360) degrees
            angle = angle % 360;
            if (angle < 0) angle += 360;

            // Swap width and height only at 90 and 270 degrees
            if (angle == 90 || angle == 270)
            {
                return (height, width);
            }

            // At 180 degrees, the width and height remain the same
            return (width, height);
        }

        public void ApplyRotationAndAdjustBoundingBox(UIElement element, double rotationAngle)
        {
            // Store the original dimensions if not already stored
            if (!_originalDimensions.ContainsKey(element))
            {
                _originalDimensions[element] = (element.RenderSize.Width, element.RenderSize.Height);
            }

            // Get the original dimensions for calculations
            var (originalWidth, originalHeight) = _originalDimensions[element];

            // Calculate the rotated bounding box dimensions based on original size
            var (newWidth, newHeight) = GetRotatedBoundingBox(originalWidth, originalHeight, rotationAngle);

            // Apply rotation
            RotateTransform rotateTransform = new RotateTransform(rotationAngle);
            element.RenderTransformOrigin = new Point(0.5, 0.5); // Rotate around the center
            element.RenderTransform = rotateTransform;

            // Set the element's size only at 90 or 270 degrees
            if (rotationAngle == 90 || rotationAngle == 270)
            {
                element.SetValue(WidthProperty, newWidth);
                element.SetValue(HeightProperty, newHeight);
            }
            else
            {
                // Reset to original dimensions at other angles
                element.SetValue(WidthProperty, originalWidth);
                element.SetValue(HeightProperty, originalHeight);
            }
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
                            Height = 40,
                            Stretch = Stretch.UniformToFill,
                            RenderTransformOrigin = new Point(0.5, 0.5)
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

                        // Apply the saved rotation
                        RotateTransform rotateTransform = new RotateTransform(element.Rotation);
                        newElement.RenderTransformOrigin = new Point(0.5, 0.5); // Rotate around center
                        newElement.RenderTransform = rotateTransform;

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
            [JsonPropertyName("ElementPositions")] public List<ElementPosition> elements { get; set; }
        }

        public class Dimensions
        {
            [JsonPropertyName("X")] public double width { get; init; }
            [JsonPropertyName("Z")] public double length { get; init; }
            [JsonPropertyName("Y")] public double height { get; init; }
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
            [JsonPropertyName("Rotation")] public double Rotation { get; init; }
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

                var rotateTransform = element.RenderTransform as RotateTransform;
                double rotation = rotateTransform?.Angle ?? 0; // Default to 0 if no rotation is applied


                if (!string.IsNullOrEmpty(elementName))
                {
                    elementPositions.Add(new ElementPosition(elementName)
                    {
                        ElementName = elementName,
                        X = left,
                        Z = top,
                        Rotation = rotation // Save the rotation
                    });
                }
            }

            return elementPositions;
        }


        // Handles element drag start inside the canvas
        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIElement clickedElement = sender as UIElement;

            if (clickedElement != null)
            {
                // Set the RenderTransformOrigin to rotate around the center
                clickedElement.RenderTransformOrigin = new Point(0.5, 0.5);

                // Raise the ElementClicked event
                ElementClicked?.Invoke(clickedElement);

                // Capture the mouse for dragging and store the initial position
                draggedElement = clickedElement;
                lastPosition = e.GetPosition(DesignCanvas);

                // Capture mouse input to allow dragging
                draggedElement.CaptureMouse();
            }

            e.Handled = true;
            draggedElement = sender as UIElement;
            if (draggedElement != null)
            {
                lastPosition = e.GetPosition(DesignCanvas);
                draggedElement.CaptureMouse();
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Raise the CanvasClicked event when clicking on empty space
            CanvasClicked?.Invoke();
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

                Debug.WriteLine(
                    $"Before Scroll: Horizontal={scrollViewer.HorizontalOffset}, Vertical={scrollViewer.VerticalOffset}");

                _lastDragPoint = currentPos;
            }
        }


        // Handle drag stop
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_lastDragPoint.HasValue)
            {
                Mouse.Capture(null); // Release the mouse capture
                _lastDragPoint = null;
            }
        }

        public void UpdateDesignCanvas(string shape, double widthInMeters, double lengthInMeters)
        {
            DesignCanvas.Width = widthInMeters;
            DesignCanvas.Height = lengthInMeters;

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