public interface IUnityClient
{
    
    internal void SendMessage(string message);
    internal void CloseConnection();
    internal void Dispose();
}