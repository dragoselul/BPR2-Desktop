using System.Windows;
using System.Windows.Controls;
using BPR2_Desktop.Views.Windows;

namespace BPR2_Desktop.Views.Pages;

public partial class DesignEditor : Page
{
    public DesignEditor()
    {
        InitializeComponent();
    }
    
    private void SetDimensions_Click(object sender, RoutedEventArgs e)
    {
        // Create an instance of the DimensionsWindow
        SetDimensions dimensionsWindow = new SetDimensions();
            
        // Get the MainWindow (or any parent Window of the current Page)
        Window? mainWindow = Window.GetWindow(this); 

        // Set MainWindow as the owner of the popup window
        if (mainWindow != null)
        {
            dimensionsWindow.Owner = mainWindow;
        }

        // Show the popup window as a modal dialog (blocking)
        dimensionsWindow.ShowDialog();  
    }
}