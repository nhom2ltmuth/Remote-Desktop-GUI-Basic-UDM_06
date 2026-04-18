namespace RemoteDesktopClient.Forms
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel bottomPanel;

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.PictureBox picRemoteScreen;
        private System.Windows.Forms.Label lblIpTitle;
        private System.Windows.Forms.Label lblPortTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            topPanel = new System.Windows.Forms.Panel();
            bottomPanel = new System.Windows.Forms.Panel();

            txtIP = new System.Windows.Forms.TextBox();
            txtPort = new System.Windows.Forms.TextBox();
            txtLog = new System.Windows.Forms.TextBox();
            lblStatus = new System.Windows.Forms.Label();
            btnConnect = new System.Windows.Forms.Button();
            btnDisconnect = new System.Windows.Forms.Button();
            picRemoteScreen = new System.Windows.Forms.PictureBox();
            lblIpTitle = new System.Windows.Forms.Label();
            lblPortTitle = new System.Windows.Forms.Label();

            topPanel.SuspendLayout();
            bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(picRemoteScreen)).BeginInit();
            SuspendLayout();

            // 
            // topPanel
            // 
            topPanel.Controls.Add(lblIpTitle);
            topPanel.Controls.Add(txtIP);
            topPanel.Controls.Add(lblPortTitle);
            topPanel.Controls.Add(txtPort);
            topPanel.Controls.Add(btnConnect);
            topPanel.Controls.Add(btnDisconnect);
            topPanel.Controls.Add(lblStatus);
            topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            topPanel.Location = new System.Drawing.Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Padding = new System.Windows.Forms.Padding(10);
            topPanel.Size = new System.Drawing.Size(1200, 55);
            topPanel.TabIndex = 0;

            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(txtLog);
            bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            bottomPanel.Location = new System.Drawing.Point(0, 570);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            bottomPanel.Size = new System.Drawing.Size(1200, 130);
            bottomPanel.TabIndex = 1;

            // 
            // txtIP
            // 
            txtIP.Location = new System.Drawing.Point(45, 14);
            txtIP.Name = "txtIP";
            txtIP.Size = new System.Drawing.Size(140, 23);
            txtIP.TabIndex = 0;

            // 
            // txtPort
            // 
            txtPort.Location = new System.Drawing.Point(240, 14);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(80, 23);
            txtPort.TabIndex = 1;

            // 
            // txtLog
            // 
            txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            txtLog.Location = new System.Drawing.Point(10, 5);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtLog.Size = new System.Drawing.Size(1180, 115);
            txtLog.TabIndex = 6;

            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new System.Drawing.Point(545, 18);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(79, 15);
            lblStatus.TabIndex = 7;
            lblStatus.Text = "Disconnected";

            // 
            // btnConnect
            // 
            btnConnect.Location = new System.Drawing.Point(340, 13);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new System.Drawing.Size(85, 25);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;

            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new System.Drawing.Point(435, 13);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new System.Drawing.Size(90, 25);
            btnDisconnect.TabIndex = 3;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;

            // 
            // picRemoteScreen
            // 
            picRemoteScreen.BackColor = System.Drawing.Color.Black;
            picRemoteScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picRemoteScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            picRemoteScreen.Location = new System.Drawing.Point(0, 55);
            picRemoteScreen.Name = "picRemoteScreen";
            picRemoteScreen.Size = new System.Drawing.Size(1200, 515);
            picRemoteScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picRemoteScreen.TabIndex = 4;
            picRemoteScreen.TabStop = false;ừ 
            // 
            // lblIpTitle
            // 
            lblIpTitle.AutoSize = true;
            lblIpTitle.Location = new System.Drawing.Point(10, 18);
            lblIpTitle.Name = "lblIpTitle";
            lblIpTitle.Size = new System.Drawing.Size(20, 15);
            lblIpTitle.TabIndex = 8;
            lblIpTitle.Text = "IP:";

            // 
            // lblPortTitle
            // 
            lblPortTitle.AutoSize = true;
            lblPortTitle.Location = new System.Drawing.Point(200, 18);
            lblPortTitle.Name = "lblPortTitle";
            lblPortTitle.Size = new System.Drawing.Size(32, 15);
            lblPortTitle.TabIndex = 9;
            lblPortTitle.Text = "Port:";

            // 
            // ClientForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1200, 700);
            Controls.Add(picRemoteScreen);
            Controls.Add(bottomPanel);
            Controls.Add(topPanel);
            Name = "ClientForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Remote Desktop Client";

            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(picRemoteScreen)).EndInit();
            ResumeLayout(false);
        }
    }
}