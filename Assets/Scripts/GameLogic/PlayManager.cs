using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Utils;

namespace GameLogic
{
    public class PlayManager : SingleMono<PlayManager>
    {
        public Transform[] lineOffsets;
        public ParticleSystem[] hitEffects;
        public SpriteRenderer[] laneSprites;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI noteSpeedText;

        public TextMeshProUGUI musicTitleText;
        public TextMeshProUGUI musicComposerText;
        public TextMeshProUGUI patternDifficultyText;
        public Image musicCoverImage;

        public bool isStarted;
        public bool isEnded;
        public float startOffset = 3;
        public VideoPlayer videoSource;
        public AudioSource musicSource;
        public AudioSource effectSource;
        public AudioClip hitSound;

        public GameObject resultCanvas;
        private void Start()
        {
            GameStatics.MusicData = ResourceManager.ParseMusicData(GameStatics.SelectedMusic);
            GameStatics.ChartData = ResourceManager.ParseChartData(GameStatics.MusicData, GameStatics.SelectedDifficulty);
            GameStatics.StartOffset = startOffset * 1000;
            GameStatics.NoteOffset = lineOffsets;
            GameStatics.GameStartTime = Time.time * 1000f;
        }

        private void FixedUpdate()
        {
            musicSource.clip ??= GameStatics.MusicData.audioClip;
            videoSource.clip ??= GameStatics.MusicData.videoClip;
            if (TimeUtils.GetCurrentTime(Time.time) >= 0 && !isStarted)
            {
                isStarted = true;
                GameStatics.Score += GameStatics.RemainingScore;
                musicSource.Play();
                videoSource.Play();
            }
            
            if (isStarted && Mathf.Abs(TimeUtils.GetCurrentTime(Time.time) - musicSource.time * 1000f) > 50f)
                musicSource.time = TimeUtils.GetCurrentTime(Time.time) / 1000f;
            if (isStarted && Mathf.Abs(TimeUtils.GetCurrentTime(Time.time) - (float) videoSource.time * 1000f) > 50f)
                videoSource.time = TimeUtils.GetCurrentTime(Time.time) / 1000f;
            
            scoreText.text = $"{GameStatics.Score:0000000}";
            noteSpeedText.text = $"x{GameStatics.NoteSpeed:F1}";
            
            musicTitleText.text = $"{GameStatics.MusicData.title}";
            musicComposerText.text = $"{GameStatics.MusicData.composer}";
            musicCoverImage.sprite = GameStatics.MusicData.cover;
            patternDifficultyText.text = $"{GameStatics.ChartData.difficulty}";
            effectSource.volume = GameStatics.HitSoundVolume * GameStatics.MasterVolume;
            musicSource.volume = GameStatics.MusicVolume * GameStatics.MasterVolume;
            if (isEnded || !Lines.Instance.IsNoteEmpty() || !NoteManager.Instance.IsThereNoMoreNotes()) return;
            isEnded = true;
            resultCanvas.SetActive(true);
        }

        public void PlayHitSound()
        {
            effectSource.Stop();
            effectSource.PlayOneShot(hitSound);
        }

        public void PlayHitEffect(int line)
        {
            hitEffects[line].Stop();
            hitEffects[line].Play();
        }
        
        public void ActiveLane(int line, bool isActive)
        {
            laneSprites[line].enabled = isActive;
        }

        public void RestartGame()
        {
            GameStatics.GameStartTime = Time.time * 1000f;
            resultCanvas.SetActive(false);
            musicSource.Stop();
            isEnded = false;
            isStarted = false;
            Lines.Instance.Clear();
            NoteManager.Instance.noteIndex = 0;
        }
    }
}
