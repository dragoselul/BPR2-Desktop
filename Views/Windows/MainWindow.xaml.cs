using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace BPR2_Desktop.Views.Windows
{
    public partial class MainWindow
    {
        public ViewModels.MainWindowViewModel ViewModel { get; }
        public MainWindow(ViewModels.MainWindowViewModel vm)
        {
            ViewModel = vm;
            DataContext = this;
            InitializeComponent();
        }

        internal void Button_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Storyboard storyboard = (Storyboard)FindResource("ButtonFadeIn");
            storyboard.Begin(button);
        }
        
        internal void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement.Position = TimeSpan.Zero;
            MediaElement.Play();
        }
    }
}