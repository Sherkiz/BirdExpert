namespace BirdExpert
{
    public enum Sex
    {
        Male,
        Female,
        None,
        All
    }
    public enum SoundType
    {
        Song,
        Alarm,
        None,
        AllSounds
    }
    public static class EnumUtilities
    {
        public static Sex GetOppositeSex(Sex sex)
        {
            return sex switch
            {
                Sex.Male => Sex.Female,
                Sex.Female => Sex.Male,
                Sex.None => Sex.All,
                Sex.All => Sex.All,
                _ => Sex.None,
            };
        }
    }
}
