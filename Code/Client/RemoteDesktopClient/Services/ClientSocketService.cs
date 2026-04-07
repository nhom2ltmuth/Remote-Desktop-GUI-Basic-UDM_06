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
        private bool isRunning;

        public Action<string> OnLog;
        public Action<string> OnMessageReceived;

        public bool IsConnected => client != null && client.Connected;

        public void Connect(string ip, int port)
        {
            try
            {
                if (IsConnected)
                {
                    OnLog?.Invoke("Already connected to server.");
                    return;
                }

                client = new TcpClient();
                client.Connect(ip, port);

                stream = client.GetStream();
                isRunning = true;

                OnLog?.Invoke($"Connected to server: {ip}:{port}");

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
            byte[] buffer = new byte[4096];

            while (isRunning)
            {
                try
                {
                    if (stream == null)
                        break;

                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        OnLog?.Invoke("Server disconnected.");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    OnLog?.Invoke("Received: " + message);
                    OnMessageReceived?.Invoke(message);
                }
                catch (Exception ex)
                {
                    if (isRunning)
                    {
                        OnLog?.Invoke("Receive error: " + ex.Message);
                    }
                    break;
                }
            }

            DisconnectInternal(false);
        }

        public void Send(string message)
        {
            try
            {
                if (!IsConnected || stream == null)
                {
                    OnLog?.Invoke("Not connected to server.");
                    return;
                }

                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);

                OnLog?.Invoke("Sent: " + message);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Send error: " + ex.Message);
            }
        }

        public void Disconnect()
        {
            DisconnectInternal(true);
        }

        private void DisconnectInternal(bool writeLog)
        {
            try
            {
                isRunning = false;

                stream?.Close();
                client?.Close();

                stream = null;
                client = null;
                receiveThread = null;

                if (writeLog)
                {
                    OnLog?.Invoke("Disconnected from server.");
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Disconnect error: " + ex.Message);
            }
        }
    }
}