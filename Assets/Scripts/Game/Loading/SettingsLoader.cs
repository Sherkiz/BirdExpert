using Newtonsoft.Json;
using UnityEngine;
using System.IO;

namespace BirdExpert
{
    public class SettingsLoader
    {
        private static string savedSettingsPath { get => Application.dataPath + "/StreamingAssets/Data/Saved/Settings/Settings.json"; }
        public static Settings currentSettings { get; private set; }
        public Settings LoadSettings()
        {
            Settings savedSettings = new(Languages.English, 0.7f);
            if (File.Exists(savedSettingsPath))
            {
                string json = File.ReadAllText(savedSettingsPath);
                JsonConvert.PopulateObject(json, savedSettings);
            }
            else
            {
                SaveSettings(savedSettings);
            }
            currentSettings = savedSettings;
            return savedSettings;
        }
        public static void SaveSettings(Settings settings)
        {
            if (!Directory.Exists(Path.GetDirectoryName(savedSettingsPath))) { Directory.CreateDirectory(Path.GetDirectoryName(savedSettingsPath)); }
            string json = JsonConvert.SerializeObject(settings);
            if (File.Exists(savedSettingsPath))
            {
                if (json == File.ReadAllText(savedSettingsPath)) return;
                else File.Delete(savedSettingsPath);
            }
            File.WriteAllText(savedSettingsPath, json);
            currentSettings = settings;
        }
    }
}
