using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using RemoteDesktopServer.Models;
using RemoteDesktopServer.Services;

namespace RemoteDesktopServer.Forms
{
    public partial class ServerForm : Form
    {
        private readonly ServerSocketService socketService;
        private readonly ScreenCaptureService screenCaptureService;
        private readonly InputControlService inputControlService;
        private readonly CommandHandlerService commandHandlerService;
        private readonly ClientSession clientSession;

        public ServerForm()
        {
            InitializeComponent();

            clientSession = new ClientSession();

            socketService = new ServerSocketService();
            screenCaptureService = new ScreenCaptureService();
            inputControlService = new InputControlService();
            commandHandlerService = new CommandHandlerService(inputControlService);

            InitializeServices();

            Load += ServerForm_Load;
            FormClosing += ServerForm_FormClosing;
        }

        private void InitializeServices()
        {
            socketService.OnLog += AddLog;
            screenCaptureService.OnLog += AddLog;
            inputControlService.OnLog += AddLog;
            commandHandlerService.OnLog += AddLog;

            socketService.OnClientConnected += session =>
            {
                clientSession.IPAddress = session.IPAddress;
                clientSession.Port = session.Port;
                clientSession.IsConnected = true;

                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        lblStatus.Text = "Client connected";
                        AddLog($"Client connected: {session.IPAddress}:{session.Port}");
                    }));
                }
                else
                {
                    lblStatus.Text = "Client connected";
                    AddLog($"Client connected: {session.IPAddress}:{session.Port}");
                }

                screenCaptureService.Start(socketService);
            };

            socketService.OnClientDisconnected += () =>
            {
                clientSession.IsConnected = false;

                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        lblStatus.Text = "Waiting for client...";
                        AddLog("Client disconnected.");
                    }));
                }
                else
                {
                    lblStatus.Text = "Waiting for client...";
                    AddLog("Client disconnected.");
                }

                screenCaptureService.Stop();
            };

            socketService.OnCommandReceived += command =>
            {
                commandHandlerService.HandleCommand(command);
            };
        }

        private void ServerForm_Load(object? sender, EventArgs e)
        {
            txtPort.Text = "9999";
            lblStatus.Text = "Server stopped";
            lblIpAddress.Text = GetLocalIPv4();

            btnStop.Enabled = false;

            AddLog("Server form loaded.");
            AddLog("Local IP: " + lblIpAddress.Text);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Port không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            socketService.Start(port);

            lblStatus.Text = "Waiting for client...";
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            AddLog($"Server started on port {port}.");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void StopServer()
        {
            screenCaptureService.Stop();
            socketService.Stop();

            clientSession.IsConnected = false;

            lblStatus.Text = "Server stopped";
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            AddLog("Server stopped.");
        }

        private void ServerForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            StopServer();
        }

        private void AddLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string>(AddLog), message);
                return;
            }

            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private string GetLocalIPv4()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                var ip = host.AddressList
                    .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

                return ip?.ToString() ?? "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }
    }
}