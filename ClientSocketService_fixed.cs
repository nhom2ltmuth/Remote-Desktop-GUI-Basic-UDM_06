using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;

public class ClientSocketService
{
    private TcpClient _client;
    private NetworkStream _stream;

    public void Connect(string ip = "127.0.0.1", int port = 5000)
    {
        _client = new TcpClient();
        _client.Connect(ip, port);
        _stream = _client.GetStream();
    }

    public Image ReceiveImage()
    {
        try
        {
            byte[] lengthBytes = new byte[4];
            int read = _stream.Read(lengthBytes, 0, 4);
            if (read < 4) return null;

            int length = BitConverter.ToInt32(lengthBytes, 0);

            byte[] imageBytes = new byte[length];
            int totalRead = 0;

            while (totalRead < length)
            {
                int bytesRead = _stream.Read(imageBytes, totalRead, length - totalRead);
                if (bytesRead == 0) break;
                totalRead += bytesRead;
            }

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }
        catch
        {
            return null;
        }
    }
}
