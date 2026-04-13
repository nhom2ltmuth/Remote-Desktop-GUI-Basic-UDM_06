using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class ClientSocketService
{
    private TcpClient client;
    private NetworkStream stream;
    private CancellationTokenSource cts;

    public event Action<byte[]> OnFrameReceived;

    public async Task ConnectAsync(string ip, int port)
    {
        client = new TcpClient();
        await client.ConnectAsync(ip, port);
        stream = client.GetStream();

        cts = new CancellationTokenSource();
        _ = Task.Run(() => ReceiveLoop(cts.Token));
    }

    private async Task ReceiveLoop(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                byte[] lengthBuffer = await ReadExactAsync(4);
                if (lengthBuffer == null) break;

                int length = BitConverter.ToInt32(lengthBuffer, 0);

                byte[] imageBuffer = await ReadExactAsync(length);
                if (imageBuffer == null) break;

                OnFrameReceived?.Invoke(imageBuffer);
            }
        }
        catch
        {
            Disconnect();
        }
    }

    private async Task<byte[]> ReadExactAsync(int size)
    {
        byte[] buffer = new byte[size];
        int total = 0;

        while (total < size)
        {
            int read = await stream.ReadAsync(buffer, total, size - total);
            if (read == 0) return null;
            total += read;
        }

        return buffer;
    }

    public void Disconnect()
    {
        try
        {
            cts?.Cancel();
            stream?.Close();
            client?.Close();
        }
        catch { }
    }
}