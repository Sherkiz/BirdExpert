using System.Collections;
using System.Collections.Generic;

namespace BirdExpert
{
    public class Names
    {
        public static List<string> latinNames = new List<string>();
        public static List<string> frenchNames = new List<string>();
        public static List<string> englishNames = new List<string>();
        public static List<string> codeNames;

        public static void RegisterBird(BirdInfo bird)
        {
            if (bird != null)
            {
                latinNames.Add(bird.GetName(Lang.Latin));
                frenchNames.Add(bird.GetName(Lang.French));
                englishNames.Add(bird.GetName(Lang.English));
            }
        }
    }
}
