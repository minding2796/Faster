using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace GameLogic
{
    public static class GameStatics
    {
        [Header("Music Data")]
        public static string SelectedMusic;
        public static MusicData MusicData;
        public static DifficultyName SelectedDifficulty;
        public static ChartData ChartData;
        
        [Header("Game Data")]
        public static float GameStartTime;
        public static readonly int[] Judges = new int[5];
        public static Transform[] NoteOffset;
        public static float Score;
        public static int Combo;
        public static int MaxCombo;
        
        [Header("Settings")]
        public static float StartOffset;
        public static float NoteSpeed;
        public static float AudioOffset;
        public static float MasterVolume;
        public static float MusicVolume;
        public static float EffectVolume;
        public static float HitSoundVolume;
        public static bool AutoPlay;
        public static string KeyBinding;


        private static MusicData[] _allMusicData; 
        public static MusicData[] GetAllMusicData()
        {
            if (_allMusicData != null) return _allMusicData;
            _allMusicData = ResourceManager.LoadAllMusicData();
            return _allMusicData;
        }

        public static float JudgePercent
        {
            get
            {
                var e = Judges[0] * 1f + Judges[1] * 0.75f + Judges[2] * 0.5f + Judges[3] * 0.1f;
                var s = Judges.Sum();
                if (s == 0) return 0;
                return e * 100f / s;
            }
        }

        public static float RemainingScore
        {
            get
            {
                var score = 0f;
                for (var i = 0; i < ChartData.notes.Count; i++) score += UnitScore;
                return 1000000f - score;
            }
        }

        public static float UnitScore => 1000000f / ChartData.notes.Count;

        public static MusicData SelectedMusicData
        {
            get
            {
                if (MusicData != null && MusicData.title == SelectedMusic) return MusicData;
                return MusicData = ResourceManager.ParseMusicData(SelectedMusic);
            }
        }

        public static void ResetGameData()
        {
            GameStartTime = 0; 
            Array.Fill(Judges, 0);
            Score = 0;
            Combo = 0;
            MaxCombo = 0;
        }
    }
    
    [Serializable]
    public class MusicData
    {
        public string title;
        public string composer;
        public string bpm;
        public float previewTime;
        public AudioClip audioClip;
        public Category[] category;
        public Sprite cover;
        public DifficultyName[] availableDifficulty;

        public bool IsContains(string word)
        {
            return 
                title.ToLower().Contains(word.ToLower()) ||
                composer.ToLower().Contains(word.ToLower()) ||
                bpm.ToLower().Contains(word.ToLower()) ||
                category.Any(c => c.ToString().ToLower().Contains(word.ToLower()));
        }
    }
    
    [Serializable]
    public class ChartData
    {
        public string difficulty;
        public List<NoteData> notes;
    }

    [Serializable]
    public enum Category
    {
        All,
        Original,
        AICompose,
    }

    [Serializable]
    public enum DifficultyName
    {
        Easy = 0,
        Normal,
        Hard,
        Impossible,
        Special,
    }
}
