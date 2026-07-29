using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace BirdExpert
{
    public enum Languages
    {
        None,
        English,
        French
    }
    public static class Language
    {
        private static Languages language;
        private static Dictionary<Languages,Dictionary<string, string>> allLanguages = new();
        private static Dictionary<string, string> currentLanguage;
        private static string languagePath = Application.dataPath + "/StreamingAssets/Data/Languages/";
        public static Action OnLanguageChanged;
        public static Languages CurrentLanguage { get => language; }
        public static void SetLanguage(Languages language)
        {
            Language.language = language;
            currentLanguage = allLanguages[language];
            OnLanguageChanged?.Invoke();
        }
        public static string GetLang(string code)
        {
            string word = "";
            if(!currentLanguage.TryGetValue(code, out word)) Debug.LogWarning("Code " + code + " not found in language " + currentLanguage);
            return word;
        }
        private static void LoadLanguage(Languages lang)
        {
            if (lang == Languages.None) return;
            string filename = languagePath + lang.ToString() + ".json";
            if (!File.Exists(filename))
            {
                Debug.LogWarning("Language " + lang.ToString() + " not found at " + filename);
                return;
            }
            string json = File.ReadAllText(filename);
            allLanguages[lang] = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
        } 
        public static void LoadAllLanguages()
        {
            foreach(var lang in Enum.GetValues(typeof(Languages)))
            {
                LoadLanguage((Languages) lang);
            }
        }
    }
}
