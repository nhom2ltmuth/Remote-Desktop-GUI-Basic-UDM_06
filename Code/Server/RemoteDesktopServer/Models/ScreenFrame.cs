using System;

namespace RemoteDesktopServer.Models
{
    internal class ScreenFrame
    {
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public DateTime CapturedAt { get; set; }
    }
}