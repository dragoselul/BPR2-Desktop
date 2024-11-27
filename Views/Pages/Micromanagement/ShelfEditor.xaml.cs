using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages;

public partial class ShelfEditor : INavigableView<ViewModels.MicroManagement.ProductViewModel>
{
    public ViewModels.MicroManagement.ProductViewModel ViewModel { get; }
    public ShelfEditor(ViewModels.MicroManagement.ProductViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        
        InitializeComponent();
    }
}