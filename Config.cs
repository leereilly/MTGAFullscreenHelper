using System;
using System.IO;
using System.Text.Json;

namespace MTGAFullscreenHelper
{
    public class Config
    {
        public string WindowTitle { get; set; } = "Magic The Gatherin";
        public string Executable { get; set; } = "mtga.exe";
        public int CheckIntervalMs { get; set; } = 1000;
        public int RestoreCount { get; set; } = 0;

        public static Config Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    var defaultConfig = new Config();
                    defaultConfig.Save(path);
                    return defaultConfig;
                }

                var json = File.ReadAllText(path);
                var config = JsonSerializer.Deserialize<Config>(json);
                return config ?? new Config();
            }
            catch (Exception)
            {
                return new Config();
            }
        }

        public void Save(string path)
        {
            try
            {
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(path, json);
            }
            catch (Exception)
            {
                // Silently fail if we can't save config
            }
        }
    }
}