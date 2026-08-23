using Reflex.Attributes;
using UnityEngine;

namespace Scripts.Audio
{
    public class SoundLauncher : MonoBehaviour
    {
        [Inject] private IAudioService _audioService;
        
        public void PlaySound(AudioClip clip)
        {
            if (!clip) return;
            
            _audioService.PlayOneShot(clip);
        }
    }
}