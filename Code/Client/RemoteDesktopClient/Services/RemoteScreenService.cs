using System.Drawing;
using System.IO;

namespace RemoteDesktopClient.Services
{
    internal class RemoteScreenService
    {
        public Image ConvertBytesToImage(byte[] imageData)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            using (Image tempImage = Image.FromStream(ms))
            {
                return new Bitmap(tempImage);
            }
        }
    }
}