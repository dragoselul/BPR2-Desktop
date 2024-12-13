using System.Windows;
using System.Windows.Input;
using BPR2_Desktop.Model.Enums;
using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages;

public partial class MacroManagementDesigner : INavigableView<ViewModels.MacroManagement.DesignerViewModel>
{
    public ViewModels.MacroManagement.DesignerViewModel ViewModel { get; }

    public MacroManagementDesigner(ViewModels.MacroManagement.DesignerViewModel vm)
    {
        ViewModel = vm;
        DataContext = this;
        InitializeComponent();
        HelixViewport.PanGesture = new MouseGesture(MouseAction.LeftClick);
    }
}