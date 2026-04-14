namespace RemoteDesktopClient.Forms
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;

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
            txtIP = new System.Windows.Forms.TextBox();
            txtPort = new System.Windows.Forms.TextBox();
            txtLog = new System.Windows.Forms.TextBox();
            lblStatus = new System.Windows.Forms.Label();
            btnConnect = new System.Windows.Forms.Button();
            btnDisconnect = new System.Windows.Forms.Button();
            picRemoteScreen = new System.Windows.Forms.PictureBox();
            lblIpTitle = new System.Windows.Forms.Label();
            lblPortTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)picRemoteScreen).BeginInit();
            SuspendLayout();
            // 
            // txtIP
            // 
            txtIP.Location = new System.Drawing.Point(58, 15);
            txtIP.Name = "txtIP";
            txtIP.Size = new System.Drawing.Size(130, 23);
            txtIP.TabIndex = 0;
            // 
            // txtPort
            // 
            txtPort.Location = new System.Drawing.Point(245, 15);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(80, 23);
            txtPort.TabIndex = 1;
            // 
            // txtLog
            // 
            txtLog.Location = new System.Drawing.Point(12, 500);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtLog.Size = new System.Drawing.Size(960, 110);
            txtLog.TabIndex = 6;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new System.Drawing.Point(548, 19);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(79, 15);
            lblStatus.TabIndex = 7;
            lblStatus.Text = "Disconnected";
            // 
            // btnConnect
            // 
            btnConnect.Location = new System.Drawing.Point(350, 14);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new System.Drawing.Size(85, 25);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new System.Drawing.Point(441, 14);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new System.Drawing.Size(90, 25);
            btnDisconnect.TabIndex = 3;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // picRemoteScreen
            // 
            picRemoteScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picRemoteScreen.Location = new System.Drawing.Point(12, 55);
            picRemoteScreen.Name = "picRemoteScreen";
            picRemoteScreen.Size = new System.Drawing.Size(960, 430);
            picRemoteScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picRemoteScreen.TabIndex = 4;
            picRemoteScreen.TabStop = false;
            // 
            // lblIpTitle
            // 
            lblIpTitle.AutoSize = true;
            lblIpTitle.Location = new System.Drawing.Point(12, 19);
            lblIpTitle.Name = "lblIpTitle";
            lblIpTitle.Size = new System.Drawing.Size(20, 15);
            lblIpTitle.TabIndex = 8;
            lblIpTitle.Text = "IP:";
            // 
            // lblPortTitle
            // 
            lblPortTitle.AutoSize = true;
            lblPortTitle.Location = new System.Drawing.Point(200, 19);
            lblPortTitle.Name = "lblPortTitle";
            lblPortTitle.Size = new System.Drawing.Size(32, 15);
            lblPortTitle.TabIndex = 9;
            lblPortTitle.Text = "Port:";
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(984, 621);
            Controls.Add(lblPortTitle);
            Controls.Add(lblIpTitle);
            Controls.Add(lblStatus);
            Controls.Add(txtLog);
            Controls.Add(picRemoteScreen);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Controls.Add(txtPort);
            Controls.Add(txtIP);
            Name = "ClientForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Remote Desktop Client";
            ((System.ComponentModel.ISupportInitialize)picRemoteScreen).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}