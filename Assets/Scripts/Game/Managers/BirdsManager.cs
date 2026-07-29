using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BirdExpert
{
    public class BirdsManager
    {
        public static Dictionary<string, BirdInfo> allBirds;
        public static List<string> spCodesList { get => allBirds.Keys.ToList(); }
        public static List<string> GetAllNamesInLang(Lang lang) => allBirds.Values.Select(bird => bird.GetName(lang)).ToList();
        private List<string> goodBirdsCodes;
        public BirdInfo GetBirdFromCode(string spCode)
        {
            return allBirds[spCode];
        }
        public BirdInfo GetBirdFromLang(Lang lang, string name)
        {
            return allBirds.Values.ToList().Find(bird => bird.GetName(lang) == name);
        }
        public BirdInfo GetRandomBird(bool skipBirds = true)
        {
            string birdCode = goodBirdsCodes[Random.Range(0, goodBirdsCodes.Count())];
            return GetBirdFromCode(birdCode);
        }
        public void AddBirdToSkip(BirdInfo bird) => goodBirdsCodes.Remove(bird.spCode);
        public void InitGoodBirds() => goodBirdsCodes = spCodesList; // Modify to include predicates based on game mode choice
    }
}
