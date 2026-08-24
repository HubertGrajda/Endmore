using System;

namespace Scripts
{
    public interface IPauseService
    {
        public event Action<bool> OnPauseStateChanged;
        public bool IsPaused { get; }
        public void AddPauseSource(object source);
        public void RemovePauseSource(object source);
    }
}