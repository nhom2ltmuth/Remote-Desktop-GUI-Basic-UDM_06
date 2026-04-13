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

        //  Kết nối tới server 
        public void Connect(string ip, int port)
        {
            try
            {
                if (IsConnected)
                {
                    OnLog?.Invoke("Đã kết nối tới server rồi.");
                    return;
                }

                client = new TcpClient();
                client.Connect(ip, port);
                stream = client.GetStream();
                isRunning = true;

                OnLog?.Invoke($"Connected to server: {ip}:{port}");

                // Bắt đầu thread nhận ảnh màn hình từ server
                receiveThread = new Thread(ReceiveImages) { IsBackground = true };
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Connection failed: " + ex.Message);
            }
        }

        // ── Nhận các frame màn hình từ server 
        //  [4 byte ảnh][JPEG]
        private void ReceiveImages()
        {
            try
            {
                while (isRunning && stream != null)
                {
                    byte[]? lenBytes = ReadExact(4);
                    if (lenBytes == null) break;

                    int imageLength = BitConverter.ToInt32(lenBytes, 0);
                    if (imageLength <= 0) break;

                    byte[]? imageBytes = ReadExact(imageLength);
                    if (imageBytes == null) break;

                    OnImageReceived?.Invoke(imageBytes);
                }
            }
            catch (Exception ex)
            {
                if (isRunning)
                    OnLog?.Invoke("Lỗi nhận dữ liệu: " + ex.Message);
            }

            DisconnectInternal(false);
        }

        // ── Đọc đúng số byte yêu cầu từ stream 
        private byte[]? ReadExact(int size)
        {
            byte[] buffer = new byte[size];
            int totalRead = 0;

            while (totalRead < size)
            {
                if (stream == null) return null;

                int bytesRead = stream.Read(buffer, totalRead, size - totalRead);
                if (bytesRead == 0) return null;

                totalRead += bytesRead;
            }

            return buffer;
        }

        // ── Gửi raw bytes (dùng để gửi lệnh điều khiển input) 
        public void SendRaw(byte[] data)
        {
            if (stream == null || !IsConnected) return;
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        // ── Ngắt kết nối 
        public void Disconnect() => DisconnectInternal(true);

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
                    OnLog?.Invoke("Disconnected from server.");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Lỗi ngắt kết nối: " + ex.Message);
            }
        }
    }
}
