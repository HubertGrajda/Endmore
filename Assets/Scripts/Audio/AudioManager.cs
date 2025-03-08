using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioMixer mainAudioMixer;
        [SerializeField] private AudioSource oneShotAudioSource;

        private const string MASTER_VOLUME_PARAM = "MasterVolume";
        private const string MUSIC_VOLUME_PARAM = "MusicVolume";
        private const string SOUNDS_VOLUME_PARAM = "SoundsVolume";
        
        private const float DECIBEL_TO_LINEAR_FACTOR = 20f;
        
        private void Start()
        {
            InitVolumeFromPlayerPrefs();
        }

        public void PlayOneShot(AudioClip clip)
        {
            if (!clip) return;
            
            if (!oneShotAudioSource)
            {
                Debug.LogError($"{name}: {nameof(oneShotAudioSource)} is not assigned. Sound will not be played.");
                return;
            }
            
            oneShotAudioSource.PlayOneShot(clip);
        }
        
        public float GetVolume(string volumeParameterName)
        {
            if (!mainAudioMixer.GetFloat(volumeParameterName, out var currentVolume))
            {
                Debug.LogError($"{name}: {nameof(mainAudioMixer)} does not contain '{volumeParameterName}' parameter.");
                return default;
            }
            
            currentVolume = MathF.Pow(10, currentVolume/DECIBEL_TO_LINEAR_FACTOR);
            return currentVolume;
        }

        public void SetVolume(string volumeParameterName, float value)
        {
            if (mainAudioMixer == null) return;
            
            value = value <= 0 ? Mathf.Epsilon : value;

            mainAudioMixer.SetFloat(volumeParameterName, Mathf.Log10(value) * DECIBEL_TO_LINEAR_FACTOR);
        }

        private void InitVolumeFromPlayerPrefs()
        {
            SetVolumeFromPlayerPrefs(MASTER_VOLUME_PARAM);
            SetVolumeFromPlayerPrefs(MUSIC_VOLUME_PARAM);
            SetVolumeFromPlayerPrefs(SOUNDS_VOLUME_PARAM);
        }
        
        private void SetVolumeFromPlayerPrefs(string volumeParameterName)
        {
            if (!PlayerPrefs.HasKey(volumeParameterName)) return;
            
            var volume = PlayerPrefs.GetFloat(volumeParameterName);
                
            SetVolume(volumeParameterName, volume);
        }
    }
}