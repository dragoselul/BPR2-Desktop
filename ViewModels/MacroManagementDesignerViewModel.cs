using System.Windows.Media.Media3D;
using BPR2_Desktop.Model.Enums;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.ViewModels;

public partial class MacroManagementDesignerViewModel : ViewModel
{
    public HelixViewport3D? HelixViewport { get; set; }
    [ObservableProperty] private string _buttonText = "Load 3D Model";
    
    [RelayCommand]
    private void LoadObjModel(string shelf)
    {
        // Create a new importer
        var importer = new ObjReader();
        ShelfTypes shelfType = ShelfTypes.DoubleSided;
        // Load the 3D model from the file
        Model3DGroup model = importer.Read(shelfType.GetPathLocation());

        // Create a ModelVisual3D to hold the 3D model
        ModelVisual3D modelVisual = new ModelVisual3D
        {
            Content = model
        };
        HelixViewport?.Children.Add(modelVisual);
    }
}