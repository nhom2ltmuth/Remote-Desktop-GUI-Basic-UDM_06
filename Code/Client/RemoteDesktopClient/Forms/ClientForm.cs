using System;
using System.Drawing;
using System.Windows.Forms;
using RemoteDesktopClient.Models;
using RemoteDesktopClient.Services;

namespace RemoteDesktopClient.Forms
{
    public partial class ClientForm : Form
    {
        private readonly ClientSocketService socketService;
        private readonly RemoteScreenService screenService;
        private readonly InputSenderService inputSenderService;
        private readonly RemoteScreenInfo remoteScreenInfo;

        public ClientForm()
        {
            InitializeComponent();

            socketService = new ClientSocketService();
            screenService = new RemoteScreenService();
            remoteScreenInfo = new RemoteScreenInfo();
            inputSenderService = new InputSenderService(socketService, remoteScreenInfo);

            InitializeServices();
            InitializeInputEvents();

            Load += ClientForm_Load;
            FormClosing += ClientForm_FormClosing;
        }

        private void ClientForm_Load(object? sender, EventArgs e)
        {
            txtIP.Text = "127.0.0.1";
            txtPort.Text = "9999";
            lblStatus.Text = "Disconnected";

            UpdateButtonState(false);
            AddLog("Client form loaded.");
        }

        private void InitializeServices()
        {
            socketService.OnLog += AddLog;

            socketService.OnImageReceived += imageBytes =>
            {
                if (InvokeRequired)
                    Invoke(new Action(() => DisplayRemoteScreen(imageBytes)));
                else
                    DisplayRemoteScreen(imageBytes);
            };

            inputSenderService.OnLog += AddLog;
        }

        private void InitializeInputEvents()
        {
            picRemoteScreen.MouseMove += PicRemoteScreen_MouseMove;
            picRemoteScreen.MouseDown += PicRemoteScreen_MouseDown;
            picRemoteScreen.MouseUp += PicRemoteScreen_MouseUp;
            picRemoteScreen.MouseWheel += PicRemoteScreen_MouseWheel;
            picRemoteScreen.MouseEnter += (s, e) => picRemoteScreen.Focus();

            KeyPreview = true;
            KeyDown += ClientForm_KeyDown;
            KeyUp += ClientForm_KeyUp;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();

            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Vui lòng nhập IP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Port không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            socketService.Connect(ip, port);

            if (socketService.IsConnected)
            {
                lblStatus.Text = "Connected";
                UpdateButtonState(true);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectClient();
        }

        private void DisconnectClient()
        {
            socketService.Disconnect();
            remoteScreenInfo.Resolution = Size.Empty;

            lblStatus.Text = "Disconnected";
            UpdateButtonState(false);
        }

        private void DisplayRemoteScreen(byte[] imageBytes)
        {
            try
            {
                Image image = screenService.ConvertBytesToImage(imageBytes);

                if (remoteScreenInfo.Resolution.IsEmpty)
                    remoteScreenInfo.Resolution = image.Size;

                Image? oldImage = picRemoteScreen.Image;
                picRemoteScreen.Image = image;
                oldImage?.Dispose();
            }
            catch (Exception ex)
            {
                AddLog("Display image error: " + ex.Message);
            }
        }

        private void PicRemoteScreen_MouseMove(object? sender, MouseEventArgs e)
        {
            inputSenderService.SendMouseMove(e.Location, picRemoteScreen.Size);
        }

        private void PicRemoteScreen_MouseDown(object? sender, MouseEventArgs e)
        {
            picRemoteScreen.Focus();
            inputSenderService.SendMouseDown(e.Button, e.Location, picRemoteScreen.Size);
        }

        private void PicRemoteScreen_MouseUp(object? sender, MouseEventArgs e)
        {
            inputSenderService.SendMouseUp(e.Button);
        }

        private void PicRemoteScreen_MouseWheel(object? sender, MouseEventArgs e)
        {
            inputSenderService.SendMouseWheel(e.Delta);
        }

        private void ClientForm_KeyDown(object? sender, KeyEventArgs e)
        {
            inputSenderService.SendKeyDown(e.KeyCode);
            e.Handled = true;
        }

        private void ClientForm_KeyUp(object? sender, KeyEventArgs e)
        {
            inputSenderService.SendKeyUp(e.KeyCode);
        }

        private void AddLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string>(AddLog), message);
                return;
            }

            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");

            if (message.Contains("Disconnected") ||
                message.Contains("Connection failed") ||
                message.Contains("Server disconnected") ||
                message.Contains("Connection closed"))
            {
                remoteScreenInfo.Resolution = Size.Empty;
                lblStatus.Text = "Disconnected";
                UpdateButtonState(false);
            }
        }

        private void UpdateButtonState(bool connected)
        {
            btnConnect.Enabled = !connected;
            btnDisconnect.Enabled = connected;
            txtIP.Enabled = !connected;
            txtPort.Enabled = !connected;
            picRemoteScreen.Enabled = connected;
        }

        private void ClientForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            picRemoteScreen.Image?.Dispose();
            socketService.Disconnect();
        }
    }
}