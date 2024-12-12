using System;
using System.Net.Sockets;
using System.Text;

namespace BPR2_Desktop.Services
{
    public class UnityClient : IUnityClient
    {
        private TcpClient client;
        private readonly string host;
        private readonly int port;

        public UnityClient(string host = "127.0.0.1", int port = 13000)
        {
            this.host = host;
            this.port = port;
        }

        

        public void SendMessage(string message)
        {
            try
            {
                Console.WriteLine("Attempting to connect to Unity...");
                using var client = new TcpClient("127.0.0.1", 13000);
                using var stream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Message sent to Unity: " + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send message: " + ex.Message);
            }
        }
    
    

        public void CloseConnection()
        {
            if (client != null)
            {
                try
                {
                    client.Close();
                    Console.WriteLine("Connection to Unity closed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to close connection: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
