using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

public partial class ClientForm : Form
{
    private ClientSocketService _client = new ClientSocketService();

    public ClientForm()
    {
        InitializeComponent();
    }

    private void ClientForm_Load(object sender, EventArgs e)
    {
        _client.Connect("127.0.0.1", 5000);

        Thread t = new Thread(ReceiveLoop);
        t.IsBackground = true;
        t.Start();
    }

    private void ReceiveLoop()
    {
        while (true)
        {
            Image img = _client.ReceiveImage();

            if (img != null)
            {
                this.Invoke(new Action(() =>
                {
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = (Image)img.Clone();
                }));
            }
        }
    }
}
