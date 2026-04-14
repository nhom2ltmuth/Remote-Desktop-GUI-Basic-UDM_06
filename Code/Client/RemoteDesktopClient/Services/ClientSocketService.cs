using System;
using System.IO;
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

                receiveThread = new Thread(ReceiveImagesLoop)
                {
                    IsBackground = true
                };
                receiveThread.Start();

                OnLog?.Invoke("Connected to server.");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Connection failed: " + ex.Message);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                isRunning = false;

                stream?.Close();
                stream = null;

                client?.Close();
                client = null;

                OnLog?.Invoke("Disconnected.");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Disconnect error: " + ex.Message);
            }
        }

        public void SendRaw(byte[] data)
        {
            try
            {
                if (!IsConnected || stream == null)
                {
                    OnLog?.Invoke("Cannot send data. Not connected.");
                    return;
                }

                lock (stream)
                {
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Send error: " + ex.Message);
                Disconnect();
            }
        }

        private void ReceiveImagesLoop()
        {
            try
            {
                while (isRunning && client != null && client.Connected && stream != null)
                {
                    byte[] lengthBytes = ReadExact(stream, 4);
                    int imageLength = BitConverter.ToInt32(lengthBytes, 0);

                    if (imageLength <= 0)
                    {
                        OnLog?.Invoke("Invalid image length received.");
                        break;
                    }

                    byte[] imageBytes = ReadExact(stream, imageLength);
                    OnImageReceived?.Invoke(imageBytes);
                }
            }
            catch (IOException)
            {
                if (isRunning)
                    OnLog?.Invoke("Server disconnected.");
            }
            catch (ObjectDisposedException)
            {
                if (isRunning)
                    OnLog?.Invoke("Connection closed.");
            }
            catch (Exception ex)
            {
                if (isRunning)
                    OnLog?.Invoke("Receive error: " + ex.Message);
            }
            finally
            {
                if (isRunning)
                    Disconnect();
            }
        }

        private byte[] ReadExact(NetworkStream stream, int size)
        {
            byte[] buffer = new byte[size];
            int totalRead = 0;

            while (totalRead < size)
            {
                int bytesRead = stream.Read(buffer, totalRead, size - totalRead);

                if (bytesRead == 0)
                    throw new IOException("Connection closed by remote host.");

                totalRead += bytesRead;
            }

            return buffer;
        }
    }
}