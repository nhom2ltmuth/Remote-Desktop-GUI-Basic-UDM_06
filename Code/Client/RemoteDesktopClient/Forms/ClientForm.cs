using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RemoteDesktopClient.Services;

namespace RemoteDesktopClient
{
    public partial class ClientForm : Form
    {
        private TextBox txtIP = null!;
        private TextBox txtPort = null!;
        private TextBox txtPassword = null!;
        private TextBox txtLog = null!;
        private Button btnConnect = null!;
        private Button btnDisconnect = null!;
        private PictureBox picRemoteScreen = null!;

        private readonly ClientSocketService socketService;
        private readonly RemoteScreenService screenService;

        // Độ phân giải màn hình remote (cập nhật khi nhận frame đầu tiên)
        private Size remoteResolution = Size.Empty;

        public ClientForm()
        {
            InitializeComponentSafe();

            socketService = new ClientSocketService();
            screenService = new RemoteScreenService();

            InitializeSocketEvents();
            InitializeInputEvents();
            UpdateButtonState(false);
        }


        //  Khởi tạo giao diện

        private void InitializeComponentSafe()
        {
            Text = "Client Remote Desktop";
            Size = new Size(950, 650);
            StartPosition = FormStartPosition.CenterScreen;

            var lblIP = new Label { Text = "IP:", Location = new Point(20, 20), AutoSize = true };
            txtIP = new TextBox { Location = new Point(50, 18), Width = 140, Text = "127.0.0.1" };

            var lblPort = new Label { Text = "Port:", Location = new Point(210, 20), AutoSize = true };
            txtPort = new TextBox { Location = new Point(250, 18), Width = 80, Text = "9999" };

            var lblPass = new Label { Text = "Password:", Location = new Point(350, 20), AutoSize = true };
            txtPassword = new TextBox { Location = new Point(430, 18), Width = 140, PasswordChar = '*' };

            btnConnect = new Button { Text = "Connect", Location = new Point(590, 15), Width = 100 };
            btnConnect.Click += BtnConnect_Click;

            btnDisconnect = new Button { Text = "Disconnect", Location = new Point(700, 15), Width = 100 };
            btnDisconnect.Click += BtnDisconnect_Click;

            picRemoteScreen = new PictureBox
            {
                Location = new Point(20, 60),
                Size = new Size(880, 460),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Black
            };

            var lblLog = new Label { Text = "Log:", Location = new Point(20, 535), AutoSize = true };
            txtLog = new TextBox
            {
                Location = new Point(20, 560),
                Size = new Size(880, 50),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            Controls.AddRange(new Control[]
            {
                lblIP, txtIP, lblPort, txtPort, lblPass, txtPassword,
                btnConnect, btnDisconnect, picRemoteScreen, lblLog, txtLog
            });
        }


        //  Đăng ký sự kiện socket

        private void InitializeSocketEvents()
        {
            socketService.OnLog = message =>
            {
                if (InvokeRequired) Invoke(new Action(() => AppendLog(message)));
                else AppendLog(message);
            };

            socketService.OnImageReceived = imageBytes =>
            {
                if (InvokeRequired) Invoke(new Action(() => DisplayRemoteScreen(imageBytes)));
                else DisplayRemoteScreen(imageBytes);
            };
        }


        //  Đăng ký sự kiện chuột và bàn phím trên PictureBox

        private void InitializeInputEvents()
        {
            picRemoteScreen.MouseMove += PicScreen_MouseMove;
            picRemoteScreen.MouseDown += PicScreen_MouseDown;
            picRemoteScreen.MouseUp += PicScreen_MouseUp;
            picRemoteScreen.MouseWheel += PicScreen_MouseWheel;

            // KeyPreview = true để form nhận phím trước các control con
            this.KeyPreview = true;
            this.KeyDown += ClientForm_KeyDown;
            this.KeyUp += ClientForm_KeyUp;
        }

        //  Chuyển đổi tọa độ PictureBox → tọa độ màn hình remote
        private Point ScaleToRemote(Point clientPoint)
        {
            if (remoteResolution.IsEmpty)
                return clientPoint;

            float scaleX = (float)remoteResolution.Width / picRemoteScreen.Width;
            float scaleY = (float)remoteResolution.Height / picRemoteScreen.Height;

            return new Point(
                (int)(clientPoint.X * scaleX),
                (int)(clientPoint.Y * scaleY)
            );
        }

        // Sự kiện di chuyển chuột 
        private void PicScreen_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!socketService.IsConnected) return;

