using UnityEngine;

namespace Scripts.Audio
{
    public class SoundLauncher : MonoBehaviour
    {
        private AudioManager _audioManager;
        
        private void Start()
        {
            _audioManager = AudioManager.Instance;
        }

        public void PlaySound(AudioClip clip)
        {
            if (!_audioManager || !clip) return;
            
            _audioManager.PlayOneShot(clip);
        }
    }
}