using System;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteDesktopClient
{
    public class Form1 : Form
    {
        TextBox txtIP;
        TextBox txtPassword;
        Button btnConnect;
        PictureBox picRemoteScreen;

        public Form1()
        {
            CreateUI();
        }

        private void CreateUI()
        {
            this.Text = "Client Remote Desktop";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- Phần IP ---
            Label lblIP = new Label();
            lblIP.Text = "IP:";
            lblIP.Location = new Point(20, 20);
            lblIP.AutoSize = true;

            txtIP = new TextBox();
            txtIP.Location = new Point(50, 18);
            txtIP.Width = 150; // Độ rộng ô IP

            // --- Phần Password ---
            Label lblPass = new Label();
            lblPass.Text = "Password:";
            lblPass.Location = new Point(220, 20); // Khoảng cách hợp lý từ ô IP sang
            lblPass.AutoSize = true;

            txtPassword = new TextBox();
            txtPassword.Location = new Point(290, 18); // Căn lề để khoảng hở giống ô IP
            txtPassword.Width = 150; // Độ rộng ô Pass bằng hệt ô IP
            txtPassword.PasswordChar = '*';

            // --- Nút Connect ---
            btnConnect = new Button();
            btnConnect.Text = "Connect";
            btnConnect.Location = new Point(460, 15);
            btnConnect.Width = 100;
            btnConnect.Click += BtnConnect_Click;

            // --- PictureBox ---
            picRemoteScreen = new PictureBox();
            picRemoteScreen.Location = new Point(20, 60);
            picRemoteScreen.Size = new Size(840, 480);
            picRemoteScreen.BorderStyle = BorderStyle.FixedSingle;
            picRemoteScreen.SizeMode = PictureBoxSizeMode.StretchImage;

            // Thêm các thành phần vào Form
            this.Controls.Add(lblIP);
            this.Controls.Add(txtIP);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnConnect);
            this.Controls.Add(picRemoteScreen);
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Xin chào! Kết nối thành công (demo)");
        }
    }
}