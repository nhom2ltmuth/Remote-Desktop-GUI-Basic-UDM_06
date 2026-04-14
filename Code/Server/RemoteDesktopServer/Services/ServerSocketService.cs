using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using RemoteDesktopServer.Models;

namespace RemoteDesktopServer.Services
{
    internal class ServerSocketService
    {
        private TcpListener? server;
        private TcpClient? client;
        private NetworkStream? stream;
        private Thread? listenThread;
        private Thread? receiveThread;
        private bool isRunning;

        public Action<string>? OnLog;
        public Action<ClientSession>? OnClientConnected;
        public Action? OnClientDisconnected;
        public Action<string>? OnCommandReceived;

        public bool IsRunning => isRunning;
        public bool IsClientConnected => client != null && client.Connected;

        public void Start(int port = 9999)
        {
            try
            {
                if (isRunning)
                {
                    OnLog?.Invoke("Server already running.");
                    return;
                }

                server = new TcpListener(IPAddress.Any, port);
                server.Start();

                isRunning = true;

                listenThread = new Thread(ListenLoop)
                {
                    IsBackground = true
                };
                listenThread.Start();

                OnLog?.Invoke($"Server started on port {port}.");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Start server error: " + ex.Message);
            }
        }

        public void Stop()
        {
            try
            {
                isRunning = false;

                DisconnectClientInternal(false);

                server?.Stop();
                server = null;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Stop server error: " + ex.Message);
            }
        }

        private void ListenLoop()
        {
            try
            {
                while (isRunning && server != null)
                {
                    OnLog?.Invoke("Waiting for client connection...");

                    TcpClient acceptedClient = server.AcceptTcpClient();

                    if (!isRunning)
                    {
                        acceptedClient.Close();
                        break;
                    }

                    DisconnectClientInternal(false);

                    client = acceptedClient;
                    stream = client.GetStream();

                    if (client.Client.RemoteEndPoint is IPEndPoint endPoint)
                    {
                        var session = new ClientSession
                        {
                            IPAddress = endPoint.Address.ToString(),
                            Port = endPoint.Port,
                            IsConnected = true
                        };

                        OnClientConnected?.Invoke(session);
                    }
                    else
                    {
                        OnClientConnected?.Invoke(new ClientSession
                        {
                            IPAddress = "Unknown",
                            Port = 0,
                            IsConnected = true
                        });
                    }

                    receiveThread = new Thread(ReceiveCommandsLoop)
                    {
                        IsBackground = true
                    };
                    receiveThread.Start();
                }
            }
            catch (SocketException)
            {
                if (isRunning)
                    OnLog?.Invoke("Socket listening error.");
            }
            catch (Exception ex)
            {
                if (isRunning)
                    OnLog?.Invoke("ListenLoop error: " + ex.Message);
            }
        }

        public void SendImage(byte[] imageBytes)
        {
            try
            {
                if (!IsClientConnected || stream == null)
                    return;

                byte[] lengthBytes = BitConverter.GetBytes(imageBytes.Length);

                lock (stream)
                {
                    stream.Write(lengthBytes, 0, lengthBytes.Length);
                    stream.Write(imageBytes, 0, imageBytes.Length);
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Send image error: " + ex.Message);
                DisconnectClientInternal(true);
            }
        }

        private void ReceiveCommandsLoop()
        {
            try
            {
                while (isRunning && IsClientConnected && stream != null)
                {
                    byte[] lengthBytes = ReadExact(stream, 4);
                    int commandLength = BitConverter.ToInt32(lengthBytes, 0);

                    if (commandLength <= 0)
                    {
                        OnLog?.Invoke("Invalid command length received.");
                        break;
                    }

                    byte[] commandBytes = ReadExact(stream, commandLength);
                    string command = Encoding.UTF8.GetString(commandBytes);

                    OnCommandReceived?.Invoke(command);
                }
            }
            catch (IOException)
            {
                if (isRunning)
                    OnLog?.Invoke("Client connection closed.");
            }
            catch (ObjectDisposedException)
            {
                if (isRunning)
                    OnLog?.Invoke("Client stream disposed.");
            }
            catch (Exception ex)
            {
                if (isRunning)
                    OnLog?.Invoke("ReceiveCommandsLoop error: " + ex.Message);
            }
            finally
            {
                DisconnectClientInternal(true);
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
                    throw new IOException("Remote host closed the connection.");

                totalRead += bytesRead;
            }

            return buffer;
        }

        private void DisconnectClientInternal(bool notify)
        {
            try
            {
                stream?.Close();
                stream = null;

                client?.Close();
                client = null;

                if (notify)
                    OnClientDisconnected?.Invoke();
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Disconnect client error: " + ex.Message);
            }
        }
    }
}