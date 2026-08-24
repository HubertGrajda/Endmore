using System;
using UnityEngine;

namespace Scripts
{
    public class TimeService : ITimeService, IDisposable
    {
        public float TargetTimeScale { get; private set; } = 1f;
        public float TimeScaleModifier { get; private set; } = 1f;

        private readonly IPauseService _pauseService;

        public TimeService(IPauseService pauseService)
        {
            _pauseService = pauseService;
            _pauseService.OnPauseStateChanged += HandlePauseStateChanged;
        }

        private void HandlePauseStateChanged(bool isPaused)
        {
            RefreshTimeScale();
        }

        private void RefreshTimeScale()
        {
            if (_pauseService is { IsPaused: true })
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = TargetTimeScale * TimeScaleModifier;
            }
        }

        public void SetTargetTimeScale(float targetTimeScale)
        {
            TargetTimeScale = targetTimeScale;
            RefreshTimeScale();
        }

        public void SetTimeScaleModifier(float modifier)
        {
            TimeScaleModifier = modifier;
            RefreshTimeScale();
        }

        public void Dispose()
        {
            if (_pauseService != null)
                _pauseService.OnPauseStateChanged -= HandlePauseStateChanged;
        }
    }
}