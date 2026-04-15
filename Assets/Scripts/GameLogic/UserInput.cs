using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace GameLogic
{
    public class UserInput : SingleMono<UserInput>
    {
        public bool[] pressed;

        private void Start()
        {
            pressed = new bool[4];
        }

        public void OnL1(InputValue context)
        {
            if (PauseUI.Instance.isPaused || PlayManager.Instance.isEnded) return;
            PlayManager.Instance.ActiveLane(0, (int)context.Get<float>() == 1);
            if (GameStatics.AutoPlay) return;
            pressed[0] = (int)context.Get<float>() == 1;
            if (pressed[0]) Lines.Instance.PressLine(0);
            else Lines.Instance.ReleaseLine(0);
        }

        public void OnL2(InputValue context)
        {
            if (PauseUI.Instance.isPaused || PlayManager.Instance.isEnded) return;
            PlayManager.Instance.ActiveLane(1, (int)context.Get<float>() == 1);
            if (GameStatics.AutoPlay) return;
            pressed[1] = (int)context.Get<float>() == 1;
            if (pressed[1]) Lines.Instance.PressLine(1);
            else Lines.Instance.ReleaseLine(1);
        }
        
        public void OnL3(InputValue context)
        {
            if (PauseUI.Instance.isPaused || PlayManager.Instance.isEnded) return;
            PlayManager.Instance.ActiveLane(2, (int)context.Get<float>() == 1);
            if (GameStatics.AutoPlay) return;
            pressed[2] = (int)context.Get<float>() == 1;
            if (pressed[2]) Lines.Instance.PressLine(2);
            else Lines.Instance.ReleaseLine(2);
        }
        
        public void OnL4(InputValue context)
        {
            if (PauseUI.Instance.isPaused || PlayManager.Instance.isEnded) return;
            PlayManager.Instance.ActiveLane(3, (int)context.Get<float>() == 1);
            if (GameStatics.AutoPlay) return;
            pressed[3] = (int)context.Get<float>() == 1;
            if (pressed[3]) Lines.Instance.PressLine(3);
            else Lines.Instance.ReleaseLine(3);
        }
    }
}
