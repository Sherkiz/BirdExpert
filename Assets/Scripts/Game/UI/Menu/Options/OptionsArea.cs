using UnityEngine;

namespace BirdExpert
{
    public class OptionsArea : UIAreaMenu
    {
        [SerializeField] private OptionsFieldManager optionsFieldManager;
        [SerializeField] private GameModeLoadingPanel loadingPanel;
        [SerializeField] private ConfirmationBox confirmationBox;
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            optionsFieldManager.Initialize(active, menuManager);
            loadingPanel.OptionsArea = this;
            loadingPanel.Initialize(active, menuManager);
            confirmationBox.Close();
        }
        public override void OpenArea()
        {
            base.OpenArea();
            optionsFieldManager.OpenArea();
            loadingPanel.CloseArea();
        }
        public void LoadGameMode(GameMode gameMode) => optionsFieldManager.LoadGameMode(gameMode);
        public void AddGameMode(GameMode gameMode) => loadingPanel.AddButton(gameMode);
        public void RemoveGameMode(GameMode gameMode) => loadingPanel.RemoveGameMode(gameMode);
        public void ResetArea() => optionsFieldManager.ResetArea();
    }
}