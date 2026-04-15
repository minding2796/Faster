using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AudioVisualizer : MonoBehaviour
    {
        public float scale;
        public int size;
        public bool reverse;
        public Image bar;
        public Color color;
        public RectTransform rectTransform;
        
        private Image[] images;
        private Transform[] transforms;
        private float[] spectrumData;
        private float barWidth;
        
        private void Start()
        {
            var count = size / 4;
            images = new Image[count];
            transforms = new Transform[count];
            spectrumData = new float[size];
            
            for (var i = 0; i < count; i++)
            {
                images[i] = Instantiate(bar, transform);
                images[i].color = color;
                transforms[i] = images[i].transform;
            }

            barWidth = rectTransform.rect.width / count;
        }

        private void Update()
        {
            if (rectTransform.hasChanged)
            {
                barWidth = rectTransform.rect.width / images.Length;
                rectTransform.hasChanged = false;
            }
            
            AudioListener.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);
            
            var count = images.Length;
            for (var i = 0; i < count; i++)
            {
                var dataIdx = reverse ? count - 1 - i : i;
                var targetHeight = spectrumData[dataIdx] * size * 4 * scale;
                var currentScale = transforms[i].localScale;
                
                var newY = Mathf.Lerp(currentScale.y, targetHeight, 0.1f);
                transforms[i].localScale = new Vector3(barWidth, newY, 1f);
            }
        }
    }
}
