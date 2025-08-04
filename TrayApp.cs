using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MTGAFullscreenHelper
{
    public class TrayApp : ApplicationContext
    {
        private NotifyIcon? trayIcon;
        private System.Threading.Timer? timer;
        private readonly Config config;
        private bool active = true;

        public TrayApp(Config config)
        {
            this.config = config;
            InitializeTrayIcon();
            StartTimer();
        }

        private void InitializeTrayIcon()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = System.Drawing.SystemIcons.Application,
                ContextMenuStrip = CreateContextMenu(),
                Visible = true,
                Text = "MTGA Fullscreen Helper (Active)"
            };
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            
            var toggleItem = new ToolStripMenuItem("Toggle Active", null, ToggleActive);
            var quitItem = new ToolStripMenuItem("Quit", null, Quit);
            
            contextMenu.Items.Add(toggleItem);
            contextMenu.Items.Add(quitItem);
            
            return contextMenu;
        }

        private void StartTimer()
        {
            timer = new System.Threading.Timer(CheckFullscreen, null, 0, config.CheckIntervalMs);
        }

        private void StopTimer()
        {
            timer?.Dispose();
            timer = null;
        }

        private void CheckFullscreen(object? state)
        {
            if (!active) return;
            
            IntPtr hwnd = FindWindowByTitle(config.WindowTitle);
            if (hwnd == IntPtr.Zero) return;

            if (IsWindowed(hwnd))
            {
                SendAltEnter(hwnd);
            }
        }

        private void ToggleActive(object? sender, EventArgs e)
        {
            active = !active;
            if (trayIcon != null)
            {
                trayIcon.Text = active ? "MTGA Fullscreen Helper (Active)" : "MTGA Fullscreen Helper (Paused)";
            }
        }

        private void Quit(object? sender, EventArgs e)
        {
            StopTimer();
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }
            Application.Exit();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopTimer();
                trayIcon?.Dispose();
            }
            base.Dispose(disposing);
        }

        // --- Win32 Interop ---

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int nIndex);
        
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        const int GWL_STYLE = -16;
        const int WS_CAPTION = 0x00C00000;

        static IntPtr FindWindowByTitle(string title)
        {
            return FindWindow(null, title);
        }

        static bool IsWindowed(IntPtr hwnd)
        {
            int style = GetWindowLong(hwnd, GWL_STYLE);
            return (style & WS_CAPTION) != 0;
        }

        static void SendAltEnter(IntPtr hwnd)
        {
            SetForegroundWindow(hwnd);
            SendKeys.SendWait("%{ENTER}"); // % is ALT in SendKeys
        }
    }
}