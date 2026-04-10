using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RemoteDesktopServer.Services
{
    internal class ServerSocketService
    {
        private TcpListener? server;
        private TcpClient? client;
        private NetworkStream? stream;
        private Thread? listenThread;
        private bool isRunning = false;

        public Action<string>? OnLog;

        public bool IsRunning => isRunning;
        public bool IsClientConnected => client != null && client.Connected;

        public void Start(int port = 9999)
        {
            if (isRunning)
            {
                OnLog?.Invoke("Server already running.");
                return;
            }

            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            isRunning = true;
            OnLog?.Invoke("Server started. Waiting for client...");

            listenThread = new Thread(ListenClient);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ListenClient()
        {
            try
            {
                while (isRunning && server != null)
                {
                    client = server.AcceptTcpClient();
                    stream = client.GetStream();
                    OnLog?.Invoke("Client connected!");
                }
            }
            catch (SocketException)
            {
                if (isRunning)
                {
                    OnLog?.Invoke("Socket error while listening.");
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Error: " + ex.Message);
            }
        }

        public void SendData(byte[] data)
        {
            try
            {
                if (stream != null && client != null && client.Connected)
                {
                    byte[] lengthBytes = BitConverter.GetBytes(data.Length);
                    stream.Write(lengthBytes, 0, lengthBytes.Length);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Send error: " + ex.Message);
            }
        }

        public void Stop()
        {
            isRunning = false;

            try
            {
                stream?.Close();
                client?.Close();
                server?.Stop();

                if (listenThread != null && listenThread.IsAlive)
                {
                    listenThread.Join(500);
                    listenThread = null;
                }

                stream = null;
                client = null;
                server = null;

                OnLog?.Invoke("Server stopped.");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Stop error: " + ex.Message);
            }
        }
    }
}