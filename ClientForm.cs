using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class ClientForm : Form
{
    private ClientSocketService clientService;

    private PictureBox pictureBox;
    private TextBox txtIP;
    private Button btnConnect;

    public ClientForm()
    {
        InitUI();
    }

    private void InitUI()
    {
        this.Text = "Remote Desktop Client";
        this.Width = 800;
        this.Height = 600;
        this.DoubleBuffered = true;

        txtIP = new TextBox() { Left = 10, Top = 10, Width = 150, Text = " " };
        btnConnect = new Button() { Left = 170, Top = 8, Width = 100, Text = "Connect" };

        pictureBox = new PictureBox()
        {
            Left = 10,
            Top = 40,
            Width = 760,
            Height = 500,
            BorderStyle = BorderStyle.FixedSingle,
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        btnConnect.Click += BtnConnect_Click;

        this.Controls.Add(txtIP);
        this.Controls.Add(btnConnect);
        this.Controls.Add(pictureBox);
    }

    private async void BtnConnect_Click(object sender, EventArgs e)
    {
        clientService = new ClientSocketService();
        clientService.OnFrameReceived += ClientService_OnFrameReceived;

        try
        {
            await clientService.ConnectAsync(txtIP.Text, 5000);
            MessageBox.Show("Connected!");
        }
        catch
        {
            MessageBox.Show("Connection failed!");
        }
    }

    private void ClientService_OnFrameReceived(byte[] data)
    {
        try
        {
            Image img;
            using (MemoryStream ms = new MemoryStream(data))
            {
                img = Image.FromStream(ms);
            }

            if (pictureBox.InvokeRequired)
            {
                pictureBox.Invoke(new Action(() =>
                {
                    UpdateImage(img);
                }));
            }
            else
            {
                UpdateImage(img);
            }
        }
        catch { }
    }

    private void UpdateImage(Image img)
    {
        var old = pictureBox.Image;
        pictureBox.Image = (Image)img.Clone();
        old?.Dispose();
        img.Dispose();
    }
}