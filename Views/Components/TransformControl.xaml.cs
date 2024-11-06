using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BPR2_Desktop.Views.Components
{
    public partial class TransformControl : UserControl
    {
        private UIElement _selectedElement;
        private readonly DesignCanvasControl _designCanvasControl;


        public TransformControl(UIElement selectedElement, DesignCanvasControl designCanvasControl)
        {
            InitializeComponent();
            _selectedElement = selectedElement;
            _designCanvasControl = designCanvasControl; // Store a reference to DesignCanvasControl


            // Initialize the rotation input field with the current rotation value of the element
            var transform = _selectedElement.RenderTransform as RotateTransform;
            if (transform != null)
            {
                RotationInput.Text = transform.Angle.ToString();
            }
            else
            {
                RotationInput.Text = "0"; // Default rotation
            }
        }

        private void RotationInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(RotationInput.Text, out double rotationAngle))
            {
                // Apply rotation to the selected element
                RotateTransform rotateTransform = new RotateTransform(rotationAngle);
                _selectedElement.RenderTransformOrigin = new Point(0.5, 0.5); // Rotate around the center
                _selectedElement.RenderTransform = rotateTransform;
                _designCanvasControl.ApplyRotationAndAdjustBoundingBox(_selectedElement, rotationAngle);
            }
        }
    }
}