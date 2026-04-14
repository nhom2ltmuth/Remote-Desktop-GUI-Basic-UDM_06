using System.Drawing;
using System.IO;

namespace RemoteDesktopClient.Services
{
    internal class RemoteScreenService
    {
        public Image ConvertBytesToImage(byte[] imageBytes)
        {
            using MemoryStream ms = new MemoryStream(imageBytes);
            using Image tempImage = Image.FromStream(ms);
            return new Bitmap(tempImage);
        }
    }
}