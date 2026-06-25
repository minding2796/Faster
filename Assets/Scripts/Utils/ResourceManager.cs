using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic;
using UnityEngine;
using UnityEngine.Video;
using YamlDotNet.Serialization;

namespace Utils
{
    public static class ResourceManager
    {
        [Obsolete("Old ChartData System is not supported anymore.")]
        public static List<NoteData> ParseChart(string title)
        {
            var strData = Resources.Load<TextAsset>("NoteData/" + title).text;
            var arr = strData.Replace(" ", "").Replace("\n", "").Split(",");
            return arr.Select(s => s.Split("|"))
                .Select(data => data.Length == 2
                    ? new NoteData { startTime = float.Parse(data[0]), endOffset = 0, line = int.Parse(data[1]) - 1 }
                    : new NoteData { startTime = float.Parse(data[0]), endOffset = float.Parse(data[2]) - float.Parse(data[0]), line = int.Parse(data[1]) - 1 })
                .ToList();
        }

        public static MusicData[] LoadAllMusicData()
        {
            return Resources.LoadAll<TextAsset>("MusicData").Select(t => ParseMusicData(t.name)).Where(t => t != null).ToArray();
        }

        public static MusicData ParseMusicData(string musicTitle)
        {
            var asset = Resources.Load<TextAsset>($"MusicData/{musicTitle}");
            if (!asset) return null;
            var strData = asset.text;
            var o = new DeserializerBuilder().Build().Deserialize<YamlMusicData>(strData);
            
            return new MusicData
            {
                title = o.Title,
                composer = o.Composer,
                bpm = o.Bpm,
                previewTime = o.PreviewTime,
                audioClip = Resources.Load<AudioClip>($"MusicData/{o.Title}/audio"),
                videoClip = Resources.Load<VideoClip>($"MusicData/{o.Title}/video"),
                cover = Resources.Load<Sprite>($"MusicData/{o.Title}/cover"),
                category = o.Category,
                availableDifficulty = o.AvailableDifficulty
            };
        }

        public static ChartData ParseChartData(MusicData musicData, DifficultyName difficulty)
        {
            var strData = Resources.Load<TextAsset>($"MusicData/{musicData.title}/{difficulty}").text;
            var o = new DeserializerBuilder().Build().Deserialize<YamlChartData>(strData);
            
            return new ChartData
            {
                difficulty = o.Difficulty,
                notes = o.Notes.Select(n => new NoteData { startTime = n.startTime, endOffset = (n.EndOffset ?? n.startTime) - n.startTime, line = n.line - 1 }).ToList()
            };
        }

        public static string GetChartDifficulty(MusicData musicData, DifficultyName difficulty)
        {
            var data = Resources.Load<TextAsset>($"MusicData/{musicData.title}/{difficulty}");
            if (!data) return $"{difficulty} Lv.??"; 
            var strData = data.text;
            var o = new DeserializerBuilder().Build().Deserialize<YamlChartData>(strData);
            return o.Difficulty;
        }

        // ReSharper disable twice ClassNeverInstantiated.Local
        [YamlSerializable]
        private class YamlMusicData
        {
            [YamlMember(Alias = "Title")]
            public string Title { get; set; }
            [YamlMember(Alias = "Artist")]
            public string Composer { get; set; }
            [YamlMember(Alias = "BPM")]
            public string Bpm { get; set; }
            [YamlMember(Alias = "SongPreviewTime")]
            public float PreviewTime { get; set; }
            [YamlMember(Alias = "Category")]
            public Category[] Category { get; set; }
            [YamlMember(Alias = "AvailableDifficulty")]
            public DifficultyName[] AvailableDifficulty { get; set; }
        }

        [YamlSerializable]
        private class YamlChartData
        {
            [YamlMember(Alias = "DifficultyName")]
            public string Difficulty { get; set; }
            [YamlMember(Alias = "HitObjects")]
            public List<YNoteData> Notes { get; set; }
            
            [YamlSerializable]
            [Serializable]
            public class YNoteData
            {
                [YamlMember(Alias = "StartTime")]
                public float startTime;
                [YamlMember(Alias = "EndTime")]
                public float? EndOffset;
                [YamlMember(Alias = "KeySounds")]
                public string[] keySounds;
                [YamlMember(Alias = "Lane")]
                public int line;
            }
        }
    }
}
