using System.Linq;
using GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class ResultElementManager : MonoBehaviour
    {
        [Header("Top UI")]
        public Image coverImage;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI composerText;
        public TextMeshProUGUI difficultyText;
        public TextMeshProUGUI isAutoPlayText;
        
        [Header("Bottom UI")]
        public TextMeshProUGUI clearStatusText;
        
        [Header("Rank Section")]
        public Image clearStatusImage;
        public Image perfectImage;
        public Image greatImage;
        public Image goodImage;
        public Image badImage;
        public Image rankCoverImage;
        public TextMeshProUGUI rankText;
        
        [Header("Score Section")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI judgePercentText;
        public TextMeshProUGUI maxComboText;
        
        [Header("Details Section")]
        public TextMeshProUGUI perfectText;
        public TextMeshProUGUI greatText;
        public TextMeshProUGUI goodText;
        public TextMeshProUGUI badText;
        public TextMeshProUGUI missText;
        public Animator isNewRecord;
        
        private ChartPlayData playData;

        private void OnEnable()
        {
            var musicData = GameStatics.MusicData;
            var chartData = GameStatics.ChartData;
            playData = ChartPlayData.FromCurrentPlayData();
            
            coverImage.sprite = musicData.cover;
            titleText.text = musicData.title;
            composerText.text = musicData.composer;
            difficultyText.text = chartData.difficulty;
            isAutoPlayText.text = GameStatics.AutoPlay ? "ON" : "OFF";

            clearStatusText.text = playData.ClearStatus;

            clearStatusImage.color = playData.ClearStatusColor;
            var sum = playData.judges.Sum();
            perfectImage.fillAmount = (float) playData.judges[0] / sum;
            greatImage.fillAmount = (float) (playData.judges[0] + playData.judges[1]) / sum;
            goodImage.fillAmount = (float) (playData.judges[0] + playData.judges[1] + playData.judges[2]) / sum;
            badImage.fillAmount = (float) (playData.judges[0] + playData.judges[1] + playData.judges[2] + playData.judges[3]) / sum;
            rankCoverImage.sprite = musicData.cover;
            rankText.text = playData.ClearRank;
            
            scoreText.text = playData.score.ToString("0000000");
            judgePercentText.text = $"{Mathf.Floor(playData.JudgePercent * 100) / 100:F2}%";
            maxComboText.text = $"Max Combo {playData.maxCombo}";
            
            perfectText.text = playData.judges[0].ToString();
            greatText.text = playData.judges[1].ToString();
            goodText.text = playData.judges[2].ToString();
            badText.text = playData.judges[3].ToString();
            missText.text = playData.judges[4].ToString();
            
            var oldData = SaveDataManager.GetPlayData(musicData, GameStatics.SelectedDifficulty);
            isNewRecord.gameObject.SetActive(!GameStatics.AutoPlay && oldData.score < playData.score);
            if (!GameStatics.AutoPlay && oldData.score < playData.score) SaveDataManager.SavePlayData(musicData, GameStatics.SelectedDifficulty, playData);

            Canvas.ForceUpdateCanvases();
        }

        public void Exit()
        {
            GameStatics.ResetGameData();
            SceneManager.LoadScene("SelectMusic");
        }

        public void Restart()
        {
            GameStatics.ResetGameData();
            SceneManager.LoadScene("GamePlay");
        }
    }
}
