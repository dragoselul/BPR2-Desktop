using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages;

public partial class Home : INavigableView<ViewModels.HomeViewModel>
{   
    public ViewModels.HomeViewModel ViewModel { get; }
    public Home(ViewModels.HomeViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        
        InitializeComponent();
    }
}