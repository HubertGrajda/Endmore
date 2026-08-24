using System;
using System.Collections.Generic;

namespace Scripts
{
    public class PauseService : IPauseService
    {
        public event Action<bool> OnPauseStateChanged;
        
        private readonly HashSet<object> _pauseSources = new();
        public bool IsPaused { get; private set; }
        
        public void AddPauseSource(object source)
        {
            if (source == null) return;
            
            var pauseSourceAdded = _pauseSources.Add(source);
    
            if (!IsPaused && pauseSourceAdded)
            {
                PauseGame();
            }
        }
    
        public void RemovePauseSource(object source)
        {
            if (source == null) return;
    
            var lastSourceRemoved = _pauseSources.Remove(source) && _pauseSources.Count == 0;
    
            if (IsPaused && lastSourceRemoved)
            {
                ResumeGame();
            }
        }

        private void PauseGame()
        {
            if (IsPaused) return;

            IsPaused = true;
            OnPauseStateChanged?.Invoke(true);
        }

        private void ResumeGame()
        {
            if (!IsPaused) return;
            
            IsPaused = false;
            OnPauseStateChanged?.Invoke(false);
        }
    }
}