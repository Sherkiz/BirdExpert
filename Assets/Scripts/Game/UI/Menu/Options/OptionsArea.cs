using UnityEngine;

namespace BirdExpert
{
    public class OptionsArea : UIAreaMenu
    {
        [SerializeField] private OptionsFieldManager optionsFieldManager;
        [SerializeField] private GameModeLoadingPanel loadingPanel;
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            optionsFieldManager.Initialize(true, menuManager);
            loadingPanel.OptionsArea = this;
            loadingPanel.Initialize(true, menuManager);
        }
        public override void OpenArea()
        {
            base.OpenArea();
            optionsFieldManager.ActualizeArea();
            loadingPanel.CloseArea();
        }
        public void LoadGameMode(GameMode gameMode) => optionsFieldManager.LoadGameMode(gameMode);
        public void AddGameMode(GameMode gameMode) => loadingPanel.AddButton(gameMode);
        public void RemoveGameMode(GameMode gameMode) => loadingPanel.DeleteGameMode(gameMode);
        public void ResetArea() => optionsFieldManager.ResetArea();
    }
}