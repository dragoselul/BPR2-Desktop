// File: TransformControlTests.cs

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BPR2_Desktop.Views.Components;
using Xunit;
 // Provided by Xunit.StaFact package

namespace BPR2_Desktop.Tests.Views.Components
{
    public class TransformControlTests
    {
        private readonly DesignCanvasControl _designCanvasControl;
        private readonly UIElement _testElement;

        public TransformControlTests()
        {
            // Initialize a real DesignCanvasControl instance
            _designCanvasControl = new DesignCanvasControl();

            // Create a test UIElement, e.g., a Button
            _testElement = new Button();
        }

        [StaFact]
        public void Initialization_WithRotateTransform_SetsRotationInputCorrectly()
        {
            // Arrange: Apply a RotateTransform with 45 degrees to the test element
            var rotateTransform = new RotateTransform(45);
            _testElement.RenderTransform = rotateTransform;

            // Act: Initialize the TransformControl
            var transformControl = new TransformControl(_testElement, _designCanvasControl);

            // Access the RotationInput TextBox
            var rotationInput = (TextBox)transformControl.FindName("RotationInput");

            // Assert: The TextBox should display "45"
            Assert.Equal("45", rotationInput.Text);
        }

        [StaFact]
        public void Initialization_WithoutRotateTransform_SetsRotationInputToZero()
        {
            // Arrange: Ensure the test element has no RenderTransform
            _testElement.RenderTransform = null;

            // Act: Initialize the TransformControl
            var transformControl = new TransformControl(_testElement, _designCanvasControl);

            // Access the RotationInput TextBox
            var rotationInput = (TextBox)transformControl.FindName("RotationInput");

            // Assert: The TextBox should display "0"
            Assert.Equal("0", rotationInput.Text);
        }

        [StaFact]
        public void RotationInput_TextChanged_WithValidInput_AppliesRotation()
        {
            // Arrange: Initialize the TransformControl
            var transformControl = new TransformControl(_testElement, _designCanvasControl);
            var rotationInput = (TextBox)transformControl.FindName("RotationInput");

            // Act: Change the TextBox text to a valid rotation angle
            rotationInput.Text = "90";

            // Assert: The UIElement's RenderTransform should be a RotateTransform with 90 degrees
            Assert.IsType<RotateTransform>(_testElement.RenderTransform);
            var rotateTransform = (RotateTransform)_testElement.RenderTransform;
            Assert.Equal(90, rotateTransform.Angle);
        }
        

        [StaFact]
        public void RotationInput_TextChanged_MultipleValidInputs_AppliesEachRotation()
        {
            // Arrange: Initialize the TransformControl
            var transformControl = new TransformControl(_testElement, _designCanvasControl);
            var rotationInput = (TextBox)transformControl.FindName("RotationInput");

            // Act & Assert:

            // First input: 30 degrees
            rotationInput.Text = "30";
            Assert.IsType<RotateTransform>(_testElement.RenderTransform);
            var rotateTransform1 = (RotateTransform)_testElement.RenderTransform;
            Assert.Equal(30, rotateTransform1.Angle);

            // Second input: 60 degrees
            rotationInput.Text = "60";
            Assert.IsType<RotateTransform>(_testElement.RenderTransform);
            var rotateTransform2 = (RotateTransform)_testElement.RenderTransform;
            Assert.Equal(60, rotateTransform2.Angle);

            // Third input: 90 degrees
            rotationInput.Text = "90";
            Assert.IsType<RotateTransform>(_testElement.RenderTransform);
            var rotateTransform3 = (RotateTransform)_testElement.RenderTransform;
            Assert.Equal(90, rotateTransform3.Angle);
        }
    }
}
