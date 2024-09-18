using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BPR2_Desktop.Home;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void SaveBlockData_Click(object sender, RoutedEventArgs e)
    {
        // string blockPosition = BlockPositionTextBox.Text;
        // string filePath = "blockData.txt";
        // File.WriteAllText(filePath, blockPosition);
        // MessageBox.Show($"Block position saved to {filePath}");
    }

    private void LaunchVR_Click(object sender, RoutedEventArgs e)
    {
        // try
        // {
        //     string unityVRPath = @"C:\Path\To\Your\UnityVRBuild.exe";
        //     Process.Start(unityVRPath);
        // }
        // catch (Exception ex)
        // {
        //     MessageBox.Show($"Failed to launch VR app: {ex.Message}");
        // }
    }

    private void SetDimensions_Click(object sender, RoutedEventArgs e)
    {
        // Create an instance of the DimensionsWindow
        SetDimensions dimensionsWindow = new SetDimensions();
            
        // Show the DimensionsWindow on top of the MainWindow
        dimensionsWindow.Owner = this;  // Set MainWindow as the owner of the DimensionsWindow
        dimensionsWindow.ShowDialog();  // Opens it as a modal dialog (blocks MainWindow until closed)
    }
}