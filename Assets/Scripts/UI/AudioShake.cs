using System.Linq;
using GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AudioShake : MonoBehaviour
    {
        [Header("연결 설정")]
        public Image image;
        public AudioSource audioSource;

        [Header("효과 설정")]
        public float sensitivity = 100f; // 민감도 (얼마나 많이 움직일 것인가)
        public float smoothTime = 0.1f;  // 부드러운 움직임 계수
        public Vector3 baseScale = Vector3.one; // 기본 크기

        private float currentVelocity;
        private RectTransform rectTransform;
        private readonly float[] sampleData = new float[256];
        
        private void Start()
        {
            if (image) rectTransform = image.transform as RectTransform;
        }
        
        [ContextMenu("Collect All Dependencies")]
        public void CollectAllDependencies() {
            image = FindAnyObjectByType<Image>();
            audioSource = FindAnyObjectByType<AudioSource>();
        }

        private void Update()
        {
            if (image) image.sprite = GameStatics.SelectedMusicData?.cover;
            if (!audioSource || !rectTransform) return;

            // 1. 오디오 데이터 분석 (RMS 방식 - 평균 진폭 계산)
            var loudness = GetLoudnessFromAudioClip();

            // 2. 민감도를 적용한 목표 스케일 계산
            var targetScale = baseScale.x + loudness * sensitivity;

            // 3. 부드럽게 크기 변경 (SmoothDamp 사용)
            var currentScale = Mathf.SmoothDamp(rectTransform.localScale.x, targetScale, ref currentVelocity, smoothTime);
            
            rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);
        }

        private float GetLoudnessFromAudioClip()
        {
            // GetOutputData는 스트리밍 중인 오디오의 '현재 출력 값'을 가져옵니다.
            // 첫 번째 인자는 데이터를 담을 배열, 두 번째 인자는 채널(0은 왼쪽/모노)입니다.
            audioSource.GetOutputData(sampleData, 0);

            var totalLoudness = sampleData.Sum(Mathf.Abs);
            return totalLoudness / sampleData.Length;
        }
    }
}