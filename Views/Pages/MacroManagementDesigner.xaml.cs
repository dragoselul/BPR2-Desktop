using System.Windows;
using BPR2_Desktop.Model.Enums;
using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages;

public partial class MacroManagementDesigner : INavigableView<ViewModels.MacroManagementDesignerViewModel>
{
    public ViewModels.MacroManagementDesignerViewModel ViewModel { get; }

    public MacroManagementDesigner(ViewModels.MacroManagementDesignerViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
    }
}