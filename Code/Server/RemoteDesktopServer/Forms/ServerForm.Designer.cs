namespace RemoteDesktopServer
{
    partial class ServerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblStatus = new Label();
            lblIP = new Label();
            lblPort = new Label();
            btnStart = new Button();
            btnStop = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblTitle.Location = new Point(30, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(208, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "REMOTE DESKTOP SERVER";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(30, 50);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(132, 20);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Waiting for client...";
            // 
            // lblIP
            // 
            lblIP.AutoSize = true;
            lblIP.Location = new Point(30, 80);
            lblIP.Name = "lblIP";
            lblIP.Size = new Size(85, 20);
            lblIP.TabIndex = 2;
            lblIP.Text = "IP: 127.0.0.1";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(30, 110);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(70, 20);
            lblPort.TabIndex = 3;
            lblPort.Text = "Port:9999";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(30, 150);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(90, 30);
            btnStart.TabIndex = 4;
            btnStart.Text = "Start ";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(150, 150);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(90, 30);
            btnStop.TabIndex = 5;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(379, 235);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(lblPort);
            Controls.Add(lblIP);
            Controls.Add(lblStatus);
            Controls.Add(lblTitle);
            Name = "ServerForm";
            Text = "Remote Desktop Server";
            Load += ServerForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblStatus;
        private Label lblIP;
        private Label lblPort;
        private Button btnStart;
        private Button btnStop;
    }
}
