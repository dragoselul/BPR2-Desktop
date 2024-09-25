using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BPR2_Desktop.Views.Components
{
    public partial class VRModeControl : UserControl
    {
        private TcpClient client; // Hold the TCP client to manage connection state

        public VRModeControl()
        {
            InitializeComponent();
        }

        private void ToggleVRMode(object sender, RoutedEventArgs e)
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
                CloseConnection(); // Close connection when VR is disabled
            }
        }

        private void SendJsonData()
        {
            if (AppState.Instance.CurrentDesignFile != null)
            {
                try
                {
                    string jsonData = File.ReadAllText(AppState.Instance.CurrentDesignFile);
                    SendMessageToUnity(jsonData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading or sending JSON file: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No design file is currently selected. Please save a design first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void SendMessageToUnity(string message)
        {
            try
            {
                Console.WriteLine("Attempting to connect to Unity...");
                client = new TcpClient("127.0.0.1", 13000); // Create the TCP client
                Console.WriteLine("Connected to Unity!");
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Message sent to Unity: " + message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send message: " + ex.Message);
            }
        }

        private void CloseConnection()
        {
            if (client != null)
            {
                try
                {
                    client.Close(); // Close the TCP client connection
                    Console.WriteLine("Connection to Unity closed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to close connection: " + ex.Message);
                }
            }
        }
    }
}
