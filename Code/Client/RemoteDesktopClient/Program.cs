using System;
using System.Windows.Forms;
using RemoteDesktopClient.Forms;

namespace RemoteDesktopClient
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientForm());
        }
    }
}