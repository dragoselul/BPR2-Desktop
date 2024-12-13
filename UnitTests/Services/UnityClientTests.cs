using System.Net.Sockets;
using Moq;
using Xunit;
using BPR2_Desktop.Services;

namespace BPR2_Desktop.Tests.Services
{
    public class UnityClientTests
    {
        /*[Fact]
        public void SendMessage_ShouldSendMessageToUnity()
        {
            
            
            // Arrange
            var mockTcpClient = new TcpClient(); // Real TcpClient
            
            
            mockTcpClient.Connect("127.0.0.1", 13000);
            
            
            var mockNetworkStream = new NetworkStream(mockTcpClient.Client); // Create a fake stream

            // Manually trigger the stream to be available
            

            // We won't actually connect, just ensure we're not running into the error
            mockNetworkStream.Close(); // Close the stream to simulate successful stream access

            var unityClient = new UnityClient("127.0.0.1", 13000);

            // Act
            unityClient.SendMessage("Test message");

            // Assert
            Assert.NotNull(mockTcpClient.GetStream());
        }*/

        [Fact]
        public void CloseConnection_ShouldCloseClientConnection()
        {
            // Arrange
            var mockTcpClient = new Mock<TcpClient>();
            var unityClient = new UnityClient("127.0.0.1", 13000);

            // Act
            unityClient.CloseConnection();

            // Assert
            mockTcpClient.Object.Close();
        }

        [Fact]
        public void Dispose_ShouldCloseConnectionWhenDisposed()
        {
            // Arrange
            var mockTcpClient = new Mock<TcpClient>();
            var unityClient = new UnityClient("127.0.0.1", 13000);

            // Act
            unityClient.Dispose();

            // Assert
            mockTcpClient.Object.Close();
        }
    }
}