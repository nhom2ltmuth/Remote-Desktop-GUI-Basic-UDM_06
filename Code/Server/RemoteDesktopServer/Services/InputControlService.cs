using System;
using System.Runtime.InteropServices;

namespace RemoteDesktopServer.Services
{
    internal class InputControlService
    {
        // ── Import các hàm WinAPI từ user32.dll 
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, IntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        // Cờ điều khiển chuột (mouse_event flags)
        private const uint MOUSEEVENTF_LEFTDOWN   = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP     = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN  = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP    = 0x0010;
        private const uint MOUSEEVENTF_WHEEL      = 0x0800;

        // Cờ điều khiển bàn phím (keybd_event flags)
        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP   = 0x0002;

        // ── Giao thức lệnh (phải khớp với phía Client) 
        // Định dạng: "CMD|<loại>|<tham số...>"
        // Di chuyển chuột  → "CMD|MM|x|y"
        // Chuột trái       → "CMD|ML|DOWN" hoặc "CMD|ML|UP"
        // Chuột phải       → "CMD|MR|DOWN" hoặc "CMD|MR|UP"
        // Cuộn chuột       → "CMD|MW|delta"
        // Nhấn phím        → "CMD|KP|virtualKeyCode"
        // Giữ phím         → "CMD|KD|virtualKeyCode"
        // Thả phím         → "CMD|KU|virtualKeyCode"

        public void Execute(string command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command)) return;

                string[] parts = command.Split('|');
                if (parts.Length < 3 || parts[0] != "CMD") return;

                string type = parts[1];

                switch (type)
                {
                    case "MM": // Di chuyển con trỏ chuột
                        if (parts.Length >= 4
                            && int.TryParse(parts[2], out int mx)
                            && int.TryParse(parts[3], out int my))
                        {
                            SetCursorPos(mx, my);
                        }
                        break;

                    case "ML": // Nút chuột trái
                        if (parts[2] == "DOWN")
                            mouse_event(MOUSEEVENTF_LEFTDOWN,  0, 0, 0, IntPtr.Zero);
                        else
                            mouse_event(MOUSEEVENTF_LEFTUP,    0, 0, 0, IntPtr.Zero);
                        break;

                    case "MR": // Nút chuột phải
                        if (parts[2] == "DOWN")
                            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, IntPtr.Zero);
                        else
                            mouse_event(MOUSEEVENTF_RIGHTUP,   0, 0, 0, IntPtr.Zero);
                        break;

                    case "MW": // Cuộn bánh xe chuột
                        if (int.TryParse(parts[2], out int delta))
                            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (uint)delta, IntPtr.Zero);
                        break;

                    case "KP": // Nhấn phím (xuống rồi lên luôn)
                        if (byte.TryParse(parts[2], out byte vkp))
                        {
                            keybd_event(vkp, 0, KEYEVENTF_KEYDOWN, IntPtr.Zero);
                            keybd_event(vkp, 0, KEYEVENTF_KEYUP,   IntPtr.Zero);
                        }
                        break;

                    case "KD": // Giữ phím xuống
                        if (byte.TryParse(parts[2], out byte vkd))
                            keybd_event(vkd, 0, KEYEVENTF_KEYDOWN, IntPtr.Zero);
                        break;

                    case "KU": // Thả phím ra
                        if (byte.TryParse(parts[2], out byte vku))
                            keybd_event(vku, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi InputControlService: " + ex.Message);
            }
        }
    }
}
