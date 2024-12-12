using System.Windows.Controls;
using BPR2_Desktop.ViewModels.UserControls;

namespace BPR2_Desktop.Views.Components.MicroManagement;

public partial class ImageDisplayPanel : UserControl
{
    public ImageDisplayPanelViewModel ViewModel { get; set; }
    
    public ImageDisplayPanel()
    {
        Loaded += ImageDisplayPanelView_Loaded; 
        InitializeComponent();
    }
    
    private void ImageDisplayPanelView_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ImageDisplayPanelViewModel vm) return;
        ViewModel = vm;
        DataContext = this;
    }
}