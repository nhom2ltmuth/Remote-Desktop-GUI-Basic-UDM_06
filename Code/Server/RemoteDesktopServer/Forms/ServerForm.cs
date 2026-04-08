using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace RemoteDesktopServer
{
    public partial class ServerForm : Form
    {
        TcpListener? server;

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

                server = new TcpListener(IPAddress.Any, 9999);
                server.Start();

                lblStatus.Text = "Waiting for client...";
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                server = null;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {

                if (server != null)
                {
                    server.Stop();
                    server = null;
                }

                lblStatus.Text = "Server stopped";
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
