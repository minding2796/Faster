using TMPro;
using UnityEngine;

namespace GameLogic.Calibration
{
    public class CalculatedText : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public float offset;

        public void ApplyOffset(float value)
        {
            offset = value;
            CalibrationMaster.Instance.sumOfOffset += offset;
            CalibrationMaster.Instance.countOffset++;
            text.text = $"{(offset > 0 ? "+" : "")}{offset:F0}ms";
        }

        public void DestroyThis()
        {
            CalibrationMaster.Instance.sumOfOffset -= offset;
            CalibrationMaster.Instance.countOffset--;
            Destroy(gameObject);
        }

        public void UpdateText(bool strikethrough)
        {
            text.text = strikethrough
                ? $"<s>{(offset > 0 ? "+" : "")}{offset:F0}ms</s>"
                : $"{(offset > 0 ? "+" : "")}{offset:F0}ms";
        }
    }
}
