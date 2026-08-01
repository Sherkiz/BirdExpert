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
        private List<string> selectedBirdsCodes;
        public BirdInfo GetBirdFromCode(string spCode)
        {
            return allBirds[spCode];
        }
        public BirdInfo GetBirdFromLang(Lang lang, string name)
        {
            BirdInfo bird = allBirds.Values.ToList().Find(bird => bird.GetName(lang) == name);
            if (bird == null)
            {
                Debug.LogWarning("Bird " + name + " (in language " + lang + ") was not found !");
            }
            return bird;
        }
        public BirdInfo GetRandomBird()
        {
            if (goodBirdsCodes.Count == 0)
            {
                goodBirdsCodes = selectedBirdsCodes;
                Debug.Log("Used all birds for this quiz, reinitializing selection");
            }
            string birdCode = goodBirdsCodes[Random.Range(0, goodBirdsCodes.Count())];
            return GetBirdFromCode(birdCode);
        }
        public void AddBirdToSkip(BirdInfo bird) => goodBirdsCodes.Remove(bird.spCode);
        public void InitGoodBirds() 
        {
            selectedBirdsCodes = spCodesList; // Modify to include predicates based on game mode choice
            goodBirdsCodes = selectedBirdsCodes;
        }
    }
}
