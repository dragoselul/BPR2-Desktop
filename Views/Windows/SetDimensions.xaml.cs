using System.Windows;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;

namespace BPR2_Desktop.Views.Windows;

public partial class SetDimensions : FluentWindow
{
    public SetDimensions()
    {
        InitializeComponent();
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
            var selectedShape = (ShapeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();

            // Prepare the data object to save
            object dataToSave;

            if (selectedShape == "Square")
            {
                // Capture values for Square
                string width = WidthTextBox.Text;
                string length = LengthTextBox.Text;
                string height = HeightTextBox.Text;
                
                // Check if any of the required fields are empty
                if (string.IsNullOrWhiteSpace(width) || string.IsNullOrWhiteSpace(length) || string.IsNullOrWhiteSpace(height))
                {
                    MessageBox.Show("Please fill in all fields for Square dimensions (Width, Length, and Height).", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Stop the method execution
                }

                dataToSave = new
                {
                    Shape = "Square",
                    Width = width,
                    Length = length,
                    Height = height
                };
            }
            else if (selectedShape == "Complicated Shape")
            {
                // Capture value for Wall Length
                string wallLength = WallLengthTextBox.Text;
                
                // Check if the wall length field is empty
                if (string.IsNullOrWhiteSpace(wallLength))
                {
                    MessageBox.Show("Please fill in the Wall Length field for the Complicated Shape.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Stop the method execution
                }

                dataToSave = new
                {
                    Shape = "Complicated Shape",
                    WallLength = wallLength
                };
            }
            else
            {
                MessageBox.Show("Please select a shape.");
                return;
            }

            // Serialize the data to JSON and write to a file
            string jsonString = JsonSerializer.Serialize(dataToSave, new JsonSerializerOptions { WriteIndented = true });
            
            // Get the path to the project's root directory
            string projectDirectory = Directory.GetCurrentDirectory();
            
            // Define the path to the data folder
            string dataDirectory = Path.Combine(projectDirectory, "Data");
            
            // Create the data directory if it doesn't exist
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            // Specify the file path (you can change the path as needed)
            string filePath = Path.Combine(dataDirectory, "dimensions.json");

            try
            {
                File.WriteAllText(filePath, jsonString);
                MessageBox.Show($"Data saved successfully to {filePath}");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save data: {ex.Message}");
                this.Close();
            }
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