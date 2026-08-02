using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine.Events;
using System.Linq;

namespace BirdExpert
{
    public class GameModesLoader
    {
        private string savedGameModesPath { get => Application.dataPath + "/StreamingAssets/Data/Saved/GameModes/"; }
        private GameMode gameMode { get => GameManager.Instance.gameMode; set => GameManager.Instance.gameMode = value; }
        public static Dictionary<string, GameMode> gameModesByName { get; private set; }
        public UnityEvent Ready;
        public GameModesLoader()
        {
            Ready = new UnityEvent();
            gameModesByName = new();
            GameManager.Instance.StartCoroutine(LoadAllGameModes());
        }
        private string GetPathFromGameModeName(string name)
        {
            string correctedName = name.Replace(' ', '_');
            return Path.Join(savedGameModesPath, correctedName + ".json");
        }
        private string GetPathFromGameMode(GameMode gameMode) => GetPathFromGameModeName(gameMode.name);
        public void SaveGameMode(GameMode gameMode)
        {
            if (!File.Exists(savedGameModesPath)) { Directory.CreateDirectory(savedGameModesPath); }
            string json = JsonConvert.SerializeObject(gameMode, Formatting.Indented);
            File.WriteAllText(GetPathFromGameMode(gameMode), json);
            gameModesByName[gameMode.name] = gameMode;
        }
        public bool TrySaveGameMode(GameMode gameMode)
        {
            if (!File.Exists(GetPathFromGameMode(gameMode))) { return false; }
            else
            {
                SaveGameMode(gameMode);
                return true;
            }
        }
        public void RenameGameMode(GameMode oldGameMode, GameMode newGameMode)
        {
            TryRemoveGameMode(oldGameMode);
            SaveGameMode(newGameMode);
        }
        public bool TryRemoveGameMode(GameMode gameMode)
        {
            if (!File.Exists(GetPathFromGameMode(gameMode)))
            {
                Debug.LogWarning("Trying to delete game mode " + gameMode.name + " but file was not found.");
                return false; 
            }
            else
            {
                DeleteGameModeFile(gameMode.name);
                gameModesByName.Remove(gameMode.name);
                return true;
            }
        }
        private void DeleteGameModeFile(string gameModeName)
        {
            File.Delete(GetPathFromGameModeName(gameModeName));
#if UNITY_EDITOR
            File.Delete(GetPathFromGameModeName(gameModeName) + ".meta");
#endif 
        }
        private GameMode LoadGameModeFromFile(string filename)
        {
            string json = File.ReadAllText(filename);
            GameMode savedSettings = new();
            JsonConvert.PopulateObject(json, savedSettings);
            return savedSettings;
        }
        public IEnumerator LoadAllGameModes()
        {
            string[] savedSettingsFiles = Directory.GetFiles(savedGameModesPath).Where(file => Path.GetExtension(file) == ".json").ToArray();
            Debug.Log("Found " + savedSettingsFiles.Length + " saved play modes.");
            if (savedSettingsFiles.Length != 0)
            {
                foreach (string file in savedSettingsFiles)
                {
                    string name = (string)JObject.Parse(File.ReadAllText(file))["name"];
                    gameModesByName[name] = LoadGameModeFromFile(file);
                }
            }
            yield return gameModesByName;
            if (gameModesByName.Count > 0) gameMode = gameModesByName.Values.First();
            else gameMode = new GameMode();
            Ready.Invoke();
        }
    }
}
