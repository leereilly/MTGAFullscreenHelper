using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

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
            this.Text = "About MTGA Fullscreen Helper";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Main panel
            var mainPanel = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                RowCount = 7,
                ColumnCount = 1,
                Padding = new Padding(30),
                MinimumSize = new Size(350, 0)
            };

            // App title
            var titleLabel = new Label
            {
                Text = "MTGA Fullscreen Helper",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 10),
                ForeColor = Color.FromArgb(0, 120, 215) // Windows blue
            };

            // Version
            var versionLabel = new Label
            {
                Text = "Version 0.0.2",
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 5)
            };

            // Description
            var descLabel = new Label
            {
                Text = "Automatically keeps Magic: The Gathering Arena in fullscreen mode",
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                MaximumSize = new Size(320, 0),
                Margin = new Padding(0, 5, 0, 10)
            };

            // Statistics
            var statsLabel = new Label
            {
                Text = $"Fullscreen restorations: {restoreCount}",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 10),
                ForeColor = Color.FromArgb(16, 124, 16) // Green
            };

            // Hackathon callout
            var hackathonPanel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0, 15, 0, 15),
                BackColor = Color.FromArgb(248, 249, 250), // Light gray background
                BorderStyle = BorderStyle.FixedSingle
            };

            var hackathonTitle = new Label
            {
                Text = "🚀 Made for For the Love of Code Hackathon",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Location = new Point(10, 8),
                ForeColor = Color.FromArgb(106, 57, 175) // Purple
            };

            var hackathonLink = new LinkLabel
            {
                Text = "gh.io/ftloc",
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Location = new Point(10, 28),
                LinkColor = Color.FromArgb(106, 57, 175)
            };
            hackathonLink.Click += (s, e) => Process.Start(new ProcessStartInfo
            {
                FileName = "https://gh.io/ftloc",
                UseShellExecute = true
            });

            var vibeCodedLabel = new Label
            {
                Text = "Vibe-coded with GitHub Copilot & Claude Sonnet 4 on the world's biggest mega ultra widescreen 🖥️✨",
                Font = new Font("Segoe UI", 8),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Location = new Point(10, 48),
                ForeColor = Color.FromArgb(87, 96, 106) // Gray
            };

            hackathonPanel.Controls.Add(hackathonTitle);
            hackathonPanel.Controls.Add(hackathonLink);
            hackathonPanel.Controls.Add(vibeCodedLabel);

            // Set panel size to fit content with padding
            hackathonPanel.Width = Math.Max(hackathonTitle.Width, 
                                          Math.Max(hackathonLink.Width, vibeCodedLabel.Width)) + 20;
            hackathonPanel.Height = 70;

            // Author and repository info
            var infoPanel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0, 10, 0, 10)
            };

            var authorLabel = new Label
            {
                Text = "Created by Lee Reilly",
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var repoLink = new LinkLabel
            {
                Text = "github.com/leereilly/MTGAFullscreenHelper",
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Location = new Point(0, 25),
                LinkColor = Color.FromArgb(0, 120, 215)
            };
            repoLink.Click += (s, e) => Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/leereilly/MTGAFullscreenHelper",
                UseShellExecute = true
            });

            // Center the labels within the info panel
            authorLabel.Left = (infoPanel.Width - authorLabel.Width) / 2;
            repoLink.Left = (infoPanel.Width - repoLink.Width) / 2;
            
            infoPanel.Controls.Add(authorLabel);
            infoPanel.Controls.Add(repoLink);
            infoPanel.Height = 50;

            // OK button
            var okButton = new Button
            {
                Text = "OK",
                Size = new Size(75, 30),
                DialogResult = DialogResult.OK,
                Margin = new Padding(0, 15, 0, 10)
            };
            okButton.Click += (s, e) => this.Close();

            var buttonPanel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            okButton.Location = new Point(0, 0);
            buttonPanel.Controls.Add(okButton);
            buttonPanel.Width = okButton.Width;
            buttonPanel.Height = okButton.Height;

            // Add all controls to main panel
            mainPanel.Controls.Add(titleLabel, 0, 0);
            mainPanel.Controls.Add(versionLabel, 0, 1);
            mainPanel.Controls.Add(descLabel, 0, 2);
            mainPanel.Controls.Add(statsLabel, 0, 3);
            mainPanel.Controls.Add(hackathonPanel, 0, 4);
            mainPanel.Controls.Add(infoPanel, 0, 5);
            mainPanel.Controls.Add(buttonPanel, 0, 6);

            // Set row styles to auto-size
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Center all items in their cells
            mainPanel.SetCellPosition(titleLabel, new TableLayoutPanelCellPosition(0, 0));
            mainPanel.SetCellPosition(versionLabel, new TableLayoutPanelCellPosition(0, 1));
            mainPanel.SetCellPosition(descLabel, new TableLayoutPanelCellPosition(0, 2));
            mainPanel.SetCellPosition(statsLabel, new TableLayoutPanelCellPosition(0, 3));
            mainPanel.SetCellPosition(hackathonPanel, new TableLayoutPanelCellPosition(0, 4));
            mainPanel.SetCellPosition(infoPanel, new TableLayoutPanelCellPosition(0, 5));
            mainPanel.SetCellPosition(buttonPanel, new TableLayoutPanelCellPosition(0, 6));

            this.Controls.Add(mainPanel);
            this.AcceptButton = okButton;
        }
    }
}
