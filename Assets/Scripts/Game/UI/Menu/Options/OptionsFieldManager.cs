using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class OptionsFieldManager : UIAreaMenu
    {
        [SerializeField] private List<UIOpenablePanel> panels;
        [SerializeField] private InfoText infoText;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button playButton;
        [SerializeField] private ConfirmationBox confirmationBox;
        private GameMode gameMode { get => GameManager.Instance.gameMode; set => GameManager.Instance.gameMode = value; }
        public GameMode currentModifiedGameMode { get; set; }
        private OptionsField[] optionsFields;
        private ScrollRect scrollRect;
        private UIOpenablePanel activePanel 
        { 
            get 
            {
                return panels.Find(panel => panel.IsOpen);
            } 
        }
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            currentModifiedGameMode = new GameMode();
            optionsFields = GetComponentsInChildren<OptionsField>(includeInactive:true);
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.content = activePanel.AreaRect;
            InitializeArea();
        }
        public override void OpenArea()
        {
            base.OpenArea();
            ActualizeArea();
        }
        public void ActualizeArea()
        {
            foreach (var option in optionsFields) option.Actualize();
        }
        private void InitializeArea()
        {
            foreach (var option in optionsFields)
            {
                option.FieldManager = this;
                option.OnSettingChanged.AddListener(CheckSettings);
            }
            foreach (var option in optionsFields) option.Initialize();
            foreach (var panel in panels) panel.button.onClick.AddListener(() => OpenPanel(panel));
            OpenPanel(panels[0]);
            infoText.Initialize();
            infoText.transform.SetAsLastSibling();
        }
        private void OpenPanel(UIOpenablePanel panel)
        {
            panel.OpenArea();
            panels.Remove(panel);
            foreach(var pan in  panels) pan.CloseArea();
            panels.Add(panel);
            scrollRect.content = activePanel.AreaRect;
        }
        public void PlayGameMode()
        {
            gameMode = currentModifiedGameMode;
            menuManager.PlayGame(gameMode);
        }
        public void SaveGameModeConfirmation()
        {
            if (GameModesLoader.gameModesByName.ContainsKey(currentModifiedGameMode.name))
            {
                if (GameModesLoader.gameModesByName[currentModifiedGameMode.name].Equals(currentModifiedGameMode)) return;
                confirmationBox.Open("gamemode-name-exists");
                confirmationBox.AddConfirmationAction(SaveGameMode);
            }
            else if (GameModesLoader.gameModesByName.Values.Any(gm => gm.Equals(currentModifiedGameMode)))
            {
                GameMode gameMode = GameModesLoader.gameModesByName.Values.Where(gm => gm.Equals(currentModifiedGameMode)).First();
                confirmationBox.Open("gamemode-content-exists");
                Debug.Log("Should rename " +  gameMode.name + " into " + currentModifiedGameMode.name);
                confirmationBox.AddConfirmationAction(() => RenameGameMode(gameMode, currentModifiedGameMode));
            }
            else
            {
                SaveNewGameMode();
            }
        }
        private void SaveGameMode()
        {
            GameManager.Instance.gameModesLoader.SaveGameMode(currentModifiedGameMode);
            currentModifiedGameMode = currentModifiedGameMode.Clone();
        }
        private void SaveNewGameMode()
        {
            menuManager.AddGameMode(currentModifiedGameMode);
            SaveGameMode();
        }
        private void RenameGameMode(GameMode oldGameMode, GameMode newGameMode)
        {
            menuManager.RemoveGameMode(oldGameMode);
            menuManager.AddGameMode(newGameMode);
            GameManager.Instance.gameModesLoader.RenameGameMode(oldGameMode, newGameMode);
            currentModifiedGameMode = currentModifiedGameMode.Clone();
        }
        public void CloseGameModesCreationConfirmation()
        {
            if (currentModifiedGameMode == new GameMode() && currentModifiedGameMode.name == "New Game Mode")
            {
                CloseGameModesCreation();
                return;
            }
            if (!AllSettingsValid())
            {
                confirmationBox.Open("gamemode-notvalid");
                confirmationBox.AddCancelAction(CloseGameModesCreation);
                confirmationBox.AddConfirmationAction(confirmationBox.Close);
                return;
            }
            bool isExistingName = GameModesLoader.gameModesByName.TryGetValue(currentModifiedGameMode.name, out GameMode savedGameMode);
            if (isExistingName)
            {
                if (!savedGameMode.Equals(currentModifiedGameMode))
                {
                    confirmationBox.Open("gamemode-notsaved");
                    confirmationBox.AddConfirmationAction(SaveGameModeConfirmation);
                    confirmationBox.AddCancelAction(CloseGameModesCreation);
                    return;
                }
            }
            else
            {
                confirmationBox.Open("gamemode-notsaved");
                confirmationBox.AddConfirmationAction(SaveGameModeConfirmation);
                confirmationBox.AddCancelAction(CloseGameModesCreation);
                return;
            }
            CloseGameModesCreation();
        }
        private void CloseGameModesCreation()
        {
            menuManager.CloseGameModesCreation();
        }
        public void SetInfoText(string text) => infoText.OpenAtMousePosition(text);
        public void CloseInfoText() => infoText.Close();
        private void CheckSettings()
        {
            bool valid = AllSettingsValid();
            saveButton.interactable = valid;
            playButton.interactable = valid;
        }
        private bool AllSettingsValid()
        {
            foreach (var option in optionsFields)
            {
                if (!option.CheckValid()) return false;
            }
            return true;
        }
        public void LoadGameMode(GameMode gameMode)
        {
            currentModifiedGameMode = gameMode.Clone();
            ActualizeArea();   
        }
        public void ResetArea()
        {
            currentModifiedGameMode = new();
            ActualizeArea();
        }
    }
}
