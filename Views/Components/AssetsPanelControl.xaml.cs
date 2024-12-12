using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BPR2_Desktop.Views.Components
{
    public partial class AssetsPanelControl : UserControl
    {
        public AssetsPanelControl()
        {
            InitializeComponent();
        }

        internal void Asset_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Start dragging the selected asset (square or polygon)
                StackPanel asset = sender as StackPanel;
                if (asset != null)
                {
                    DataObject data = new DataObject(asset.Tag.ToString());
                    DragDrop.DoDragDrop(asset, data, DragDropEffects.Copy);
                }
            }
        }
    }
}