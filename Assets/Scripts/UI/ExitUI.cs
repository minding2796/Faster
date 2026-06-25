using UnityEngine;
using Utils;

namespace UI
{
    public class ExitUI : SingleMono<ExitUI>
    {
        private static readonly int Hide = Animator.StringToHash("hide");
        private static readonly int Show = Animator.StringToHash("show");
        public RectTransform[] buttons;
        public RectTransform selectArrow;
        public int selectedButton;
        public Animator animator;
        public bool isShown;

        private void Update()
        {
            var vector2 = selectArrow.anchoredPosition;
            vector2.y = buttons[selectedButton].anchoredPosition.y;
            selectArrow.anchoredPosition = vector2;
        }
        
        public void SelectButton(bool isDown)
        {
            selectedButton = isDown ? selectedButton + 1 : selectedButton - 1;
            if (selectedButton < 0) selectedButton = buttons.Length - 1;
            if (selectedButton >= buttons.Length) selectedButton = 0;
        }
        
        public void Toggle()
        {
            if (isShown)
            {
                animator.SetTrigger(Hide);
                return;
            }
            selectedButton = 0;
            animator.SetTrigger(Show);
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void Continue()
        {
            animator.SetTrigger(Hide);
        }
    }
}
