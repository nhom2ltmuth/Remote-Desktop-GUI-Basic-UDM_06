namespace RemoteDesktopServer.Forms
{
    partial class ServerForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIpAddress;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblPortTitle;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.Label lblIpTitle;

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
            txtPort = new System.Windows.Forms.TextBox();
            txtLog = new System.Windows.Forms.TextBox();
            lblStatus = new System.Windows.Forms.Label();
            lblIpAddress = new System.Windows.Forms.Label();
            btnStart = new System.Windows.Forms.Button();
            btnStop = new System.Windows.Forms.Button();
            lblPortTitle = new System.Windows.Forms.Label();
            lblStatusTitle = new System.Windows.Forms.Label();
            lblIpTitle = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // txtPort
            // 
            txtPort.Location = new System.Drawing.Point(70, 15);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(80, 23);
            txtPort.TabIndex = 0;
            // 
            // txtLog
            // 
            txtLog.Location = new System.Drawing.Point(12, 90);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtLog.Size = new System.Drawing.Size(760, 320);
            txtLog.TabIndex = 5;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new System.Drawing.Point(410, 20);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(83, 15);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Server stopped";
            // 
            // lblIpAddress
            // 
            lblIpAddress.AutoSize = true;
            lblIpAddress.Location = new System.Drawing.Point(70, 55);
            lblIpAddress.Name = "lblIpAddress";
            lblIpAddress.Size = new System.Drawing.Size(58, 15);
            lblIpAddress.TabIndex = 7;
            lblIpAddress.Text = "127.0.0.1";
            // 
            // btnStart
            // 
            btnStart.Location = new System.Drawing.Point(180, 14);
            btnStart.Name = "btnStart";
            btnStart.Size = new System.Drawing.Size(90, 25);
            btnStart.TabIndex = 1;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new System.Drawing.Point(280, 14);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(90, 25);
            btnStop.TabIndex = 2;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // lblPortTitle
            // 
            lblPortTitle.AutoSize = true;
            lblPortTitle.Location = new System.Drawing.Point(12, 19);
            lblPortTitle.Name = "lblPortTitle";
            lblPortTitle.Size = new System.Drawing.Size(32, 15);
            lblPortTitle.TabIndex = 8;
            lblPortTitle.Text = "Port:";
            // 
            // lblStatusTitle
            // 
            lblStatusTitle.AutoSize = true;
            lblStatusTitle.Location = new System.Drawing.Point(360, 20);
            lblStatusTitle.Name = "lblStatusTitle";
            lblStatusTitle.Size = new System.Drawing.Size(42, 15);
            lblStatusTitle.TabIndex = 9;
            lblStatusTitle.Text = "Status:";
            // 
            // lblIpTitle
            // 
            lblIpTitle.AutoSize = true;
            lblIpTitle.Location = new System.Drawing.Point(12, 55);
            lblIpTitle.Name = "lblIpTitle";
            lblIpTitle.Size = new System.Drawing.Size(20, 15);
            lblIpTitle.TabIndex = 10;
            lblIpTitle.Text = "IP:";
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(784, 431);
            Controls.Add(lblIpTitle);
            Controls.Add(lblStatusTitle);
            Controls.Add(lblPortTitle);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(lblIpAddress);
            Controls.Add(lblStatus);
            Controls.Add(txtLog);
            Controls.Add(txtPort);
            Name = "ServerForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Remote Desktop Server";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}