            Point remote = ScaleToRemote(e.Location);
            SendCommand($"CMD|MM|{remote.X}|{remote.Y}");
        }

        //  Sự kiện nhấn nút chuột 
        private void PicScreen_MouseDown(object? sender, MouseEventArgs e)
        {
            if (!socketService.IsConnected) return;

            // Di chuyển chuột đến đúng vị trí trước khi click
            Point remote = ScaleToRemote(e.Location);
            SendCommand($"CMD|MM|{remote.X}|{remote.Y}");

            if (e.Button == MouseButtons.Left)
                SendCommand("CMD|ML|DOWN");
            else if (e.Button == MouseButtons.Right)
                SendCommand("CMD|MR|DOWN");
        }

        //  Sự kiện thả nút chuột 
        private void PicScreen_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!socketService.IsConnected) return;

            if (e.Button == MouseButtons.Left)
                SendCommand("CMD|ML|UP");
            else if (e.Button == MouseButtons.Right)
                SendCommand("CMD|MR|UP");
        }

        //  Sự kiện cuộn bánh xe chuột 
        private void PicScreen_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (!socketService.IsConnected) return;
            SendCommand($"CMD|MW|{e.Delta}");
        }

        //  Sự kiện giữ phím bàn phím 
        private void ClientForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (!socketService.IsConnected) return;
            SendCommand($"CMD|KD|{(byte)e.KeyCode}");
            e.Handled = true; // ngăn form xử lý phím mặc định
        }

        //  Sự kiện thả phím bàn phím 
        private void ClientForm_KeyUp(object? sender, KeyEventArgs e)
        {
            if (!socketService.IsConnected) return;
            SendCommand($"CMD|KU|{(byte)e.KeyCode}");
        }

        //  Đóng gói và gửi lệnh qua socket 
        private void SendCommand(string command)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(command);
                byte[] lenBytes = BitConverter.GetBytes(data.Length);
                socketService.SendRaw(lenBytes);
                socketService.SendRaw(data);
            }
            catch (Exception ex)
            {
                AppendLog("Lỗi gửi lệnh: " + ex.Message);
            }
        }
        //  Kết nối / Ngắt kết nối

        private void BtnConnect_Click(object? sender, EventArgs e)
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

        private void BtnDisconnect_Click(object? sender, EventArgs e)
        {
            socketService.Disconnect();
            UpdateButtonState(false);
        }

        //  Hiển thị màn hình remote

        private void DisplayRemoteScreen(byte[] imageBytes)
        {
            try
            {
                Image image = screenService.ConvertBytesToImage(imageBytes);

                // Lưu độ phân giải remote để tính toán tọa độ chuột chính xác
                if (remoteResolution.IsEmpty)
                    remoteResolution = image.Size;

                Image? oldImage = picRemoteScreen.Image;
                picRemoteScreen.Image = image;
                oldImage?.Dispose(); // giải phóng ảnh cũ tránh memory leak
            }
            catch (Exception ex)
            {
                AppendLog("Lỗi hiển thị ảnh: " + ex.Message);
            }
        }

        private void AppendLog(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");

            if (message.Contains("Connected to server"))
                UpdateButtonState(true);

            if (message.Contains("Disconnected") ||
                message.Contains("Connection failed") ||
                message.Contains("Server disconnected"))
            {
                remoteResolution = Size.Empty;
                UpdateButtonState(false);
            }
        }

        private void UpdateButtonState(bool connected)
        {
            btnConnect.Enabled = !connected;
            btnDisconnect.Enabled = connected;
            txtIP.Enabled = !connected;
            txtPort.Enabled = !connected;

            // Bật/tắt nhận input tùy theo trạng thái kết nối
            picRemoteScreen.Enabled = connected;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            picRemoteScreen.Image?.Dispose();
            socketService.Disconnect();
            base.OnFormClosing(e);
        }
    }
}
