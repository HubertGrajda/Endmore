using System;
using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class Timer : MonoBehaviour
    {
        public event Action<TimeSpan> OnSecondTick;
        
        private Coroutine _countingCoroutine;
        
        private bool _isActive;
        private float _currentTime;
        private int _currentSecond;
        
        public TimeSpan ElapsedTime => TimeSpan.FromSeconds(_currentTime);
        
        public void StartTimer()
        {
            if (_countingCoroutine != null)
            {
                Debug.LogWarning($"{name}: Timer is already running!");
                return;
            }
            
            _countingCoroutine = StartCoroutine(StartCounting());
        }

        public void StopTimer()
        {
            if (_countingCoroutine == null) return;

            StopCoroutine(_countingCoroutine);
            
            _isActive = false;
            _countingCoroutine = null;
        }

        private IEnumerator StartCounting()
        {
            _currentTime = 0f;
            _isActive = true;
            
            while (_isActive)
            {
                _currentTime += Time.deltaTime;
                var seconds = (int)_currentTime;

                if (_currentSecond != seconds)
                {
                    _currentSecond = seconds;
                    OnSecondTick?.Invoke(ElapsedTime);
                }
                
                yield return null;
            }
        }

        private void OnDisable()
        {
            StopTimer();
        }
    }
}