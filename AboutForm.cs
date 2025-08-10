using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MTGAFullscreenHelper
{
    public partial class AboutForm : Form
    {
        private readonly int restoreCount;

        public AboutForm(int restoreCount)
        {
            this.restoreCount = restoreCount;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Basic form settings
            Text = "About MTGA Fullscreen Helper";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.FromArgb(250, 251, 252);

            var version = typeof(AboutForm).Assembly.GetName().Version?.ToString() ?? "(unknown)";
            // Derived fun stats
            var joulesSpent = 1.5 * restoreCount; // J
            var secondsSaved = 0.42 * restoreCount; // s
            
            // Format energy units
            string energyText = joulesSpent >= 1000 
                ? $"{joulesSpent / 1000:0.##} kilojoules" 
                : $"{joulesSpent:0.#} joules";
            
            // Format time units
            string timeText;
            if (secondsSaved >= 3600) // 1 hour or more
            {
                var hours = (int)(secondsSaved / 3600);
                var minutes = (int)((secondsSaved % 3600) / 60);
                var secs = (int)(secondsSaved % 60);
                timeText = hours > 0 && minutes > 0 
                    ? $"{hours}h {minutes}m {secs}s"
                    : hours > 0 
                        ? $"{hours}h {secs}s"
                        : $"{minutes}m {secs}s";
            }
            else if (secondsSaved >= 60) // 1 minute or more
            {
                var minutes = (int)(secondsSaved / 60);
                var secs = (int)(secondsSaved % 60);
                timeText = $"{minutes}m {secs}s";
            }
            else
            {
                timeText = $"{secondsSaved:0.##} seconds";
            }

            // Header panel with gradient
            var headerPanel = new Panel
            {
                Height = 110,
                Dock = DockStyle.Top,
                Padding = new Padding(15, 12, 15, 12)
            };
            headerPanel.Paint += (s, e) =>
            {
                using var brush = new LinearGradientBrush(headerPanel.ClientRectangle,
                    Color.FromArgb(88, 48, 136), Color.FromArgb(30, 30, 48), LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
            };

            // App logo (PNG)
            var logoPath = System.IO.Path.Combine(AppContext.BaseDirectory, "MTGAFullScreenHelper.png");
            Image? logo = null;
            if (System.IO.File.Exists(logoPath))
            {
                try { logo = Image.FromFile(logoPath); } catch { logo = null; }
            }

            var iconBox = new PictureBox
            {
                Image = logo ?? SystemIcons.Application.ToBitmap(),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(64, 64),
                Location = new Point(10, 18),
                BackColor = Color.Transparent
            };

            var titleLabel = new Label
            {
                Text = "MTGA Fullscreen Helper",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(90, 18),
                BackColor = Color.Transparent
            };

            var subtitleLabel = new Label
            {
                Text = $"Version {version}",
                ForeColor = Color.Gainsboro,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(93, 54),
                BackColor = Color.Transparent
            };

            headerPanel.Controls.Add(iconBox);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(subtitleLabel);

            // Main content area (scroll-safe auto layout panel)
            var contentPanel = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 1,
            };
            contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Description / purpose card
            contentPanel.Controls.Add(CreateCard(new Control[]
            {
                CreateHeading("Purpose"),
                CreateTextLabel("Automatically keeps Magic: The Gathering Arena in true fullscreen by detecting windowed fallbacks and issuing ALT+ENTER only when needed.")
            }));

            // Stats card
            contentPanel.Controls.Add(CreateCard(new Control[]
            {
                CreateHeading("Usage"),
                CreateMetricLabel($"Fullscreen restorations this run: {restoreCount}"),
                CreateSmallLabel($"You've saved {energyText} and {timeText} from unnecessary ALT and ENTER keystrokes.")
            }));

            // Support card
            contentPanel.Controls.Add(CreateCard(new Control[]
            {
                CreateHeading("Support"),
                CreateSmallLabel("It all adds up! If this saved you a lot of heartache you can show your appreciation:"),
                CreateCoffeeButton(),
                CreateSmallLabel("(I'll likely use the money to buy more Magic cards though)")
            }));

            // Links & credits card
            contentPanel.Controls.Add(CreateCard(new Control[]
            {
                CreateHeading("Links"),
                CreateLinkLabel("Source Repository", "https://github.com/leereilly/MTGAFullscreenHelper"),
                CreateLinkLabel("Report an Issue", "https://github.com/leereilly/MTGAFullscreenHelper/issues")
            }));

            // Footer credits
            contentPanel.Controls.Add(CreateFooterWithLink("Built with ♥ by Lee Reilly, GitHub Copilot, and Claude Sonnet 4 ", "#ForTheLoveOfCode", "https://gh.io/ftloc", ""));

            // Buttons panel
            var buttonsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                Padding = new Padding(15, 5, 15, 15),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            var closeButton = new Button
            {
                Text = "Close",
                DialogResult = DialogResult.OK,
                AutoSize = true,
                FlatStyle = FlatStyle.System
            };
            closeButton.Click += (s, e) => Close();
            buttonsPanel.Controls.Add(closeButton);
            AcceptButton = closeButton;

            // Root container
            var root = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill
            };
            root.Controls.Add(contentPanel);
            root.Controls.Add(buttonsPanel);
            Controls.Add(root);
            Controls.Add(headerPanel);
        }

        // --- UI helper factory methods ---
        private static Label CreateHeading(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 30),
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 6)
        };

        private static Label CreateTextLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 10), // Increased from 9 to 10
            ForeColor = Color.FromArgb(55, 60, 65),
            AutoSize = true,
            MaximumSize = new Size(480, 0),
            Margin = new Padding(0, 0, 0, 4)
        };

        private static Label CreateMetricLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(16, 124, 16),
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 2)
        };

        private static Label CreateAccentLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 10, FontStyle.Bold), // Increased from 9 to 10
            ForeColor = Color.FromArgb(106, 57, 175),
            AutoSize = true,
            Margin = new Padding(0, 4, 0, 2)
        };

        private static Label CreateSmallLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 10), // Changed from 9 to 10, removed italics
            ForeColor = Color.FromArgb(87, 96, 106),
            AutoSize = true,
            MaximumSize = new Size(480, 0),
            Margin = new Padding(0, 0, 0, 4)
        };

        private static LinkLabel CreateLinkLabel(string text, string url)
        {
            var link = new LinkLabel
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Underline), // Increased from 9 to 10
                LinkColor = Color.FromArgb(0, 102, 204),
                ActiveLinkColor = Color.FromArgb(10, 132, 255),
                Margin = new Padding(0, 0, 0, 3)
            };
            link.Click += (s, e) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                }
                catch { }
            };
            return link;
        }

        private static Button CreateCoffeeButton()
        {
            var button = new Button
            {
                Text = "☕ Buy me a coffee",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                BackColor = Color.FromArgb(255, 221, 87), // Coffee-like yellow color
                ForeColor = Color.FromArgb(52, 35, 19), // Dark brown text
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 4, 0, 4),
                Padding = new Padding(12, 6, 12, 6),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderColor = Color.FromArgb(210, 180, 50);
            button.FlatAppearance.BorderSize = 1;
            
            button.Click += (s, e) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = "https://buymeacoffee.com/leereilly", UseShellExecute = true });
                }
                catch { }
            };
            
            // Add hover effects
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(255, 235, 120);
            button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(255, 221, 87);
            
            return button;
        }

        private static Label CreateFooterLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 8, FontStyle.Regular),
            ForeColor = Color.FromArgb(120, 120, 130),
            AutoSize = true,
            Margin = new Padding(0, 18, 0, 4)
        };

        private static Panel CreateInlineTextWithLink(string beforeText, string linkText, string url)
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0, 0, 0, 4)
            };

            var beforeLabel = new Label
            {
                Text = beforeText,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(87, 96, 106),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 0)
            };

            var link = new LinkLabel
            {
                Text = linkText,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Underline),
                LinkColor = Color.FromArgb(0, 102, 204),
                ActiveLinkColor = Color.FromArgb(10, 132, 255),
                Margin = new Padding(0, 0, 0, 0)
            };
            link.Click += (s, e) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                }
                catch { }
            };

            panel.Controls.Add(beforeLabel);
            panel.Controls.Add(link);
            return panel;
        }

        private static Panel CreateFooterWithLink(string beforeText, string linkText, string url, string afterText)
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0, 18, 0, 4),
                Anchor = AnchorStyles.None // Center the panel
            };

            var beforeLabel = new Label
            {
                Text = beforeText,
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = Color.FromArgb(120, 120, 130),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var link = new LinkLabel
            {
                Text = linkText,
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Underline),
                LinkColor = Color.FromArgb(120, 120, 130),
                ActiveLinkColor = Color.FromArgb(10, 132, 255),
                Margin = new Padding(0, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            link.Click += (s, e) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                }
                catch { }
            };

            panel.Controls.Add(beforeLabel);
            panel.Controls.Add(link);
            
            // Only add afterText if it's not empty
            if (!string.IsNullOrEmpty(afterText))
            {
                var afterLabel = new Label
                {
                    Text = afterText,
                    Font = new Font("Segoe UI", 8, FontStyle.Regular),
                    ForeColor = Color.FromArgb(120, 120, 130),
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 0),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                panel.Controls.Add(afterLabel);
            }
            
            return panel;
        }

        private static Panel CreateCard(Control[] children)
        {
            var panel = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(14, 12, 14, 12),
                Margin = new Padding(0, 0, 0, 12),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Width = 480, // Set consistent width for all cards
                MinimumSize = new Size(480, 0),
                MaximumSize = new Size(480, 0) // Force maximum width to prevent expansion
            };
            panel.Paint += (s, e) =>
            {
                var r = panel.ClientRectangle;
                r.Width -= 1; r.Height -= 1;
                using var path = RoundedRect(r, 10);
                using var shadow = new Pen(Color.FromArgb(220, 230, 240));
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(shadow, path);
            };

            var layout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill,
                Width = 452, // Account for padding (480 - 28)
                MaximumSize = new Size(452, 0) // Constrain inner layout width
            };
            foreach (var c in children)
            {
                layout.Controls.Add(c);
            }
            panel.Controls.Add(layout);
            return panel;
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
