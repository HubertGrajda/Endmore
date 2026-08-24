using System;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Audio
{
    public class AudioService : MonoService<IAudioService>, IAudioService
    {
        [SerializeField] private AudioMixer mainAudioMixer;
        [SerializeField] private AudioSource oneShotAudioSource;

        private const string MASTER_VOLUME_PARAM = "MasterVolume";
        private const string MUSIC_VOLUME_PARAM = "MusicVolume";
        private const string SOUNDS_VOLUME_PARAM = "SoundsVolume";

        private const float MIN_LINEAR_VOLUME = 0.0001f;
        private const float MIN_DB_VOLUME = -80f;
        private const float DECIBEL_TO_LINEAR_FACTOR = 20f;

        [Inject] private IPauseService _pauseService;

        private void Awake()
        {
            if (oneShotAudioSource != null)
                oneShotAudioSource.ignoreListenerPause = true;
        }

        private void Start()
        {
            InitVolumeFromPlayerPrefs();
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
        
        private void AddListeners()
        {
            if (_pauseService != null)
                _pauseService.OnPauseStateChanged += ToggleAudioPause;
        }

        private void RemoveListeners()
        {
            if (_pauseService != null)
                _pauseService.OnPauseStateChanged -= ToggleAudioPause;
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
            if (mainAudioMixer == null || !mainAudioMixer.GetFloat(volumeParameterName, out var currentVolumeDb))
            {
                Debug.LogError($"{name}: {nameof(mainAudioMixer)} does not contain '{volumeParameterName}' parameter.");
                return default;
            }
            
            return MathF.Pow(10, currentVolumeDb / DECIBEL_TO_LINEAR_FACTOR);
        }

        public void SetVolume(string volumeParameterName, float value)
        {
            if (mainAudioMixer == null) return;
            
            var dbVolume = value > MIN_LINEAR_VOLUME
                ? Mathf.Log10(Mathf.Clamp01(value)) * DECIBEL_TO_LINEAR_FACTOR 
                : MIN_DB_VOLUME;

            mainAudioMixer.SetFloat(volumeParameterName, dbVolume);
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

        public void ToggleAudioPause(bool isPaused)
        {
            if (isPaused)
            {
                PauseAudio();
            }
            else
            {
                ResumeAudio();
            }
        }
        
        public void PauseAudio()
        {
            AudioListener.pause = true;
        }

        public void ResumeAudio()
        {
            AudioListener.pause = false;
        }
    }
}