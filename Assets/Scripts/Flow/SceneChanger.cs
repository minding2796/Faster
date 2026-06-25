using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Flow
{
    public class SceneChanger : MonoBehaviour
    {
        public ExitUI exitUI;
        
        public void OnAnyKey(InputValue value)
        {
            if ((int)value.Get<float>() != 1) return;
            SelectMusic();
        }

        public void OnExit(InputValue value)
        {
            exitUI.Toggle();
        }

        public void OnDifficultySelect(InputValue value)
        {
            
            if (!ExitUI.Instance.isShown) return;
            var v = value.Get<float>();
            if (v != 0) ExitUI.Instance.SelectButton(v > 0);
        }

        public void StartGame()
        {
            SceneManager.LoadScene("GamePlay");
        }

        public void SelectMusic()
        {
            SceneManager.LoadScene("SelectMusic");
        }
    }
}
