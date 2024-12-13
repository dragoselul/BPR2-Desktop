using System.Windows.Controls;
using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages.MicroManagement;

public partial class ShelfManager : INavigableView<ViewModels.MicroManagement.ShelfManagerViewModel>
{
    public ViewModels.MicroManagement.ShelfManagerViewModel ViewModel { get; }
    public ShelfManager(ViewModels.MicroManagement.ShelfManagerViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }
}