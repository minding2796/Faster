using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace GameLogic.Calibration
{
    public class CalibrationMaster : SingleMono<CalibrationMaster>
    {
        private static readonly int Beat = Animator.StringToHash("beat");
        public Animator beatCircle;
        
        public TextMeshProUGUI offsetValueText;
        public TextMeshProUGUI averageOffsetText;
        public Slider offsetValueSlider;
        public float timer = 1200f;
        
        public CalculatedText calculatedText;
        public RectTransform content;

        public float sumOfOffset;
        public int countOffset;
        private float AverageOffset => countOffset == 0 ? 0 : sumOfOffset / countOffset;
        
        public AudioSource musicSource;
        public bool isStarted;
        
        public void OnEnter(InputValue value)
        {
            var offset = TimeUtils.GetCurrentTime(Time.time) - timer;
            if (Mathf.Abs(offset) > 600f) return;
            Instantiate(calculatedText, content).ApplyOffset(offset);
            content.anchoredPosition = Vector2.zero;
            timer += 2400f;
            beatCircle.SetTrigger(Beat);
        }

        private void Start()
        {
            GameStatics.GameStartTime = Time.time * 1000f;
            GameStatics.StartOffset = 0f;
        }

        private void FixedUpdate()
        {
            if (TimeUtils.GetCurrentTime(Time.time) >= 0 && !isStarted)
            {
                isStarted = true;
                musicSource.Play();
            }
            var offset = TimeUtils.GetCurrentTime(Time.time) - timer;
            if (offset > 600f) timer += 2400f;
            offsetValueText.text = $"{GameStatics.AudioOffset:F0}ms";
            offsetValueSlider.SetValueWithoutNotify(GameStatics.AudioOffset);
            averageOffsetText.text = $"{AverageOffset:F0}ms";
        }

        public void ResetOffset()
        {
            sumOfOffset = 0f;
            countOffset = 0;
            for (var i = 0; i < content.childCount; i++) Destroy(content.GetChild(i).gameObject);
        }

        public void ApplyCalculatedOffset()
        {
            SetOffset(AverageOffset);
        }

        public static void SetOffset(float value)
        {
            GameStatics.AudioOffset = Mathf.Clamp(value, -600f, 600f);
            SaveDataManager.SaveSettings();
        }
        
        public static void AddOffset(float value) => SetOffset(GameStatics.AudioOffset + value);

        public void OnExit(InputValue value) => Exit();

        public static void Exit()
        {
            SceneManager.LoadScene("SelectMusic");
        }
    }
}
