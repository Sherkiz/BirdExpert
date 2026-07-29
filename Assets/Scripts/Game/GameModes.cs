using System;
using UnityEditor.Build.Pipeline;
using static BirdExpert.GameSettings;

namespace BirdExpert
{
    public class GameMode : IEquatable<GameMode>
    {
        public string name;
        public GameObjective objective;
        public GameObjectiveLimitSettings objectiveLimitSettings;
        public int birdNumberLimit;
        public int timeLimit;
        public DataPresenceSettings soundPresenceSetting;
        public DataPresenceSettings imagePresenceSetting;
        public SoundType soundSetting;
        public ImageSettings imageSetting;
        public AnswerSettings answerSetting;
        public Lang lang;
        public Lang hintLang;
        public bool isFavorite;
        public bool traductionMode;
        public bool allowHabitatHint;
        public bool allowFoodHint;
        public string habitat;
        public int rarityLimit;
        public string family;
        public string order;
        public string food;

        public GameMode()
        {
            name = "New Game Mode";
            objective = GameObjective.NumberedQuizz;
            objectiveLimitSettings = GameObjectiveLimitSettings.Fixed;
            birdNumberLimit = 10;
            timeLimit = 0;
            soundPresenceSetting = DataPresenceSettings.Never;
            imagePresenceSetting = DataPresenceSettings.Always;
            soundSetting = SoundType.None;
            imageSetting = ImageSettings.Base;
            answerSetting = AnswerSettings.End;
            lang = Lang.English;
            hintLang = Lang.None;
            isFavorite = false;
            traductionMode = false;
            allowHabitatHint = false;
            allowFoodHint = false;
            habitat = "";
            rarityLimit = 50;
            family = "";
            order = "";
            food = "";
        }
        public GameMode(GameMode gameMode)
        {
            name = gameMode.name;
            objective = gameMode.objective;
            objectiveLimitSettings = gameMode.objectiveLimitSettings;
            birdNumberLimit = gameMode.birdNumberLimit;
            timeLimit = gameMode.timeLimit;
            soundPresenceSetting = gameMode.soundPresenceSetting;
            imagePresenceSetting = gameMode.imagePresenceSetting;
            soundSetting = gameMode.soundSetting;
            imageSetting = gameMode.imageSetting;
            answerSetting = gameMode.answerSetting;
            lang = gameMode.lang;
            hintLang = gameMode.hintLang;
            isFavorite = gameMode.isFavorite;
            traductionMode = gameMode.traductionMode;
            allowHabitatHint = gameMode.allowHabitatHint;
            allowFoodHint = gameMode.allowFoodHint;
            habitat = gameMode.habitat;
            rarityLimit = gameMode.rarityLimit;
            family = gameMode.family;
            order = gameMode.order;
            food = gameMode.food;
        }
        public GameMode Clone() => new GameMode(this);
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(traductionMode);
            hash.Add(allowHabitatHint);
            hash.Add(allowFoodHint);
            hash.Add(objective);
            hash.Add(objectiveLimitSettings);
            hash.Add(birdNumberLimit);
            hash.Add(timeLimit);
            hash.Add(soundPresenceSetting);
            hash.Add(imagePresenceSetting);
            hash.Add(soundSetting);
            hash.Add(imageSetting);
            hash.Add(answerSetting);
            hash.Add(lang);
            hash.Add(hintLang);
            hash.Add(habitat);
            hash.Add(rarityLimit);
            hash.Add(family);
            hash.Add(order);
            hash.Add(food);
            return hash.ToHashCode();
        }
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (other.GetType() != typeof(GameMode)) return false;
            return Equals((GameMode)other);
        }
        public bool Equals(GameMode other)
        {
            if (objective != other.objective) return false;
            if (objectiveLimitSettings != other.objectiveLimitSettings) return false;
            if (birdNumberLimit != other.birdNumberLimit) return false;
            if (timeLimit != other.timeLimit) return false;
            if (soundPresenceSetting != other.soundPresenceSetting) return false;
            if (imagePresenceSetting != other.imagePresenceSetting) return false;
            if (soundSetting != other.soundSetting) return false;
            if (answerSetting != other.answerSetting) return false;
            if (imageSetting != other.imageSetting) return false;
            if (lang != other.lang) return false;
            if (hintLang != other.hintLang) return false;
            if (traductionMode != other.traductionMode) return false;
            if (allowHabitatHint != other.allowHabitatHint) return false;
            if (allowFoodHint != other.allowFoodHint) return false;
            if (habitat != other.habitat) return false;
            if (rarityLimit != other.rarityLimit) return false;
            if (family != other.family) return false;
            if (order != other.order) return false;
            if (food != other.food) return false;
            return true;
        }

        public static bool operator == (GameMode GameMode, GameMode other)
        {
            return Equals(GameMode, other);
        }
        public static bool operator !=(GameMode GameMode, GameMode other)
        {
            return !Equals(GameMode, other);
        }
    }

    public class GameSettings
    {
        public enum DataPresenceSettings
        {
            Never,
            OnlyWhenNeeded,
            Always
        }
        public enum ImageSettings
        {
            Base,
            Realistic
        }
        public enum GameObjective
        {
            NumberedQuizz,
            TimedQuizz
        }
        public enum GameObjectiveLimitSettings
        {
            Custom,
            Fixed
        }
        public enum AnswerSettings
        {
            Direct,
            End
        }
    }
}