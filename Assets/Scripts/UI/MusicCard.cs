using System;
using System.Linq;
using GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MusicCard : MonoBehaviour
    {
        private static readonly int Deselect = Animator.StringToHash("deselect");
        private static readonly int Reselect = Animator.StringToHash("reselect");
        public Animator animator;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI composerText;
        public TextMeshProUGUI bpmText;
        public TextMeshProUGUI categoryText;
        public Image coverImage;
        public TextMeshProUGUI[] difficultyList;
        public MusicData musicData;

        private bool isExpanded;
        private ChartPlayData playData;
        private DifficultyName currentDifficulty;
        public TextMeshProUGUI statusText;
        public TextMeshProUGUI clearStatusText;
        public int index;

        public void UpdateMusicData()
        {
            titleText.text = musicData.title;
            composerText.text = musicData.composer;
            bpmText.text = $"BPM : {musicData.bpm}";
            categoryText.text = $"{musicData.category[0]}";
            coverImage.sprite = musicData.cover;
            foreach (var difficulty in difficultyList)
            {
                (difficulty.transform.parent as RectTransform)!.anchoredPosition = Vector2.zero;
                difficulty.transform.parent.parent.gameObject.SetActive(false);
            }
            foreach (var available in musicData.availableDifficulty)
            {
                difficultyList[(int)available].transform.parent.parent.gameObject.SetActive(true);
                difficultyList[(int)available].text = ResourceManager.GetChartDifficulty(musicData, available);
            }
        }

        public void UpdateDummyData()
        {
            titleText.text = musicData.title;
            composerText.text = musicData.composer;
            bpmText.text = $"BPM : {musicData.bpm}";
            categoryText.text = $"{musicData.category[0]}";
            coverImage.sprite = musicData.cover;
        }

        private void UpdateSelection(DifficultyName difficulty)
        {
            (difficultyList[(int)currentDifficulty].transform.parent as RectTransform)!.anchoredPosition = Vector2.zero;
            (difficultyList[(int)difficulty].transform.parent as RectTransform)!.anchoredPosition = new Vector2(140, 0);
            currentDifficulty = difficulty;
            GameStatics.SelectedDifficulty = difficulty;
            playData = SaveDataManager.GetPlayData(musicData, difficulty);
        }

        [VisibleEnum(typeof(DifficultyName))]
        public void SelectDifficulty(int difficulty)
        {
            UpdateSelection((DifficultyName) difficulty);
        }

        public void NextDifficulty(bool isNext = true)
        {
            var currentIndex = Array.IndexOf(musicData.availableDifficulty, currentDifficulty);
            var nextIndex = isNext ? currentIndex + 1 : currentIndex - 1;
            if (nextIndex < 0) nextIndex = musicData.availableDifficulty.Length - 1;
            if (nextIndex >= musicData.availableDifficulty.Length) nextIndex = 0;
            UpdateSelection(musicData.availableDifficulty[nextIndex]);
        }

        private void Update()
        {
            if (!isExpanded) return;
            statusText.text = $"{playData.score:0000000} {Mathf.Floor(playData.JudgePercent * 100) / 100:F2}%";
            clearStatusText.text = playData.ClearStatus;
        }

        public void Expand()
        {
            if (isExpanded) return;
            UpdateSelection(musicData.availableDifficulty.Contains(GameStatics.SelectedDifficulty) ? GameStatics.SelectedDifficulty : musicData.availableDifficulty[0]);
            isExpanded = true;
            animator.SetTrigger(Reselect);
            GameStatics.SelectedMusic = musicData.title;
        }
        
        public void Collapse()
        {
            if (!isExpanded) return;
            isExpanded = false;
            (difficultyList[(int)currentDifficulty].transform.parent as RectTransform)!.anchoredPosition = Vector2.zero;
            animator.SetTrigger(Deselect);
        }

        public void Select()
        {
            if (isExpanded) SceneManager.LoadScene("GamePlay");
            else MusicViewManager.Instance.MoveTo(index);
        }

        [VisibleEnum(typeof(CursorType))]
        public void SetCursor(int type)
        {
            CustomCursor.Instance.SetCursor(type);
        }
    }
}
