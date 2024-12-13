using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages;

public partial class Home : INavigableView<ViewModels.MacroManagement.HomeViewModel>
{   
    public ViewModels.MacroManagement.HomeViewModel ViewModel { get; }
    public Home(ViewModels.MacroManagement.HomeViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        
        InitializeComponent();
    }

 
}