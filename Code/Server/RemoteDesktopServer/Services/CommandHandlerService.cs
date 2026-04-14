using System;
using System.Windows.Forms;

namespace RemoteDesktopServer.Services
{
    internal class CommandHandlerService
    {
        private readonly InputControlService inputControlService;

        public Action<string>? OnLog;

        public CommandHandlerService(InputControlService inputControlService)
        {
            this.inputControlService = inputControlService;
        }

        public void HandleCommand(string command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command))
                    return;

                string[] parts = command.Split('|');

                if (parts.Length < 2 || parts[0] != "CMD")
                {
                    OnLog?.Invoke("Invalid command format: " + command);
                    return;
                }

                switch (parts[1])
                {
                    case "MM":
                        HandleMouseMove(parts);
                        break;
                    case "ML":
                        HandleLeftMouse(parts);
                        break;
                    case "MR":
                        HandleRightMouse(parts);
                        break;
                    case "MW":
                        HandleMouseWheel(parts);
                        break;
                    case "KD":
                        HandleKeyDown(parts);
                        break;
                    case "KU":
                        HandleKeyUp(parts);
                        break;
                    default:
                        OnLog?.Invoke("Unknown command: " + command);
                        break;
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("HandleCommand error: " + ex.Message);
            }
        }

        private void HandleMouseMove(string[] parts)
        {
            if (parts.Length < 4)
                return;

            if (int.TryParse(parts[2], out int x) && int.TryParse(parts[3], out int y))
            {
                inputControlService.MoveMouse(x, y);
            }
        }

        private void HandleLeftMouse(string[] parts)
        {
            if (parts.Length < 3)
                return;

            if (parts[2] == "DOWN")
                inputControlService.LeftMouseDown();
            else if (parts[2] == "UP")
                inputControlService.LeftMouseUp();
        }

        private void HandleRightMouse(string[] parts)
        {
            if (parts.Length < 3)
                return;

            if (parts[2] == "DOWN")
                inputControlService.RightMouseDown();
            else if (parts[2] == "UP")
                inputControlService.RightMouseUp();
        }

        private void HandleMouseWheel(string[] parts)
        {
            if (parts.Length < 3)
                return;

            if (int.TryParse(parts[2], out int delta))
            {
                inputControlService.MouseWheel(delta);
            }
        }

        private void HandleKeyDown(string[] parts)
        {
            if (parts.Length < 3)
                return;

            if (int.TryParse(parts[2], out int keyCode))
            {
                inputControlService.KeyDown((Keys)keyCode);
            }
        }

        private void HandleKeyUp(string[] parts)
        {
            if (parts.Length < 3)
                return;

            if (int.TryParse(parts[2], out int keyCode))
            {
                inputControlService.KeyUp((Keys)keyCode);
            }
        }
    }
}