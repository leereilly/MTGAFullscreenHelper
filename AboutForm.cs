using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D; // added for gradient

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
                CreateHeading("Session Stats"),
                CreateMetricLabel($"Fullscreen restorations this run: {restoreCount}"),
                CreateSmallLabel($"≈ {joulesSpent:0.#} J spent · ≈ {secondsSaved:0.##} sec saved")
            }));

            // Hackathon + tooling card
            contentPanel.Controls.Add(CreateCard(new Control[]
            {
                CreateHeading("Built For"),
                CreateAccentLabel("🚀 For the Love of Code Hackathon"),
                CreateLinkLabel("gh.io/ftloc", "https://gh.io/ftloc"),
                CreateSmallLabel("Vibe‑coded with GitHub Copilot & Claude Sonnet 4 on an absurdly wide display 🖥️✨")
            }));

            // Links & credits card
            contentPanel.Controls.Add(CreateCard(new Control[]
            {
                CreateHeading("Links"),
                CreateLinkLabel("Source Repository", "https://github.com/leereilly/MTGAFullscreenHelper"),
                CreateLinkLabel("Report an Issue", "https://github.com/leereilly/MTGAFullscreenHelper/issues"),
                CreateLinkLabel("Buy me a coffee ☕", "https://buymeacoffee.com/leereilly")
            }));

            // Footer credits
            contentPanel.Controls.Add(CreateFooterLabel("Created by Lee Reilly · MIT Licensed"));

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
            Font = new Font("Segoe UI", 9),
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
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            ForeColor = Color.FromArgb(106, 57, 175),
            AutoSize = true,
            Margin = new Padding(0, 4, 0, 2)
        };

        private static Label CreateSmallLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 8, FontStyle.Italic),
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
                Font = new Font("Segoe UI", 9, FontStyle.Underline),
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

        private static Label CreateFooterLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 8, FontStyle.Regular),
            ForeColor = Color.FromArgb(120, 120, 130),
            AutoSize = true,
            Margin = new Padding(0, 18, 0, 4)
        };

        private static Panel CreateCard(Control[] children)
        {
            var panel = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(14, 12, 14, 12),
                Margin = new Padding(0, 0, 0, 12),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
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
                Dock = DockStyle.Fill
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
