using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace RemoteDesktopServer
{
    public partial class ServerForm : Form
    {
        private TcpListener? server;
        private Thread? listenThread;
        private bool isRunning = false;

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            lblStatus.Text = "Server stopped";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (isRunning)
                {
                    MessageBox.Show("Server is already running.");
                    return;
                }

                server = new TcpListener(IPAddress.Any, 9999);
                server.Start();

                isRunning = true;
                lblStatus.Text = "Waiting for client...";
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                listenThread = new Thread(ListenForClient);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi động server: " + ex.Message);
                isRunning = false;
                server = null;
                lblStatus.Text = "Server stopped";
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        private void ListenForClient()
        {
            try
            {
                while (isRunning && server != null)
                {
                    TcpClient client = server.AcceptTcpClient();

                    UpdateStatus("Client connected: " + client.Client.RemoteEndPoint);

                    // Tạm thời chỉ test kết nối rồi đóng client
                    // Sau này có thể xử lý nhận/gửi dữ liệu ở đây
                    client.Close();

                    UpdateStatus("Waiting for client...");
                }
            }
            catch (SocketException)
            {
                // Xảy ra khi server.Stop() lúc đang AcceptTcpClient()
                if (isRunning)
                {
                    UpdateStatus("Socket error while listening.");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Error: " + ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                isRunning = false;

                if (server != null)
                {
                    server.Stop();
                    server = null;
                }

                if (listenThread != null && listenThread.IsAlive)
                {
                    listenThread.Join(500);
                    listenThread = null;
                }

                lblStatus.Text = "Server stopped";
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi dừng server: " + ex.Message);
            }
        }

        private void UpdateStatus(string message)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() =>
                {
                    lblStatus.Text = message;
                }));
            }
            else
            {
                lblStatus.Text = message;
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                isRunning = false;

                if (server != null)
                {
                    server.Stop();
                    server = null;
                }
            }
            catch
            {
            }
        }
    }
}