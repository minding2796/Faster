using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    public class GamePlayUIInput : MonoBehaviour
    {
        public void OnSwitchButton(InputValue value)
        {
            if (PlayManager.Instance.isEnded) return;
            if (!PauseUI.Instance.isPaused) return;
            var v = value.Get<float>();
            if (v != 0) PauseUI.Instance.SelectButton(v > 0);
        }

        public void OnEnter(InputValue value)
        {
            if (PlayManager.Instance.isEnded)
            {
                GameStatics.ResetGameData();
                SceneManager.LoadScene("SelectMusic");
                return;
            }
            
            if (!PauseUI.Instance.isPaused) return;
            switch (PauseUI.Instance.selectedButton)
            {
                case 0:
                    PauseUI.Instance.Continue();
                    break;
                case 1:
                    PauseUI.Instance.Restart();
                    break;
                case 2:
                    PauseUI.Instance.Exit();
                    break;
            }
        }

        public void OnSpeed(InputValue value)
        {
            if (PlayManager.Instance.isEnded) return;
            Settings.AddSpeed(value.Get<float>() * 0.1f);
        }

        public void OnRestart(InputValue value)
        {
            PauseUI.Instance.isPaused = false;
            PlayManager.Instance.musicSource.UnPause();
            PauseUI.Instance.animator.ResetControllerState();
            GameStatics.ResetGameData();
            PlayManager.Instance.RestartGame();
        }

        public void OnPause(InputValue value)
        {
            if (PlayManager.Instance.isEnded)
            {
                GameStatics.ResetGameData();
                SceneManager.LoadScene("SelectMusic");
                return;
            }
            if (PauseUI.Instance.isPaused) PauseUI.Instance.Continue();
            else PauseUI.Instance.OpenUI();
        }
    }
}
