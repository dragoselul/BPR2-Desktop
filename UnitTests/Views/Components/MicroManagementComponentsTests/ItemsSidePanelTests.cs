using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BPR2_Desktop.ViewModels.MicroManagement;
using BPR2_Desktop.ViewModels.UserControls;
using BPR2_Desktop.Views.Components.MicroManagement;
using Xunit;


namespace BPR2_Desktop.Tests.Views.Components.MicroManagement
{
    public class ItemsSidePanelTests
    {
        private Window HostControl(UserControl control)
        {
            var window = new Window
            {
                Width = 800,
                Height = 600,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                ShowInTaskbar = false,
                Visibility = Visibility.Hidden,
                Content = control
            };
            window.Show();
            control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            control.Arrange(new Rect(control.DesiredSize));
            return window;
        }

        [StaFact]
        public void ItemsSidePanel_ShouldInitializeWithCorrectElements()
        {
            var viewModel = new ItemListViewModel();
            var control = new ItemsListView();
            var window = HostControl(control);
            Task.Delay(500).Wait();

            var grid = control.Content as Grid;
            Assert.NotNull(grid);
            Assert.Equal(5, grid.RowDefinitions.Count);

            var itemsTextBlock = grid.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Text == "Items");
            Assert.NotNull(itemsTextBlock);
            Assert.Equal(24, itemsTextBlock.FontSize);
            Assert.Equal(FontWeights.Bold, itemsTextBlock.FontWeight);

            var categorySelector = grid.Children.OfType<ComboBox>().FirstOrDefault();
            Assert.NotNull(categorySelector);
            Assert.Equal(2, Grid.GetRow(categorySelector));

            var searchBox = grid.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "SearchBox");
            Assert.NotNull(searchBox);
            Assert.Equal(3, Grid.GetRow(searchBox));
            
            window.Close();
        }
        
        private ScrollViewer GetScrollViewer(ListView listView)
        {
            if (VisualTreeHelper.GetChildrenCount(listView) == 0)
                return null;

            var border = VisualTreeHelper.GetChild(listView, 0) as Border;
            if (border == null || VisualTreeHelper.GetChildrenCount(border) == 0)
                return null;

            var scrollViewer = VisualTreeHelper.GetChild(border, 0) as ScrollViewer;
            return scrollViewer;
        }
        
        /*[StaFact]
        public void OnScrollChanged_ShouldExecuteWithoutException()
        {
            var vm = new BPR2_Desktop.ViewModels.MicroManagement.ItemSidePanelViewModel();
            var control = new BPR2_Desktop.Views.Components.MicroManagement.ItemsSidePanel(vm);
            var window = HostControl(control);
            Task.Delay(500).Wait();

            var grid = control.Content as Grid;
            Assert.NotNull(grid);

            var listView = grid.Children.OfType<ListView>().FirstOrDefault(lv => lv.Name == "ListView");
            Assert.NotNull(listView);

            var scrollViewer = GetScrollViewer(listView);
            Assert.NotNull(scrollViewer);

            scrollViewer.ScrollToVerticalOffset(scrollViewer.ScrollableHeight);

            var scrollChangedArgs = new ScrollChangedEventArgs(
                ScrollChangedEvent,
                scrollViewer.VerticalOffset,
                scrollViewer.VerticalOffset,
                scrollViewer.HorizontalOffset,
                scrollViewer.HorizontalOffset)
            {
                RoutedEvent = ScrollViewer.ScrollChangedEvent,
                Source = scrollViewer
            };

            var exception = Record.Exception(() => control.OnScrollChanged(listView, scrollChangedArgs));
            Assert.Null(exception);

            window.Close();
        }*/
        
        
    }
}
