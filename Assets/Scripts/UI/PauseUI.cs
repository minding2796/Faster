using GameLogic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class PauseUI : SingleMono<PauseUI>
    {
        private static readonly int Hide = Animator.StringToHash("hide");
        private static readonly int Show = Animator.StringToHash("show");
        public RectTransform[] buttons;
        public RectTransform selectArrow;
        public int selectedButton;
        public Animator animator;
        public Slider timeTracker;
        public bool isPaused;

        private void Update()
        {
            if (isPaused) GameStatics.GameStartTime += Time.deltaTime * 1000f;
            var vector2 = selectArrow.anchoredPosition;
            vector2.y = buttons[selectedButton].anchoredPosition.y;
            selectArrow.anchoredPosition = vector2;
            timeTracker.SetValueWithoutNotify(PlayManager.Instance.musicSource.time / GameStatics.MusicData.audioClip.length);
        }
        
        public void SelectButton(bool isDown)
        {
            selectedButton = isDown ? selectedButton + 1 : selectedButton - 1;
            if (selectedButton < 0) selectedButton = buttons.Length - 1;
            if (selectedButton >= buttons.Length) selectedButton = 0;
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

        public void OpenUI()
        {
            selectedButton = 0;
            isPaused = true;
            PlayManager.Instance.musicSource.Pause();
            animator.SetTrigger(Show);
        }

        public void Continue()
        {
            isPaused = false;
            PlayManager.Instance.musicSource.UnPause();
            animator.SetTrigger(Hide);
        }
    }
}
