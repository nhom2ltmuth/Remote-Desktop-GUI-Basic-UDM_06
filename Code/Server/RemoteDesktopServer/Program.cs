using System;
using System.Windows.Forms;
using RemoteDesktopServer.Forms;

namespace RemoteDesktopServer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerForm());
        }
    }
}