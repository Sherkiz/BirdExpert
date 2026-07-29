
namespace BirdExpert
{
    public class Favorite : CustomToggle
    {
        public override void Actualize()
        {
            base.Actualize();
            toggle.isOn = settings.isFavorite;
        }

        protected override void ToggleUsed(bool value)
        {
            settings.isFavorite = value;
            base.ToggleUsed(value);
        }
    }
}
