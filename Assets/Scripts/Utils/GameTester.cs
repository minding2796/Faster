using GameLogic;
using UnityEngine;

namespace Utils
{
    // ReSharper disable StringLiteralTypo
    public class GameTester : MonoBehaviour
    {
        public string musicName;
        public DifficultyName difficulty;
        public bool autoPlay;
        [Range(0, 1)]
        public float hitSoundVolume;

        private void Awake()
        {
            SetStatics();
        }

        private void Update()
        {
            PlayManager.Instance.effectSource.volume = hitSoundVolume;
        }

        private void SetStatics()
        {
            GameStatics.AutoPlay = autoPlay;
            GameStatics.NoteSpeed = 6f;
            GameStatics.SelectedMusic = musicName;
            GameStatics.SelectedDifficulty = difficulty;
        }
    }
}
