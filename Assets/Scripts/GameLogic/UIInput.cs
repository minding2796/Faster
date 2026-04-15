using UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils;

namespace GameLogic
{
    public class UIInput : SingleMono<UIInput>
    {
        private bool isFocusedInputField;

        public void FocusInputField(bool value)
        {
            isFocusedInputField = value;
        }
        
        public void OnMusicSelect(InputValue value)
        {
            if (isFocusedInputField) return;
            var v = value.Get<float>();
            if (v != 0) MusicViewManager.Instance.MoveToNext(v > 0);
        }
        
        public void OnDifficultySelect(InputValue value)
        {
            if (isFocusedInputField) return;
            var v = value.Get<float>();
            if (v != 0) MusicViewManager.Instance.SelectedMusicCard.NextDifficulty(v > 0);
        }
        
        public void OnOpenSetting(InputValue value)
        {
            if (isFocusedInputField) return;
            Settings.Instance.ToggleSettingsUI();
        }

        public void OnOpenCategory(InputValue value)
        {
            if (isFocusedInputField) return;
            MusicFilter.Instance.ToggleCategoryUI();
        }

        public void OnEnter(InputValue value)
        {
            if (isFocusedInputField) return;
            SceneManager.LoadScene("GamePlay");
        }

        public void OnSpeed(InputValue value)
        {
            if (isFocusedInputField) return;
            Settings.AddSpeed(value.Get<float>() * 0.1f);
        }

        public void OnAutoPlay(InputValue value)
        {
            if (isFocusedInputField) return;
            Settings.ToggleAutoPlay();
        }

        public void OnExit(InputValue value)
        {
            SceneManager.LoadScene("Title");
        }
    }
}
