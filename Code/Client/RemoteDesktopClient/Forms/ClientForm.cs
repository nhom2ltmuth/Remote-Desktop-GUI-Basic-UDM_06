using System;
using System.Drawing;
using System.Windows.Forms;
using RemoteDesktopClient.Services;

namespace RemoteDesktopClient
{
    public partial class ClientForm : Form
    {
        private TextBox txtIP;
        private TextBox txtPort;
        private TextBox txtPassword;
        private TextBox txtLog;
        private Button btnConnect;
        private Button btnDisconnect;
        private PictureBox picRemoteScreen;

        private readonly ClientSocketService socketService;
        private readonly RemoteScreenService screenService;

        public ClientForm()
        {
            InitializeComponentSafe();

            socketService = new ClientSocketService();
            screenService = new RemoteScreenService();

            InitializeSocketEvents();
            UpdateButtonState(false);
        }

        private void InitializeComponentSafe()
        {
            Text = "Client Remote Desktop";
            Size = new Size(950, 650);
            StartPosition = FormStartPosition.CenterScreen;

            Label lblIP = new Label
            {
                Text = "IP:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtIP = new TextBox
            {
                Location = new Point(50, 18),
                Width = 140,
                Text = "127.0.0.1"
            };

            Label lblPort = new Label
            {
                Text = "Port:",
                Location = new Point(210, 20),
                AutoSize = true
            };

            txtPort = new TextBox
            {
                Location = new Point(250, 18),
                Width = 80,
                Text = "9999"
            };

            Label lblPass = new Label
            {
                Text = "Password:",
                Location = new Point(350, 20),
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                Location = new Point(430, 18),
                Width = 140,
                PasswordChar = '*'
            };

            btnConnect = new Button
            {
                Text = "Connect",
                Location = new Point(590, 15),
                Width = 100
            };
            btnConnect.Click += BtnConnect_Click;

            btnDisconnect = new Button
            {
                Text = "Disconnect",
                Location = new Point(700, 15),
                Width = 100
            };
            btnDisconnect.Click += BtnDisconnect_Click;

            picRemoteScreen = new PictureBox
            {
                Location = new Point(20, 60),
                Size = new Size(880, 460),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Black
            };

            Label lblLog = new Label
            {
                Text = "Log:",
                Location = new Point(20, 535),
                AutoSize = true
            };

            txtLog = new TextBox
            {
                Location = new Point(20, 560),
                Size = new Size(880, 50),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            Controls.Add(lblIP);
            Controls.Add(txtIP);
            Controls.Add(lblPort);
            Controls.Add(txtPort);
            Controls.Add(lblPass);
            Controls.Add(txtPassword);
            Controls.Add(btnConnect);
            Controls.Add(btnDisconnect);
            Controls.Add(picRemoteScreen);
            Controls.Add(lblLog);
            Controls.Add(txtLog);
        }

        private void InitializeSocketEvents()
        {
            socketService.OnLog = message =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => AppendLog(message)));
                }
                else
                {
                    AppendLog(message);
                }
            };

            socketService.OnImageReceived = imageBytes =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => DisplayRemoteScreen(imageBytes)));
                }
                else
                {
                    DisplayRemoteScreen(imageBytes);
                }
            };
        }

        private void BtnConnect_Click(object sender, EventArgs e)
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
            UpdateButtonState(socketService.IsConnected);
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            socketService.Disconnect();
            UpdateButtonState(false);
        }

        private void DisplayRemoteScreen(byte[] imageBytes)
        {
            try
            {
                Image image = screenService.ConvertBytesToImage(imageBytes);

                Image oldImage = picRemoteScreen.Image;
                picRemoteScreen.Image = image;
                oldImage?.Dispose();
            }
            catch (Exception ex)
            {
                AppendLog("Display image error: " + ex.Message);
            }
        }

        private void AppendLog(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");

            if (message.Contains("Connected to server"))
            {
                UpdateButtonState(true);
            }

            if (message.Contains("Disconnected") ||
                message.Contains("Connection failed") ||
                message.Contains("Server disconnected"))
            {
                UpdateButtonState(false);
            }
        }

        private void UpdateButtonState(bool connected)
        {
            btnConnect.Enabled = !connected;
            btnDisconnect.Enabled = connected;
            txtIP.Enabled = !connected;
            txtPort.Enabled = !connected;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            picRemoteScreen.Image?.Dispose();
            socketService.Disconnect();
            base.OnFormClosing(e);
        }
    }
}