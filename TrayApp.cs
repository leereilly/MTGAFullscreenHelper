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
        private readonly string configPath;
        private bool active = true;
        private DateTime lastRestoreTime = DateTime.MinValue;
        private bool hasBeenFullscreenOnce = false;

        public TrayApp(Config config, string configPath)
        {
            this.config = config;
            this.configPath = configPath;
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
                Text = GetTrayText()
            };
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            
            var toggleItem = new ToolStripMenuItem("Toggle Active", null, ToggleActive);
            var resetCounterItem = new ToolStripMenuItem("Reset Counter", null, ResetCounter);
            var quitItem = new ToolStripMenuItem("Quit", null, Quit);
            
            contextMenu.Items.Add(toggleItem);
            contextMenu.Items.Add(resetCounterItem);
            contextMenu.Items.Add(quitItem);
            
            return contextMenu;
        }

        private string GetTrayText()
        {
            string status = active ? "Active" : "Paused";
            if (active && !hasBeenFullscreenOnce)
            {
                status = "Waiting for game to load";
            }
            return $"MTGA Fullscreen Helper ({status}) - Restored: {config.RestoreCount}";
        }

        private void UpdateTrayText()
        {
            if (trayIcon != null)
            {
                trayIcon.Text = GetTrayText();
            }
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

            bool isCurrentlyWindowed = IsWindowed(hwnd);
            bool wasWaitingForGame = !hasBeenFullscreenOnce;
            
            // If the game is currently fullscreen, mark that we've seen it fullscreen
            if (!isCurrentlyWindowed)
            {
                hasBeenFullscreenOnce = true;
                // Update tray text if we just transitioned from waiting to active
                if (wasWaitingForGame)
                {
                    UpdateTrayText();
                }
                return;
            }
            
            // Only try to restore if we've seen the game in fullscreen at least once
            if (!hasBeenFullscreenOnce)
            {
                return; // Game is still loading/starting up
            }

            // Game has been fullscreen before and is now windowed - restore it
            TimeSpan timeSinceLastRestore = DateTime.Now - lastRestoreTime;
            if (timeSinceLastRestore.TotalSeconds >= 2.0) // 2 second cooldown
            {
                SendAltEnter(hwnd);
                config.RestoreCount++;
                config.Save(configPath);
                UpdateTrayText();
                lastRestoreTime = DateTime.Now;
            }
        }

        private void ToggleActive(object? sender, EventArgs e)
        {
            active = !active;
            // Reset the fullscreen detection when toggling
            if (active)
            {
                hasBeenFullscreenOnce = false;
            }
            UpdateTrayText();
        }

        private void ResetCounter(object? sender, EventArgs e)
        {
            config.RestoreCount = 0;
            hasBeenFullscreenOnce = false;
            config.Save(configPath);
            UpdateTrayText();
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
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        const int GWL_STYLE = -16;
        const int WS_CAPTION = 0x00C00000;
        const int WS_THICKFRAME = 0x00040000;

        static IntPtr FindWindowByTitle(string title)
        {
            return FindWindow(null, title);
        }

        static bool IsWindowed(IntPtr hwnd)
        {
            int style = GetWindowLong(hwnd, GWL_STYLE);
            
            // Check if it has window decorations (title bar, borders)
            bool hasDecorations = (style & (WS_CAPTION | WS_THICKFRAME)) != 0;
            
            // Also check if the window covers the entire screen
            if (GetWindowRect(hwnd, out RECT rect))
            {
                int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                
                bool coversFullScreen = (rect.Left <= 0 && rect.Top <= 0 && 
                                       rect.Right >= screenWidth && rect.Bottom >= screenHeight);
                
                // If it has decorations OR doesn't cover full screen, it's windowed
                return hasDecorations || !coversFullScreen;
            }
            
            // Fallback to just checking decorations
            return hasDecorations;
        }

        static void SendAltEnter(IntPtr hwnd)
        {
            SetForegroundWindow(hwnd);
            SendKeys.SendWait("%{ENTER}"); // % is ALT in SendKeys
        }
    }
}