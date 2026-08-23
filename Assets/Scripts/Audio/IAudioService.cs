using UnityEngine;

namespace Scripts.Audio
{
    public interface IAudioService
    {
        void PlayOneShot(AudioClip clip);
        float GetVolume(string volumeParameterName);
        void SetVolume(string volumeParameterName, float value);
    }
}