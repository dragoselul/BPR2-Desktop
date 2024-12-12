
using System.Reflection;
using System.Windows;
using BPR2_Desktop.ViewModels;

namespace BPR2_Desktop.Tests.ViewModels
{
    public class ViewModelTests
    {
        private class TestViewModel : ViewModel
        {
            public bool OnNavigatedToCalled { get; private set; }
            public bool OnNavigatedFromCalled { get; private set; }

            public override void OnNavigatedTo()
            {
                OnNavigatedToCalled = true;
            }

            public override void OnNavigatedFrom()
            {
                OnNavigatedFromCalled = true;
            }
        }

        /*[Fact]
        public async Task OnNavigatedToAsync_ShouldCallOnNavigatedTo()
        {
            var viewModel = new TestViewModel();
            await viewModel.OnNavigatedToAsync();
            Assert.True(viewModel.OnNavigatedToCalled);
        }

        [Fact]
        public async Task OnNavigatedFromAsync_ShouldCallOnNavigatedFrom()
        {
            var viewModel = new TestViewModel();
            await viewModel.OnNavigatedFromAsync();
            Assert.True(viewModel.OnNavigatedFromCalled);
        }*/

        // [Fact]
        // public async Task DispatchAsync_ShouldInvokeActionOnUIThread()
        // {
        //     var viewModel = new TestViewModel();
        //     var actionCalled = false;
        //     await viewModel.DispatchAsync(() => actionCalled = true, CancellationToken.None);
        //     Assert.True(actionCalled);
        // }

        [Fact]
        public async Task DispatchAsync_ShouldNotInvokeActionIfCancelled()
        {
            // Arrange
            var viewModel = new TestViewModel();
            var actionCalled = false;
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Use reflection to invoke DispatchAsync (since it's protected)
            var dispatchAsyncMethod = typeof(ViewModel).GetMethod("DispatchAsync", BindingFlags.NonPublic | BindingFlags.Static);
            var cancellationToken = cts.Token;

            // Act
            await (Task)dispatchAsyncMethod.Invoke(viewModel, new object[] { (Action)(() => actionCalled = true), cancellationToken });
            
            // Assert
            Assert.False(actionCalled);
        }

        
    }
}