using System.Windows.Controls;
using BPR2_Desktop.Helpers;
using BPR2_Desktop.Views.Components.MicroManagement;
using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages.MicroManagement;


public partial class ProductViewer : INavigableView<ViewModels.MicroManagement.ProductViewModel>
{
    public ViewModels.MicroManagement.ProductViewModel ViewModel { get; }
    public ProductViewer(ViewModels.MicroManagement.ProductViewModel vm)
    {
        InitializeComponent();
        ViewModel = vm;
        DataContext = this;
    }
}