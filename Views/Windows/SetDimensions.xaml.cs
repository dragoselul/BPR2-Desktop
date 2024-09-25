using System.Windows;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using BPR2_Desktop.Views.Components;
using BPR2_Desktop.Views.Pages;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;

namespace BPR2_Desktop.Views.Windows;

public partial class SetDimensions : FluentWindow
{
    private readonly DesignCanvasControl _designCanvasControl;  // Reference to the design canvas control
    private readonly DesignEditor _designEditor;  // Reference to DesignEditor
    public SetDimensions(DesignCanvasControl designCanvasControl, DesignEditor designEditor)
    {
        InitializeComponent();
        _designCanvasControl = designCanvasControl;  // Store the reference
        _designEditor = designEditor;

        // Initially hide both grids
        SquareDimensionsGrid.Visibility = Visibility.Collapsed;
        ComplicatedShapeGrid.Visibility = Visibility.Collapsed;
    }
    
    private void ShapeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        var selectedShape = (ShapeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();

        if (selectedShape == "Square")
        {
            // Show the width, length, and height input boxes
            SquareDimensionsGrid.Visibility = Visibility.Visible;
            ComplicatedShapeGrid.Visibility = Visibility.Collapsed;
        }
        else if (selectedShape == "Complicated Shape")
        {
            // Show the wall length input box for complicated shape
            SquareDimensionsGrid.Visibility = Visibility.Collapsed;
            ComplicatedShapeGrid.Visibility = Visibility.Visible;
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var selectedShape = (ShapeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        if (selectedShape == "Square")
        {
            if (double.TryParse(WidthTextBox.Text, out double width) && double.TryParse(LengthTextBox.Text, out double length) && double.TryParse(LengthTextBox.Text, out double height))
            {
                // Update the canvas in DesignEditor and pass the dimensions
                _designEditor.DesignCanvasControl.UpdateDesignCanvas("Square", width, length);
                _designEditor.UpdateDimensions(width, length, height);  // Store dimensions in DesignEditor
            }
            else
            {
                MessageBox.Show("Please enter valid numeric values for width and length.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
        else if (selectedShape == "Complicated Shape")
        {
            if (double.TryParse(WallLengthTextBox.Text, out double wallLength))
            {
                // Update the canvas in DesignEditor and pass the dimensions
                _designEditor.DesignCanvasControl.UpdateDesignCanvas("Complicated Shape", wallLength, wallLength);
                _designEditor.UpdateDimensions(wallLength, wallLength, wallLength);  // Store dimensions in DesignEditor
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric value for wall length.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        this.Close();  // Close the SetDimensions window
    }
    




    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    
    // Helper method to get the value from TextBox based on Grid row index
    private string GetTextBoxValue(Grid grid, int rowIndex)
    {
        var textBox = grid.Children[rowIndex * grid.ColumnDefinitions.Count + 1] as System.Windows.Controls.TextBox;
        return textBox?.Text ?? string.Empty;
    }
}