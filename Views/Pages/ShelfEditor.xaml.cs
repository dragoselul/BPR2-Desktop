using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages;

public partial class ShelfEditor : INavigableView<ViewModels.ProductViewModel>
{
    public ViewModels.ProductViewModel ViewModel { get; }
    public ShelfEditor(ViewModels.ProductViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        
        InitializeComponent();
    }
}