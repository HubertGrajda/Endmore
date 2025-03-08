using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts
{
    public class ScenesManager : Singleton<ScenesManager>
    {
        [SerializeField] private Image fader;
        [SerializeField] private float fadeDuration = 1f;

        private const float FADE_IN_ALPHA = 1F;
        private const float FADE_OUT_ALPHA = 0F;
        
        public event Action OnSceneChange;
        public event Action OnSceneChanged;

        private bool _isBusy;
        
        public void LaunchSceneByName(string sceneName)
        {
            if (_isBusy) return;
            if (string.IsNullOrWhiteSpace(sceneName)) return;
            
            _isBusy = true;
            OnSceneChange?.Invoke();

            if (fader == null)
            {
                LoadScene(sceneName);
                return;
            }
            
            fader.DOFade(FADE_IN_ALPHA, fadeDuration)
                .SetUpdate(true)
                .OnComplete(() => LoadScene(sceneName));
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (fader)
            {
                fader.DOFade(FADE_OUT_ALPHA, fadeDuration).SetUpdate(true);
            }
            
            _isBusy = false;
            OnSceneChanged?.Invoke();
        }
    }
}