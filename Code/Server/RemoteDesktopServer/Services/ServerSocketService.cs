using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteDesktopServer.Services
{
    internal class ServerSocketService
    {
        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        private Thread listenThread;

        public Action<string> OnLog; // dùng để hiển thị log lên UI

        // Start server
        public void StartServer(string ip, int port)
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse(ip), port);
                listener.Start();

                OnLog?.Invoke("Server started. Waiting for client...");

                listenThread = new Thread(ListenClient);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Error: " + ex.Message);
            }
        }

        // Lắng nghe client
        private void ListenClient()
        {
            try
            {
                client = listener.AcceptTcpClient();
                stream = client.GetStream();

                OnLog?.Invoke("Client connected!");

                ReceiveData();
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Error: " + ex.Message);
            }
        }

        // Nhận dữ liệu
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
                        OnLog?.Invoke("Client disconnected.");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    OnLog?.Invoke("Received: " + message);
                }
                catch
                {
                    OnLog?.Invoke("Connection lost.");
                    break;
                }
            }
        }

        //  Gửi dữ liệu
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

        //  Dừng server
        public void Stop()
        {
            try
            {
                stream?.Close();
                client?.Close();
                listener?.Stop();

                OnLog?.Invoke("Server stopped.");
            }
            catch { }
        }
    }
}