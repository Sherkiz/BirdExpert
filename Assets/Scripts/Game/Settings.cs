namespace BirdExpert
{
    public class Settings
    {
        public Languages language;
        public float audioVolume;

        public Settings(Languages language = Languages.English, float audioVolume = 0.7f)
        {
            this.language = language;
            this.audioVolume = audioVolume;
        }
    }
}
