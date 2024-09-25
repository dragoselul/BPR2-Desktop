using System.IO;
using System.Windows;
using System.Windows.Controls;
using BPR2_Desktop.Views.Components;
using BPR2_Desktop.Views.Windows;
using Microsoft.Win32;

namespace BPR2_Desktop.Views.Pages;

public partial class DesignEditor : Page
{
    
    public DesignEditor()
    {
        InitializeComponent();
    }
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var designCanvasControl = FindName("DesignCanvasControl") as DesignCanvasControl;

        if (designCanvasControl != null)
        {
            // If a design is loaded, update the existing file
            if (AppState.Instance.CurrentDesignFile != null)
            {
                designCanvasControl.SaveElementPositionsToFile(AppState.Instance.CurrentDesignFile);
            }
            else
            {
                // If no design is loaded, ask for a new design name
                var inputDialog = new InputDialog("Enter the name for the new design:");
                if (inputDialog.ShowDialog() == true)
                {
                    string designName = inputDialog.ResponseText;

                    string projectDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName;
                    string dataDirectory = Path.Combine(projectDirectory, "Data");

                    if (!Directory.Exists(dataDirectory))
                    {
                        Directory.CreateDirectory(dataDirectory);
                    }

                    string filePath = Path.Combine(dataDirectory, $"{designName}.json");
                    designCanvasControl.SaveElementPositionsToFile(filePath);

                    // Store the file path in AppState for access across files
                    AppState.Instance.CurrentDesignFile = filePath;

                    MessageBox.Show($"Design saved as {designName}.json", "Save Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }


    
    private void SetDimensions_Click(object sender, RoutedEventArgs e)
    {
        SetDimensions dimensionsWindow = new SetDimensions();
        
        Window? mainWindow = Window.GetWindow(this); 

        if (mainWindow != null)
        {
            dimensionsWindow.Owner = mainWindow;
        }

        dimensionsWindow.ShowDialog();  
    }


    private void LoadDesign_Click(object sender, RoutedEventArgs e)
    {
        string projectDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName;
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
                designCanvasControl.LoadDesignFromFile(selectedFile);

                // Update the current design file in AppState
                AppState.Instance.CurrentDesignFile = selectedFile;
            }
        }
    }
}