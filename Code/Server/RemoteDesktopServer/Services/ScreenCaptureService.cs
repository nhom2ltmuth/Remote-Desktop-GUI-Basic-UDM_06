using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using RemoteDesktopServer.Models;

namespace RemoteDesktopServer.Services
{
    internal class ScreenCaptureService
    {
        private Thread? captureThread;
        private bool isRunning;
        private ServerSocketService? socketService;

        public Action<string>? OnLog;

        public bool IsRunning => isRunning;

        public void Start(ServerSocketService serverSocketService)
        {
            if (isRunning)
            {
                OnLog?.Invoke("Screen capture already running.");
                return;
            }

            socketService = serverSocketService;
            isRunning = true;

            captureThread = new Thread(CaptureLoop)
            {
                IsBackground = true
            };
            captureThread.Start();

            OnLog?.Invoke("Screen capture started.");
        }

        public void Stop()
        {
            isRunning = false;
            OnLog?.Invoke("Screen capture stopped.");
        }

        private void CaptureLoop()
        {
            try
            {
                while (isRunning)
                {
                    if (socketService == null || !socketService.IsClientConnected)
                    {
                        Thread.Sleep(200);
                        continue;
                    }

                    ScreenFrame frame = CaptureScreenFrame();
                    socketService.SendImage(frame.ImageData);

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("CaptureLoop error: " + ex.Message);
            }
        }

        private ScreenFrame CaptureScreenFrame()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;

            using Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);

            using MemoryStream ms = new MemoryStream();
            ImageCodecInfo? jpegCodec = GetJpegEncoder();

            if (jpegCodec != null)
            {
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 50L);
                bitmap.Save(ms, jpegCodec, encoderParams);
            }
            else
            {
                bitmap.Save(ms, ImageFormat.Jpeg);
            }

            return new ScreenFrame
            {
                ImageData = ms.ToArray(),
                CapturedAt = DateTime.Now
            };
        }

        private ImageCodecInfo? GetJpegEncoder()
        {
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                    return codec;
            }

            return null;
        }
    }
}