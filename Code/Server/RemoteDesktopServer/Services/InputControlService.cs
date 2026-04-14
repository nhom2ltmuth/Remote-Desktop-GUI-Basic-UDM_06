using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RemoteDesktopServer.Services
{
    internal class InputControlService
    {
        public Action<string>? OnLog;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        public void MoveMouse(int x, int y)
        {
            try
            {
                Cursor.Position = new Point(x, y);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("MoveMouse error: " + ex.Message);
            }
        }

        public void LeftMouseDown()
        {
            try
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("LeftMouseDown error: " + ex.Message);
            }
        }

        public void LeftMouseUp()
        {
            try
            {
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("LeftMouseUp error: " + ex.Message);
            }
        }

        public void RightMouseDown()
        {
            try
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("RightMouseDown error: " + ex.Message);
            }
        }

        public void RightMouseUp()
        {
            try
            {
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("RightMouseUp error: " + ex.Message);
            }
        }

        public void MouseWheel(int delta)
        {
            try
            {
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (uint)delta, 0);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("MouseWheel error: " + ex.Message);
            }
        }

        public void KeyDown(Keys key)
        {
            try
            {
                keybd_event((byte)key, 0, 0, 0);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("KeyDown error: " + ex.Message);
            }
        }

        public void KeyUp(Keys key)
        {
            try
            {
                keybd_event((byte)key, 0, KEYEVENTF_KEYUP, 0);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("KeyUp error: " + ex.Message);
            }
        }
    }
}