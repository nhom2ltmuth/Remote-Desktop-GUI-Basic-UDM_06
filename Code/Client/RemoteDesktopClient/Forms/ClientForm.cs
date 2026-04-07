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

        private ClientSocketService socketService;

        public ClientForm()
        {
            InitializeComponentSafe();
            InitializeSocket();
        }

        private void InitializeComponentSafe()
        {
            this.Text = "Client Remote Desktop";
            this.Size = new Size(950, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblIP = new Label();
            lblIP.Text = "IP:";
            lblIP.Location = new Point(20, 20);
            lblIP.AutoSize = true;

            txtIP = new TextBox();
            txtIP.Location = new Point(50, 18);
            txtIP.Width = 140;
            txtIP.Text = "127.0.0.1";

            Label lblPort = new Label();
            lblPort.Text = "Port:";
            lblPort.Location = new Point(210, 20);
            lblPort.AutoSize = true;

            txtPort = new TextBox();
            txtPort.Location = new Point(250, 18);
            txtPort.Width = 80;
            txtPort.Text = "5000";

            Label lblPass = new Label();
            lblPass.Text = "Password:";
            lblPass.Location = new Point(350, 20);
            lblPass.AutoSize = true;

            txtPassword = new TextBox();
            txtPassword.Location = new Point(430, 18);
            txtPassword.Width = 140;
            txtPassword.PasswordChar = '*';

            btnConnect = new Button();
            btnConnect.Text = "Connect";
            btnConnect.Location = new Point(590, 15);
            btnConnect.Width = 100;
            btnConnect.Click += BtnConnect_Click;

            btnDisconnect = new Button();
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.Location = new Point(700, 15);
            btnDisconnect.Width = 100;
            btnDisconnect.Click += BtnDisconnect_Click;

            picRemoteScreen = new PictureBox();
            picRemoteScreen.Location = new Point(20, 60);
            picRemoteScreen.Size = new Size(880, 460);
            picRemoteScreen.BorderStyle = BorderStyle.FixedSingle;
            picRemoteScreen.SizeMode = PictureBoxSizeMode.StretchImage;
            picRemoteScreen.BackColor = Color.Black;

            Label lblLog = new Label();
            lblLog.Text = "Log:";
            lblLog.Location = new Point(20, 535);
            lblLog.AutoSize = true;

            txtLog = new TextBox();
            txtLog.Location = new Point(20, 560);
            txtLog.Size = new Size(880, 50);
            txtLog.Multiline = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.ReadOnly = true;

            this.Controls.Add(lblIP);
            this.Controls.Add(txtIP);
            this.Controls.Add(lblPort);
            this.Controls.Add(txtPort);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnConnect);
            this.Controls.Add(btnDisconnect);
            this.Controls.Add(picRemoteScreen);
            this.Controls.Add(lblLog);
            this.Controls.Add(txtLog);
        }

        private void InitializeSocket()
        {
            socketService = new ClientSocketService();

            socketService.OnLog = (message) =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => AppendLog(message)));
                }
                else
                {
                    AppendLog(message);
                }
            };
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();

            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Port không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Vui lòng nhập IP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            socketService.Connect(ip, port);
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            socketService.Disconnect();
        }

        private void AppendLog(string message)
        {
            txtLog.AppendText(message + Environment.NewLine);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            socketService?.Disconnect();
            base.OnFormClosing(e);
        }
    }
}