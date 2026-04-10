using System;
using System.Net.Sockets;
using System.Threading;

namespace RemoteDesktopClient.Services
{
    internal class ClientSocketService
    {
        private TcpClient? client;
        private NetworkStream? stream;
        private Thread? receiveThread;
        private bool isRunning;

        public Action<string>? OnLog;
        public Action<byte[]>? OnImageReceived;

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

                receiveThread = new Thread(ReceiveImages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Connection failed: " + ex.Message);
            }
        }

        private void ReceiveImages()
        {
            try
            {
                while (isRunning && stream != null)
                {
                    byte[]? lengthBytes = ReadExact(4);
                    if (lengthBytes == null)
                        break;

                    int imageLength = BitConverter.ToInt32(lengthBytes, 0);
                    if (imageLength <= 0)
                        break;

                    byte[]? imageBytes = ReadExact(imageLength);
                    if (imageBytes == null)
                        break;

                    OnImageReceived?.Invoke(imageBytes);
                }
            }
            catch (Exception ex)
            {
                if (isRunning)
                {
                    OnLog?.Invoke("Receive error: " + ex.Message);
                }
            }

            DisconnectInternal(false);
        }

        private byte[]? ReadExact(int size)
        {
            byte[] buffer = new byte[size];
            int totalRead = 0;

            while (totalRead < size)
            {
                if (stream == null)
                    return null;

                int bytesRead = stream.Read(buffer, totalRead, size - totalRead);
                if (bytesRead == 0)
                    return null;

                totalRead += bytesRead;
            }

            return buffer;
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