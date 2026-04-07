using System;
using System.Drawing;
using System.IO;

namespace RemoteDesktopClient.Services
{
    internal class RemoteScreenService
    {
        public Image ConvertBytesToImage(byte[] imageData)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                return Image.FromStream(ms);
            }
        }
    }
}