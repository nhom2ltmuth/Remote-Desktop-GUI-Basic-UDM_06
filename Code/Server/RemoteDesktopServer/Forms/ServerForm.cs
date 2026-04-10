using System;
using System.Threading;
using System.Windows.Forms;
using RemoteDesktopServer.Services;

namespace RemoteDesktopServer
{
    public partial class ServerForm : Form
    {
        private readonly ServerSocketService socketService;
        private readonly ScreenCaptureService screenService;

        private Thread? sendScreenThread;
        private bool isSending;

        public ServerForm()
        {
            InitializeComponent();

            socketService = new ServerSocketService();
            screenService = new ScreenCaptureService();

            socketService.OnLog += UpdateStatus;
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
                if (socketService.IsRunning)
                {
                    MessageBox.Show("Server is already running.");
                    return;
                }

                socketService.Start(9999);

                btnStart.Enabled = false;
                btnStop.Enabled = true;

                isSending = true;
                sendScreenThread = new Thread(SendScreenLoop);
                sendScreenThread.IsBackground = true;
                sendScreenThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi động server: " + ex.Message);
                StopServerUI();
            }
        }

        private void SendScreenLoop()
        {
            while (isSending)
            {
                try
                {
                    if (socketService.IsClientConnected)
                    {
                        byte[] screenBytes = screenService.CaptureScreenBytes();
                        socketService.SendData(screenBytes);
                    }

                    Thread.Sleep(200);
                }
                catch (Exception ex)
                {
                    UpdateStatus("Send screen error: " + ex.Message);
                    Thread.Sleep(500);
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                StopServer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi dừng server: " + ex.Message);
            }
        }

        private void StopServer()
        {
            isSending = false;

            if (sendScreenThread != null && sendScreenThread.IsAlive)
            {
                sendScreenThread.Join(500);
                sendScreenThread = null;
            }

            socketService.Stop();
            StopServerUI();
        }

        private void StopServerUI()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(StopServerUI));
                return;
            }

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lblStatus.Text = "Server stopped";
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
                StopServer();
            }
            catch
            {
            }
        }
    }
}