namespace RemoteDesktopServer.Models
{
    internal class ClientSession
    {
        public string IPAddress { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool IsConnected { get; set; }
    }
}