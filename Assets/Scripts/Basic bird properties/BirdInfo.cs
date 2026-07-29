using System;
using System.Linq;
using UnityEngine;

namespace BirdExpert
{
    public struct BirdImage
    {
        public Sprite image;
        public Sex sex;
        public GameSettings.ImageSettings imageType;
        public BirdImage(Sprite image, Sex sex)
        {
            this.image = image;
            this.sex = sex;
            this.imageType = GameSettings.ImageSettings.Base;
        }
    }
    public struct BirdSound
    {
        public AudioClip sound;
        public SoundType type;

        public BirdSound(AudioClip sound, SoundType type)
        {
            this.sound = sound;
            this.type = type;
        }
    }

    public struct MorphoCaracteristics
    {
        public float length;
        public float weight;
        public MorphoCaracteristics(float length, float weight)
        {
            this.length = length;
            this.weight = weight;
        }
    }

    public struct ClassifInfos
    {
        public string order;
        public string family;
        public ClassifInfos(string order, string family)
        {
            this.order = order;
            this.family = family;
        }
    }
    public class BirdInfo
    {
        private BirdBaseData baseData;
        public bool hasSexualDimorphism;

        public BirdInfo(BirdBaseData baseData)
        {
            this.baseData = baseData;
        }
        public void AddImage(BirdImage image)
        {
            baseData.AddImage(image);
        }

        public void AddSound(BirdSound sound)
        {
            baseData.AddSound(sound);
        }
        public string GetName(Lang lang)
        {
            return baseData.GetName(lang);
        }

        public string spCode { get => baseData.code_sp; } 
        public MorphoCaracteristics Morpho { get => baseData.Morpho; }
        public ClassifInfos Classif { get => baseData.Classif;}
        public bool NeedsSound { get => baseData.difficult_visual_identification; }
        public bool NeedsImage { get => baseData.difficult_sound_identification; }
        public string[] AllHabitatsNames { get => baseData.habitat; } 
        public string HabitatName(int index) { return baseData.GetHabitatName(index); }
        public string[] AllTrophicNiches { get => baseData.food; } 
        public string TrophicNiche(int index) { return baseData.GetTrophicNiche(index); }
        public bool IsInHabitat(string habitat) { return AllHabitatsNames.Contains(habitat); }
        public bool IsTrophicNiche(string food) { return AllTrophicNiches.Contains(food); }

        public int Rarity { get => baseData.rarity; }

        private BirdImage[] AllImages {get => baseData.allImages; }
        private BirdImage GetImage(int index) { return baseData.allImages[index]; }
        public BirdImage GetRandomImage(GameSettings.ImageSettings imageType, Sex sex = Sex.All)
        {
            BirdImage[] selectedImages;
            if (sex == Sex.All) { selectedImages = AllImages; }
            else { selectedImages = AllImages.Where(image => image.sex == sex).ToArray(); }
            selectedImages = AllImages.Where(image => image.imageType == imageType).ToArray();
            if (selectedImages.Length == 0) { return new(null, Sex.None); }
            if (selectedImages.Length == 1) return selectedImages[0];
            else { return selectedImages[UnityEngine.Random.Range(0, selectedImages.Length)]; }
        }

        private bool HasImageOfSex(Sex sex)
        {
            if (sex == Sex.All) { return AllImages.Length > 0; }
            else { return Array.Exists(AllImages, image => image.sex == sex); }
        }

        public BirdSound[] AllSounds { get => baseData.allSounds; }
        public BirdSound[] GetAllAudioClips(SoundType type = SoundType.AllSounds)
        {
            if (type == SoundType.AllSounds) return AllSounds;
            else return AllSounds.Where(sound => sound.type == type).ToArray();
        }
        public BirdSound GetAudioClip(int index) { return AllSounds[index]; }

        public BirdSound GetRandomSound(SoundType type = SoundType.AllSounds)
        {
            BirdSound[] selectedSounds;
            if (type == SoundType.AllSounds) { selectedSounds = AllSounds; }
            else { selectedSounds = AllSounds.Where(sound => sound.type == type).ToArray(); }
            if (selectedSounds.Length == 0) { return new(null, SoundType.None); }
            else { return selectedSounds[UnityEngine.Random.Range(0, selectedSounds.Length)]; }
        }
        private bool HasSoundOfType(SoundType type)
        {
            if (type == SoundType.AllSounds) { return AllSounds.Length > 0; }
            else { return Array.Exists(AllSounds, sound => sound.type == type); }
        }

        public BirdSound GetRandomSound(SoundType typePriority = SoundType.Song, bool findAnyway = true)
        {
            if (!HasSoundOfType(typePriority))
            {
                if (!findAnyway) return new();
                else return GetRandomSound(type: SoundType.AllSounds);
            }
            return GetRandomSound(typePriority);
        }

        public void Initialize() 
        { 
            baseData.Initialize();
            hasSexualDimorphism = (HasImageOfSex(Sex.Male) || HasImageOfSex(Sex.Female));
        }
    }
}
