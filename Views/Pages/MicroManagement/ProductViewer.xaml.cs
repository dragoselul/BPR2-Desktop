using BPR2_Desktop.ViewModels.MicroManagement;
using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages.MicroManagement;

public partial class ProductViewer : INavigableView<ProductViewModel>
{
    public ProductViewModel ViewModel { get; }

    public ProductViewer(ProductViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }
}