using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GameLogic;
using UnityEngine;

namespace Utils
{
    public static class SaveDataManager
    {
        private static readonly BinaryFormatter Formatter = new();

        public static ChartPlayData GetPlayData(MusicData musicData, DifficultyName difficulty)
        {
            if (!File.Exists($"{Application.persistentDataPath}/SaveData/{musicData.title}/{difficulty}.dat"))
                return new ChartPlayData
                {
                    score = 0,
                    maxCombo = 0,
                    judges = new int[5]
                };
            using var stream = File.Open($"{Application.persistentDataPath}/SaveData/{musicData.title}/{difficulty}.dat", FileMode.Open);
            return Formatter.Deserialize(stream) as ChartPlayData;
        }

        public static void SavePlayData(MusicData musicData, DifficultyName difficulty, ChartPlayData data)
        {
            if (!File.Exists($"{Application.persistentDataPath}/SaveData/{musicData.title}"))
            {
                Directory.CreateDirectory($"{Application.persistentDataPath}/SaveData/{musicData.title}");
                File.Create($"{Application.persistentDataPath}/SaveData/{musicData.title}/{difficulty}.dat").Close();
            }
            using var stream = File.Open($"{Application.persistentDataPath}/SaveData/{musicData.title}/{difficulty}.dat", FileMode.Create);
            Formatter.Serialize(stream, data);
        }

        public static SettingData GetSettings()
        {
            if (!File.Exists($"{Application.persistentDataPath}/SaveData/settings.dat"))
                return new SettingData
                {
                    noteSpeed = 6,
                    audioOffset = 0,
                    masterVolume = 1,
                    musicVolume = 1,
                    effectVolume = 1,
                    hitSoundVolume = 1,
                    autoPlay = false,
                    keyBindingJson = ""
                };
            using var stream = File.Open($"{Application.persistentDataPath}/SaveData/settings.dat", FileMode.Open);
            return Formatter.Deserialize(stream) as SettingData;
        }

        public static void SaveSettings()
        {
            var data = new SettingData
            {
                noteSpeed = GameStatics.NoteSpeed,
                audioOffset = GameStatics.AudioOffset,
                masterVolume = GameStatics.MasterVolume,
                musicVolume = GameStatics.MusicVolume,
                effectVolume = GameStatics.EffectVolume,
                hitSoundVolume = GameStatics.HitSoundVolume,
                autoPlay = GameStatics.AutoPlay,
                keyBindingJson = GameStatics.KeyBinding
            };
            if (!File.Exists($"{Application.persistentDataPath}/SaveData/settings.dat"))
            {
                Directory.CreateDirectory($"{Application.persistentDataPath}/SaveData");
                File.Create($"{Application.persistentDataPath}/SaveData/settings.dat").Close();
            }

            using var stream = File.Open($"{Application.persistentDataPath}/SaveData/settings.dat", FileMode.Open);
            Formatter.Serialize(stream, data);
        }
    }

    [Serializable]
    public class ChartPlayData
    {
        public float score;
        public int maxCombo;
        public int[] judges;

        public string ClearStatus
        {
            get
            {
                if (JudgePercent < 60 && judges.Any(i => i != 0)) return "<color #820816>Failed..</color>";
                if (judges[3] > 0 || judges[4] > 0) return "<color #00FF09>CLEAR!</color>";
                if (judges[2] > 0 || judges[1] > 0) return "<color #FFFE00>NO MISS!</color>";
                return judges[0] > 0 ? "<color #E23DFF>Perfect Play!</color>" : "<color #000000>No Data</color>";
            }
        }
        
        public Color ClearStatusColor
        {
            get
            {
                if (JudgePercent < 60 && judges.Any(i => i != 0)) return new Color(0.51f, 0.03f, 0.09f);
                if (judges[3] > 0 || judges[4] > 0) return new Color(0f, 1f, 0.04f);
                if (judges[2] > 0 || judges[1] > 0) return new Color(1f, 1f, 0f);
                return judges[0] > 0 ? new Color(0.89f, 0.24f, 1f) : Color.black;
            }
        }

        public string ClearRank
        {
            get
            {
                if (judges.All(i => i == 0)) return "<color #FFFFFF>X</color>";
                return JudgePercent switch
                {
                    >= 100 => "<color #E23DFF>P</color>",
                    >= 97 => "<color #FFFE00>S</color>",
                    >= 90 => "<color #FF004F>A</color>",
                    >= 80 => "<color #00FF09>B</color>",
                    >= 70 => "<color #00B5FF>C</color>",
                    >= 60 => "<color #A33EA9>D</color>",
                    _ => "<color #912E05>F</color>"
                };
            }
        }

        public float JudgePercent
        {
            get
            {
                var e = judges[0] * 1f + judges[1] * 0.75f + judges[2] * 0.5f + judges[3] * 0.1f;
                var s = judges.Sum();
                if (s == 0) return 0;
                return e * 100f / s;
            }
        }

        public static ChartPlayData FromCurrentPlayData()
        {
            return new ChartPlayData
            {
                judges = GameStatics.Judges,
                maxCombo = GameStatics.MaxCombo,
                score = GameStatics.Score
            };
        }
    }

    [Serializable]
    public class SettingData
    {
        public float noteSpeed;
        public float audioOffset;
        public float masterVolume;
        public float musicVolume;
        public float effectVolume;
        public float hitSoundVolume;
        public bool autoPlay;
        public string keyBindingJson;
    }
}