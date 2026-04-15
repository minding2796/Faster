using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace GameLogic
{
    public class Settings : SingleMono<Settings>
    {
        private static readonly int Show = Animator.StringToHash("show");
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI speedTextInUI;
        public TextMeshProUGUI autoText;
        public TextMeshProUGUI autoTextInUI;
        public TextMeshProUGUI masterVolumeInUI;
        public TextMeshProUGUI musicVolumeInUI;
        public TextMeshProUGUI effectVolumeInUI;
        public TextMeshProUGUI hitSoundVolumeInUI;
        public Animator settingsUI;
        
        public Slider speedSlider;
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider effectVolumeSlider;
        public Slider hitSoundVolumeSlider;
        
        [Header("Key Bindings")]
        public GameObject keyBindingsUI;
        public PlayerInput uiInput;
        public InputActionAsset inputActionAsset;
        
        private void Start()
        {
            var settings = SaveDataManager.GetSettings();
            GameStatics.NoteSpeed = settings.noteSpeed;
            GameStatics.AutoPlay = settings.autoPlay;
            GameStatics.AudioOffset = settings.audioOffset;
            GameStatics.MasterVolume = settings.masterVolume;
            GameStatics.MusicVolume = settings.musicVolume;
            GameStatics.EffectVolume = settings.effectVolume;
            GameStatics.HitSoundVolume = settings.hitSoundVolume;
            GameStatics.KeyBinding = settings.keyBindingJson;
            inputActionAsset.LoadBindingOverridesFromJson(settings.keyBindingJson);
        }

        private void Update()
        {
            speedText.text = $"{GameStatics.NoteSpeed:F1}";
            speedTextInUI.text = $"{GameStatics.NoteSpeed:F1}";
            autoText.text = GameStatics.AutoPlay ? "ON" : "OFF";
            autoTextInUI.text = GameStatics.AutoPlay ? "ON" : "OFF";
            masterVolumeInUI.text = $"{GameStatics.MasterVolume * 100:F0}%";
            musicVolumeInUI.text = $"{GameStatics.MusicVolume * 100:F0}%";
            effectVolumeInUI.text = $"{GameStatics.EffectVolume * 100:F0}%";
            hitSoundVolumeInUI.text = $"{GameStatics.HitSoundVolume * 100:F0}%";
            speedSlider.value = GameStatics.NoteSpeed;
            masterVolumeSlider.value = GameStatics.MasterVolume;
            musicVolumeSlider.value = GameStatics.MusicVolume;
            effectVolumeSlider.value = GameStatics.EffectVolume;
            hitSoundVolumeSlider.value = GameStatics.HitSoundVolume;
        }
        
        public static void ToggleAutoPlay()
        {
            GameStatics.AutoPlay = !GameStatics.AutoPlay;
            SaveDataManager.SaveSettings();
        }
        
        public void ToggleSettingsUI()
        {
            settingsUI.SetBool(Show, !settingsUI.GetBool(Show));
            // GameStatics.AutoPlay = !GameStatics.AutoPlay;
        }
        
        public static void SetSpeed(float speed)
        {
            GameStatics.NoteSpeed = Mathf.Clamp(speed, 1f, 9.9f);
            SaveDataManager.SaveSettings();
        }
        
        public static void AddSpeed(float speed) => SetSpeed(GameStatics.NoteSpeed + speed);
        
        public static void SetHitSoundVolume(float volume)
        {
            GameStatics.HitSoundVolume = Mathf.Clamp(volume, 0f, 1f);
            SaveDataManager.SaveSettings();
        }
        
        public static void AddHitSoundVolume(float volume) => SetHitSoundVolume(GameStatics.HitSoundVolume + volume);
        
        public static void SetMusicVolume(float volume)
        {
            GameStatics.MusicVolume = Mathf.Clamp(volume, 0f, 1f);
            SaveDataManager.SaveSettings();
        }
        
        public static void AddMusicVolume(float volume) => SetMusicVolume(GameStatics.MusicVolume + volume);
        
        public static void SetEffectVolume(float volume)
        {
            GameStatics.EffectVolume = Mathf.Clamp(volume, 0f, 1f);
            SaveDataManager.SaveSettings();
        }
        
        public static void AddEffectVolume(float volume) => SetEffectVolume(GameStatics.EffectVolume + volume);

        public static void SetMasterVolume(float volume)
        {
            GameStatics.MasterVolume = Mathf.Clamp(volume, 0f, 1f);
            SaveDataManager.SaveSettings();
        }
        public static void AddMasterVolume(float volume) => SetMasterVolume(GameStatics.MasterVolume + volume);

        public void OpenKeyBound()
        {
            uiInput.enabled = false;
            keyBindingsUI.SetActive(true);
        }

        public void OffsetCalibration()
        {
            SceneManager.LoadScene("OffsetCalibration");
        }
    }
}
