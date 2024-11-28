using System.Windows.Controls;
using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages.MicroManagement;

public partial class ProductViewer : INavigableView<ViewModels.MicroManagement.ProductViewModel>
{
    public ViewModels.MicroManagement.ProductViewModel ViewModel { get; }
    public ProductViewer(ViewModels.MicroManagement.ProductViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        
        InitializeComponent();
    }
}