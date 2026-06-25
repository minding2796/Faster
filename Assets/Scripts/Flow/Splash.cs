using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Flow
{
    public class Splash : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        private bool started;
        
        public void OnClick(InputValue value) => SceneManager.LoadScene("Title");
        
        public void OnAnyKey(InputValue value)
        {
            if ((int)value.Get<float>() != 1) return;
            SceneManager.LoadScene("Title");
        }

        private void Update()
        {
            switch (videoPlayer.isPlaying)
            {
                case true:
                    started = true;
                    break;
                case false when started:
                    SceneManager.LoadScene("Title");
                    break;
            }
        }
    }
}
