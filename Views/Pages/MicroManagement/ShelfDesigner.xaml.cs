using System.Windows.Controls;
using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages.MicroManagement;

public partial class ShelfDesigner : INavigableView<ViewModels.MicroManagement.ShelfDesignerViewModel>
{
    public ViewModels.MicroManagement.ShelfDesignerViewModel ViewModel { get; }
    
    public ShelfDesigner(ViewModels.MicroManagement.ShelfDesignerViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }
}