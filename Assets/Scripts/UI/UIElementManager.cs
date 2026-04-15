using GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UIElementManager : SingleMono<UIElementManager>
    {
        private static readonly int Updated = Animator.StringToHash("Updated");
        [SerializeField] private TextMeshProUGUI judgementText;
        [SerializeField] private TextMeshProUGUI judgementPercent;
        [SerializeField] private TextMeshProUGUI comboText;
        [SerializeField] private Animator comboAnimator;
        [SerializeField] private Animator judgeAnimator;
        [SerializeField] private Slider timeTracker;
        
        private Coroutine judgementCoroutine;

        private void Update()
        {
            if (comboText.text != $"{GameStatics.Combo}")
            {
                comboAnimator.SetTrigger(Updated);
                comboText.text = $"{GameStatics.Combo}";
            }
            judgementPercent.text = $"{Mathf.Floor(GameStatics.JudgePercent * 100) / 100:F2}%";
            timeTracker.SetValueWithoutNotify(PlayManager.Instance.musicSource.time / GameStatics.MusicData.audioClip.length);
        }

        public void SetJudge(string judge)
        {
            judgementText.text = judge;
            judgeAnimator.SetTrigger(Updated);
        }
    }
}
