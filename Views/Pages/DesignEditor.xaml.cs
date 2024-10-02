using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using BPR2_Desktop.Views.Components;
using BPR2_Desktop.Views.Windows;
using Microsoft.Win32;

namespace BPR2_Desktop.Views.Pages;

public partial class DesignEditor : Page
{
    private string currentDesignFile = null; // To track the loaded design file
    private double _currentWidth = 0;
    private double _currentLength = 0;
    private double _currentHeight = 0;

    public DesignEditor()
    {
        InitializeComponent();
    }

    public void UpdateDimensions(double width, double length, double height)
    {
        _currentWidth = width;
        _currentLength = length;
        _currentHeight = height;
    }

    // Method to retrieve the current dimensions
    public (double width, double length, double height) GetDimensions()
    {
        return (_currentWidth, _currentLength, _currentHeight);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var designCanvasControl = FindName("DesignCanvasControl") as DesignCanvasControl;

        if (designCanvasControl != null)
        {
            // If a design is loaded, update the existing file
            if (AppState.Instance.CurrentDesignFile != null)
            {
                SaveDesignToFile(AppState.Instance.CurrentDesignFile, designCanvasControl);
            }
            else
            {
                // If no design is loaded, ask for a new design name
                var inputDialog = new InputDialog("Enter the name for the new design:");
                if (inputDialog.ShowDialog() == true)
                {
                    string designName = inputDialog.ResponseText;

                    // Define the project directory and data folder
                    string projectDirectory = Directory
                        .GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName;
                    string dataDirectory = Path.Combine(projectDirectory, "Data");

                    // Create the Data directory if it doesn't exist
                    if (!Directory.Exists(dataDirectory))
                    {
                        Directory.CreateDirectory(dataDirectory);
                    }

                    // Define the file path
                    string filePath = Path.Combine(dataDirectory, $"{designName}.json");


                    // Store the file path in AppState for access across files
                    AppState.Instance.CurrentDesignFile = filePath;

                    // Save the design to the new file
                    SaveDesignToFile(filePath, designCanvasControl);


                    currentDesignFile = filePath; // Update the current design file

                    MessageBox.Show($"Design saved as {designName}.json", "Save Complete", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }
    }


    private void SaveDesignToFile(string filePath, DesignCanvasControl designCanvasControl)
    {
        // Retrieve the element positions
        var elements = designCanvasControl?.GetElementPositions();

        // Retrieve the stored dimensions from DesignEditor
        var (width, length, height) = GetDimensions(); // Use the stored dimensions

        if (width == 0 || length == 0)
        {
            MessageBox.Show("Error: Dimensions are not set correctly.", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        // Create the JSON structure with both dimensions and elements
        var designData = new
        {
            dimensions = new
            {
                X = width,
                Z = length,
                Y = height
            },
            ElementPositions = elements == null ? [] : elements
        };

        // Serialize the data to JSON
        string jsonString = JsonSerializer.Serialize(designData, new JsonSerializerOptions { WriteIndented = true });

        try
        {
            // Write the data to the JSON file
            File.WriteAllText(filePath, jsonString);
            MessageBox.Show($"Design saved successfully to {filePath}", "Save Complete", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save design: {ex.Message}");
        }
    }

    private void SetDimensions_Click(object sender, RoutedEventArgs e)
    {
        var setDimensionsWindow = new SetDimensions(DesignCanvasControl, this);
        Window? mainWindow = Window.GetWindow(this);

        if (mainWindow != null)
        {
            setDimensionsWindow.Owner = mainWindow;
        }

        setDimensionsWindow.ShowDialog(); // Show the SetDimensions window
    }
    
    private void LoadDesign_Click(object sender, RoutedEventArgs e)
    {
        string projectDirectory =
            Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName;
        string dataDirectory = Path.Combine(projectDirectory, "Data");
        
        if (!Directory.Exists(dataDirectory))
        {
            MessageBox.Show("Data folder does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var designFiles = Directory.GetFiles(dataDirectory, "*.json");

        if (designFiles.Length == 0)
        {
            MessageBox.Show("No saved designs found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Title = "Select a Design",
            Filter = "JSON Files (*.json)|*.json",
            InitialDirectory = dataDirectory
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFile = openFileDialog.FileName;

            var designCanvasControl = FindName("DesignCanvasControl") as DesignCanvasControl;
            if (designCanvasControl != null)
            {
                designCanvasControl.LoadDesignFromFile(selectedFile,this);
                AppState.Instance.CurrentDesignFile = selectedFile;
            }
        }
    }
}