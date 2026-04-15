using System;
using GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MusicController : MonoBehaviour
    {
        public AudioSource audioSource;
        public Slider slider;
        public TextMeshProUGUI currentTime, maxTime;
        private bool pauseState;
        private MusicData prevMusicData;

        private void Update()
        {
            if (GameStatics.SelectedMusicData != prevMusicData || (!audioSource.isPlaying && audioSource.time >= audioSource.clip.length))
            {
                prevMusicData = GameStatics.SelectedMusicData;
                audioSource.clip = null;
                audioSource.clip = prevMusicData.audioClip;
                audioSource.time = prevMusicData.previewTime;
                audioSource.Play();
            }
            slider.SetValueWithoutNotify(audioSource.time / audioSource.clip.length);
            currentTime.text = GetTimeFromFloat(audioSource.time);
            maxTime.text = GetTimeFromFloat(audioSource.clip.length);
            audioSource.volume = GameStatics.MusicVolume * GameStatics.MasterVolume;
        }

        private static string GetTimeFromFloat(float time)
        {
            var t = (int) time;
            return t / 60 + ":" + (t % 60 < 10 ? "0" : "") + t % 60;
        }

        public void TimeChange(float value)
        {
            audioSource.time = Math.Max(Math.Min(value * audioSource.clip.length, audioSource.clip.length-0.001f), 0);
            if (!audioSource.isPlaying && !pauseState) audioSource.Play();
        }

        public void Pause(bool status)
        {
            pauseState = status;
            if (pauseState) audioSource.Pause();
            else audioSource.Play();
        }

        public void Skip()
        {
            audioSource.time = Math.Min(audioSource.clip.length-0.001f, audioSource.time + 10);
        }

        public void Reverse()
        {
            audioSource.time = Math.Max(0, audioSource.time - 10);
        }

        public void GotoStart()
        {
            audioSource.time = 0;
        }
        
        public void GotoEnd()
        {
            audioSource.time = audioSource.clip.length - 0.001f;
        }
    }
}
