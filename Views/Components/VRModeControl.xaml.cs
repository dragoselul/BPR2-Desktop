using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BPR2_Desktop.Views.Components;

public partial class VRModeControl : UserControl
{
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
        if (VRModeToggle.IsChecked.Value)
        {
            VRModeToggle.Background = Brushes.Green;
        }
        else
        {
            VRModeToggle.Background = Brushes.Red;
        }
        SendMessageToUnity("spawn");
    }

    public void SendMessageToUnity(string message)
    {
        try
        {
            Console.WriteLine("Attempting to connect to Unity...");
            using (TcpClient client = new TcpClient("127.0.0.1", 13000)) // Ensure the IP and port match Unity's listener
            {
                Console.WriteLine("Connected to Unity!");
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Message sent to Unity: " + message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send message: " + ex.Message);
        }
    }

    
}