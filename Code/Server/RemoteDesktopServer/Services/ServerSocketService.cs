using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteDesktopServer.Services
{
    internal class ServerSocketService
    {
        private TcpListener? server;
        private TcpClient? client;
        private NetworkStream? stream;
        private Thread? listenThread;
        private Thread? receiveThread;
        private bool isRunning = false;

        private readonly InputControlService inputControl = new InputControlService();

        public Action<string>? OnLog;

        public bool IsRunning => isRunning;
        public bool IsClientConnected => client != null && client.Connected;

        // ── Khởi động server, lắng nghe kết nối từ client
        public void Start(int port = 9999)
        {
            if (isRunning)
            {
                OnLog?.Invoke("Server đang chạy rồi.");
                return;
            }

            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            isRunning = true;
            OnLog?.Invoke("Server đã khởi động. Đang chờ client kết nối...");

            listenThread = new Thread(ListenClient) { IsBackground = true };
            listenThread.Start();
        }

        // ── Chấp nhận kết nối và bắt đầu nhận lệnh từ client
        private void ListenClient()
        {
            try
            {
                while (isRunning && server != null)
                {
                    client = server.AcceptTcpClient();
                    stream = client.GetStream();
                    OnLog?.Invoke("Client đã kết nối!");

                    // Bắt đầu thread nhận lệnh điều khiển từ client
                    receiveThread = new Thread(ReceiveCommands) { IsBackground = true };
                    receiveThread.Start();
                }
            }
            catch (SocketException)
            {
                if (isRunning)
                    OnLog?.Invoke("Lỗi socket khi đang lắng nghe.");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Lỗi: " + ex.Message);
            }
        }

        // ── Nhận lệnh điều khiển chuột/bàn phím từ client 
        // Giao thức: [4 byte độ dài][N byte chuỗi UTF-8]
        // Chuỗi có dạng: "CMD|<loại>|<tham số>"
        private void ReceiveCommands()
        {
            try
            {
                while (isRunning && stream != null)
                {
                    byte[]? lenBytes = ReadExact(4);
                    if (lenBytes == null) break;

                    int len = BitConverter.ToInt32(lenBytes, 0);
                    if (len <= 0 || len > 4096) continue; // kiểm tra độ dài hợp lệ

                    byte[]? data = ReadExact(len);
                    if (data == null) break;

                    string command = Encoding.UTF8.GetString(data);

                    // Chỉ xử lý các lệnh bắt đầu bằng "CMD|"
                    if (command.StartsWith("CMD|"))
                    {
                        inputControl.Execute(command);
                    }
                }
            }
            catch (Exception ex)
            {
                if (isRunning)
                    OnLog?.Invoke("Lỗi nhận lệnh: " + ex.Message);
            }
        }

        // ── Đọc đúng số byte yêu cầu từ stream 
        private byte[]? ReadExact(int size)
        {
            if (stream == null) return null;
            byte[] buffer = new byte[size];
            int totalRead = 0;

            while (totalRead < size)
            {
                int bytesRead = stream.Read(buffer, totalRead, size - totalRead);
                if (bytesRead == 0) return null;
                totalRead += bytesRead;
            }

            return buffer;
        }

        // ── Gửi dữ liệu màn hình sang client 
        // [4 byte ảnh][JPEG]
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
                OnLog?.Invoke("Lỗi gửi dữ liệu: " + ex.Message);
            }
        }

        //  Dừng server và giải phóng tài nguyên 
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

                if (receiveThread != null && receiveThread.IsAlive)
                {
                    receiveThread.Join(500);
                    receiveThread = null;
                }

                stream = null;
                client = null;
                server = null;

                OnLog?.Invoke("Server đã dừng.");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Lỗi khi dừng server: " + ex.Message);
            }
        }
    }
}
