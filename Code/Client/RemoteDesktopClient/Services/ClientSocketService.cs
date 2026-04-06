
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteDesktopClient.Services
{
    internal class ClientSocketService
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;

        public Action<string> OnLog;

        public void Connect(string ip, int port)
        {
            try
            {
                client = new TcpClient();
                client.Connect(ip, port);

                stream = client.GetStream();

                OnLog?.Invoke("Connected to server!");

                receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Connection failed: " + ex.Message);
            }
        }

        private void ReceiveData()
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        OnLog?.Invoke("Server disconnected.");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    OnLog?.Invoke("Received: " + message);
                }
                catch
                {
                    OnLog?.Invoke("Disconnected.");
                    break;
                }
            }
        }

        public void Send(string message)
        {
            try
            {
                if (stream != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    OnLog?.Invoke("Sent: " + message);
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Send error: " + ex.Message);
            }
        }

        public void Disconnect()
        {
            try
            {
                stream?.Close();
                client?.Close();
                OnLog?.Invoke("Disconnected from server.");
            }
            catch { }
        }
    }
}