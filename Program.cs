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

            var config = Config.Load("config.json");
            Application.Run(new TrayApp(config));
        }
    }
}