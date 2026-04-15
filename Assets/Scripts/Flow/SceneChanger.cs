using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Flow
{
    public class SceneChanger : MonoBehaviour
    {
        public void OnAnyKey(InputValue value)
        {
            if ((int)value.Get<float>() != 1) return;
            SelectMusic();
        }

        public void OnExit(InputValue value)
        {
            Application.Quit();
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
