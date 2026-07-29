using UnityEngine;

namespace BirdExpert
{
    public class BottomPanel : UIAreaMenu
    {
        [SerializeField] private GamemodeNamesDisplayer namesDisplayer;
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            namesDisplayer.Initialize(false, menuManager);
        }
        public void AddGameMode(GameMode gameMode) => namesDisplayer.AddButton(gameMode);
        public void RemoveGameMode(GameMode gameMode) => namesDisplayer.RemoveButton(gameMode);
    }
}
