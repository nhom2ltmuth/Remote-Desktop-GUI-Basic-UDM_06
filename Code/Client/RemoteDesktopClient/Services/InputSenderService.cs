using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RemoteDesktopClient.Models;

namespace RemoteDesktopClient.Services
{
    internal class InputSenderService
    {
        private readonly ClientSocketService socketService;
        private readonly RemoteScreenInfo remoteScreenInfo;

        public Action<string>? OnLog;

        public InputSenderService(ClientSocketService socketService, RemoteScreenInfo remoteScreenInfo)
        {
            this.socketService = socketService;
            this.remoteScreenInfo = remoteScreenInfo;
        }

        public Point ScaleToRemote(Point clientPoint, Size displaySize)
        {
            if (remoteScreenInfo.Resolution.IsEmpty || displaySize.Width == 0 || displaySize.Height == 0)
                return clientPoint;

            float scaleX = (float)remoteScreenInfo.Resolution.Width / displaySize.Width;
            float scaleY = (float)remoteScreenInfo.Resolution.Height / displaySize.Height;

            return new Point(
                (int)(clientPoint.X * scaleX),
                (int)(clientPoint.Y * scaleY)
            );
        }

        public void SendMouseMove(Point clientPoint, Size displaySize)
        {
            if (!socketService.IsConnected) return;

            Point remote = ScaleToRemote(clientPoint, displaySize);
            SendCommand($"CMD|MM|{remote.X}|{remote.Y}");
        }

        public void SendMouseDown(MouseButtons button, Point clientPoint, Size displaySize)
        {
            if (!socketService.IsConnected) return;

            Point remote = ScaleToRemote(clientPoint, displaySize);
            SendCommand($"CMD|MM|{remote.X}|{remote.Y}");

            if (button == MouseButtons.Left)
                SendCommand("CMD|ML|DOWN");
            else if (button == MouseButtons.Right)
                SendCommand("CMD|MR|DOWN");
        }

        public void SendMouseUp(MouseButtons button)
        {
            if (!socketService.IsConnected) return;

            if (button == MouseButtons.Left)
                SendCommand("CMD|ML|UP");
            else if (button == MouseButtons.Right)
                SendCommand("CMD|MR|UP");
        }

        public void SendMouseWheel(int delta)
        {
            if (!socketService.IsConnected) return;

            SendCommand($"CMD|MW|{delta}");
        }

        public void SendKeyDown(Keys key)
        {
            if (!socketService.IsConnected) return;

            SendCommand($"CMD|KD|{(int)key}");
        }

        public void SendKeyUp(Keys key)
        {
            if (!socketService.IsConnected) return;

            SendCommand($"CMD|KU|{(int)key}");
        }

        private void SendCommand(string command)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(command);
                byte[] lengthBytes = BitConverter.GetBytes(data.Length);

                socketService.SendRaw(lengthBytes);
                socketService.SendRaw(data);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("Send command error: " + ex.Message);
            }
        }
    }
}