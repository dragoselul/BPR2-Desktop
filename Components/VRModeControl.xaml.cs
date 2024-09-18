using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BPR2_Desktop.Components;

public partial class VRModeControl : UserControl
{
    public VRModeControl()
    {
        InitializeComponent();
    }

    private void ToggleVRMode(object sender, RoutedEventArgs e)
    {
        if (VRModeToggle.IsChecked == null)
        {
            return;
        }
        if (VRModeToggle.IsChecked.Value)
        {
            VRModeToggle.Background = Brushes.Green;
            ToggleText.Text = "ON";
        }
        else
        {
            VRModeToggle.Background = Brushes.Red;
            ToggleText.Text = "OFF";
        }
    }
}