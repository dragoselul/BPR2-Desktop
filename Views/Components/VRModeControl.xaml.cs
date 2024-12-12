using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BPR2_Desktop.Services;

namespace BPR2_Desktop.Views.Components
{
    public partial class VRModeControl : UserControl
    {
        private readonly UnityClient unityClient;
        public string LastMessage { get; private set; }

        public VRModeControl()
        {
            InitializeComponent();
        }
        
        public VRModeControl( IUnityClient unityClient)
        {
            InitializeComponent();
            unityClient = new UnityClient() ?? throw new ArgumentNullException(nameof(unityClient));
        }

        internal void ToggleVRMode(object sender, RoutedEventArgs e)
        {
            if (VRModeToggle.IsChecked == null)
            {
                return;
            }

            // Change background color based on the toggle state
            if (VRModeToggle.IsChecked.Value)
            {
                VRModeToggle.Background = Brushes.Green;
                SendJsonData(); // Send JSON when VR is enabled

                // Bounce back to off state after sending
                VRModeToggle.IsChecked = false;
                VRModeToggle.Background = Brushes.Red; // Reset background color
            }
            else
            {
                VRModeToggle.Background = Brushes.Red;
                unityClient.CloseConnection(); // Close connection when VR is disabled
            }
        }

        internal void SendJsonData()
        {
            if (AppState.Instance.CurrentDesignFile != null)
            {
                try
                {
                    string jsonData = File.ReadAllText(AppState.Instance.CurrentDesignFile);
                    unityClient.SendMessage(jsonData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading or sending JSON file: " + ex.Message);
                }
            }
            else
            {
                LastMessage = "No design file is currently selected. Please save a design first.";
                MessageBox.Show(LastMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
