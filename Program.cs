using System;
using System.Windows.Forms;

namespace MTGAFullscreenHelper
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string configPath = "config.json";
            var config = Config.Load(configPath);
            Application.Run(new TrayApp(config, configPath));
        }
    }
}