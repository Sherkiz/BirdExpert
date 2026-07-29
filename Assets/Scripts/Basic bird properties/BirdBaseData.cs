using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BirdExpert
{
    public class BirdBaseData : ScriptableObject
    {
        public string code_sp;
        public string name_lat;
        public string name_fr;
        public string name_en;
        public float length;
        public float weight;
        public string order;
        public string family;
        public string[] habitat;
        public string[] food;
        public bool sedentary;
        public string[] similarPhysicalBirds;
        public string[] similarSoundsBirds;
        public bool difficult_visual_identification;
        public bool difficult_sound_identification;
        public int visual_difficulty_level;
        public int sound_difficulty_level;
        public int rarity;
        public BirdImage[] allImages;
        public BirdSound[] allSounds;

        private List<BirdImage> imagesList;
        private List<BirdSound> soundsList;

        private void Awake()
        {
            imagesList = new();
            soundsList = new();
        }

        public void AddImage(BirdImage image)
        {
            imagesList.Add(image);
        }

        public void AddSound(BirdSound sound)
        {
            soundsList.Add(sound);
        }

        public string GetName(Lang lang)
        {
            switch (lang)
            {
                case Lang.English: return name_en;
                case Lang.French: return name_fr;
                case Lang.Latin: return name_lat;
            }
            return null;
        }

        public MorphoCaracteristics Morpho
        {
            get => new(length, weight);
        }
        public ClassifInfos Classif
        {
            get => new(order, family);
        }

        public string GetHabitatName(int index) { return habitat[index]; }
        public string GetTrophicNiche(int index) { return food[index]; }
        public void Initialize()
        {
            allImages = imagesList.ToArray();
            allSounds = soundsList.ToArray();
            imagesList = null;
            soundsList = null;
        }
    }
}
